using System;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(BuffComponent))]
    [Event(SceneType.Current)]
    public class AfterAddBuff_CreateBuffView: AEvent<Scene, EventType.AfterAddBuff>
    {
        protected override async ETTask Run(Scene scene, EventType.AfterAddBuff args)
        {
            if (args.Buff.Config.EffectId != 0)
            {
                EventSystem.Instance.Publish(scene,new EventType.AddEffect
                {
                    EffectId = args.Buff.Config.EffectId,
                    Parent = args.Buff,
                    Unit = args.Buff.GetParent<BuffComponent>().unit
                });
            }
        }

    }
}