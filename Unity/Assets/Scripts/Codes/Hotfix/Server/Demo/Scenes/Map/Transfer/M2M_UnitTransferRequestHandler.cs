using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Server
{
	[ActorMessageHandler(SceneType.Map)]
	public class M2M_UnitTransferRequestHandler : AMActorRpcHandler<Scene, M2M_UnitTransferRequest, M2M_UnitTransferResponse>
	{
		protected override async ETTask Run(Scene scene, M2M_UnitTransferRequest request, M2M_UnitTransferResponse response)
		{
			await ETTask.CompletedTask;
			UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
			Unit unit = MongoHelper.Deserialize<Unit>(request.Unit);
			
			unitComponent.AddChild(unit);
			unitComponent.Add(unit);

			List<Entity> entities = new List<Entity>();
			foreach (byte[] bytes in request.Entitys)
			{
				entities.Add(MongoHelper.Deserialize<Entity>(bytes));
			}

			foreach (var item in request.Map)
			{
				var entity = entities[item.ChildIndex];
				Entity parent;
				if (item.ParentIndex == -1)//父组件为自己
					parent = unit;
				else
					parent = entities[item.ParentIndex];
				
				if (item.IsChild == 0)
					parent.AddComponent(entity);
				else
					parent.AddChild(entity);
			}
			
			
			unit.AddComponent<MoveComponent>();
			unit.AddComponent<PathfindingComponent, string>(scene.Name);
			unit.Position = new float3(-10, 0, -10);
			
			unit.AddComponent<MailBoxComponent>();

			// 通知客户端开始切场景
			M2C_StartSceneChange m2CStartSceneChange = new M2C_StartSceneChange() {SceneInstanceId = scene.InstanceId, SceneName = scene.Name};
			MessageHelper.SendToClient(unit, m2CStartSceneChange);
			
			// 通知客户端创建My Unit
			M2C_CreateMyUnit m2CCreateUnits = new M2C_CreateMyUnit();
			m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
			MessageHelper.SendToClient(unit, m2CCreateUnits);
			
			var numericComponent = unit.GetComponent<NumericComponent>();
			// 加入aoi
			var aoiu = unit.AddComponent<AOIUnitComponent,float3,quaternion, UnitType,int>
					(unit.Position,unit.Rotation,unit.Type,numericComponent.GetAsInt(NumericType.AOI));
			
			aoiu.AddSphereCollider(0.5f);
			
			// 解锁location，可以接收发给Unit的消息
			await LocationProxyComponent.Instance.UnLock(LocationType.Unit, unit.Id, request.OldInstanceId, unit.InstanceId);
		}
	}
}