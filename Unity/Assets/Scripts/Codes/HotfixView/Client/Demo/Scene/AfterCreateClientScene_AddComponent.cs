namespace ET.Client
{
    [Event(SceneType.Client)]
    public class AfterCreateClientScene_AddComponent: AEvent<Scene, EventType.AfterCreateClientScene>
    {
        protected override async ETTask Run(Scene scene, EventType.AfterCreateClientScene args)
        {
            scene.AddComponent<UIPathComponent>();
            scene.AddComponent<UIEventComponent>();
            scene.AddComponent<RedDotComponent>();
            
            //输入订阅组件
            scene.AddComponent<InputComponent>();
            scene.AddComponent<InputWatcherComponent>();
            
            scene.AddComponent<UIComponent>();
            
            //摄像机与场景管理
            scene.AddComponent<CameraManagerComponent>();
            
            scene.AddComponent<ResourcesLoaderComponent>();
            await ETTask.CompletedTask;
        }
    }
}