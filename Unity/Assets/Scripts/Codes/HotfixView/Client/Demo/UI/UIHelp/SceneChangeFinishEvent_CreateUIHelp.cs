namespace ET.Client
{
    [Event(SceneType.Current)]
    public class SceneChangeFinishEvent_CreateUIHelp : AEvent<Scene, EventType.SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, EventType.SceneChangeFinish args)
        {
            await UIComponent.Instance.ShowWindowAsync<DlgHelp>();
            await UIComponent.Instance.ShowWindowAsync<DlgSkill>();
        }
    }
}
