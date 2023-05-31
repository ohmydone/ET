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
            UnitComponent unitComponent = self.DomainScene().GetComponent<UnitComponent>();
            SpellComponent spellComponent = unitComponent.My.GetComponent<SpellComponent>();
            int count = 10;
            self.AddUIScrollItems(ref self.ScrollItemSkils,count);
            self.View.ELSV_SkillLoopHorizontalScrollRect.SetVisible(true,count);
        }

        public static void ItemReFresh(this DlgSkill self,Transform arg1, int arg2)
        {
            Scroll_ItemSkil itemSkil = self.ScrollItemSkils[arg2].BindTrans(arg1);
            itemSkil.ELabel_IndexText.text = arg2.ToString();
        }

        public static void HideWindow(this DlgSkill self)
        {
            
        }
    }
}