using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET.Client;
using UnityEngine;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class KeyCodeComponent:Entity,IAwake,IDestroy
    {
        [StaticField]
        public static KeyCodeComponent Instance;
        
        public Dictionary<int,int> KeyMap { get; set; }//[逻辑按键：物理按键编号]

        /// <summary>
        /// 默认键位
        /// </summary>
        public readonly List<string> DefaultKeyCodeMap = new List<string>()
        {
            OperaID.Slot1,
            OperaID.Slot2,
            OperaID.Slot3,
            OperaID.Slot4,
            OperaID.Slot5,
            OperaID.Slot6,
        };
    }
}
