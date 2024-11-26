using EntityStates;
using RoR2.Skills;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Characters.Survivors.Rifter.SkillStates
{
    public class SlipstreamIn : BaseSkillState
    {
        public static float duration = 0.3f;

        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;

        public Animator modelAnimator;

        private int utilityStateHash = Animator.StringToHash("Utility");
        private int utilityPlaybackHash = Animator.StringToHash("Utility.playbackRate");


        public override void OnEnter()
        {
            base.OnEnter();
            modelAnimator = GetModelAnimator();
            if (modelAnimator)
            {
                PlayCrossfade("Gesture, Override", utilityStateHash, utilityPlaybackHash, .3f, .25f);
            }
            //if (base.isAuthority)
            //{
            //    SmallHop(base.characterMotor, 1f);
            //}


        }

        public override void FixedUpdate()
        {
            if (base.fixedAge > duration && base.isAuthority)
            {
                outer.SetNextState(new Slipstream());
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


