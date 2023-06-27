using System;
using System.Collections.Generic;

namespace ET
{
    [ObjectSystem]
    public class SkillStepComponentAwakeSystem : AwakeSystem<SkillStepComponent>
    {
        protected override void Awake(SkillStepComponent self)
        {
            SkillStepComponent.Instance = self;
            self.Params = DictionaryComponent<int, List<object[]>>.Create();
            self.StepType = DictionaryComponent<int, List<SkillStepType>>.Create();
            self.TimeLine = DictionaryComponent<int, List<int>>.Create();
        }
    }
    [ObjectSystem]
    public class SkillStepComponentDestroySystem : DestroySystem<SkillStepComponent>
    {
        protected override void Destroy(SkillStepComponent self)
        {
            SkillStepComponent.Instance = null;
            self.Params.Dispose();
            self.StepType.Dispose();
            self.TimeLine.Dispose();
        }
    }

    [FriendOf(typeof(SkillStepComponent))]
    [FriendOf(typeof(SkillAbility))]
    public static class SkillStepComponentSystem
    {
        public static List<int> GetSkillStepTimeLine(this SkillStepComponent self,int configId)
        {
            if (!self.TimeLine.ContainsKey(configId))
            {
                List<int> timeline = self.TimeLine[configId] = new List<int>();
                SkillStepConfig config = SkillStepConfigCategory.Instance.Get(configId);
                for(int i = 0; i < config.SkillStepDatas.Count; i++)
                {
                    timeline.Add(config.SkillStepDatas[i].TriggerTime);
                }
                return timeline;
            }
            else
            {
                return self.TimeLine[configId];
            }
        }
        
        public static List<SkillStepType> GetSkillStepType(this SkillStepComponent self,int configId)
        {
            if (!self.StepType.ContainsKey(configId))
            {
                var steptype = self.StepType[configId] = new List<SkillStepType>();
                SkillStepConfig config = SkillStepConfigCategory.Instance.Get(configId);
                for(int i = 0; i < config.SkillStepDatas.Count; i++)
                {
                    steptype.Add(config.SkillStepDatas[i].StepStyle);
                }
                return steptype;
            }
            else
            {
                return self.StepType[configId];
            }
        }
        
        public static List<object[]> GetSkillStepParas(this SkillStepComponent self,int configId)
        {
            if (!self.Params.ContainsKey(configId))
            {
                var paras = self.Params[configId] = new List<object[]>();
                SkillStepConfig config = SkillStepConfigCategory.Instance.Get(configId);
                for(int i = 0; i < config.SkillStepDatas.Count; i++)
                {
                    var list = (string[]) config.SkillStepDatas[i].StepParameter;
                    
                    object[] temp=new object[config.SkillStepDatas[i].StepParameter.Length];
                    for (int j = 0; j < config.SkillStepDatas[i].StepParameter.Length; j++)
                    {
                        temp[j] = config.SkillStepDatas[i].StepParameter[j];
                    }
                    
                    paras.Add(temp);
                }
                return paras;
            }
            else
            {
                return self.Params[configId];
            }
            
        }
    }
}