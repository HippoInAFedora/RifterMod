using RoR2;
using EntityStates;
using RifterMod.Survivors.Rifter.SkillStates;
using RifterMod.Characters.Survivors.Rifter.Components;
using UnityEngine;
using RifterMod.Survivors.Rifter;

namespace RifterMod.Characters.Survivors.Rifter.SkillStates
{
    internal class PortalAuxLocate : BaseSkillState
    {
        public Vector3 portalMainPosition;

        public static GameObject teleportLocatorPrefab = EntityStates.Huntress.ArrowRain.areaIndicatorPrefab;
        public static GameObject teleportLocatorInstance;
        public GameObject firstInstance;

        private CharacterModel characterModel;
        private HurtBoxGroup hurtboxGroup;
        private Transform modelTransform;

        private float duration = 5f;

        private float stopwatch;
        public override void OnEnter()
        {
            base.OnEnter();
            modelTransform = GetModelTransform();
            if ((bool)modelTransform)
            {
                characterModel = modelTransform.GetComponent<CharacterModel>();
                hurtboxGroup = modelTransform.GetComponent<HurtBoxGroup>();
            }
            if ((bool)hurtboxGroup)
            {
                HurtBoxGroup hurtBoxGroup = hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            }
            if ((bool)teleportLocatorPrefab)
            {
                teleportLocatorInstance = Object.Instantiate(teleportLocatorPrefab);
                teleportLocatorInstance.transform.localScale = new Vector3(5f, 5f, 5f);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if ((bool)characterMotor)
            {
                characterMotor.velocity = Vector3.zero;
            }
            if ((bool)rigidbodyMotor)
            {
                rigidbodyMotor.moveVector = Vector3.zero;
            }
            if (!base.inputBank.skill3.down)
            {
                outer.SetNextState(new PortalAuxDrop
                {
                    portalMainPosition = portalMainPosition,
                });
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
            if (teleportLocatorInstance)
            {
                Destroy(teleportLocatorInstance);
            }
            if (firstInstance)
            {
                Destroy(firstInstance);
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
            return InterruptPriority.PrioritySkill;
        }
    }
}


