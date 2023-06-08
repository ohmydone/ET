using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    public static class UnitFactory
    {
        public static Unit Create(Scene scene, long id, UnitType unitType)
        {
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            //Unit unit = unitComponent.AddChildWithId<Unit, int>(id,);
            
            switch (unitType)
            {
                case UnitType.Player:
                {
                    Unit unit = unitComponent.AddChildWithId<Unit, int>(id, 1001);
                    unit.AddComponent<MoveComponent>();
                    unit.Position = new float3(-10, 0, -10);
			
                    NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
                    numericComponent.Set(NumericType.SpeedBase, 6f); // 速度是6米每秒
                    numericComponent.Set(NumericType.AOIBase, 2); // 视野2格
                    numericComponent.Set(NumericType.HpBase, 1000); // 生命1000
                    numericComponent.Set(NumericType.MaxHpBase, 1000); // 最大生命1000
                    numericComponent.Set(NumericType.LvBase,1); //1级
                    numericComponent.Set(NumericType.ATKBase,100); //100攻击
                    numericComponent.Set(NumericType.DEFBase,500); //500防御
                    
                    unitComponent.Add(unit);
                    return unit;
                }
                default:
                    throw new Exception($"not such unit type: {unitType}");
            }
        }
    }
}