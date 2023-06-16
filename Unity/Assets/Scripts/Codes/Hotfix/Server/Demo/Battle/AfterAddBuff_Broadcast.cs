namespace ET.Server
{
    // 添加BUFF
    [Event(SceneType.Map)]
    [FriendOf(typeof(BuffComponent))]
    [FriendOf(typeof(Buff))]
    public class AfterAddBuff_Broadcast: AEvent<Scene,ET.EventType.AfterAddBuff>
    {
        protected override async ETTask Run(Scene scene, ET.EventType.AfterAddBuff args)
        {
            var unit = args.Buff.GetParent<BuffComponent>().unit;
            M2C_AddBuff msg = new M2C_AddBuff { ConfigId = args.Buff.ConfigId, Timestamp = args.Buff.Timestamp, UnitId = unit.Id, };
            MessageHelper.Broadcast(unit,msg);
        }
    }
}