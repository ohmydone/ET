using System.Collections.Generic;
using MongoDB.Bson;
using Unity.Mathematics;

namespace ET.Server
{
    [FriendOf(typeof(MoveComponent))]
    public static class TransferHelper
    {
        public static async ETTask TransferAtFrameFinish(Unit unit, long sceneInstanceId, string sceneName)
        {
            await Game.WaitFrameFinish();

            await TransferHelper.Transfer(unit, sceneInstanceId, sceneName);
        }
        

        public static async ETTask Transfer(Unit unit, long sceneInstanceId, string sceneName)
        {
            // location加锁
            long unitId = unit.Id;
            long unitInstanceId = unit.InstanceId;
            
            M2M_UnitTransferRequest request = new M2M_UnitTransferRequest() {Entitys = new List<byte[]>()};
            request.OldInstanceId = unitInstanceId;
            request.Unit = unit.ToBson();
            foreach (Entity entity in unit.Components.Values)
            {
                if (entity is ITransfer)
                {
                    request.Entitys.Add(entity.ToBson());
                }
            }
            unit.Dispose();
            
            await LocationProxyComponent.Instance.Lock(LocationType.Unit, unitId, unitInstanceId);
            await ActorMessageSenderComponent.Instance.Call(sceneInstanceId, request);
        }
        
        /// <summary>
        /// 大地图切换区域
        /// </summary>
        /// <param name="aoiU"></param>
        /// <param name="sceneInstanceId"></param>
        public static async ETTask AreaTransfer(AOIUnitComponent aoiU, long sceneInstanceId)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Transfer, aoiU.Id))
            {
                if (aoiU.IsDisposed||aoiU.IsGhost()) return;
                var unit = aoiU.GetParent<Unit>();
                //由于是一步步移动过去的，所以不涉及客户端加载场景，服务端自己内部处理好数据转移就好
                M2M_UnitAreaTransferRequest request = new M2M_UnitAreaTransferRequest();
                ListComponent<int> Stack = ListComponent<int>.Create();
                request.Unit = unit;
                Entity curEntity = unit;
                Stack.Add(-1);
                while (Stack.Count > 0)
                {
                    var index = Stack[Stack.Count - 1];
                    if (index != -1)
                    {
                        curEntity = request.Entitys[index];
                    }

                    Stack.RemoveAt(Stack.Count - 1);
                    foreach (Entity entity in curEntity.Components.Values)
                    {
                        if (entity is ITransfer)
                        {
                            var childIndex = request.Entitys.Count;
                            request.Entitys.Add(entity);
                            Stack.Add(childIndex);
                            request.Map.Add(new RecursiveEntitys { ChildIndex = childIndex, ParentIndex = index, IsChild = 0 });
                        }
                    }

                    foreach (Entity entity in curEntity.Children.Values)
                    {
                        if (entity is ITransfer)
                        {
                            var childIndex = request.Entitys.Count;
                            request.Entitys.Add(entity);
                            Stack.Add(childIndex);
                            request.Map.Add(new RecursiveEntitys { ChildIndex = childIndex, ParentIndex = index, IsChild = 1 });
                        }
                    }
                }

                Stack.Dispose();
                // 删除Mailbox,让发给Unit的ActorLocation消息重发
                unit.RemoveComponent<MailBoxComponent>();

                long oldInstanceId = unit.InstanceId;
                // location加锁
                await LocationProxyComponent.Instance.Lock(LocationType.Unit,unit.Id, unit.InstanceId);
                aoiU.GetComponent<GhostComponent>().IsGoast = true;
                M2M_UnitAreaTransferResponse response =
                        await ActorMessageSenderComponent.Instance.Call(sceneInstanceId, request) as M2M_UnitAreaTransferResponse;
                await LocationProxyComponent.Instance.UnLock(LocationType.Unit,unit.Id, oldInstanceId, response.NewInstanceId);
            }
        }
        
        /// <summary>
        /// 大地图到边缘注册到其他地图
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="sceneInstanceId"></param>
        public static void AreaAdd(Unit unit, long sceneInstanceId)
        {
            //由于是一步步移动过去的，所以不涉及客户端加载场景，服务端自己内部处理好数据转移就好
            M2M_UnitAreaAdd request = new M2M_UnitAreaAdd();
            ListComponent<int> Stack = ListComponent<int>.Create();
            request.Unit = unit;
            Entity curEntity = unit;
            Stack.Add(-1);
            while (Stack.Count > 0)
            {
                var index = Stack[Stack.Count - 1];
                if (index != -1)
                {
                    curEntity = request.Entitys[index];
                }
                Stack.RemoveAt(Stack.Count - 1);
                foreach (Entity entity in curEntity.Components.Values)
                {
                    if (entity is ITransfer)
                    {
                        var childIndex = request.Entitys.Count;
                        request.Entitys.Add(entity);
                        Stack.Add(childIndex);
                        request.Map.Add(new RecursiveEntitys
                        {
                            ChildIndex = childIndex,
                            ParentIndex = index,
                            IsChild = 0
                        });
                    }
                }
                foreach (Entity entity in curEntity.Children.Values)
                {
                    if (entity is ITransfer)
                    {
                        var childIndex = request.Entitys.Count;
                        request.Entitys.Add(entity);
                        Stack.Add(childIndex);
                        request.Map.Add(new RecursiveEntitys
                        {
                            ChildIndex = childIndex,
                            ParentIndex = index,
                            IsChild = 1
                        });
                    }
                }
            }
            Stack.Dispose();
            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            if (moveComponent != null)
            {
                if (!moveComponent.IsArrived())
                {
                    request.MoveInfo = new MoveInfo();
                    for (int i = moveComponent.N; i < moveComponent.Targets.Count; ++i)
                    {
                        float3 pos = moveComponent.Targets[i];
                        request.MoveInfo.Points.Add(pos);
                    }
                }
            }
            ActorMessageSenderComponent.Instance.Send(sceneInstanceId, request);
        }
        
        /// <summary>
        /// 在其他区域创建
        /// </summary>
        /// <param name="aoiU"></param>
        /// <param name="sceneInstanceId"></param>
        public static void AreaCreate(AOIUnitComponent aoiU, long sceneInstanceId)
        {
            var unit = aoiU.GetParent<Unit>();
            aoiU.GetComponent<GhostComponent>().IsGoast = true;
            //由于是一步步移动过去的，所以不涉及客户端加载场景，服务端自己内部处理好数据转移就好
            M2M_UnitAreaCreate request = new M2M_UnitAreaCreate();
            ListComponent<int> Stack = ListComponent<int>.Create();
            request.Unit = unit;
            Entity curEntity = unit;
            Stack.Add(-1);
            while (Stack.Count > 0)
            {
                var index = Stack[Stack.Count - 1];
                if (index != -1)
                {
                    curEntity = request.Entitys[index];
                }
                Stack.RemoveAt(Stack.Count - 1);
                foreach (Entity entity in curEntity.Components.Values)
                {
                    if (entity is ITransfer)
                    {
                        var childIndex = request.Entitys.Count;
                        request.Entitys.Add(entity);
                        Stack.Add(childIndex);
                        request.Map.Add(new RecursiveEntitys
                        {
                            ChildIndex = childIndex,
                            ParentIndex = index,
                            IsChild = 0
                        });
                    }
                }
                foreach (Entity entity in curEntity.Children.Values)
                {
                    if (entity is ITransfer)
                    {
                        var childIndex = request.Entitys.Count;
                        request.Entitys.Add(entity);
                        Stack.Add(childIndex);
                        request.Map.Add(new RecursiveEntitys
                        {
                            ChildIndex = childIndex,
                            ParentIndex = index,
                            IsChild = 1
                        });
                    }
                }
            }
            Stack.Dispose();

            ActorMessageSenderComponent.Instance.Send(sceneInstanceId, request);
            
        }
        
    }
}