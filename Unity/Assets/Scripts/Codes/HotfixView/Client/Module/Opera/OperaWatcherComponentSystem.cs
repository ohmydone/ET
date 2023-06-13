using System;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof (OperaWatcherComponent))]
    public static class OperaWatcherComponentSystem
    {
        [ObjectSystem]
        public class OperaWatcherComponentAwakeSystem: AwakeSystem<OperaWatcherComponent>
        {
            protected override void Awake(OperaWatcherComponent self)
            {
                OperaWatcherComponent.Instance = self;
                self.Init();
                
                self.RegisterInputEntity(OperaComponent.Instance);
            }
        }

        [ObjectSystem]
        public class OperaWatcherComponentDestorySystem: DestroySystem<OperaWatcherComponent>
        {
            protected override void Destroy(OperaWatcherComponent self)
            {
                OperaWatcherComponent.Instance = null;
            }
        }
        
        
        public class OperaWatcherComponentLoadSystem: LoadSystem<OperaWatcherComponent>
        {
            protected override void Load(OperaWatcherComponent self)
            {
                
            }
        }

        static void Init(this OperaWatcherComponent self)
        {
            self.typeMapAttr = new UnOrderMultiMap<object, OperaSystemAttribute>();
            self.OperaEntitys = new List<Entity>();
            self.sortList = new LinkedList<Tuple<object, Entity, string>>();
            var types = EventSystem.Instance.GetTypes(TypeInfo<OperaSystemAttribute>.Type);
            self.typeSystems = new TypeSystems(types.Count);
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(TypeInfo<OperaSystemAttribute>.Type, false);
                if(attrs.Length<=0)return;
                var obj = Activator.CreateInstance(type);
                if (obj is ISystemType iSystemType)
                {
                    bool has = false;
                    for (int i = 0; i < attrs.Length; i++)
                    {
                        var attr = attrs[i] as OperaSystemAttribute;
                        self.typeMapAttr.Add(obj,attr);
                        OperaComponent.Instance.AddListenter(attr.ActionName);
                        has = true;
                    }

                    if (has)
                    {
                        TypeSystems.OneTypeSystems oneTypeSystems = self.typeSystems.GetOrCreateOneTypeSystems(iSystemType.Type());
                        oneTypeSystems.Map.Add(iSystemType.SystemType(),obj);
                    }
                }
            }
            
        }

        public static void RunCheck(this OperaWatcherComponent self,string actionName)
        {
            for (var node = self.sortList.Last; node != null; node = node.Previous)
            {
                var component = node.Value.Item2;
                var system = node.Value.Item1;
                if (system is IOperaSystem iOperaSystem)
                {
                    if (actionName == node.Value.Item3)
                    {
                        iOperaSystem.Run(component, actionName);
                    }
                }
            }
        }
        
        
        public static void RegisterInputEntity(this OperaWatcherComponent self,Entity entity)
        {
            if(self==null) return;
            if (!self.OperaEntitys.Contains(entity))
            {
                self.OperaEntitys.Add(entity);
                List<object> iInputSystems = self.typeSystems.GetSystems(entity.GetType(), TypeInfo<IOperaSystem>.Type);
                if (iInputSystems != null)
                {
                    for (int i = 0; i < iInputSystems.Count; i++)
                    {
                        self.AddInputSystem(entity,iInputSystems[i]);
                    }
                }
            }
        }
        
        public static void RemoveInputEntity(this OperaWatcherComponent self,Entity entity)
        {
            self.OperaEntitys.Remove(entity);
            List<object> iInputSystems = self.typeSystems.GetSystems(entity.GetType(), TypeInfo<IOperaSystem>.Type);
            if (iInputSystems != null)
            {
                for (int i = 0; i < iInputSystems.Count; i++)
                {
                    self.RemoveInputSystem(iInputSystems[i]);

                }
            }
        }
        
        
        public static void AddInputSystem(this OperaWatcherComponent self,Entity entity,object inputSystem)
        {
            if (!(inputSystem is IOperaSystem))
            {
                return;
            }

            if (self.typeMapAttr.TryGetValue(inputSystem, out var attrs))
            {
                for (int j = 0; j < attrs.Count; j++)
                {
                    var attr = attrs[j];
                    string actionName = attr.ActionName;
  
                    bool isAdd = false;
                    for (var node = self.sortList.Last; node!=null; node=node.Previous)
                    {

                            self.sortList.AddAfter(node,new Tuple<object, Entity, string>(inputSystem, entity, actionName));
                            isAdd = true;
                            break;
                        
                    }
                    if (!isAdd)
                    {
                        self.sortList.AddFirst(new Tuple<object, Entity, string>(inputSystem, entity, actionName));
                    }
                            
                }
            }
            else
            {
                Log.Error("RegisterInputEntity attr miss! type="+inputSystem.GetType().Name);
            }
        }
        
        public static void RemoveInputSystem(this OperaWatcherComponent self, object inputSystem)
        {
            if (!(inputSystem is IOperaSystem))
            {
                return;
            }

            if (self.typeMapAttr.TryGetValue(inputSystem, out var attrs))
            {
                for (int j = 0; j < attrs.Count; j++)
                {
                    var attr = attrs[j];
                    string actionName = attr.ActionName;
                    for (var node = self.sortList.Last; node!=null;node = node.Previous)
                    {
                        if (node.Value.Item1 == inputSystem&&node.Value.Item3 == actionName)
                        {
                            self.sortList.Remove(node);
                            break;
                        }
                    }
                }
            }
            else
            {
                Log.Error("RemoveInputEntity attr miss! type="+inputSystem.GetType().Name);
            }
        }

    }
}