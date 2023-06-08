namespace ET.Client
{
	[Event(SceneType.Client)]
	public class LoginFinish_CreateLobbyUI: AEvent<Scene, EventType.LoginFinish>
	{
		protected override async ETTask Run(Scene scene, EventType.LoginFinish args)
		{
			await UIComponent.Instance.ShowWindowAsync<DlgLobby>(); 
		}
	}
}
