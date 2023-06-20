using ET.EventType;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class OnPointSelectHandler: AEvent<Scene,ET.EventType.OnPointSelect>
    {
        protected override async ETTask Run(Scene scene, OnPointSelect a)
        {
            SpellPreviewComponent com = UnitComponent.Instance.My.GetComponent<CombatUnitComponent>().GetComponent<SpellPreviewComponent>();
            com.OnInputPoint(a.pos);
        }
    }
}

