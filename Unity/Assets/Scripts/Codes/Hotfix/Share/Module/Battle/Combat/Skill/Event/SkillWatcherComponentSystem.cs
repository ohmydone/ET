using System;
using System.Collections.Generic;

namespace ET
{
    [FriendOf(typeof(SkillWatcherComponent))]
    public static class SkillWatcherComponentSystem
    {
        [ObjectSystem]
        public class SkillWatcherComponentAwakeSystem : AwakeSystem<SkillWatcherComponent>
        {
            protected override void Awake(SkillWatcherComponent self)
            {
                SkillWatcherComponent.Instance = self;
                self.Init();
            }
        }

	
        public class SkillWatcherComponentLoadSystem : LoadSystem<SkillWatcherComponent>
        {
            protected override void Load(SkillWatcherComponent self)
            {
                self.Init();
            }
        }

        private static void Init(this SkillWatcherComponent self)
        {
            self.allWatchers = new Dictionary<int, List<ISkillWatcher>>();

            var types = EventSystem.Instance.GetTypes(TypeInfo<SkillWatcherAttribute>.Type);
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(TypeInfo<SkillWatcherAttribute>.Type, false);

                for (int i = 0; i < attrs.Length; i++)
                {
                    SkillWatcherAttribute numericWatcherAttribute = (SkillWatcherAttribute)attrs[i];
                    ISkillWatcher obj = (ISkillWatcher)Activator.CreateInstance(type);
                    if (!self.allWatchers.ContainsKey(numericWatcherAttribute.SkillStepType))
                    {
                        self.allWatchers.Add(numericWatcherAttribute.SkillStepType, new List<ISkillWatcher>());
                    }
                    self.allWatchers[numericWatcherAttribute.SkillStepType].Add(obj);
                }
            }
        }

        public static void Run(this SkillWatcherComponent self, int type,SkillPara para)
        {
            List<ISkillWatcher> list;
            if (!self.allWatchers.TryGetValue(type, out list))
            {
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                ISkillWatcher numericWatcher = list[i];
                numericWatcher.Run(para);
            }
        }
    }
}