using System;
using System.Collections.Generic;

namespace ET
{
    [FriendOf(typeof(ConditionWatcherComponent))]
    public static class ConditionWatcherComponentSystem
    {
        [ObjectSystem]
        public class ConditionWatcherComponentAwakeSystem : AwakeSystem<ConditionWatcherComponent>
        {
            protected override void Awake(ConditionWatcherComponent self)
            {
                ConditionWatcherComponent.Instance = self;
                self.Init();
            }
        }

	
        public class ConditionWatcherComponentLoadSystem : LoadSystem<ConditionWatcherComponent>
        {
            protected override void Load(ConditionWatcherComponent self)
            {
                self.Init();
            }
        }

        private static void Init(this ConditionWatcherComponent self)
        {
            self.allWatchers = new Dictionary<string, List<IConditionWatcher>>();

            var types = EventSystem.Instance.GetTypes(TypeInfo<ConditionWatcherAttribute>.Type);
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(TypeInfo<ConditionWatcherAttribute>.Type, false);

                for (int i = 0; i < attrs.Length; i++)
                {
                    ConditionWatcherAttribute numericWatcherAttribute = (ConditionWatcherAttribute)attrs[i];
                    IConditionWatcher obj = (IConditionWatcher)Activator.CreateInstance(type);
                    if (!self.allWatchers.ContainsKey(numericWatcherAttribute.ConditionType))
                    {
                        self.allWatchers.Add(numericWatcherAttribute.ConditionType, new List<IConditionWatcher>());
                    }
                    self.allWatchers[numericWatcherAttribute.ConditionType].Add(obj);
                }
            }
        }

        public static bool Run(this ConditionWatcherComponent self, string type,SkillPara para)
        {
            List<IConditionWatcher> list;
            if (!self.allWatchers.TryGetValue(type, out list))
            {
                return false;
            }

            bool res = true;
            for (int i = 0; i < list.Count; i++)
            {
                IConditionWatcher numericWatcher = list[i];
                res &= numericWatcher.Run(para);
            }

            return res;
        }
    }
}