using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ET.Client
{
    [FriendOf(typeof(OperaComponent))]
    public static class OperaComponentSystem
    {
        [ObjectSystem]
        public class OperaComponentAwakeSystem : AwakeSystem<OperaComponent>
        {
            protected override void Awake(OperaComponent self)
            {
                self.mapMask = LayerMask.GetMask("Map");
                self.InputActionAsset=ResComponent.Instance.LoadAsset<InputActionAsset>(ResPathHelper.GetInputPath("Default"));
                var rebinds = PlayerPrefs.GetString("rebinds");
                if (!string.IsNullOrEmpty(rebinds))
                    self.InputActionAsset.LoadBindingOverridesFromJson(rebinds);
                
                
                self.Slot1 = self.InputActionAsset.FindAction(OperaID.Slot1);
                self.Slot2 = self.InputActionAsset.FindAction(OperaID.Slot2);
                self.ShowInfo = self.InputActionAsset.FindAction(OperaID.ShowInfo);
            }
        }

        [ObjectSystem]
        public class  OperaComponentDestorySystem: DestroySystem<OperaComponent>
        {
            protected override void Destroy(OperaComponent self)
            {
                
            }
        }
        
        [ObjectSystem]
        public class OperaComponentUpdateSystem : UpdateSystem<OperaComponent>
        {
            protected override void Update(OperaComponent self)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 1000, self.mapMask))
                    {
                        C2M_PathfindingResult c2MPathfindingResult = new C2M_PathfindingResult();
                        c2MPathfindingResult.Position = hit.point;
                        self.ClientScene().GetComponent<SessionComponent>().Session.Send(c2MPathfindingResult);
                    }
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    CodeLoader.Instance.LoadHotfix();
                    EventSystem.Instance.Load();
                    Log.Debug("hot reload success!");
                }
            
                if (Input.GetKeyDown(KeyCode.T))
                {
                    C2M_TransferMap c2MTransferMap = new C2M_TransferMap();
                    self.ClientScene().GetComponent<SessionComponent>().Session.Call(c2MTransferMap).Coroutine();
                }

                if (self.Slot1.WasPerformedThisFrame())
                {
                    EventSystem.Instance.Publish(self.ClientScene(), new EventType.DoOpera() { OperaId = OperaID.Slot1 });
                }
                if (self.Slot2.WasPerformedThisFrame())
                {
                    EventSystem.Instance.Publish(self.ClientScene(), new EventType.DoOpera() { OperaId = OperaID.Slot2 });
                }
                
                if (self.ShowInfo.WasPerformedThisFrame())
                {
                    EventSystem.Instance.Publish(self.ClientScene(), new EventType.DoOpera() { OperaId = OperaID.ShowInfo });
                }
            }
        }

        public static void Save(this OperaComponent self)
        {
            var rebinds = self.InputActionAsset.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString("rebinds", rebinds);
        }
    }
}