namespace ET
{
    [Event(SceneType.Client)]
    public class ChangePosition_NotifyMoveBuffWatcher:  AEvent<Scene,EventType.ChangePosition>
    {
        protected override async ETTask Run(Scene scene,EventType.ChangePosition args)
        {
            BuffComponent bc = args.Unit.GetComponent<CombatUnitComponent>()?.GetComponent<BuffComponent>();
            if (bc != null)
            {
                bc.AfterMove(args.Unit,args.OldPos);
            }
            
        }
    }
}