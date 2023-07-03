using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using UnityEngine;

namespace ET
{
    [ComponentOf(typeof (Unit))]
    /// <summary>
    /// 所拥有的的东西
    /// </summary>
    public class ThingComponent: Entity, IAwake, IDestroy
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> IdThingMap = new Dictionary<int, long>();
    }
}