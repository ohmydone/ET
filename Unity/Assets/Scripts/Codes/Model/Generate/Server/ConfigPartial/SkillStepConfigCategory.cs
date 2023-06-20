using System.Collections;
using System.Collections.Generic;

namespace ET
{
    public partial class SkillStepConfigCategory 
    {
        public MultiMap<int, SkillStepConfig> groups;
        public MultiDictionary<int, string, SkillStepConfig> idGroupMap;
        partial void PostInit()
        {
            groups = new MultiMap<int, SkillStepConfig>();
            idGroupMap = new MultiDictionary<int, string, SkillStepConfig>();
            for (int i = 0; i < _dataList.Count; i++)
            {
                groups.Add(_dataList[i].SkillId,_dataList[i]);
                idGroupMap.Add(_dataList[i].SkillId,this._dataList[i].Group,_dataList[i]);
            }
        }

        public List<SkillStepConfig> GetSkillGroups(int skillId)
        {
            if(groups.TryGetValue(skillId,out var res))
            {
                return res;
            }
            return null;
        }
        
        public SkillStepConfig GetSkillGroup(int skillId,string group)
        {
            if(idGroupMap.TryGetValue(skillId,group,out var res))
            {
                return res;
            }
            return null;
        }
    }
}
