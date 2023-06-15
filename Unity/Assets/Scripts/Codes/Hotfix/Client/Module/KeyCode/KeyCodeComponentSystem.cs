using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class KeyCodeComponentAwakeSystem : AwakeSystem<KeyCodeComponent>
    {
        protected override void Awake(KeyCodeComponent self)
        {
// #if !NOT_UNITY
//             var jstr = PlayerPrefs.GetString(CacheKeys.KeyCodeSetting);
//             if (!string.IsNullOrEmpty(jstr))
//             {
//                 try
//                 {
//                     self.KeyMap = JsonHelper.FromJson<Dictionary<int, int>>(jstr);
//                 }
//                 catch (Exception e)
//                 {
//                     Log.Error(e);
//                 }
//             }
// #endif
            
            KeyCodeComponent.Instance = self;
        }
    }
    [ObjectSystem]
    public class KeyCodeComponentDestroySystem : DestroySystem<KeyCodeComponent>
    {
        protected override void Destroy(KeyCodeComponent self)
        {
            KeyCodeComponent.Instance = null;
        }
    }
    [FriendOf(typeof(KeyCodeComponent))]
    public static class KeyCodeComponentSystem 
    {
        public static void Save(this KeyCodeComponent self)
        {
#if !NOT_UNITY
            //PlayerPrefs.SetString(CacheKeys.KeyCodeSetting, JsonHelper.ToJson(self.KeyMap));
#endif
        }
        
        
    }
}