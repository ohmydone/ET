using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
namespace ET
{
    /// <summary>
    /// 立方体触发器(有个外接球，先判断是否在外接球里,再判断是否在立方体里)
    /// </summary>
    [ComponentOf(typeof(AOITrigger))]
    public class OBBComponent:Entity,IAwake<float3>,IDestroy
    {
        /// <summary>
        /// 长宽高
        /// </summary>
        public float3 Scale;
        
        public class TempPosRot
        {
            public float3 Pos;
            public quaternion Rot;
        }

        public TempPosRot LastVertexPosRot;
        public ListComponent<float3> LastVertex;
        public TempPosRot LastSidesPosRot;
        public ListComponent<Ray> LastSides;
    }
}