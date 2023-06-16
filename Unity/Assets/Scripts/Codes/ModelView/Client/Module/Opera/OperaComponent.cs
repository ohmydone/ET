using System;

using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class OperaComponent: Entity, IAwake, IInput,IDestroy
    {
        public Vector3 ClickPoint;

        public int mapMask;

        public C2M_PathfindingResult frameClickMap = new C2M_PathfindingResult();
    }
}