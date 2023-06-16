
using System.Collections.Generic;
using ET.EventType;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof(CombatUnitComponent))]
    public class AfterCombatUnitComponentCreate_Init:AEvent<Scene,EventType.AfterCombatUnitComponentCreate>
    {
        protected override async ETTask Run(Scene scene, AfterCombatUnitComponentCreate a)
        {
            var self = a.CombatUnitComponent;
            if (UnitComponent.Instance.My== self.unit)
            {
                self.AddComponent<SpellPreviewComponent>();
            }
            await ETTask.CompletedTask;
        }
    }
}