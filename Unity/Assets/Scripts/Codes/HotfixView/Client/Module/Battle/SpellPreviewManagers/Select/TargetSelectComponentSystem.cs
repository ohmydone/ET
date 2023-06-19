using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ObjectSystem]
    public class TargetSelectComponentAwakeSystem : AwakeSystem<TargetSelectComponent>
    {
        protected override void Awake(TargetSelectComponent self)
        {
            //CursorImage = GetComponent<Image>();
            self.CursorColor = Color.white;
            self.waiter = ETTask<GameObject>.Create(); 
            
            self.Init().Coroutine();
             
            self.HeroObj =UnitComponent.Instance.My.GetComponent<GameObjectComponent>().GameObject;
            InputWatcherComponent.Instance.RegisterInputEntity(self);
        }
    }
    [ObjectSystem]
    [FriendOf(typeof(AOIUnitComponent))]
    public class TargetSelectComponentUpdateSystem : UpdateSystem<TargetSelectComponent>
    {
        protected override void Update(TargetSelectComponent self)
        {
            if (self.RangeCircleObj == null||!self.IsShow) return;
            self.RangeCircleObj.transform.position = self.HeroObj.transform.position;
            self.CursorImage.rectTransform.anchoredPosition = Input.mousePosition*UIComponent.Instance.ScreenSizeflag;
            
            if (RaycastHelper.CastUnitObj(CameraManagerComponent.Instance.MainCamera(), out var obj))
            {
                var uidC = obj.GetComponentInParent<Unit>();
                if (uidC != null)
                {
                    var unit = UnitComponent.Instance?.GetChild<Unit>(uidC.Id);
                    var canUse = self.CanSkillToUnit(unit);
                    if (canUse)
                    {
                        if (self.TargetLimitType == SkillAffectTargetType.EnemyTeam)
                        {
                            self.CursorImage.color = Color.red;
                        }
                        else if (self.TargetLimitType == SkillAffectTargetType.SelfTeam||self.TargetLimitType == SkillAffectTargetType.Self)
                        {
                            self.CursorImage.color = Color.green;
                        }
                        return;
                    }
                }
            }
            self.CursorImage.color = self.CursorColor;
            
        }
    }
    [InputSystem((int)KeyCode.Mouse0,InputType.KeyDown,100)]
    public class TargetSelectComponentInputSystem_Load : InputSystem<TargetSelectComponent>
    {
        public override void Run(TargetSelectComponent self, int key, int type, ref bool stop)
        {
            if (self.RangeCircleObj == null||!self.IsShow) return;
            stop = self.RunCheck();
        }
    }
    [ObjectSystem]
    public class TargetSelectComponentDestroySystem : DestroySystem<TargetSelectComponent>
    {
        protected override void Destroy(TargetSelectComponent self)
        {
            GameObject.DestroyImmediate(self.gameObject); 
            GameObject.DestroyImmediate(self.CursorImage.gameObject); 
            //GameObjectPoolComponent.Instance?.RecycleGameObject(self.CursorImage.gameObject);
            InputWatcherComponent.Instance?.RemoveInputEntity(self);
        }
    }
    [SelectSystem]
    [FriendOf(typeof(TargetSelectComponent))]
    public class TargetSelectComponentShowSelectSystem : ShowSelectSystem<TargetSelectComponent,Action<Unit>, int[]>
    {
        public override async ETTask OnShow(TargetSelectComponent self ,Action<Unit> onSelectedCallback, int[] previewRange)
        {
            if (previewRange == null || previewRange.Length == 0)//不填或者填非正数表示无限距离
            {
                self.Distance = 0;
            }
            else if (previewRange.Length != 1)
            {
                Log.Error("技能预览配置错误！！！");
                return;
            }
            if (self.waiter != null) await self.waiter;
            self.Distance = previewRange[0];
            self.gameObject.SetActive(true);
            Cursor.visible = false;
            self.CursorImage.gameObject.SetActive(true);
            self.RangeCircleObj.transform.localScale = Vector3.one*self.Distance;
            //self.OnSelectTargetCallback = onSelectedCallback;
            self.IsShow = true;
        }
    }

    [SelectSystem]
    [FriendOf(typeof(TargetSelectComponent))]
    public class TargetSelectComponentHideSelectSystem : HideSelectSystem<TargetSelectComponent>
    {
        public override void OnHide(TargetSelectComponent self)
        {
            self.IsShow = false;
            if (self.waiter != null) return;
            Cursor.visible = true;
            self.CursorImage.gameObject.SetActive(false);
            self.gameObject.SetActive(false);
        }
    }
    [SelectSystem]
    [FriendOf(typeof(TargetSelectComponent))]
    public class TargetSelectComponentAutoSpellSystem : AutoSpellSystem<TargetSelectComponent,Action<Unit>, int[]>
    {
        public override void OnAutoSpell(TargetSelectComponent self ,Action<Unit> onSelectedCallback, int[] previewRange)
        {
            if (previewRange == null || previewRange.Length == 0)//不填或者填非正数表示无限距离
            {
                self.Distance = 0;
            }
            else if (previewRange.Length != 1)
            {
                Log.Error("技能预览配置错误！！！");
                return;
            }
            self.Distance = previewRange[0];
            //self.OnSelectTargetCallback = onSelectedCallback;
            self.IsShow = true;
            self.RunCheck();
        }
    }

    [FriendOf(typeof(TargetSelectComponent))]
    public static class TargetSelectComponentSystem
    {
        public static async ETTask Init(this TargetSelectComponent self)
        {
            using (ListComponent<ETTask> tasks = ListComponent<ETTask>.Create())
            {
                tasks.Add(self.GetTargetIcon());
                tasks.Add(self.GetTargetSelectManager());
                await ETTaskHelper.WaitAll(tasks);
                self.waiter.SetResult(self.gameObject);
                self.waiter = null;
            }

        }
        private static async ETTask GetTargetIcon(this TargetSelectComponent self)
        {
            string targetPath = ResPathHelper.GetSpellPreviewPath("TargetIcon");
            var obj = await ResComponent.Instance.LoadAssetAsync<GameObject>(targetPath);
            GameObject go = GameObject.Instantiate(obj);
            self.CursorImage = go.GetComponent<Image>();
            self.CursorImage.transform.parent = GlobalComponent.Instance.PopUpRoot.transform;
            self.CursorImage.transform.localPosition = Vector3.zero;
            self.CursorImage.rectTransform.anchoredPosition = Input.mousePosition;
            if (!self.IsShow)
            {
                self.CursorImage.gameObject.SetActive(false);
            }
        }
        
        private static async ETTask GetTargetSelectManager(this TargetSelectComponent self)
        {
            string path = ResPathHelper.GetSpellPreviewPath("TargetSelectManager");
            var obj = await ResComponent.Instance.LoadAssetAsync<GameObject>(path);
            GameObject go = GameObject.Instantiate(obj);
            self.RangeCircleObj = go.transform.Find("RangeCircle").gameObject;
            self.gameObject = go;
            if (!self.IsShow)
            {
                self.gameObject.SetActive(false);
            }
        }
        
        public static Ray GetRay(this TargetSelectComponent self,float dis = 100f)
        {
            var ray = CameraManagerComponent.Instance.MainCamera().ScreenPointToRay(Input.mousePosition);
            return new Ray
            {
                Dir = ray.direction,
                Start = ray.origin,
                Distance = dis
            };
        }

        public static bool CanSkillToUnit(this TargetSelectComponent self,Unit unit)
        {
            // 根据UnitType判断
            // var aoiU = unit?.GetComponent<AOIUnitComponent>();
            // if (aoiU == null) return false;
            //
            // UnitType[] res = null;
            // if (self.TargetLimitType == SkillAffectTargetType.EnemyTeam)
            //     res = new []{ UnitType.Monster};
            // else if (self.TargetLimitType == SkillAffectTargetType.SelfTeam||self.TargetLimitType == SkillAffectTargetType.Self)
            //     res = new []{ UnitType.Player};
            // for (int i = 0; i < res.Length; i++)
            // {
            //     if (res[i] == aoiU.Type || res[i] == UnitType.ALL)
            //     {
            //         if (self.distance > 0)
            //         {
            //             if (self.Mode == 0)
            //             {
            //                 var pos1 = new Vector2(unit.Position.x, unit.Position.z);
            //                 var pos2 = new Vector2(self.HeroObj.transform.position.x, self.HeroObj.transform.position.z);
            //                 if (Vector2.Distance(pos1, pos2) >= self.distance)
            //                 {
            //                     return false;
            //                 }
            //             }
            //         }
            //         return true;
            //     }
            // }
            // return false;
            
            if (self.Distance > 0)
            {
                //测试，只要不是自己就是敌人
                if (self.Mode == 0)
                {
                    var pos1 = new Vector2(unit.Position.x, unit.Position.z);
                    var pos2 = new Vector2(self.HeroObj.transform.position.x, self.HeroObj.transform.position.z);
                    if (Vector2.Distance(pos1, pos2) >= self.Distance)
                    {
                        return false;
                    }
                }
            }

            if (self.TargetLimitType == SkillAffectTargetType.EnemyTeam)
                return unit.Id != self.Id;
            if (self.TargetLimitType == SkillAffectTargetType.SelfTeam||self.TargetLimitType == SkillAffectTargetType.Self)
                return unit.Id == self.Id;
            return false;
        }
        
        public static bool RunCheck(this TargetSelectComponent self)
        {
            
            if (RaycastHelper.CastUnitObj(CameraManagerComponent.Instance.MainCamera(), out var obj))
            {
                var uidC = obj.GetComponentInParent<Unit>();
                if (uidC != null)
                {
                    var unit = UnitComponent.Instance?.GetChild<Unit>(uidC.Id);
                    var canUse = self.CanSkillToUnit(unit);
                    if (canUse)
                    {
                        SelectWatcherComponent.Instance.Hide(self);
                        EventSystem.Instance.Publish(self.DomainScene(),new EventType.OnSelectTarget()
                        {
                            Unit = unit
                        });
                        return true;
                    }
                }
            }
            SelectWatcherComponent.Instance.Hide(self);
            return false;
        }
    }
}