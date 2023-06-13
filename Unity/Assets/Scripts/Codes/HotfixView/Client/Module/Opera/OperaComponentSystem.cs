using System;
using System.Collections.Generic;
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
                OperaComponent.Instance = self;
                self.ActionForListen = new List<string>();
                self.mapMask = LayerMask.GetMask("Map");
                self.InputActionAsset=ResComponent.Instance.LoadAsset<InputActionAsset>(ResPathHelper.GetInputPath("Default"));
                var rebinds = PlayerPrefs.GetString("rebinds");
                if (!string.IsNullOrEmpty(rebinds))
                    self.InputActionAsset.LoadBindingOverridesFromJson(rebinds);
                
                self.Actions = new Dictionary<string, InputAction>();
                foreach (InputActionMap inputActionMap in self.InputActionAsset.actionMaps)
                {
                    foreach (InputAction inputAction in inputActionMap.actions)
                    {
                        self.Actions.Add(inputAction.name,inputAction);
                    }
                }
                //OperaWatcherComponent.Instance.RegisterInputEntity(self);
            }
        }

        [ObjectSystem]
        public class  OperaComponentDestorySystem: DestroySystem<OperaComponent>
        {
            protected override void Destroy(OperaComponent self)
            {
                //OperaWatcherComponent.Instance.RemoveInputEntity(self);
            }
        }
        
        [OperaSystem(OperaID.Fire)]
        public class OperaComponentInputSystem_Move: OperaSystem<OperaComponent>
        {
            protected override void Run(OperaComponent self, string actionName)
            {
                UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                UnityEngine.RaycastHit hit;
                if (UnityEngine.Physics.Raycast(ray, out hit, 1000, self.mapMask))
                {
                    C2M_PathfindingResult c2MPathfindingResult = new C2M_PathfindingResult();
                    c2MPathfindingResult.Position = hit.point;
                    self.ClientScene().GetComponent<SessionComponent>().Session.Send(c2MPathfindingResult);
                }
            }
        }
        
        [OperaSystem(OperaID.ReLoad)]
        public class OperaComponentInputSystem_ReLoad: OperaSystem<OperaComponent>
        {
            protected override void Run(OperaComponent self, string actionName)
            {
                CodeLoader.Instance.LoadHotfix();
                EventSystem.Instance.Load();
                Log.Debug("hot reload success!");
            }
        }
        
        [ObjectSystem]
        public class OperaComponentUpdateSystem : UpdateSystem<OperaComponent>
        {
            protected override void Update(OperaComponent self)
            {
                foreach (var selfAction in self.Actions)
                {
                    if (selfAction.Value.WasPerformedThisFrame())
                    {
                        OperaWatcherComponent.Instance?.RunCheck(selfAction.Key);
                    }
                }
            }
        }

        public static void Save(this OperaComponent self)
        {
            var rebinds = self.InputActionAsset.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString("rebinds", rebinds);
        }
        
        public static void AddListenter(this OperaComponent self, string actionName)
        {
            if (!self.ActionForListen.Contains(actionName))
            {
                self.ActionForListen.Add(actionName);
            }
        }
    }
}