using EntityStates;
using IL.RoR2.Skills;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class Slipstream : BaseSkillState
    {
        public static float duration = 0.3f;

        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = global::EntityStates.Commando.DodgeState.dodgeFOV;

        private Vector3 finalPosition;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 startPosition;

        private CharacterModel characterModel;
        private HurtBoxGroup hurtboxGroup;
        private Transform modelTransform;

        private float stopwatch;

        public override void OnEnter()
        {
            base.OnEnter();
            animator = GetModelAnimator();
            modelTransform = GetModelTransform();
            base.characterBody.AddBuff(Rifter.RifterBuffs.riftTeleportableBuff);
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
            if (base.isAuthority)
            {
                if ((bool)base.characterMotor)
                {
                    forwardDirection = inputBank.aimDirection.normalized;
                }
                finalPosition = base.transform.position + (forwardDirection * RifterStaticValues.riftSecondaryDistance);
                startPosition = base.transform.position;
            }

        }

        public override void FixedUpdate()
        {
            stopwatch += Time.fixedDeltaTime;
            if ((bool)base.characterMotor && (bool)base.characterDirection)
            {
                base.characterMotor.velocity = Vector3.zero;
                base.characterMotor.rootMotion += forwardDirection * (RifterStaticValues.riftSecondaryDistance/duration * Time.fixedDeltaTime);
            }
            if (stopwatch >= duration && base.isAuthority)
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
            if ((bool)base.characterMotor)
            {
                base.characterMotor.disableAirControlUntilCollision = false;
            }
            base.OnExit();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            forwardDirection = reader.ReadVector3();
        }
    }

}
    

