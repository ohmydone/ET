using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(SpellComponent))]
    public class Spell : Entity,IAwake<int>
    {
        public int ConfigId { get; set; } //配置表id
        
        [BsonIgnore]
        public SpellConfig Config => SpellConfigCategory.Instance.Get(this.ConfigId);
    }
}
