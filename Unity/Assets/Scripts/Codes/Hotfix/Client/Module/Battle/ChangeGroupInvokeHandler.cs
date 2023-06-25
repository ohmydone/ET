using ET.EventType;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class ChangeGroupInvokeHandler:AEvent<Scene,EventType.Battle_ChangeGroup>
    {
        protected override async ETTask Run(Scene scene, Battle_ChangeGroup a)
        {
            var stepPara = a.Para.GetCurSkillStepPara();
            var unit = a.Para.From;
            var spell = unit.GetComponent<SpellComponent>();
            spell.WaitStep(SkillStepType.ChangeGroup);
            await ETTask.CompletedTask;
        }

    }
}
