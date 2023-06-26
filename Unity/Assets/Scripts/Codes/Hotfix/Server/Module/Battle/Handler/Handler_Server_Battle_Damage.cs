using ET.EventType;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class Handler_Server_Battle_Damage: AEvent<Scene,EventType.Battle_Damage>
    {
        protected override async ETTask Run(Scene scene, Battle_Damage a)
        {
            BattleHelper.Damage(a.FromU,a.ToU,a.Value,ghost:a.Skill?.GetComponent<GhostComponent>());
            await ETTask.CompletedTask;
        }
    }
}

