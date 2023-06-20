﻿using Unity.Mathematics;
namespace ET.Client
{
    [FriendOf(typeof(SkillAbility))]
    public static class BattleHelper
    {
        public static void UseSkill(this SkillAbility skill, float3 pos,long id = 0)
        {
            C2M_UseSkill msg = new C2M_UseSkill()
            {
                SkillConfigId = skill.ConfigId,
                X = pos.x,
                Y = pos.y,
                Z = pos.z,
                Id = id
            };
            skill.ClientScene().GetComponent<SessionComponent>().Session.Send(msg);
        }

        public static void Damage(CombatUnitComponent from, CombatUnitComponent to, float value)
        {
            // 由于AOI机制客户端可能from为空，但不应该影响to的逻辑处理
            Unit fU = from?.GetParent<Unit>();
            Unit tU = to.GetParent<Unit>();
            NumericComponent t = tU.GetComponent<NumericComponent>();
            var buffF = from?.GetComponent<BuffComponent>();
            var buffT = to.GetComponent<BuffComponent>();
            DamageInfo info = DamageInfo.Create();
            info.Value = value;
            buffF?.BeforeDamage(fU, tU, info);
            buffT.BeforeDamage(fU, tU, info);
            int damageValue = (int)info.Value;
            info.Value = damageValue;
            if (damageValue != 0)
            {
                int realValue = damageValue;
                int now = t.GetAsInt(NumericType.Hp);
                int nowBaseValue = now - realValue;
                t.Set(NumericType.HpBase, nowBaseValue);
                EventSystem.Instance.Publish(from.ClientScene(), new EventType.AfterCombatUnitGetDamage()
                {
                    From = from, 
                    Unit = to, 
                    DamageValue = damageValue, 
                    RealValue = realValue,
                    NowBaseValue = nowBaseValue,
                });
            }
            buffT.AfterDamage(fU, tU, info);
            buffF?.AfterDamage(fU, tU, info);
            info.Dispose();
        }
    }

}