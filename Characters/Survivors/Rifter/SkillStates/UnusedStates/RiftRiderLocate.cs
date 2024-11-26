using EntityStates;
using IL.RoR2.Skills;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Characters.Survivors.Rifter.SkillStates.UnusedStates
{
    public class RiftRiderLocate : BaseState
    {
        //This is deprecated. Still exists solely for EntanglementLocate to work correctly
        public static GameObject teleportLocatorPrefab = EntityStates.Huntress.ArrowRain.areaIndicatorPrefab;
        public static GameObject teleportLocatorInstance;

        public static float duration = 0.9f;

        public static float initialSpeedCoefficient = 1f;

        public static float finalSpeedCoefficient = 1f;


        public Vector3 forwardDirection;

        public static float dodgeFOV = 2f;

        private float stopwatch;

        public override void OnEnter()
        {
            base.OnEnter();
            if ((bool)teleportLocatorPrefab)
            {
                teleportLocatorInstance = Object.Instantiate(teleportLocatorPrefab);
                teleportLocatorInstance.transform.localScale = new Vector3(7f, 7f, 7f);
            }
            characterMotor.velocity.y = Mathf.Max(characterMotor.velocity.y, 0f);
            if (isAuthority && (bool)inputBank)
            {
                forwardDirection = -Vector3.ProjectOnPlane(inputBank.aimDirection, Vector3.up);
            }
            characterDirection.moveVector = -forwardDirection;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if ((bool)cameraTargetParams)
            {
                cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, stopwatch / duration);
            }
            if ((bool)characterMotor && (bool)characterDirection)
            {
                Vector3 velocity = characterMotor.velocity;
                Vector3 velocity2 = forwardDirection * (moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, stopwatch / duration));
                characterMotor.velocity = velocity2;
                characterMotor.velocity.y = velocity.y;
                characterMotor.moveDirection = forwardDirection;
            }
            if (stopwatch >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
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
                if (Physics.Raycast(GetAimRay(), out var hitInfo, maxDistance, LayerIndex.world.mask))
                {
                    teleportLocatorInstance.transform.position = hitInfo.point;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if ((bool)cameraTargetParams)
            {
                cameraTargetParams.fovOverride = -1f;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }

}


