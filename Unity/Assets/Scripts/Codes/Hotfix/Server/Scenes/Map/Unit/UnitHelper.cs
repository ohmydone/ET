using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    [FriendOf(typeof(MoveComponent))]
    [FriendOf(typeof(NumericComponent))]
    [FriendOf(typeof(CombatUnitComponent))]
    [FriendOf(typeof(BuffComponent))]
    [FriendOf(typeof(Buff))]
    public static class UnitHelper
    {
        public static UnitInfo CreateUnitInfo(Unit unit)
        {
            UnitInfo unitInfo = new UnitInfo();
            NumericComponent nc = unit.GetComponent<NumericComponent>();
            unitInfo.UnitId = unit.Id;
            unitInfo.ConfigId = unit.ConfigId;
            unitInfo.Type = (int)unit.Type;
            unitInfo.Position = unit.Position;
            unitInfo.Forward = unit.Forward;
            unitInfo.SkillIds = new List<int>();
            
            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            if (moveComponent != null)
            {
                if (!moveComponent.IsArrived())
                {
                    unitInfo.MoveInfo = new MoveInfo() { Points = new List<float3>() };
                    unitInfo.MoveInfo.Points.Add(unit.Position);
                    for (int i = moveComponent.N; i < moveComponent.Targets.Count; ++i)
                    {
                        float3 pos = moveComponent.Targets[i];
                        unitInfo.MoveInfo.Points.Add(pos);
                    }
                }
            }

            unitInfo.KV = new Dictionary<int, long>();

            foreach ((int key, long value) in nc.NumericDic)
            {
                unitInfo.KV.Add(key, value);
            }
            #region 战斗数据

            var cuc = unit.GetComponent<CombatUnitComponent>();
            if (cuc != null)
            {
                //技能
                unitInfo.SkillIds.AddRange(cuc.IdSkillMap.Keys);
                var buffC = cuc.GetComponent<BuffComponent>();
                if (buffC != null)
                {
                    for (int i = 0; i < buffC.AllBuff.Count; i++)
                    {
                        var buff = buffC.GetChild<Buff>(buffC.AllBuff[i]);
                        unitInfo.BuffIds.Add(buff.ConfigId);
                        unitInfo.BuffTimestamp.Add(buff.Timestamp);
                        unitInfo.BuffSourceIds.Add(buff.FromUnitId);
                    }
                }
            }
            
            #endregion
            return unitInfo;
        }
        
        // 获取看见unit的玩家，主要用于广播
        public static Dictionary<long, AOIEntity> GetBeSeePlayers(this Unit self)
        {
            return self.GetComponent<AOIEntity>().GetBeSeePlayers();
        }
        
        /// <summary>
        /// 获取看见unit的玩家，主要用于广播,注意不能Dispose
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static List<AOIUnitComponent> GetBeSeeUnits(this Unit self)
        {
            return self.GetComponent<AOIUnitComponent>().GetBeSeeUnits();
        }
        
        public static void NoticeUnitAdd(Unit unit, Unit sendUnit)
        {
            M2C_CreateUnits createUnits = new M2C_CreateUnits();
            createUnits.Units.Add(CreateUnitInfo(sendUnit));
            MessageHelper.SendToClient(unit, createUnits);
        }
        
        public static void NoticeUnitRemove(Unit unit, Unit sendUnit)
        {
            M2C_RemoveUnits removeUnits = new M2C_RemoveUnits();
            removeUnits.Units.Add(sendUnit.Id);
            MessageHelper.SendToClient(unit, removeUnits);
        }
        
        public static void NoticeUnitsAdd(Unit unit, List<AOIUnitComponent> sendUnit)
        {
            M2C_CreateUnits createUnits = new M2C_CreateUnits();
            for (int i = 0; i < sendUnit.Count; i++)
            {
                if (unit.Id == sendUnit[i].Id) continue;
                createUnits.Units.Add(CreateUnitInfo(sendUnit[i].GetParent<Unit>()));
            }

            if (createUnits.Units.Count > 0)
            {
                MessageHelper.SendToClient(unit, createUnits);
            }
            
        }
        
        public static void NoticeUnitsRemove(Unit unit, List<long> sendUnit)
        {
            M2C_RemoveUnits removeUnits = new M2C_RemoveUnits();
            removeUnits.Units = sendUnit;
            MessageHelper.SendToClient(unit, removeUnits);
        }
    }
}