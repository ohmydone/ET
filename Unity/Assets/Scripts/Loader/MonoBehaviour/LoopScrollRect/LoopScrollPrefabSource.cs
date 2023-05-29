using System;
using UnityEngine;
using System.Collections;
using ET;

namespace UnityEngine.UI
{
    public interface LoopScrollPrefabSource
    {
        GameObject GetObject(int index);

        void ReturnObject(Transform trans,bool isDestroy = false);
    }
    
    
    
    [System.Serializable]
    public class LoopScrollPrefabSourceInstance : LoopScrollPrefabSource
    {
        private string path = "Assets/Bundles/UI/Item/";
        
        public string prefabName;
        public int poolSize = 5;

        private bool inited = false;
        public virtual GameObject GetObject(int index)
        {
            try
            {
                if(!inited)
                {
                    GameObjectPoolHelper.InitPool(path+ prefabName, poolSize);
                    inited = true;
                }
                return GameObjectPoolHelper.GetObjectFromPool(path+ prefabName);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }
        
        public virtual void ReturnObject(Transform go , bool isDestroy = false)
        {
            try
            {
                if (isDestroy)
                {
                    UnityEngine.GameObject.Destroy(go.gameObject);
                }
                else
                {
                    GameObjectPoolHelper.ReturnObjectToPool(go.gameObject);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
    
}
