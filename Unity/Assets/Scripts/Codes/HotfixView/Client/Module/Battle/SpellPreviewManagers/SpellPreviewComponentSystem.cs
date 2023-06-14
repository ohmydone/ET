using System;
using System.Collections.Generic;
using ET.Client;
using UnityEngine;
namespace ET
{
    [ObjectSystem]
    [FriendOf(typeof(KeyCodeComponent))]
    public class SpellPreviewComponentAwakeSystem : AwakeSystem<SpellPreviewComponent,Dictionary<string,int>>
    {
        protected override void Awake(SpellPreviewComponent self,Dictionary<string,int> info)
        {
            self.Enable = true;
            if (info != null)
            {
                var combatU = self.GetParent<CombatUnitComponent>();
                foreach (var item in KeyCodeComponent.Instance.DefaultKeyCodeMap)
                {
                    var keyCode = item;
                    if (info.ContainsKey(keyCode) && combatU.TryGetSkillAbility(info[keyCode],out var skill))
                    {
                        self.BindSkillKeyCode(keyCode, skill);
                    }
                }
            }
            else
            {
                self.BindSkillKeyDefault();
            }
            InputWatcherComponent.Instance.RegisterInputEntity(self);
        }
    }
    [ObjectSystem]
    [FriendOf(typeof(KeyCodeComponent))]
    public class SpellPreviewComponentAwakeSystem1: AwakeSystem<SpellPreviewComponent>
    {
        protected override void Awake(SpellPreviewComponent self)
        {
            self.Enable = true;
            self.BindSkillKeyDefault();
            InputWatcherComponent.Instance.RegisterInputEntity(self);
        }
    }
    [ObjectSystem]
    [FriendOf(typeof(KeyCodeComponent))]
    public class SpellPreviewComponentDestroySystem1: DestroySystem<SpellPreviewComponent>
    {
        protected override void Destroy(SpellPreviewComponent self)
        {
            InputWatcherComponent.Instance.RemoveInputEntity(self);
        }
    }
    [OperaSystem(OperaID.Slot1)]
    [OperaSystem(OperaID.Slot2)]
    [OperaSystem(OperaID.Slot3)]
    [OperaSystem(OperaID.Slot4)]
    [OperaSystem(OperaID.Slot5)]
    [OperaSystem(OperaID.Slot6)]
    public class SpellPreviewComponentInputSystem_Spell : OperaSystem<SpellPreviewComponent>
    {
        protected override void Run(SpellPreviewComponent self, string actionName)
        {
            KeyCodeComponent keyCode = KeyCodeComponent.Instance;
            if (keyCode != null)
            {
                var CurCombat = self.GetParent<CombatUnitComponent>();
                var spellPreviewComponent = CurCombat?.GetComponent<SpellPreviewComponent>();
                if (spellPreviewComponent == null)
                {
                    return;
                }
                if (spellPreviewComponent.InputSkills.ContainsKey(actionName))
                {
                    var spellSkill = spellPreviewComponent.InputSkills[actionName];
                    if (spellSkill == null || !spellSkill.CanUse()) return;
                    spellPreviewComponent.PreviewingSkill = spellSkill;
                    spellPreviewComponent.EnterPreview();
                }
            }
        }
        
    }
    
    
    [FriendOf(typeof(SpellPreviewComponent))]
    [FriendOf(typeof(CombatUnitComponent))]
    [FriendOf(typeof(KeyCodeComponent))]
    public static class SpellPreviewComponentSystem
    {
        /// <summary>
        /// 设置是否生效
        /// </summary>
        /// <param name="self"></param>
        /// <param name="enable"></param>
        public static void SetEnable(this SpellPreviewComponent self, bool enable)
        {
            if (self.Enable)
            {
                self.CancelPreview();
            }
            self.Enable = enable;
        }
        
        /// <summary>
        /// 使用默认按键配置,技能绑定按键
        /// </summary>
        /// <param name="self"></param>
        public static void BindSkillKeyDefault(this SpellPreviewComponent self)
        {
            var combatU = self.GetParent<CombatUnitComponent>();
            Log.Info("使用默认按键配置,技能绑定按键");
            int i = 0;
            foreach (var item in combatU.IdSkillMap)
            {
                if (i < KeyCodeComponent.Instance.DefaultKeyCodeMap.Count)
                {
                    var keyCode = KeyCodeComponent.Instance.DefaultKeyCodeMap[i];
                    self.BindSkillKeyCode(keyCode, combatU.GetChild<SkillAbility>(item.Value));
                }
                else
                {
                    break;
                }

                i++;
            }
        }
        /// <summary>
        /// 绑定技能与按键
        /// </summary>
        /// <param name="self"></param>
        /// <param name="keyCode"></param>
        /// <param name="ability"></param>
        public static void BindSkillKeyCode(this SpellPreviewComponent self, string keyCode, SkillAbility ability)
        {
            self.InputSkills[keyCode]=ability;
        }
        /// <summary>
        /// 进入预览
        /// </summary>
        /// <param name="self"></param>
        /// <param name="auto">只能施法？</param>
        public static void EnterPreview(this SpellPreviewComponent self,bool auto = true)
        {
            if (!self.Enable) return;
            self.CancelPreview();
            self.Previewing = true;
            //伤害作用对象(0自身1己方2敌方)
            var affectTargetType = self.PreviewingSkill.SkillConfig.DamageTarget;
            //技能预览类型(0大圈选一个目标，1大圈选小圈)
            var previewType = self.PreviewingSkill.SkillConfig.PreviewType;
            // Log.Info("affectTargetType"+affectTargetType+" targetSelectType"+targetSelectType+" previewType"+previewType);
            
            //0大圈选一个目标
            if (previewType == SkillPreviewType.SelectTarget)
            {
                var comp = self.GetComponent<TargetSelectComponent>();
                if (comp==null)
                {
                    comp = self.AddComponent<TargetSelectComponent>();
                }
                comp.TargetLimitType = affectTargetType;
                if (auto)
                {
                    SelectWatcherComponent.Instance.AutoSpell<Action<Unit>,int[]>(comp,(a)=> { self.OnSelectedTarget(a); },
                        self.PreviewingSkill.SkillConfig.PreviewRange);
                }
                else
                {
                    comp.Mode = self.PreviewingSkill.SkillConfig.Mode;
                    SelectWatcherComponent.Instance.Show<Action<Unit>,int[]>(comp,(a)=> { self.OnSelectedTarget(a); },
                        self.PreviewingSkill.SkillConfig.PreviewRange).Coroutine();
                    self.CurSelect = comp;
                }
            }
            //1大圈选小圈
            else if (previewType == SkillPreviewType.SelectCircularInCircularArea)
            {
                var comp = self.GetComponent<PointSelectComponent>();
                if (comp==null)
                {
                    comp = self.AddComponent<PointSelectComponent>();
                }
                
                if (auto)
                {
                    SelectWatcherComponent.Instance.AutoSpell<Action<Vector3>, int[]>(comp, (a) => { self.OnInputPoint(a); },
                        self.PreviewingSkill.SkillConfig.PreviewRange);
                }
                else
                {
                    comp.Mode = self.PreviewingSkill.SkillConfig.Mode;
                    SelectWatcherComponent.Instance.Show<Action<Vector3>, int[]>(comp, (a) => { self.OnInputPoint(a); },
                        self.PreviewingSkill.SkillConfig.PreviewRange).Coroutine();
                    self.CurSelect = comp;
                }
                
            }
            //2矩形
            else if (previewType == SkillPreviewType.SelectRectangleArea)
            {
                var comp = self.GetComponent<DirectRectSelectComponent>();
                if (comp==null)
                {
                    comp = self.AddComponent<DirectRectSelectComponent>();
                }
               
                if (auto)
                {
                    SelectWatcherComponent.Instance.AutoSpell<Action<Vector3>, int[]>(comp, (a) => { self.OnInputDirect(a); },
                        self.PreviewingSkill.SkillConfig.PreviewRange);
                }
                else
                {
                    comp.Mode = self.PreviewingSkill.SkillConfig.Mode;
                    SelectWatcherComponent.Instance.Show<Action<Vector3>, int[]>(comp, (a) => { self.OnInputDirect(a); },
                        self.PreviewingSkill.SkillConfig.PreviewRange).Coroutine();
                    self.CurSelect = comp;
                }
            }
            //自身圆心的圆
            else if (previewType == SkillPreviewType.SelectCircularArea)
            {
                var comp = self.GetComponent<CircularSelectComponent>();
                if (comp==null)
                {
                    comp = self.AddComponent<CircularSelectComponent>();
                }
                
                if (auto)
                {
                    SelectWatcherComponent.Instance.AutoSpell<Action<Vector3>,int[]>(comp,(a)=> { self.OnInputPoint(a); },
                        self.PreviewingSkill.SkillConfig.PreviewRange);
                }
                else
                {
                    comp.Mode = self.PreviewingSkill.SkillConfig.Mode;
                    SelectWatcherComponent.Instance.Show<Action<Vector3>,int[]>(comp,(a)=> { self.OnInputPoint(a); },
                        self.PreviewingSkill.SkillConfig.PreviewRange).Coroutine();
                    self.CurSelect = comp;
                }
               
            }
            //自动
            else
            {
                Log.Error("未处理的施法类型"+previewType);
            }
            
            
        }

        public static void CancelPreview(this SpellPreviewComponent self)
        {
            self.Previewing = false;
            if(self.CurSelect!=null)
                SelectWatcherComponent.Instance.Hide(self.CurSelect);
        }
        
        private static void OnSelectedTarget(this SpellPreviewComponent self,Unit unit)
        {
            if (self.PreviewingSkill.SkillConfig.Mode == 0)
            {
#if SERVER //纯客户端单机游戏去掉
                self.MoveAndSpellComp.SpellWithTarget(self.PreviewingSkill, unit?.GetComponent<CombatUnitComponent>());
#else
                self.PreviewingSkill.UseSkill(Vector3.zero,unit.Id);
#endif
            }
            else
            {
                self.MoveAndSpellComp.SpellWithTarget(self.PreviewingSkill, unit?.GetComponent<CombatUnitComponent>());
            }
        }   

        private static void OnInputPoint(this SpellPreviewComponent self,Vector3 point)
        {
            if (self.PreviewingSkill.SkillConfig.Mode == 0)
            {
#if SERVER //纯客户端单机游戏去掉
                self.SpellComp.SpellWithPoint(self.PreviewingSkill, point);
#else
                self.PreviewingSkill.UseSkill(point);
#endif
            }
            else
            {
                self.MoveAndSpellComp.SpellWithPoint(self.PreviewingSkill, point);
            }
        }

        private static void OnInputDirect(this SpellPreviewComponent self, Vector3 point)
        {
            if (self.PreviewingSkill.SkillConfig.Mode == 0)
            {
#if SERVER //纯客户端单机游戏去掉
                self.SpellComp.SpellWithDirect(self.PreviewingSkill, point);
#else
                self.PreviewingSkill.UseSkill(point);
#endif
            }
            else
            {
                self.MoveAndSpellComp.SpellWithDirect(self.PreviewingSkill, point);
            }
        }

        public static void SelectTargetsWithDistance(this SpellPreviewComponent self,Vector3 point)
        {
            
        }
        
    }
}