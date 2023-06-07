namespace ET
{
    [ObjectSystem]
    public class UnitAwakeSystem: AwakeSystem<Unit, int>
    {
        protected override void Awake(Unit self, int configId)
        {
            self.ConfigId = configId;
        }
    }

    public static class UnitSystem
    {
        public static bool IsGhost(this Unit self)
        {
            // var aoi = self.GetComponent<AOIUnitComponent>();
            // if (aoi != null)
            // {
            //     var ghost = aoi.GetComponent<GhostComponent>();
            //     if (ghost != null)
            //     {
            //         return ghost.IsGoast;
            //     }
            // }
            return false;
        }
    }
}