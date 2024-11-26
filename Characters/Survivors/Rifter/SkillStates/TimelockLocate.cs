using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    internal class TimelockLocate : BaseSkillState
    {

        public static GameObject teleportLocatorPrefab = global::EntityStates.Huntress.ArrowRain.areaIndicatorPrefab;
        public static GameObject teleportLocatorInstance;

        public Animator modelAnimator;

        private int specialStateHash = Animator.StringToHash("Special Start");
        private int specialPlaybackHash = Animator.StringToHash("Special.playbackRate");

        private int inTimelock = Animator.StringToHash("inTimelock");



        public override void OnEnter()
        {
            base.OnEnter();
            modelAnimator = GetModelAnimator();
            
            if (modelAnimator)
            {
                modelAnimator.SetBool(inTimelock, true);
                PlayCrossfade("Gesture, Override", specialStateHash, specialPlaybackHash, .1f, .1f);
            }
            if ((bool)teleportLocatorPrefab)
            {
                teleportLocatorInstance = Object.Instantiate(teleportLocatorPrefab);
                teleportLocatorInstance.transform.localScale = new Vector3(10f, 10f, 10f);
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority && (bool)inputBank)
            {
                if (inputBank.skill4.justReleased)
                {
                    outer.SetNextState(new TimelockDrop());
                }
            }

        }

        public override void Update()
        {
            base.Update();
            UpdateAreaIndicator();
        }

        private void UpdateAreaIndicator()
        {
            if ((bool)teleportLocatorInstance)
            {
                float maxDistance = RifterStaticValues.riftSpecialDistance;
                teleportLocatorInstance.transform.position = GetAimRay().GetPoint(maxDistance);
                if (Physics.Raycast(GetAimRay(), out var hitInfo, maxDistance, LayerIndex.CommonMasks.bullet))
                {
                    teleportLocatorInstance.transform.position = hitInfo.point;
                }
            }
        }

        public override void OnExit()
        {
            if ((bool)teleportLocatorInstance)
            {
                Destroy(teleportLocatorInstance);
            }
            if (modelAnimator)
            {
                modelAnimator.SetBool(inTimelock, false);
            }
            
            base.OnExit();
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

    }
}
