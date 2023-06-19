using ET.EventType;

namespace ET.Client
{
    [Event(SceneType.Map)]
    public class OnSelectTargetHandler: AEvent<Scene,ET.EventType.OnSelectTarget>
    {
        protected override async ETTask Run(Scene scene, OnSelectTarget a)
        {
            
        }
    }
}

