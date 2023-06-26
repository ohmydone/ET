using System;

namespace ET.Client
{
    [FriendOf(typeof(GameObjectComponent))]
    public static class GameObjectComponentSystem
    {
        [ObjectSystem]
        public class DestroySystem: DestroySystem<GameObjectComponent>
        {
            protected override void Destroy(GameObjectComponent self)
            {
                UnityEngine.Object.Destroy(self.GameObject);
            }
        }
        
        
        // /// <summary>
        // /// 获取ReferenceCollector里的物体
        // /// </summary>
        // /// <param name="self"></param>
        // /// <param name="name"></param>
        // /// <typeparam name="T"></typeparam>
        // /// <returns></returns>
        // public static T GetCollectorObj<T>(this GameObjectComponent self,string name)where T:UnityEngine.Object
        // {
        //     if (self.Collector == null)
        //     {
        //         self.Collector = self.GameObject.GetComponent<ReferenceCollector>();
        //     }
        //
        //     return self.Collector?.Get<T>(name);
        // }
    }
}