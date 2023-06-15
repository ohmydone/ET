using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    public static class UnitFactory
    {
        public static Unit Create(Scene currentScene, UnitInfo unitInfo)
        {
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(unitInfo.UnitId, unitInfo.ConfigId);
            unitComponent.Add(unit);

            unit.Position = unitInfo.Position;
            unit.Forward = unitInfo.Forward;

            switch (unit.Type)
            {
                case UnitType.Monster:
                case UnitType.Player:
                {
                    NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
                    foreach (var kv in unitInfo.KV)
                    {
                        numericComponent.Set(kv.Key, kv.Value);
                    }

                    unit.AddComponent<MoveComponent>();
                    if (unitInfo.MoveInfo != null)
                    {
                        if (unitInfo.MoveInfo.Points.Count > 0)
                        {
                            unitInfo.MoveInfo.Points[0] = unit.Position;
                            unit.MoveToAsync(unitInfo.MoveInfo.Points).Coroutine();
                        }
                    }
                    
                    unit.AddComponent<AOIUnitComponent,Vector3,Quaternion, UnitType>(unitInfo.Position,unit.Rotation,unit.Type);
                    CombatUnitComponent combatU;
                    if (unitInfo.SkillIds != null)
                    {
                        combatU = unit.AddComponent<CombatUnitComponent,List<int>>(unitInfo.SkillIds);
				        
                    }
                    else
                    {
                        combatU = unit.AddComponent<CombatUnitComponent>();
                    }

                    if (unitInfo.BuffIds != null&&unitInfo.BuffIds.Count>0)
                    {
                        var buffC = combatU.GetComponent<BuffComponent>();
                        buffC.Init(unitInfo.BuffIds, unitInfo.BuffTimestamp,unitInfo.BuffSourceIds);

                    }

                    
                    unit.AddComponent<ObjectWait>();

                    unit.AddComponent<XunLuoPathComponent>();

                    break;
                }
                case UnitType.Skill:
                {
                    NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
                    foreach (var kv in unitInfo.KV)
                    {
                        numericComponent.Set(kv.Key, kv.Value);
                    }

                    unit.AddComponent<MoveComponent>();
                    if (unitInfo.MoveInfo != null)
                    {
                        if (unitInfo.MoveInfo.Points.Count > 0)
                        {
                            unitInfo.MoveInfo.Points[0] = unit.Position;
                            unit.MoveToAsync(unitInfo.MoveInfo.Points).Coroutine();
                        }
                    }
                    unit.AddComponent<ObjectWait>();
                    break;
                }
            }
            EventSystem.Instance.Publish(unit.DomainScene(), new EventType.AfterUnitCreate() { Unit = unit });
            return unit;
        }
    }
}