using ET.EventType;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class ChangeGroupInvokeHandler:AEvent<Scene,EventType.Battle_ChangeGroup>
    {
        protected override async ETTask Run(Scene scene, Battle_ChangeGroup a)
        {
            var stepPara = a.Para.GetCurSkillStepPara();
            var unit = a.Para.From;
            var spell = unit.GetComponent<SpellComponent>();
            if (unit.unit.IsGhost())
            {
                spell.WaitStep(SkillStepType.ChangeGroup);
                return;
            }
            if(StepParaHelper.TryParseString(ref stepPara.Paras[0], out var condition))
            {
                var res = ConditionWatcherComponent.Instance.Run(condition,a.Para);
                M2C_ChangeSkillGroup msg = new M2C_ChangeSkillGroup {  UnitId = unit.Id, Result = res?1:0,Timestamp = TimeHelper.ServerNow()};
                MessageHelper.Broadcast(unit.unit,msg);
                StepParaHelper.TryParseString(ref stepPara.Paras[1], out var suc);
                if (res)
                {
                    spell.ChangeGroup(suc);
                }
                else if(stepPara.Paras.Length >= 3)
                {
                    StepParaHelper.TryParseString(ref stepPara.Paras[2], out var fail);
                    spell.ChangeGroup(fail);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
