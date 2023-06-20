using Unity.Mathematics;
using UnityEngine;

namespace ET
{
    public struct RaycastHit
    {
        public float3 Hit;

        public AOITrigger Trigger;

        public float Distance;
    }
}