﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    [FriendOf(typeof (KeyCodeComponent))]
    public class SpellPreviewComponentAwakeSystem: AwakeSystem<SpellPreviewComponent, Dictionary<int, int>>
    {
        protected override void Awake(SpellPreviewComponent self, Dictionary<int, int> info)
        {
            self.Enable = true;
            if (info != null)
            {
                var combatU = self.GetParent<CombatUnitComponent>();
                foreach (var item in KeyCodeComponent.Instance.KeyMap)
                {
                    var keyCode = item.Key;
                    if (info.ContainsKey(keyCode) && combatU.TryGetSkillAbility(info[keyCode], out var skill))
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
    [FriendOf(typeof (KeyCodeComponent))]
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
    [FriendOf(typeof (KeyCodeComponent))]
    public class SpellPreviewComponentDestroySystem1: DestroySystem<SpellPreviewComponent>
    {
        protected override void Destroy(SpellPreviewComponent self)
        {
            InputWatcherComponent.Instance.RemoveInputEntity(self);
        }
    }

    [InputSystem(KeyCodeType.Skill1, InputType.KeyDown)]
    [InputSystem(KeyCodeType.Skill2, InputType.KeyDown)]
    [InputSystem(KeyCodeType.Skill3, InputType.KeyDown)]
    [InputSystem(KeyCodeType.Skill4, InputType.KeyDown)]
    [InputSystem(KeyCodeType.Skill5, InputType.KeyDown)]
    [InputSystem(KeyCodeType.Skill6, InputType.KeyDown)]
    [InputSystem(KeyCodeType.Skill7, InputType.KeyDown)]
    public class SpellPreviewComponentInputSystem_Spell: InputSystem<SpellPreviewComponent>
    {
        public override void Run(SpellPreviewComponent self, int key, int type, ref bool stop)
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

                if (spellPreviewComponent.InputSkills.ContainsKey(key))
                {
                    var spellSkill = spellPreviewComponent.InputSkills[key];
                    if (spellSkill == null || !spellSkill.CanUse()) return;
                    spellPreviewComponent.PreviewingSkill = spellSkill;
                    spellPreviewComponent.EnterPreview();
                }
            }
        }
    }

    [FriendOf(typeof (SpellPreviewComponent))]
    [FriendOf(typeof (CombatUnitComponent))]
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
            int[] SkillKeys = new[]
            {
                KeyCodeType.Skill1, KeyCodeType.Skill2, KeyCodeType.Skill3, KeyCodeType.Skill4, KeyCodeType.Skill5, KeyCodeType.Skill6,
                KeyCodeType.Skill7
            };

            int i = 0;
            foreach (var item in combatU.IdSkillMap)
            {
                if (i < SkillKeys.Length)
                {
                    var keyCode = SkillKeys[i];
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
        public static void BindSkillKeyCode(this SpellPreviewComponent self, int keyCode, SkillAbility ability)
        {
            self.InputSkills[keyCode] = ability;
        }

        /// <summary>
        /// 进入预览
        /// </summary>
        /// <param name="self"></param>
        /// <param name="auto">只能施法？</param>
        public static void EnterPreview(this SpellPreviewComponent self, bool auto = false)
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
                if (comp == null)
                {
                    comp = self.AddComponent<TargetSelectComponent>();
                }

                comp.TargetLimitType = affectTargetType;
                if (auto)
                {
                    SelectWatcherComponent.Instance.AutoSpell<Action<Unit>, int[]>(comp, (a) => { self.OnSelectedTarget(a); },
                        self.PreviewingSkill.SkillConfig.PreviewRange);
                }
                else
                {
                    comp.Mode = self.PreviewingSkill.SkillConfig.Mode;
                    SelectWatcherComponent.Instance.Show<Action<Unit>, int[]>(comp, (a) => { self.OnSelectedTarget(a); },
                        self.PreviewingSkill.SkillConfig.PreviewRange).Coroutine();
                    self.CurSelect = comp;
                }
            }
            //1大圈选小圈
            else if (previewType == SkillPreviewType.SelectCircularInCircularArea)
            {
                var comp = self.GetComponent<PointSelectComponent>();
                if (comp == null)
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
                if (comp == null)
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
                if (comp == null)
                {
                    comp = self.AddComponent<CircularSelectComponent>();
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
            //自动
            else
            {
                Log.Error("未处理的施法类型" + previewType);
            }
        }

        public static void CancelPreview(this SpellPreviewComponent self)
        {
            self.Previewing = false;
            if (self.CurSelect != null)
                SelectWatcherComponent.Instance.Hide(self.CurSelect);
        }

        public static void OnSelectedTarget(this SpellPreviewComponent self, Unit unit)
        {
            if (self.PreviewingSkill.SkillConfig.Mode == 0)
            {
#if SERVER //纯客户端单机游戏去掉
                self.MoveAndSpellComp.SpellWithTarget(self.PreviewingSkill, unit?.GetComponent<CombatUnitComponent>());
#else
                self.PreviewingSkill.UseSkill(Vector3.zero, unit.Id);
#endif
            }
            else
            {
                self.MoveAndSpellComp.SpellWithTarget(self.PreviewingSkill, unit?.GetComponent<CombatUnitComponent>());
            }
        }

        public static void OnInputPoint(this SpellPreviewComponent self, Vector3 point)
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

        public static void OnInputDirect(this SpellPreviewComponent self, Vector3 point)
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

        public static void SelectTargetsWithDistance(this SpellPreviewComponent self, Vector3 point)
        {
        }
    }
}