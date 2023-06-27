namespace ET
{
    public class SkillWatcherAttribute : BaseAttribute
    {
        public SkillStepType SkillStepType { get; }

        public SkillWatcherAttribute(SkillStepType type)
        {
            this.SkillStepType = type;
        }
    }
}