using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class SkillStepComponent:Entity,IAwake,IDestroy
    {
        [StaticField]
        public static SkillStepComponent Instance;
        public DictionaryComponent<int, List<int>> TimeLine;
        public DictionaryComponent<int, List<int>> StepType;
        public DictionaryComponent<int, List<object[]>> Params;
    }
}