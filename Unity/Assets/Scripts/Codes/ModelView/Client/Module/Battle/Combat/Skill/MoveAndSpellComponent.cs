using UnityEngine;
using MongoDB.Bson.Serialization.Attributes;
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
        public Vector3 Point;

        [BsonIgnore]
        public CombatUnitComponent Target { get; set; }
    }
}