using System;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    [ObjectSystem]
    public class OperaComponentAwakeSystem : AwakeSystem<OperaComponent>
    {
        protected override void Awake(OperaComponent self)
        {
            self.mapMask = LayerMask.GetMask("Map");
            InputWatcherComponent.Instance.RegisterInputEntity(self);
        }
    }
    
    [ObjectSystem]
    public class OperaComponentDestroySystem : DestroySystem<OperaComponent>
    {
        protected override void Destroy(OperaComponent self)
        {
            InputWatcherComponent.Instance.RemoveInputEntity(self);
        }
    }
    
    
    [InputSystem((int)KeyCode.Mouse0,InputType.KeyDown)]
    public class OperaComponentInputSystem_Move : InputSystem<OperaComponent>
    {
        public override void Run(OperaComponent self, int key, int type, ref bool stop)
        {
            var unit = UnitComponent.Instance.My;
            if (unit == null) return;
            if (unit.GetComponent<MoveComponent>()==null)
            {
                Log.Error("暂时无法移动");
                return ;
            }
            UnityEngine.Ray ray = CameraManagerComponent.Instance.MainCamera().ScreenPointToRay(Input.mousePosition);
            UnityEngine.RaycastHit hit;
            if (UnityEngine.Physics.Raycast(ray, out hit, 1000, self.mapMask))
            {
                self.ClickPoint = hit.point;
                self.frameClickMap.Position = self.ClickPoint;
                unit.ClientScene().GetComponent<SessionComponent>().Session.Send(self.frameClickMap);
                unit.GetComponent<CombatUnitComponent>().GetComponent<MoveAndSpellComponent>().Cancel();//取消施法
            }
        }
    }
    
    [FriendOf(typeof(OperaComponent))]
    public static class OperaComponentSystem
    {

    }
}