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
        }

        public static void ShowWindow(this DlgGM self, Entity contextData = null)
        {
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
    }
}