using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class ThingComponentAwakeSystem: AwakeSystem<ThingComponent>
    {
        protected override void Awake(ThingComponent self)
        {
            
        }
    }

    [FriendOf(typeof (ThingComponent))]
    public static class ThingComponentSystem
    {
        public static void AddThing(this ThingComponent self, int configId,int count)
        {
            Thing thing = null;
            if (self.IdThingMap.ContainsKey(configId))
            {
                thing= self.GetChild<Thing>(self.IdThingMap[configId]);
            }
            else
            {
                thing= self.AddChild<Thing,int>(configId);
                self.IdThingMap.Add(configId, thing.Id);
            }
            thing.Add(count);
        }
        
        public static void ThingConsume(this ThingComponent self, int configId,int count)
        {
            Thing thing = null;
            if (self.IdThingMap.ContainsKey(configId))
            {
                thing= self.GetChild<Thing>(self.IdThingMap[configId]);
            }
            else
            {
                thing= self.AddChild<Thing,int>(configId);
                self.IdThingMap.Add(configId, thing.Id);
            }
            thing.Add(count);
        }
        
    }
}