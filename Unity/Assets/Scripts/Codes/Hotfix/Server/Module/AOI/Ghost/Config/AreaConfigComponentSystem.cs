using System;

namespace ET
{
    [FriendOf(typeof(AreaConfigComponent))]
    public static class AreaConfigComponentSystem
    {
        public class AwakeSystem: AwakeSystem<AreaConfigComponent>
        {
            protected override void Awake(AreaConfigComponent self)
            {
                AreaConfigComponent.Instance = self;
            }
        }
        
    }
}