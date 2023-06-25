using ET.EventType;
using Unity.Mathematics;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class HandlerBattle_SpellWithTarget:AEvent<Scene,EventType.Battle_SpellWithTarget>
    {
        protected override async ETTask Run(Scene scene, Battle_SpellWithTarget a)
        {
            a.SpellComponent.SpellWithTarget(a.SkillAbility,a.Target.GetComponent<CombatUnitComponent>());
            await ETTask.CompletedTask;
        }
    }
}

