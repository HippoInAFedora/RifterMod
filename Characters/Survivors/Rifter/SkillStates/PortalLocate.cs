using EntityStates;
using RifterMod.Characters.Survivors.Rifter.Components;
using RifterMod.Characters.Survivors.Rifter.SkillStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    internal class PortalLocate : BaseSkillState
    {
        private float duration = .25f;

        public Vector3 position;
        public Quaternion rotation;

        private HurtBoxGroup hurtboxGroup;
        private Transform modelTransform;


        public static GameObject teleportLocatorPrefab = EntityStates.Huntress.ArrowRain.areaIndicatorPrefab;
        public static GameObject teleportLocatorInstance;
        public static GameObject teleportLocatorInstance2;


        public static float stopwatch;

        public override void OnEnter()
        {
            base.OnEnter();
            position = base.transform.position + Vector3.up * 2f;
            Ray aimRay = base.GetAimRay();
            modelTransform = GetModelTransform();
            if ((bool)modelTransform)
            {
                hurtboxGroup = modelTransform.GetComponent<HurtBoxGroup>();
            }
            if ((bool)hurtboxGroup)
            {
                HurtBoxGroup hurtBoxGroup = hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            }

            if (base.isAuthority)
            {
                base.characterMotor.velocity = -60f * aimRay.direction;               
            }

            if (teleportLocatorPrefab)
            {
                teleportLocatorInstance = Object.Instantiate(teleportLocatorPrefab, position, base.transform.rotation);
                teleportLocatorInstance.transform.localScale = Vector3.one * 5f;

                teleportLocatorInstance2 = Object.Instantiate(teleportLocatorPrefab, position, base.transform.rotation);
                teleportLocatorInstance2.transform.localScale = Vector3.one * 5f;
            }

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (base.isAuthority && base.fixedAge > duration)
            {
                if ((bool)characterMotor)
                {
                    characterMotor.velocity = Vector3.zero;
                }
                if ((bool)rigidbodyMotor)
                {
                    rigidbodyMotor.moveVector = Vector3.zero;
                }
                if (!base.inputBank.skill3.down && base.isAuthority)
                {
                    outer.SetNextState(new PortalDrop
                    {
                        portalMainPosition = position,
                    });
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
            if ((bool)teleportLocatorInstance2)
            {
                float maxDistance = RifterStaticValues.riftSpecialDistance;
                teleportLocatorInstance2.transform.position = GetAimRay().GetPoint(maxDistance);
                if (Physics.Raycast(GetAimRay(), out var hitInfo, maxDistance, LayerIndex.world.mask))
                {
                    teleportLocatorInstance2.transform.position = hitInfo.point;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (teleportLocatorInstance)
            {
                Destroy(teleportLocatorInstance);
            }
            if (teleportLocatorInstance2)
            {
                Destroy(teleportLocatorInstance2);
            }
            if ((bool)hurtboxGroup)
            {
                HurtBoxGroup hurtBoxGroup = hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
