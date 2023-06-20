using ET.EventType;

namespace ET
{
    [Event(SceneType.Client)]
    public class ChangePosition_MoveAOIUnit: AEvent<Scene,EventType.ChangePosition>
    {
        protected override async ETTask Run(Scene scene, ChangePosition a)
        {
            AOIUnitComponent aoiUnitComponent = a.Unit.GetComponent<AOIUnitComponent>();
            if (aoiUnitComponent == null || aoiUnitComponent.IsDisposed) 
                await ETTask.CompletedTask;
            aoiUnitComponent.Move(a.Unit.Position).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}