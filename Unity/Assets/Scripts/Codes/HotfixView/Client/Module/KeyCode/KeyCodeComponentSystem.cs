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
            self.Reset(self.KeyMap);
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
        
        public static Dictionary<int, int> GetKeys(this KeyCodeComponent self)
        {
            return self.DefaultKeyCodeMap;
        }
        
        public static void Reset(this KeyCodeComponent self,Dictionary<int, int> old = null)
        {
            self.KeyMap = new Dictionary<int, int>();
            foreach (var item in self.DefaultKeyCodeMap)
            {
                var key = item.Key;
                if (old != null && old.TryGetValue(key, out var val))
                {
                    self.KeyMap[key] = val;
                }
                else
                {
                    self.KeyMap[key] = item.Value;
                }
            }
        }
    }
}