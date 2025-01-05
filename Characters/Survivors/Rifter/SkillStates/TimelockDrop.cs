using EntityStates;
using RifterMod.Characters.Survivors.Rifter.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    internal class TimelockDrop : BaseSkillState
    {
        public float radius = 10f;
        private float duration = .5f;

        public Vector3 position;
        public Quaternion rotation;

        public GameObject timelockPrefab = RifterAssets.timelockMain;
        private GameObject timelockInstance;

        private BuffWard buffWard;
        private TeamFilter teamFilter;
        private EntityLocator entityLocator;

        private int specialStateHash = Animator.StringToHash("Special End");
        private int specialPlaybackHash = Animator.StringToHash("Special.playbackRate");

        private int inTimelock = Animator.StringToHash("inTimelock");

        public Animator modelAnimator;


        public override void OnEnter()
        {
            base.OnEnter();
            float maxDistance = RifterStaticValues.riftSpecialDistance;
            modelAnimator = GetModelAnimator();
            if (modelAnimator)
            {
                modelAnimator.SetBool(inTimelock, false);
                PlayCrossfade("Gesture, Override", specialStateHash, specialPlaybackHash, .1f, .1f);
            }

            position = GetAimRay().GetPoint(maxDistance);
            if (Physics.Raycast(GetAimRay(), out var hitInfo, maxDistance, LayerIndex.CommonMasks.bullet))
            {
                position = hitInfo.point;
            }
            if (NetworkServer.active)
            {
                timelockInstance = Object.Instantiate(timelockPrefab, position, base.transform.rotation);

                DestroyOnRift destroyOnRift = timelockInstance.GetComponent<DestroyOnRift>();
                destroyOnRift.owner = base.gameObject;

                teamFilter = timelockInstance.GetComponent<TeamFilter>();
                teamFilter.teamIndex = base.teamComponent.teamIndex;
                entityLocator = timelockInstance.GetComponent<EntityLocator>();
                buffWard = timelockInstance.GetComponent<BuffWard>();
                if (buffWard)
                {
                    buffWard.buffDef = RifterBuffs.superShatterDebuff;
                    buffWard.buffDuration = 1f;
                    buffWard.Networkradius = radius;
                }

                Deployable component = timelockInstance.GetComponent<Deployable>();
                if ((bool)component && (bool)base.characterBody.master)
                {
                    base.characterBody.master.AddDeployable(component, RifterSurvivor.timelockSlot);
                }
                NetworkServer.Spawn(timelockInstance);               
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge > duration && base.isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

    }
}
