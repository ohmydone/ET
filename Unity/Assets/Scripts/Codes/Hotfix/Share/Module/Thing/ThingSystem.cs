using System.Collections;
using System.Collections.Generic;
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
    }
}
