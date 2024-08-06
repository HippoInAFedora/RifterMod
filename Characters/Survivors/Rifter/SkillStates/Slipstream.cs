using EntityStates;
using RoR2.Skills;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Characters.Survivors.Rifter.SkillStates
{
    public class Slipstream : BaseSkillState
    {
        public static float duration = 0.15f;
        private float speed = 12f;

        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;

        private Vector3 finalPosition;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 startPosition;
        private bool startedStateGrounded;

        private CharacterModel characterModel;
        private HurtBoxGroup hurtboxGroup;
        private Transform modelTransform;

        private GameObject slipstreamIn = RifterAssets.slipstreamInEffect;
        private GameObject slipstreamOut = RifterAssets.slipstreamOutEffect;

        private float stopwatch;


        public override void OnEnter()
        {
            base.OnEnter();
            animator = GetModelAnimator();
            modelTransform = GetModelTransform();
            if ((bool)modelTransform)
            {
                characterModel = modelTransform.GetComponent<CharacterModel>();
                hurtboxGroup = modelTransform.GetComponent<HurtBoxGroup>();
            }
            if ((bool)characterModel)
            {
                characterModel.invisibilityCount++;
            }
            if ((bool)hurtboxGroup)
            {
                HurtBoxGroup hurtBoxGroup = hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            }
            if (isAuthority)
            {
                if ((bool)characterMotor)
                {
                    forwardDirection = ((inputBank.moveVector == Vector3.zero) ? characterDirection.forward : inputBank.moveVector).normalized;
                    startedStateGrounded = base.characterMotor.isGrounded;
                }

                EffectData inEffect = new EffectData();
                inEffect.origin = base.characterBody.corePosition;
                inEffect.scale = 3f;
                EffectManager.SpawnEffect(slipstreamIn, inEffect, true);

                finalPosition = transform.position + forwardDirection * speed;
                startPosition = transform.position;
            }


        }

        public override void FixedUpdate()
        {
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch < duration && (bool)characterMotor && (bool)characterDirection)
            {
                Vector3 num = Vector3.zero;
                num = (!startedStateGrounded) ? forwardDirection + new Vector3(0, .5f, 0) : forwardDirection;
                characterMotor.velocity = Vector3.zero;
                characterMotor.rootMotion += num * (speed / duration * Time.fixedDeltaTime);
            }
            if (stopwatch > duration && isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            if (cameraTargetParams) cameraTargetParams.fovOverride = -1f;
            if ((bool)characterModel)
            {
                characterModel.invisibilityCount--;
            }
            if ((bool)hurtboxGroup)
            {
                HurtBoxGroup hurtBoxGroup = hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            }
            if ((bool)characterMotor)
            {
                characterMotor.disableAirControlUntilCollision = false;
            }

            EffectData outEffect = new EffectData();
            outEffect.scale = 3f;
            outEffect.origin = base.characterBody.corePosition;
            EffectManager.SpawnEffect(slipstreamOut, outEffect, true);

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }

}


