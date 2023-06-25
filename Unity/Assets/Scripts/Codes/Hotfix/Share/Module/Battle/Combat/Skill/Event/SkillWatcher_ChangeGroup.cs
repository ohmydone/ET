using ET.EventType;

namespace ET
{
    /// <summary>
    /// 判断切换Group
    /// </summary>
    [SkillWatcher(SkillStepType.ChangeGroup)]
    [FriendOf(typeof(SkillAbility))]
    public class SkillWatcher_ChangeGroup : ISkillWatcher
    {
        public void Run(SkillPara para)
        {

            if (para.GetCurSkillStepPara().Paras.Length == 0)
            {
                Log.Error(para.SkillConfigId+"判断切换Group参数数量不对"+para.GetCurSkillStepPara().Paras.Length);
                return;
            }
            
            var stepPara = para.GetCurSkillStepPara();
            var unit = para.From;
            var spell = unit.GetComponent<SpellComponent>();
            if (stepPara.Paras.Length == 1)
            {
                if(StepParaHelper.TryParseString(ref stepPara.Paras[0], out var group))
                {
                    spell.ChangeGroup(group);
                }
            }
            else if (stepPara.Paras.Length >= 2)
            {
                EventSystem.Instance.Publish(unit.DomainScene(),new Battle_ChangeGroup()
                {
                    Para = para
                });
            }

        }
    }
}