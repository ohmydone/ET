using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [InputSystem((int)KeyCode.F1, InputType.KeyDown)]
    public class KeyDown_F1: InputSystem<UIComponent>
    {
        public override void Run(UIComponent self, int key, int type, ref bool stop)
        {
            if (self.IsWindowVisible(WindowID.WindowID_GM))
            {
                self.HideWindow<DlgGM>();
            }
            else
            {
                self.ShowWindow<DlgGM>();
            }
        }
    }

    [FriendOf(typeof (DlgGM))]
    public static class DlgGMSystem
    {
        public static void RegisterUIEvent(this DlgGM self)
        {
            self.View.EBtn_SureButton.onClick.AddListener(() => { self.ClickSure().Coroutine(); });
            self.View.EBtn_DoGMButton.onClick.AddListener(() => { self.ClickDoGM().Coroutine(); });

            self.View.ELSV_GMLoopVerticalScrollRect.AddItemRefreshListener(((transform, i) => { self.ItemReFreshGM(transform, i); }));
            self.View.ELSV_GMParLoopVerticalScrollRect.AddItemRefreshListener(((transform, i) => { self.ItemReFreshGMPar(transform, i); }));
           
        }

        public static void ShowWindow(this DlgGM self, Entity contextData = null)
        {
            var confgs = GMConfigCategory.Instance.GetAll();
            self.AddUIScrollItems(ref self.ScrollItemGMs, confgs.Count);
            
            
            self.View.ELSV_GMLoopVerticalScrollRect.SetVisible(true, confgs.Count);
            self.ScrollItemGMs[1].EBtn_ChooseButton.onClick.Invoke();
        }

        private static async ETTask ClickSure(this DlgGM self)
        {
            var txt = self.View.EInp_GmInputField.text;
            if (!string.IsNullOrEmpty(txt))
            {
                var res = await self.ClientScene().GetComponent<SessionComponent>().Session.Call(new C2M_GM() { GM = txt });
                //Log.Debug(res.Message);
            }
        }

        private static async ETTask ClickDoGM(this DlgGM self)
        {
        }

        public static void ItemReFreshGM(this DlgGM self, Transform arg1, int arg2)
        {
            Scroll_ItemGM item = self.ScrollItemGMs[arg2].BindTrans(arg1);
            GMConfig config=GMConfigCategory.Instance.DataList[arg2];
            item.DataId=config.Id;
            item.Bind(config);
            item.EBtn_ChooseButton.onClick.AddListener(() =>
            {
                self.ClickItemGM((int)item.DataId);
            });
        }

        private static void ClickItemGM(this DlgGM self,int id)
        {
            self.ChooseGM=GMConfigCategory.Instance.Get(id);
            self.AddUIScrollItems(ref self.ScrollItemGMPars, self.ChooseGM.Para.Count);
            self.View.ELSV_GMParLoopVerticalScrollRect.SetVisible(true, self.ChooseGM.Para.Count);
        }

        
        public static void ItemReFreshGMPar(this DlgGM self, Transform arg1, int arg2)
        {
            Scroll_ItemGMPar item = self.ScrollItemGMPars[arg2].BindTrans(arg1);
            item.Bind(self.ChooseGM.Para[arg2]);
        }
    }
    
    [ObjectSystem]
    public static class Scroll_ItemGMSystem 
    {
        public static void Bind(this Scroll_ItemGM self ,GMConfig config)
        {
            self.ELab_NameText.text = config.Name;
        }
    }
    [ObjectSystem]
    public static class Scroll_ItemGMParSystem 
    {
        public static void Bind(this Scroll_ItemGMPar self ,string par)
        {
            self.ELab_NameText.text = par;
        }
    }
}