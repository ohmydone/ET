using ET.EventType;

namespace ET.Client
{
    [Event(SceneType.Map)]
    public class OnDirectRectSelectHandler: AEvent<Scene,ET.EventType.OnDirectRectSelect>
    {
        protected override async ETTask Run(Scene scene, OnDirectRectSelect a)
        {
            SpellPreviewComponent com = UnitComponent.Instance.My.GetComponent<CombatUnitComponent>().GetComponent<SpellPreviewComponent>();
            com.OnInputDirect(a.pos);
        }
    }
}

