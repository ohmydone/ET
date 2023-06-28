using Unity.Mathematics;

namespace ET
{
    [BuffDamageWatcher(BuffSubType.Chant)]
    public class AfterDamage_TryInterruptChant: IDamageBuffWatcher
    {
        public void AfterDamage(Unit attacker, Unit target, Buff buff, DamageInfo info)
        {
#if DOTNET //纯客户端单机游戏去掉
            if (info.Value>0 && buff.BuffChantConfig.DamageInterrupt == 1)
            {
                var sc = target.GetComponent<CombatUnitComponent>()?.GetComponent<SpellComponent>();
                if (sc != null&&sc.CanInterrupt())
                {
                    var bc = target.GetComponent<CombatUnitComponent>()?.GetComponent<BuffComponent>();
                    bc.RemoveByOther(buff.Id);
                }
            }
#endif
        }

        public void BeforeDamage(Unit attacker, Unit target, Buff buff, DamageInfo info)
        {
            
        }
    }
    
    [MoveBuffWatcher(BuffSubType.Chant)]
    public class AfterMove_TryInterruptChant: IMoveBuffWatcher
    {
        public void AfterMove(Unit target, Buff buff, float3 before)
        {
#if DOTNET //纯客户端单机游戏去掉
            if (buff.BuffChantConfig.MoveInterrupt == 1)
            {
                var sc = target.GetComponent<CombatUnitComponent>()?.GetComponent<SpellComponent>();
                if (sc != null&&sc.CanInterrupt() && math.lengthsq(target.Position-before)>0.01)
                {
                    var bc = target.GetComponent<CombatUnitComponent>()?.GetComponent<BuffComponent>();
                    bc.RemoveByOther(buff.Id);
                }
            }
#endif
        }
        
    }
}