
namespace ET
{
    [Event(SceneType.Map)]
    public class ChangeRotation_TurnAOIUnit: AEvent<Scene,EventType.ChangeRotation>
    {
        protected override async ETTask Run(Scene scene,EventType.ChangeRotation args)
        {
            AOIUnitComponent aoiUnitComponent = args.Unit?.GetComponent<AOIUnitComponent>();
            if (aoiUnitComponent == null || aoiUnitComponent.IsDisposed) return;
            aoiUnitComponent.Turn(args.Unit.Rotation);
            await ETTask.CompletedTask;
        }
    }
}