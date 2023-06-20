using Unity.Mathematics;

namespace ET
{
    public struct Ray
    {
        /// <summary>
        /// 方向
        /// </summary>
        public float3 Dir;
        /// <summary>
        /// 起点
        /// </summary>
        public float3 Start;
        /// <summary>
        /// 长度
        /// </summary>
        public float Distance;
        /// <summary>
        /// 长度
        /// </summary>
        public float SqrDistance;
        /// <summary>
        /// 世界空间转模型空间
        /// </summary>
        /// <param name="self"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Ray WorldToModel(Ray self, quaternion b, float3 c)
        {
            return new Ray
            {
                //quaternion mul float3 is rotate   
                Dir =math.mul(math.inverse(b),self.Dir),
                Start =math.mul(math.inverse(b),(self.Start - c)),
                Distance = self.Distance,
                SqrDistance = self.Distance*self.Distance
            };
        }
        
    }
}