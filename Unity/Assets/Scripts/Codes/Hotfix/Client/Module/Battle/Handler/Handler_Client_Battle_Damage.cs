using ET.EventType;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class Handler_Client_Battle_Damage: AEvent<Scene,EventType.Battle_Damage>
    {
        protected override async ETTask Run(Scene scene, Battle_Damage a)
        {
            BattleHelper.Damage(a.FromU,a.ToU,a.Value);
            await ETTask.CompletedTask;
        }
    }
}

