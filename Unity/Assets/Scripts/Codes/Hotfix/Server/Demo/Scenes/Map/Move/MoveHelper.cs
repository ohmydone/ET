using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;

namespace ET.Server
{
    public static class MoveHelper
    {
        // 可以多次调用，多次调用的话会取消上一次的协程
        public static async ETTask FindPathMoveToAsync(this Unit unit, float3 target, ETCancellationToken cancellationToken = null)
        {
            float speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
            if (speed < 0.01)
            {
                unit.SendStop(2);
                return;
            }

            using var list = ListComponent<float3>.Create();
            
            unit.GetComponent<PathfindingComponent>().Find(unit.Position, target, list);

            List<float3> path = list;
            if (path.Count < 2)
            {
                unit.SendStop(3);
                return;
            }
                
            // 广播寻路路径
            M2C_PathfindingResult m2CPathfindingResult = new M2C_PathfindingResult();
            m2CPathfindingResult.Position = unit.Position;
            m2CPathfindingResult.Id = unit.Id;
            for (int i = 0; i < list.Count; ++i)
            {
                float3 vector3 = list[i];
                m2CPathfindingResult.Points.Add(vector3);
            }
            MessageHelper.Broadcast(unit, m2CPathfindingResult);

            bool ret = await unit.GetComponent<MoveComponent>().MoveToAsync(path, speed);
            if (ret) // 如果返回false，说明被其它移动取消了，这时候不需要通知客户端stop
            {
                unit.SendStop(0);
            }
        }

        public static async ETTask MoveToAsync(this Unit unit, List<float3> path, ETCancellationToken cancellationToken = null)
        {
            float speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
            if (speed < 0.01)
            {
                unit.SendStop(-1);
                return;
            }
            // 广播寻路路径
            M2C_PathfindingResult m2CPathfindingResult = new M2C_PathfindingResult();
            m2CPathfindingResult.Position = unit.Position;
            m2CPathfindingResult.Id = unit.Id;
            for (int i = 0; i < path.Count; ++i)
            {
                float3 vector3 = path[i];
                m2CPathfindingResult.Points.Add(vector3);
            }
            MessageHelper.Broadcast(unit, m2CPathfindingResult);

            bool ret = await unit.GetComponent<MoveComponent>().MoveToAsync(path, speed);
            if (ret) // 如果返回false，说明被其它移动取消了，这时候不需要通知客户端stop
            {
                unit.SendStop(0);
            }
        }
        public static void Stop(this Unit unit, int error)
        {
            unit.GetComponent<MoveComponent>().Stop(error == 0);
            unit.SendStop(error);
        }
        // error: 0表示协程走完正常停止
        public static void SendStop(this Unit unit, int error)
        {
            MessageHelper.Broadcast(unit, new M2C_Stop()
            {
                Error = error,
                Id = unit.Id, 
                Position = unit.Position,
                Rotation = unit.Rotation
            });
        }
    }
}