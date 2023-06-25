using ET.EventType;
using Unity.Mathematics;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class HandlerBattle_SpellWithPoint:AEvent<Scene,EventType.Battle_SpellWithPoint>
    {
        protected override async ETTask Run(Scene scene, Battle_SpellWithPoint a)
        {
            a.SkillAbility.UseSkill(a.Point);
            await ETTask.CompletedTask;
        }
    }
}

