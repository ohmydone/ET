using Unity.Mathematics;

namespace ET
{
    namespace EventType
    {
        public struct SceneChangeStart
        {
        }
        
        public struct SceneChangeFinish
        {
        }
        
        public struct AfterCreateClientScene
        {
        }
        
        public struct AfterCreateCurrentScene
        {
        }

        public struct AppStartInitFinish
        {
        }

        public struct LoginFinish
        {
        }

        public struct EnterMapFinish
        {
        }

        public struct AfterUnitCreate
        {
            public Unit Unit;
        }

        public struct DoOpera
        {
            public string OperaId;
        }
        
        public struct AddEffect
        {
            public int EffectId;
            public Unit Unit;
            public Entity Parent;
        }
        
        public struct Battle_ChangeGroup
        {
            public SkillPara Para;
        }
        
        public struct Battle_SpellWithTarget
        {
            public SpellComponent SpellComponent;
            public SkillAbility SkillAbility;
            public Unit Target;
        }
        
        public struct Battle_SpellWithPoint
        {
            public SpellComponent SpellComponent;
            public SkillAbility SkillAbility;
            public float3 Point;
        }

    }
}