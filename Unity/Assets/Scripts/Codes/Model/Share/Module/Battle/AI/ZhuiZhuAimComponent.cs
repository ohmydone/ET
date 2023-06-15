using System;
namespace ET
{
    [ComponentOf(typeof(Unit))]
    public class ZhuiZhuAimComponent:Entity,IAwake<Unit>
    {
        public Unit Aim { get; set; }
        //public Action OnArrived { get; set; }
    }
}