using UnityEngine;
using MongoDB.Bson.Serialization.Attributes;
using Unity.Mathematics;

namespace ET
{
    [ComponentOf(typeof(CombatUnitComponent))]
    public class MoveAndSpellComponent:Entity,IAwake,IDestroy,ITransfer
    {
        [BsonIgnore]
        public SkillAbility Skill { get; set; }

        [BsonIgnore]
        public long TimerId;
        [BsonIgnore]
        public float3 Point;

        [BsonIgnore]
        public CombatUnitComponent Target { get; set; }
    }
}