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
        public static float duration = 0.18f;
        private float speed = 12f;

        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;


        private Vector3 forwardDirection;
        private bool startedStateGrounded;

        private CharacterModel characterModel;
        private HurtBoxGroup hurtboxGroup;
        private Transform modelTransform;

        private GameObject slipstreamIn = RifterAssets.slipstreamInEffect;
        private GameObject slipstreamOut = RifterAssets.slipstreamOutEffect;

        public Animator modelAnimator;

        private int utilityStateHash = Animator.StringToHash("Utility");
        private int utilityPlaybackHash = Animator.StringToHash("Utility.playbackRate");

        private int utilityFullBodyStateHash = Animator.StringToHash("FullBody Descend");

        private float stopwatch;


        public override void OnEnter()
        {
            base.OnEnter();
            modelTransform = GetModelTransform();
            modelAnimator = GetModelAnimator();
            if (modelAnimator)
            {
                PlayCrossfade("FullBody, Override", utilityFullBodyStateHash, utilityPlaybackHash, .3f, .25f);
                PlayCrossfade("Gesture, Override", utilityStateHash, utilityPlaybackHash, .3f, .25f);
            }
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
            if (NetworkServer.active)
            {
                Util.CleanseBody(base.characterBody, removeDebuffs: true, removeBuffs: false, removeCooldownBuffs: false, removeDots: true, removeStun: true, removeNearbyProjectiles: false);
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
                inEffect.scale = 2.5f;
                EffectManager.SpawnEffect(slipstreamIn, inEffect, true);

            }


        }

        public override void FixedUpdate()
        {
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch < duration && (bool)characterMotor && (bool)characterDirection)
            {
                base.characterBody.isSprinting = true;
                Vector3 num = Vector3.zero;
                //num = (!startedStateGrounded) ? forwardDirection + new Vector3(0, .5f, 0) : forwardDirection;
                num = forwardDirection;
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
            if (!outer.destroying)
            {
                modelTransform = GetModelTransform();
                if ((bool)modelTransform)
                {
                    TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(modelTransform.gameObject);
                    temporaryOverlay.duration = 0.6f;
                    temporaryOverlay.animateShaderAlpha = true;
                    temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    temporaryOverlay.destroyComponentOnEnd = true;
                    temporaryOverlay.originalMaterial = RifterAssets.matTeleport;
                    temporaryOverlay.AddToCharacterModel(modelTransform.GetComponent<CharacterModel>());
                }
            }
            EffectData outEffect = new EffectData();
            outEffect.scale = 2.5f;
            outEffect.origin = base.characterBody.corePosition;
            EffectManager.SpawnEffect(slipstreamOut, outEffect, true);

            if (base.skillLocator.secondary.stock < base.skillLocator.secondary.maxStock)
            {
                base.skillLocator.secondary.AddOneStock();
            }

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }

}


