using EntityStates;
using IL.RoR2.Skills;
using R2API;
using RifterMod.Characters.Survivors.Rifter.SkillStates;
using RifterMod.Modules;
using RifterMod.Survivors.Rifter;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class ChainedWorldsChargeup : RiftBase
    {
        public static GameObject areaIndicatorPrefab = global::EntityStates.Huntress.ArrowRain.areaIndicatorPrefab;
        public static GameObject areaIndicatorInstance;
        float stopwatch;
        float blastWatch;


        CharacterBody body;

        int blastNum;


        bool specialReleasedOnce;

        float chargeDuration;

        public override void OnEnter()
        {
            base.OnEnter();
            if (base.cameraTargetParams)
            {
                cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
            }
            blastNum = 0;
            chargeDuration = .75f / base.attackSpeedStat;
            body = base.characterBody;
            if ((bool)areaIndicatorPrefab)
            {

                areaIndicatorInstance = UnityEngine.Object.Instantiate(areaIndicatorPrefab);
                areaIndicatorInstance.transform.localScale = new Vector3(BlastRadius(), BlastRadius(), BlastRadius());
            }
        }

        public override void Update()
        {
            base.Update();
            UpdateAreaIndicator();
        }

        private Vector3 GetNumPosition(int num)
        {
            float num2 = RiftDistance() / 5 * (num + 1);
            Vector3 location = base.GetAimRay().GetPoint(num2);
            Vector3 position = location;
            if (Physics.SphereCast(base.characterBody.corePosition, 0.05f, base.GetAimRay().direction, out var raycastHit, num2, LayerIndex.world.mask, QueryTriggerInteraction.Collide))
            {
                position = raycastHit.point;
            }
            return position;
        }

        private void UpdateAreaIndicator()
        {
            if ((bool)areaIndicatorInstance)
            {
                areaIndicatorInstance.transform.position = GetNumPosition(blastNum);
                areaIndicatorInstance.transform.localScale = new Vector3(BlastRadius(), BlastRadius(), BlastRadius());
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.inputBank.skill4.justReleased)
            {
                specialReleasedOnce = true;
            }

            stopwatch += Time.fixedDeltaTime;
            if (blastNum <= 4)
            {
                blastWatch += Time.fixedDeltaTime;
                if (blastWatch > chargeDuration)
                {
                    blastNum++;
                    blastWatch = 0;
                }
            }
            if (base.isAuthority && ((base.inputBank.skill4.justPressed && specialReleasedOnce) || base.inputBank.skill1.justPressed || base.inputBank.skill2.justPressed))
            {
                outer.SetNextState(new ChainedWorlds
                {
                    blastMax = blastNum + 1,
                });
            }
            if (base.isAuthority && blastNum > 4)
            {
                outer.SetNextState(new ChainedWorlds
                {
                    blastMax = 5,
                });
            }

        }

        public override void OnExit()
        {
            if ((bool)areaIndicatorInstance)
            {
                EntityState.Destroy(areaIndicatorInstance.gameObject);
            }
            if (base.cameraTargetParams)
            {
                cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Standard);
            }
            base.OnExit();
        }

        public override float BlastRadius()
        {
            return 12f;
        }

        public override float BlastDamage()
        {
            return base.characterBody.damage * RifterStaticValues.chainedWorldsCoefficient;
        }

    }
}


