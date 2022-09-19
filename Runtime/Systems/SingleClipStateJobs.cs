﻿using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Entities;

namespace DMotion
{
    [BurstCompile]
    internal partial struct UpdateSingleClipStatesJob
    {
        internal float DeltaTime;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Execute(
            ref DynamicBuffer<ClipSampler> clipSamplers,
            in DynamicBuffer<SingleClipState> singleClipStates,
            in DynamicBuffer<PlayableState> playableStates
        )
        {
            for (var i = 0; i < singleClipStates.Length; i++)
            {
                if (playableStates.TryGetWithId(singleClipStates[i].PlayableId, out var playable))
                {
                    SingleClipStateUtils
                        .UpdateSamplers(singleClipStates[i], DeltaTime, playable, ref clipSamplers);
                }
            }
        }
    }
    
    [BurstCompile]
    internal partial struct CleanSingleClipStatesJob
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Execute(
            ref DynamicBuffer<SingleClipState> singleClipStates,
            in DynamicBuffer<PlayableState> playableStates
        )
        {
            for (int i = singleClipStates.Length - 1; i >= 0; i--)
            {
                if (!playableStates.TryGetWithId(singleClipStates[i].PlayableId, out _))
                {
                    singleClipStates.RemoveAtSwapBack(i);
                }
            }
        }
    }
}