using System.Collections;
using System.Collections.Generic;
using ET.EventType;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class DoOperaHandler: AEvent<Scene, EventType.DoOpera>
    {
        protected override async ETTask Run(Scene scene, DoOpera a)
        {
            switch (a.OperaId)
            {
                case OperaID.Fire:
                    break;
                case OperaID.Slot1:
                case OperaID.Slot2:
                    break;
                case OperaID.ShowInfo:
                    UIComponent uiComponent= UIComponent.Instance;
                    if (uiComponent.IsWindowVisible(WindowID.WindowID_Info))
                    {
                        uiComponent.HideWindow<DlgInfo>();
                    }
                    else
                    {
                        uiComponent.ShowWindow<DlgInfo>();
                    }
                    break;
            }
            await ETTask.CompletedTask;
        }
    }
}