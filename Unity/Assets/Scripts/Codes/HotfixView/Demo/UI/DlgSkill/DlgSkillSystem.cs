using System.Collections;
using System.Collections.Generic;
using System;
using ET.Client;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendOf(typeof (DlgSkill))]
    public static class DlgSkillSystem
    {
        public static void RegisterUIEvent(this DlgSkill self)
        {
            self.View.ELSV_SkillLoopHorizontalScrollRect.AddItemRefreshListener(((transform, i) =>
            {
                self.ItemReFresh(transform,i);
            }));
        }

        public static void ShowWindow(this DlgSkill self, Entity contextData = null)
        {
            SpellComponent spellComponent = UnitComponent.Instance.My.GetComponent<CombatUnitComponent>().GetComponent<SpellComponent>();
             var list = new List<SkillAbility>();
            foreach (long id in spellComponent.Children.Keys)
            {
                SkillAbility spell = spellComponent.GetChild<SkillAbility>(id);
                list.Add(spell);
            }
            
            self.AddUIScrollItems(ref self.ScrollItemSkils,list.Count);
            self.View.ELSV_SkillLoopHorizontalScrollRect.SetVisible(true,list.Count);
        }

        public static void ItemReFresh(this DlgSkill self,Transform arg1, int arg2)
        {
            Scroll_ItemSkil itemSkil = self.ScrollItemSkils[arg2].BindTrans(arg1);
            itemSkil.BindSkill("");
        }

        public static void HideWindow(this DlgSkill self)
        {
            
        }
    }
    
}