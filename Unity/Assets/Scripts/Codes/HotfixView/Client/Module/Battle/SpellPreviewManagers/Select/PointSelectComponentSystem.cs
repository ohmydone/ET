using UnityEngine;
using System;
namespace ET.Client
{
    [ObjectSystem]
    public class PointSelectManagerAwakeSystem : AwakeSystem<PointSelectComponent>
    {
        protected override void Awake(PointSelectComponent self)
        {
            self.waiter = ETTask<GameObject>.Create();
            string path =ResPathHelper.GetSpellPreviewPath("PointSelectManager"); 
            //var obj= ResComponent.Instance.LoadAsset<GameObject>(path);
            //self.gameObject =GameObject.Instantiate(obj);
            self.gameObject = GameObjectPoolComponent.Instance.GetGameObject(path);
            self.RangeCircleObj =  self.gameObject.transform.Find("RangeCircle").gameObject;
            self.SkillPointObj=  self.gameObject.transform.Find("SkillPointPreview").gameObject;
            self.SkillPointObj.SetActive(true);
            self.waiter.SetResult( self.gameObject);
            self.waiter = null;
            if (!self.IsShow)
            {
                self.gameObject.SetActive(false);
            }
            self.HeroObj = UnitComponent.Instance.My.GetComponent<GameObjectComponent>().GameObject;
            InputWatcherComponent.Instance.RegisterInputEntity(self);
        }
    }
    [ObjectSystem]
    public class PointSelectManagerUpdateSystem : UpdateSystem<PointSelectComponent>
    {
        protected override void Update(PointSelectComponent self)
        {
            if (self.RangeCircleObj == null||!self.IsShow) return;
            self.RangeCircleObj.transform.position = self.HeroObj.transform.position;
            if (RaycastHelper.CastMapPoint(CameraManagerComponent.Instance.MainCamera(), out var hitPoint))
            {
                var nowpos = self.HeroObj.transform.position;
                if (Vector2.Distance(new Vector2(nowpos.x, nowpos.z), new Vector2(hitPoint.x, hitPoint.z)) >
                    self.distance)
                {
                    var dir =new Vector3(hitPoint.x - nowpos.x,0, hitPoint.z - nowpos.z).normalized;
                    hitPoint = nowpos + dir * self.distance;
                }
                self.SkillPointObj.transform.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
            }
        }
    }
    [InputSystem((int)KeyCode.Mouse0,InputType.KeyDown,100)]
    public class PointSelectComponentInputSystem_Load : InputSystem<PointSelectComponent>
    {
        public override void Run(PointSelectComponent self, int key, int type, ref bool stop)
        {
            if (self.RangeCircleObj == null||!self.IsShow) return;
            stop = self.RunCheck();

        }
    }
    [ObjectSystem]
    public class PointSelectManagerDestroySystem : DestroySystem<PointSelectComponent>
    {
        protected override void Destroy(PointSelectComponent self)
        {
            GameObject.DestroyImmediate(self.gameObject); 
            InputWatcherComponent.Instance?.RemoveInputEntity(self);
        }
    }
    
    [SelectSystem]
    [FriendOf(typeof(PointSelectComponent))]
    public class PointSelectComponentShowSelectSystem : ShowSelectSystem<PointSelectComponent,Action< Vector3>, int[]>
    {
        public override async ETTask OnShow(PointSelectComponent self ,Action< Vector3> onSelectedCallback, int[] previewRange)
        {
            if (previewRange == null || previewRange.Length != 2)
            {
                Log.Error("技能预览配置错误！！！");
                return;
            }
            if (self.waiter != null) await self.waiter;
            self.distance = previewRange[0];
            self.range = previewRange[1];
            self.gameObject.SetActive(true);
            self.RangeCircleObj.transform.localScale = Vector3.one*self.distance;
            self.SkillPointObj.transform.localScale = Vector3.one*self.range;
            //self.OnSelectPointCallback = onSelectedCallback;
            self.IsShow = true;
            
        }
    }

    [SelectSystem]
    [FriendOf(typeof(PointSelectComponent))]
    public class PointSelectComponentHideSelectSystem : HideSelectSystem<PointSelectComponent>
    {
        public override void OnHide(PointSelectComponent self)
        {
            self.IsShow = false;
            if (self.waiter != null) return;
            self.gameObject.SetActive(false);
        }
    }
    [SelectSystem]
    [FriendOf(typeof(PointSelectComponent))]
    public class PointSelectComponentAutoSpellSystem : AutoSpellSystem<PointSelectComponent,Action<Vector3>, int[]>
    {
        public override void OnAutoSpell(PointSelectComponent self ,Action<Vector3> onSelectedCallback, int[] previewRange)
        {
            if (previewRange == null || previewRange.Length != 2)
            {
                Log.Error("技能预览配置错误！！！");
                return;
            }
            self.distance = previewRange[0];
            self.range = previewRange[1];
            //self.OnSelectPointCallback = onSelectedCallback;
            self.RunCheck();
        }
    }
    [FriendOf(typeof(PointSelectComponent))]
    public static class PointSelectComponentSystem
    {
        public static bool RunCheck(this PointSelectComponent self)
        {
            
            if (RaycastHelper.CastMapPoint(CameraManagerComponent.Instance.MainCamera(), out var hitPoint))
            {
                var nowpos = self.HeroObj.transform.position;
                if (Vector2.Distance(new Vector2(nowpos.x, nowpos.z), new Vector2(hitPoint.x, hitPoint.z)) >
                    self.distance)
                {
                    var dir =new Vector3(hitPoint.x - nowpos.x,0, hitPoint.z - nowpos.z).normalized;
                    hitPoint = nowpos + dir * self.distance;
                }
                SelectWatcherComponent.Instance.Hide(self);
                EventSystem.Instance.Publish(self.DomainScene(),new EventType.OnPointSelect()
                {
                    pos = hitPoint
                });
                return true;
            }

            return false;
        }
    }
}