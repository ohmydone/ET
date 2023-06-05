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
                    break;
                case OperaID.Slot2:
                    break;
                case OperaID.ShowInfo:
                    
                    break;
            }
            await ETTask.CompletedTask;
        }
    }
}