using EntityStates;
using RifterMod.Characters.Survivors.Rifter.Components;
using RifterMod.Characters.Survivors.Rifter.SkillStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    internal class PortalMainDrop : BaseSkillState
    {
        private float duration = .25f;

        public Vector3 position;
        public Quaternion rotation;


        public static GameObject teleportLocatorPrefab = EntityStates.Huntress.ArrowRain.areaIndicatorPrefab;
        public static GameObject teleportLocatorInstance;


        public static float stopwatch;

        public override void OnEnter()
        {
            base.OnEnter();
            position = base.transform.position;
            Ray aimRay = base.GetAimRay();

            if (base.isAuthority)
            {
                base.characterMotor.velocity = -60f * aimRay.direction;
            }

            if (NetworkServer.active)
            {
                teleportLocatorInstance = Object.Instantiate(teleportLocatorPrefab, position, base.transform.rotation);
                teleportLocatorInstance.transform.localScale = Vector3.one * 5f;

                

            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (base.isAuthority && base.fixedAge > duration)
            {
                outer.SetNextState(new PortalAuxLocate
                {
                    portalMainPosition = teleportLocatorInstance.transform.position,
                    firstInstance = teleportLocatorInstance
                });
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
