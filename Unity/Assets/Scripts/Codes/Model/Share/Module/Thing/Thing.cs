using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ChildOf(typeof (ThingComponent))]
    public class Thing: Entity, IAwake<int>, IAwake<int, int>, IDestroy
    {
        public int ConfigId;
        public int Count;
    }
}