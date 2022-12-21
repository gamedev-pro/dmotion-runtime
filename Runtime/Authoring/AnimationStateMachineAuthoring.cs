using System.Linq;
using Latios.Authoring;
using Latios.Kinemation;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Assertions;

namespace DMotion.Authoring
{
    public static class StateMachineEditorConstants
    {
        public const string DMotionPath = "DMotion";
    }

    [DisallowMultipleComponent]
    public class AnimationStateMachineAuthoring : MonoBehaviour
    {
        public GameObject Owner;
        public Animator Animator;
        public StateMachineAsset StateMachineAsset;

        public RootMotionMode RootMotionMode;
        public bool EnableEvents = true;
        private void Reset()
        {
            if (Animator == null)
            {
                Animator = GetComponent<Animator>();
            }

            if (Animator != null && Owner == null)
            {
                Owner = Animator.gameObject;
            }
        }
    }

    struct AnimationStateMachineBakeItem : ISmartBakeItem<AnimationStateMachineAuthoring>
    {
        private AnimationStateMachineAuthoring Authoring;
        private Entity Owner;
        private SmartBlobberHandle<SkeletonClipSetBlob> clipsBlobHandle;
        private SmartBlobberHandle<StateMachineBlob> stateMachineBlobHandle;
        private SmartBlobberHandle<ClipEventsBlob> clipEventsBlobHandle;

        public bool Bake(AnimationStateMachineAuthoring authoring, IBaker baker)
        {
            ValidateStateMachine(authoring);
            Authoring = authoring;
            Owner = baker.GetEntity(authoring.Owner);
            clipsBlobHandle = baker.RequestCreateBlobAsset(authoring.Animator, authoring.StateMachineAsset.Clips);
            stateMachineBlobHandle = baker.RequestCreateBlobAsset(authoring.StateMachineAsset);
            clipEventsBlobHandle = baker.RequestCreateBlobAsset(authoring.StateMachineAsset.Clips);
            return true;
        }

        public void PostProcessBlobRequests(EntityManager dstManager, Entity entity)
        {
            var stateMachineBlob = stateMachineBlobHandle.Resolve(dstManager);
            var clipsBlob = clipsBlobHandle.Resolve(dstManager);
            var clipEventsBlob = clipEventsBlobHandle.Resolve(dstManager);

            AnimationStateMachineConversionUtils.AddStateMachineSystemComponents(dstManager, entity,
                Authoring.StateMachineAsset,
                stateMachineBlob,
                clipsBlob,
                clipEventsBlob);
            AnimationStateMachineConversionUtils.AddAnimationStateSystemComponents(dstManager, entity);

            if (Authoring.EnableEvents && Authoring.StateMachineAsset.Clips.Any(c => c.Events.Length > 0))
            {
                dstManager.GetOrCreateBuffer<RaisedAnimationEvent>(entity);
            }

            if (Owner == Entity.Null)
            {
                Owner = entity;
            }

            if (Owner != entity)
            {
                AnimationStateMachineConversionUtils.AddAnimatorOwnerComponents(dstManager, Owner, entity);
            }

            AnimationStateMachineConversionUtils.AddRootMotionComponents(dstManager, Owner, entity,
                Authoring.RootMotionMode);
        }

        private void ValidateStateMachine(AnimationStateMachineAuthoring authoring)
        {
            if (authoring.StateMachineAsset != null)
            {
                foreach (var s in authoring.StateMachineAsset.States)
                {
                    foreach (var c in s.Clips)
                    {
                        Assert.IsTrue(c != null && c.Clip != null,
                            $"State ({s.name}) in State Machine {authoring.StateMachineAsset.name} has invalid clips");
                    }
                }
            }
        }
    }
}