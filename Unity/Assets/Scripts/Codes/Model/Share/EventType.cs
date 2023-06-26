using System.Collections.Generic;
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
        
        

        
       

        #region Battle
        public struct Battle_Damage
        {
            public CombatUnitComponent FromU;
            public CombatUnitComponent ToU;
            public AOIUnitComponent Skill;
            public float Value;
        }
        
        public struct AddEffect
        {
            public int EffectId;
            public Unit Unit;
            public Entity Parent;
        }
        public struct AfterAddBuff
        {
            public Buff Buff;
        }

        public struct AfterRemoveBuff
        {
            public Buff Buff;
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

        #endregion
        
        #region AOI
        public struct AOIRemoveUnit
        {
            public AOIUnitComponent Receive;
            public List<AOIUnitComponent> Units;
        }

        public struct AOIRegisterUnit
        {
            public AOIUnitComponent Receive;
            public List<AOIUnitComponent> Units;
        }

        public struct ChangeGrid
        {
            public AOIUnitComponent Unit;
            public AOICell NewCell;
            public AOICell OldCell;
        }
        #endregion
    }
}