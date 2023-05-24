namespace ET.Client
{
	[Event(SceneType.Client)]
	public class LoginFinish_RemoveLoginUI: AEvent<Scene, EventType.LoginFinish>
	{
		protected override async ETTask Run(Scene scene, EventType.LoginFinish args)
		{
			scene.GetComponent<UIComponent>().HideWindow<DlgLogin>();
			await ETTask.CompletedTask;
		}
	}
}
