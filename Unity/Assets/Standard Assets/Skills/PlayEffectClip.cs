using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ET
{
    [Serializable]
    public class PlayEffectClip: PlayableAsset, ITimelineClipAsset
    {
        /// <summary>
        /// GameObject in the scene to control, or the parent of the instantiated prefab.
        /// </summary>
        [SerializeField] public ExposedReference<GameObject> sourceGameObject;
        
        /// <summary>
        /// Prefab object that will be instantiated.
        /// </summary>
        [SerializeField]
        public GameObject prefabGameObject;

        private static HashSet<GameObject> s_CreatedPrefabs = new HashSet<GameObject>();

        /// <summary>
        /// Creates the root of a Playable subgraph to control the contents of the game object.
        /// </summary>
        /// <param name="graph">PlayableGraph that will own the playable</param>
        /// <param name="go">The GameObject that triggered the graph build</param>
        /// <returns>The root playable of the subgraph</returns>
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {

            
            
            // case 989856
            if (prefabGameObject != null)
            {
                if (s_CreatedPrefabs.Contains(prefabGameObject))
                {
                    Debug.LogWarningFormat(
                        "Control Track Clip ({0}) is causing a prefab to instantiate itself recursively. Aborting further instances.", name);
                    return Playable.Create(graph);
                }

                s_CreatedPrefabs.Add(prefabGameObject);
            }

            Playable root = Playable.Null;
            var playables = new List<Playable>();

            GameObject sourceObject = sourceGameObject.Resolve(graph.GetResolver());
            if (prefabGameObject != null)
            {
                var playable = PlayEffectBehaviour.Create(graph, prefabGameObject, sourceObject.transform);
                sourceObject = playable.GetBehaviour().prefabInstance;
                playables.Add(playable);
            }
            
            if (prefabGameObject != null)
                s_CreatedPrefabs.Remove(prefabGameObject);

            if (!root.IsValid())
                root = Playable.Create(graph);

            return root;
        }

        public ClipCaps clipCaps { get; }
    }
}