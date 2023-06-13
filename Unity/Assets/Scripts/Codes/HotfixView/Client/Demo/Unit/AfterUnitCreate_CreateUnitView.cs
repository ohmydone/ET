using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AfterUnitCreate_CreateUnitView: AEvent<Scene, EventType.AfterUnitCreate>
    {
        protected override async ETTask Run(Scene scene, EventType.AfterUnitCreate args)
        {
            Unit unit = args.Unit;
            // Unit View层
            // 这里可以改成异步加载，demo就不搞了
            switch (unit.Type)
            {
                case UnitType.Monster:
                case UnitType.Player:
                {
                    GameObject bundleGameObject = await ResComponent.Instance.LoadAssetAsync<GameObject>(ResPathHelper.GetUnitPath("Unit"));
                    GameObject prefab = bundleGameObject.Get<GameObject>("Skeleton");
                    GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
                    go.transform.position = unit.Position;
                    unit.AddComponent<GameObjectComponent>().GameObject = go;
                    unit.AddComponent<AnimationComponent,GameObject>(go);
                    unit.GetComponent<AnimationComponent>().Play(AnimClipType.Idle);
                    unit.AddComponent<CameraComponent>().Unit=unit;
                    
                    // var SkillIds = new List<int>(){1001,1002,1003,1004,1005,1006,1007};//初始技能
                    // CombatUnitComponent combatU = unit.AddComponent<CombatUnitComponent,List<int>>(SkillIds);
                    
                    break;
                }
                case UnitType.Skill:
                    
                    break;
            }

            

            await ETTask.CompletedTask;
        }
    }
}