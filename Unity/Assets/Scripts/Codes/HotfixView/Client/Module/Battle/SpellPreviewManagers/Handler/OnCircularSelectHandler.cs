using ET.EventType;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class OnCircularSelectHandler: AEvent<Scene,ET.EventType.OnCircularSelect>
    {
        protected override async ETTask Run(Scene scene, OnCircularSelect a)
        {
            SpellPreviewComponent com = UnitComponent.Instance.My.GetComponent<CombatUnitComponent>().GetComponent<SpellPreviewComponent>();
            com.OnInputPoint(a.pos);
        }
    }
}

