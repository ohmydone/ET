using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ET
{
    [FriendOf(typeof(Thing))]
    public static class ThingSystem 
    {
        public static void Add(this Thing self, int count)
        {
            self.Count += count;
        }
        
        public static void Consume(this Thing self, int count)
        {
            self.Count -= count;
            self.Count=math.max(0,self.Count);
        }
    }
}
