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
            if (!outer.destroying)
            {
                modelTransform = GetModelTransform();
                if ((bool)modelTransform)
                {
                    TemporaryOverlay temporaryOverlay = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                    temporaryOverlay.duration = 0.6f;
                    temporaryOverlay.animateShaderAlpha = true;
                    temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    temporaryOverlay.destroyComponentOnEnd = true;
                    temporaryOverlay.originalMaterial = RifterAssets.matTeleport;
                    temporaryOverlay.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
                    //TemporaryOverlay temporaryOverlay2 = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                    //temporaryOverlay2.duration = 0.7f;
                    //temporaryOverlay2.animateShaderAlpha = true;
                    //temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    //temporaryOverlay2.destroyComponentOnEnd = true;
                    //temporaryOverlay2.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
                    //temporaryOverlay2.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
                    //}
                }
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


