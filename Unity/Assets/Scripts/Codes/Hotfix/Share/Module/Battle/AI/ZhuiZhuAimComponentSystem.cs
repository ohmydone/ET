using System;
namespace ET
{
    [ObjectSystem]
    public class ZhuiZhuAimComponentAwakeSystem:AwakeSystem<ZhuiZhuAimComponent,Unit>
    {
        protected override void Awake(ZhuiZhuAimComponent self,Unit a)
        {
            self.Aim = a;
            //self.OnArrived = b;
        }
    }

    [ObjectSystem]
    [FriendOf(typeof(ZhuiZhuAimComponent))]
    public static class ZhuiZhuAimComponentSystem
    {
        public static void Arrived(this ZhuiZhuAimComponent self)
        {
            self.Aim = null;
            
            //self.OnArrived?.Invoke();
            //self.OnArrived = null;
        }
    }
}