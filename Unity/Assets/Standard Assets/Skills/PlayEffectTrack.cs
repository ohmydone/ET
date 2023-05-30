using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ET
{
    [TrackClipType(typeof(PlayEffectClip))]
    public class PlayEffectTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return base.CreateTrackMixer(graph, go, inputCount);
        }
    }
}
