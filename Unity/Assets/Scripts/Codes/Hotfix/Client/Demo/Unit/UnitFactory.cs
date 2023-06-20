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
            if (unit.Type == UnitType.Player)
            {
                unitComponent.My = unit;
            }
            
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

                    unit.AddComponent<AOIUnitComponent, float3, Quaternion, UnitType>(unitInfo.Position, unit.Rotation, unit.Type);
                    CombatUnitComponent combatU;
                    if (unitInfo.SkillIds != null)
                    {
                        combatU = unit.AddComponent<CombatUnitComponent, List<int>>(unitInfo.SkillIds);
                    }
                    else
                    {
                        combatU = unit.AddComponent<CombatUnitComponent>();
                    }

                    if (unitInfo.BuffIds != null && unitInfo.BuffIds.Count > 0)
                    {
                        var buffC = combatU.GetComponent<BuffComponent>();
                        buffC.Init(unitInfo.BuffIds, unitInfo.BuffTimestamp, unitInfo.BuffSourceIds);
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

                    unit.AddComponent<AOIUnitComponent, float3, Quaternion, UnitType>(unit.Position, unit.Rotation, unit.Type);
                    unit.AddComponent<ObjectWait>();
                    break;
                }
            }

            EventSystem.Instance.Publish(unit.DomainScene(), new EventType.AfterUnitCreate() { Unit = unit });
            return unit;
        }
        
        /// <summary>
    /// 创建技能触发体（单机用）
    /// </summary>
    /// <param name="currentScene"></param>
    /// <param name="configId"></param>
    /// <param name="pos"></param>
    /// <param name="rota"></param>
    /// <param name="para"></param>
    /// <returns></returns>
    public static Unit CreateSkillCollider(Scene currentScene, int configId, float3 pos, Quaternion rota, SkillPara para)
    {
        UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
        Unit unit = unitComponent.AddChild<Unit, int>(configId);

        unit.Position = pos;
        unit.Rotation = rota;
        var collider = SkillJudgeConfigCategory.Instance.Get(configId);
        if (collider.ColliderType == ColliderType.Target) //朝指定位置方向飞行碰撞体
        {
            var numc = unit.AddComponent<NumericComponent>();

            numc.Set(NumericType.SpeedBase, collider.Speed);
            var moveComp = unit.AddComponent<MoveComponent>();
            Log.Info(pos + " " + pos +math.normalize(para.Position - pos)* collider.Speed * collider.Time / 1000f);
            List<float3> target = new List<float3>();
            target.Add(pos);
            target.Add(pos + math.normalize(para.Position - pos) * collider.Speed * collider.Time / 1000f);
            moveComp.MoveToAsync(target, collider.Speed).Coroutine();
            unit.AddComponent<SkillColliderComponent, SkillPara, float3>(para, para.Position);
        }
        else if (collider.ColliderType == ColliderType.Aim) //锁定目标飞行
        {
            var numc = unit.AddComponent<NumericComponent>();
            numc.Set(NumericType.SpeedBase, collider.Speed);
            unit.AddComponent<MoveComponent>();
            unit.AddComponent<ZhuiZhuAimComponent, Unit>(para.To.unit);
            unit.AddComponent<AIComponent, int, int>(2, 50);
            unit.AddComponent<SkillColliderComponent, SkillPara, long>(para, para.To.Id);
        }

        unit.AddComponent<AOIUnitComponent, float3, Quaternion, UnitType>(pos, rota, unit.Type);
        return unit;
    }
    }

    
}