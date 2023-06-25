using ET.EventType;
using Unity.Mathematics;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class HandlerBattle_SpellWithPoint:AEvent<Scene,EventType.Battle_SpellWithPoint>
    {
        protected override async ETTask Run(Scene scene, Battle_SpellWithPoint a)
        {
            a.SpellComponent.SpellWithPoint(a.SkillAbility,a.Point);
            await ETTask.CompletedTask;
        }
    }
}

