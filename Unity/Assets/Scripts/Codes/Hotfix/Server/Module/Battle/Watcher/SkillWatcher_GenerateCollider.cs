using System.Collections.Generic;
using System;
using Unity.Mathematics;

namespace ET.Server
{
    /// <summary>
    /// 生成碰撞体
    /// </summary>
    [SkillWatcher(SkillStepType.GenerateCollider)]
    public class SkillWatcher_GenerateCollider: ISkillWatcher
    {
        public void Run(SkillPara para)
        {
            if (para.From.unit.IsGhost()) return; //纯客户端单机游戏去掉
            var stepPara = para.GetCurSkillStepPara();
            Log.Info("SkillWatcher_GenerateCollider");
            if (StepParaHelper.TryParseInt(ref stepPara.Paras[0], out var colliderId))
            {
                SkillJudgeConfig collider = SkillJudgeConfigCategory.Instance.Get(colliderId);
                if (collider != null)
                {
                    var aoiUnit = para.From.unit.GetComponent<AOIUnitComponent>();
                    var scene = aoiUnit.Scene.GetParent<Scene>();
                    Unit unit = null;
                    float3 FromUnitPos = para.From.unit.Position;
                    float3 ToUnitPos = float3.zero;
                    if (para.To != null)
                        ToUnitPos = para.To.unit.Position;

                    #region 创建碰撞体AOIUnit

                    if (collider.ColliderType == ColliderType.FixedPosition) //固定位置碰撞体
                    {
                        if (collider.StartPosType == StartPosType.Self)
                            unit = UnitFactory.CreateSkillCollider(scene, colliderId, FromUnitPos, para.Rotation, para);
                        else if (collider.StartPosType == StartPosType.Aim && para.To != null)
                        {
                            unit = UnitFactory.CreateSkillCollider(scene, colliderId, ToUnitPos, para.Rotation, para);
                        }
                        else if (collider.StartPosType == StartPosType.MousePos)
                            unit = UnitFactory.CreateSkillCollider(scene, colliderId, para.Position, para.Rotation, para);
                        else
                        {
                            Log.Info("目标未指定,或触发体类型不存在");
                            return;
                        }
                    }
                    else if (collider.ColliderType == ColliderType.FixedRotation) //固定方向碰撞体
                    {
                        var dir = math.normalize(new float3(para.Position.x - FromUnitPos.x, para.Position.y - FromUnitPos.y,
                            para.Position.z - FromUnitPos.z));
                        if (collider.ColliderShape == ColliderShape.OBB) //立方找到中点
                        {
                            var point = FromUnitPos + dir * collider.ColliderPara[2] / 2;
                            if (collider.StartPosType == StartPosType.Self)
                            {
                                unit = UnitFactory.CreateSkillCollider(scene, colliderId, point, para.Rotation, para);
                            }
                            else
                            {
                                Log.Info("目标未指定,或触发体类型不存在");
                                return;
                            }
                        }
                        else
                        {
                            Log.Info("目标未指定,或触发体类型不存在");
                            return;
                        }
                    }
                    else if (collider.ColliderType == ColliderType.Target) //朝指定位置方向飞行碰撞体
                    {
                        float3 startPos = FromUnitPos;
                        if (collider.StartPosType == StartPosType.Self)
                            startPos = FromUnitPos;
                        else if (collider.StartPosType == StartPosType.Aim && para.To != null)
                            startPos = ToUnitPos;
                        else if (collider.StartPosType == StartPosType.MousePos)
                            startPos = para.Position;
                        else
                        {
                            Log.Info("目标未指定,或触发体类型不存在");
                            return;
                        }

                        unit = UnitFactory.CreateSkillCollider(scene, colliderId, startPos, para.Rotation, para);
                    }
                    else if (collider.ColliderType == ColliderType.Aim) //锁定目标飞行
                    {
                        float3 startPos = FromUnitPos;
                        if (collider.StartPosType == StartPosType.Self && para.To != null)
                            startPos = FromUnitPos;
                        else if (collider.StartPosType == StartPosType.Aim && para.To != null)
                            startPos = ToUnitPos;
                        else if (collider.StartPosType == StartPosType.MousePos && para.To != null)
                            startPos = para.Position;
                        else
                        {
                            Log.Info("目标未指定,或触发体类型不存在");
                            return;
                        }

                        unit = UnitFactory.CreateSkillCollider(scene, colliderId, startPos, para.Rotation, para);
                    }
                    else if (collider.ColliderType == ColliderType.Immediate) //立刻结算
                    {
                        if (collider.StartPosType == StartPosType.Self)
                            EventSystem.Instance.Publish(para.DomainScene(), new EventType.OnSkillTrigger
                            {
                                From = aoiUnit,
                                To = aoiUnit,
                                Para = stepPara,
                                Type = AOITriggerType.Enter,
                                Config = para.SkillConfig,
                                Cost = para.Cost,
                                CostId = para.CostId,
                            });
                        else if (collider.StartPosType == StartPosType.Aim && para.To != null)
                            EventSystem.Instance.Publish(para.DomainScene(),new EventType.OnSkillTrigger
                            {
                                From = aoiUnit,
                                To = para.To.unit.GetComponent<AOIUnitComponent>(),
                                Para = stepPara,
                                Type = AOITriggerType.Enter,
                                Config = para.SkillConfig,
                                Cost = para.Cost,
                                CostId = para.CostId,
                            });
                        else if (collider.StartPosType == StartPosType.MousePos)
                        {
                            unit = UnitFactory.CreateSkillCollider(scene, colliderId, para.Position, para.Rotation, para);
                            return;
                        }
                        else
                        {
                            Log.Info("目标未指定,或触发体类型不存在");
                            return;
                        }
                    }
                    else
                    {
                        Log.Error("碰撞体类型未处理" + collider.ColliderType);
                        return;
                    }

                    #endregion
                }
            }
        }
    }
}