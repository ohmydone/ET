using ET.EventType;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class OnSelectTargetHandler: AEvent<Scene,ET.EventType.OnSelectTarget>
    {
        protected override async ETTask Run(Scene scene, OnSelectTarget a)
        {
            SpellPreviewComponent com = UnitComponent.Instance.My.GetComponent<CombatUnitComponent>().GetComponent<SpellPreviewComponent>();
            com.OnSelectedTarget(a.Unit);
        }
    }
}

