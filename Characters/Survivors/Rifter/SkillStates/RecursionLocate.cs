using EntityStates;
using IL.RoR2.Skills;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RecursionLocate : BaseState
    {
        public static GameObject teleportLocatorPrefab = global::EntityStates.Huntress.ArrowRain.areaIndicatorPrefab;
        public static GameObject teleportLocatorInstance;

        public override void OnEnter()
        {
            base.OnEnter();
            if ((bool)teleportLocatorPrefab)
            {
                teleportLocatorInstance = Object.Instantiate(teleportLocatorPrefab);
                teleportLocatorInstance.transform.localScale = new Vector3(12, 12, 12);
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority && (bool)inputBank)
            {
                if (inputBank.skill4.justReleased)
                {
                    outer.SetNextState(new Recursion{
                        blastNum = 0,
                        basePosition = teleportLocatorInstance.transform.position
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
            if ((bool)teleportLocatorInstance)
            {
                float maxDistance = 500f;
                teleportLocatorInstance.transform.position = GetAimRay().GetPoint(maxDistance);
                if (Physics.Raycast(GetAimRay(), out var hitInfo, maxDistance, LayerIndex.world.mask))
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
            base.OnExit();
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }

}


