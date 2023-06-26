using UnityEngine;
using System;
namespace ET.Client
{
    [ObjectSystem]
    public class DirectRectSelectComponentAwakeSystem : AwakeSystem<DirectRectSelectComponent>
    {
        protected override void Awake(DirectRectSelectComponent self)
        {
            self.waiter = ETTask<GameObject>.Create(); 

            string path =ResPathHelper.GetSpellPreviewPath("DirectRectSelectManager"); 
            // var obj= ResComponent.Instance.LoadAsset<GameObject>(path);
            // self.gameObject =GameObject.Instantiate(obj);
            
            self.gameObject= GameObjectPoolComponent.Instance.GetGameObject(path);
            self.DirectObj = self.gameObject.transform.GetChild(0).gameObject;
            self.AreaObj = self.DirectObj.transform.GetChild(0).gameObject;
            self.waiter.SetResult(self.gameObject);
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
    public class DirectRectSelectComponentUpdateSystem : UpdateSystem<DirectRectSelectComponent>
    {
        protected override void Update(DirectRectSelectComponent self)
        {
            if (self.DirectObj == null||!self.IsShow) return;
            self.DirectObj.transform.position = new Vector3( self.HeroObj.transform.position.x, self.HeroObj.transform.position.y,  self.HeroObj.transform.position.z);
            if (RaycastHelper.CastMapPoint(CameraManagerComponent.Instance.MainCamera(), out var hitPoint))
            {
                self.DirectObj.transform.forward = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z) -  self.DirectObj.transform.position;
            }
        }
    }
    [ObjectSystem]
    public class DirectRectSelectComponentDestroySystem : DestroySystem<DirectRectSelectComponent>
    {
        protected override void Destroy(DirectRectSelectComponent self)
        {
            GameObject.DestroyImmediate(self.gameObject); 
            InputWatcherComponent.Instance?.RemoveInputEntity(self);
        }
    }
        
    [InputSystem((int)KeyCode.Mouse0,InputType.KeyDown,100)]
    public class DirectRectSelectComponentInputSystem_Load : InputSystem<DirectRectSelectComponent>
    {
        public override void Run(DirectRectSelectComponent self, int key, int type, ref bool stop)
        {
            if (self.DirectObj == null||!self.IsShow) return;
            stop = self.RunCheck();
        }
    }
    [SelectSystem]
    [FriendOf(typeof(DirectRectSelectComponent))]
    public class DirectRectSelectComponentShowSelectSystem : ShowSelectSystem<DirectRectSelectComponent,Action<Vector3>, int[]>
    {
        public override async ETTask OnShow(DirectRectSelectComponent self ,Action<Vector3> onSelectedCallback, int[] previewRange)
        {
            if (previewRange == null || previewRange.Length != 2)
            {
                Log.Error("技能预览配置错误！！！");
                return;
            }
            if (self.waiter != null) await self.waiter;
            self.distance = previewRange[0];
            self.width = previewRange[1];
            self.gameObject.SetActive(true);
            //self.OnSelectedCallback = onSelectedCallback;
            self.SetArea(self.distance, self.width);
            self.IsShow = true;
        }
    }

    [SelectSystem]
    [FriendOf(typeof(DirectRectSelectComponent))]
    public class DirectRectSelectComponentHideSelectSystem : HideSelectSystem<DirectRectSelectComponent>
    {
        public override void OnHide(DirectRectSelectComponent self)
        {
            self.IsShow = false;
            if (self.waiter != null) return;
            self.gameObject.SetActive(false);
        }
    }
    
    [SelectSystem]
    [FriendOf(typeof(DirectRectSelectComponent))]
    public class DirectRectSelectComponentAutoSpellSystem : AutoSpellSystem<DirectRectSelectComponent,Action<Vector3>, int[]>
    {
        public override void OnAutoSpell(DirectRectSelectComponent self ,Action<Vector3> onSelectedCallback, int[] previewRange)
        {
            if (previewRange == null || previewRange.Length != 2)
            {
                Log.Error("技能预览配置错误！！！");
                return;
            }
            self.distance = previewRange[0];
            self.width = previewRange[1];
            //self.OnSelectedCallback = onSelectedCallback;
            self.RunCheck();
        }
    }
    
    [FriendOf(typeof(DirectRectSelectComponent))]
    public static class DirectRectSelectComponentSystem
    {
        public static void SetArea(this DirectRectSelectComponent self,float length, float width)
        {
            self.AreaObj.transform.localScale = new Vector3(width, length, 10);
            self.AreaObj.transform.localPosition = Vector3.zero;
        }

        public static bool RunCheck(this DirectRectSelectComponent self)
        {
            if (RaycastHelper.CastMapPoint(CameraManagerComponent.Instance.MainCamera(), out var hitPoint))
            {
                SelectWatcherComponent.Instance.Hide(self);
                EventSystem.Instance.Publish(self.DomainScene(),new EventType.OnDirectRectSelect()
                {
                    
                    pos = hitPoint
                });
                return true;
            }
            return false;
        }
    }
}