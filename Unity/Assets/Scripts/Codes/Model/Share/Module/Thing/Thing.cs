using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using UnityEngine;

namespace ET
{
    [ChildOf(typeof (ThingComponent))]
    public class Thing: Entity, IAwake<int>, IAwake<int, int>, IDestroy
    {
        public int ConfigId;
        public int Count;
        
        [BsonIgnore]
        public ThingConfig Config => ThingConfigCategory.Instance.Get(this.ConfigId);
    }
}