using ET.EventType;
using Unity.Mathematics;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class HandlerBattle_SpellWithTarget:AEvent<Scene,EventType.Battle_SpellWithTarget>
    {
        protected override async ETTask Run(Scene scene, Battle_SpellWithTarget a)
        {
            a.SkillAbility.UseSkill(float3.zero,a.Target.GetComponent<CombatUnitComponent>().Id);
            await ETTask.CompletedTask;
        }
    }
}

