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

        #region Move
        public struct MoveStart
        {
            public Unit Unit;
        }

        public struct MoveStop
        {
            public Unit Unit;
        }
        
        #endregion

        #region Unit

        public struct ChangePosition
        {
            public Unit Unit;
            public float3 OldPos;
        }

        public struct ChangeRotation
        {
            public Unit Unit;
        }

        #endregion
        
       

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
        public struct AfterCombatUnitComponentCreate
        {
            public CombatUnitComponent CombatUnitComponent;
        }
        /// <summary>
        /// 当受到伤害或回复
        /// </summary>
        public struct AfterCombatUnitGetDamage
        {
            public CombatUnitComponent From;
            public CombatUnitComponent Unit;
            public long DamageValue;//计算伤害值
            public long RealValue;//生命变化值.正数少血，负数加血
            public long NowBaseValue;//当前生命base值
            //public GhostComponent Ghost;//SkillUnit的Ghost
        }
    
        /// <summary>
        /// 当技能触发
        /// </summary>
        public struct OnSkillTrigger
        {
            public AOITriggerType Type;
            public AOIUnitComponent Skill;
            public AOIUnitComponent From;
            public AOIUnitComponent To;
            public SkillStepPara Para;
            public List<int> CostId;
            public List<int> Cost;
            public SkillConfig Config;
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