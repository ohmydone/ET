using System;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Server
{
	[ActorMessageHandler(SceneType.Map)]
	public class M2M_UnitTransferRequestHandler : AMActorRpcHandler<Scene, M2M_UnitTransferRequest, M2M_UnitTransferResponse>
	{
		protected override async ETTask Run(Scene scene, M2M_UnitTransferRequest request, M2M_UnitTransferResponse response)
		{
			UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
			Unit unit = request.Unit;
			
			unitComponent.AddChild(unit);
			unitComponent.Add(unit);

			foreach (Entity entity in request.Entitys)
			{
				unit.AddComponent(entity);
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
			var aoiu = unit.AddComponent<AOIUnitComponent,Vector3,Quaternion, UnitType,int>
					(unit.Position,unit.Rotation,unit.Type,numericComponent.GetAsInt(NumericType.AOI));
			
			aoiu.AddSphereCollider(0.5f);
			
			// 解锁location，可以接收发给Unit的消息
			await LocationProxyComponent.Instance.UnLock(LocationType.Unit, unit.Id, request.OldInstanceId, unit.InstanceId);
		}
	}
}