using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class SkillAbilityAwakeSystem : AwakeSystem<SkillAbility,int>
    {
        protected override void Awake(SkillAbility self, int a)
        {
            self.ConfigId = a;
            self.Groups = new Dictionary<string, long>();
            self.LastSpellOverTime = TimeHelper.ServerNow()-self.SkillConfig.CDTime;
            self.LastSpellTime = TimeHelper.ServerNow()-self.SkillConfig.CDTime;
            var groups = SkillStepConfigCategory.Instance.GetAll(); 
            for (int i = 0; i < groups.Count; i++)
            {
                var group = self.AddChild<SkillAbilityGroup, int>(groups[i].Id);
                self.Groups.Add(groups[i].Group,group.Id);
            }
        }
    }
    [FriendOf(typeof(SkillAbility))]
    public static class SkillAbilitySystem
    {
        /// <summary>
        /// 是否可用
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool CanUse(this SkillAbility self)
        {
            return true;
        }
        public static SkillAbilityGroup GetGroup(this SkillAbility self,string group)
        {
            if (self.Groups.TryGetValue(group, out var res))
            {
                return self.GetChild<SkillAbilityGroup>(res);
            }

            return null;
        }
        
        public static void UseSkill(this SkillAbility skill, Vector3 pos,long id = 0)
        {
            
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
                // EventSystem.Instance.Publish(new EventType.AfterCombatUnitGetDamage()
                // {
                //     From = from, 
                //     Unit = to, 
                //     DamageValue = damageValue, 
                //     RealValue = realValue,
                //     NowBaseValue = nowBaseValue,
                // });
            }
            buffT.AfterDamage(fU, tU, info);
            buffF?.AfterDamage(fU, tU, info);
            info.Dispose();
        }
        
    }
}