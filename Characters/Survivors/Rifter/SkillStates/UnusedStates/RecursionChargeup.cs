using EntityStates;
using IL.RoR2.Skills;
using R2API;
using RifterMod.Characters.Survivors.Rifter.SkillStates;
using RifterMod.Modules;
using RifterMod.Survivors.Rifter;
using RifterMod.Survivors.Rifter.SkillStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Characters.Survivors.Rifter.SkillStates.UnusedStates
{
    public class RecursionChargeup : RiftBase
    {
        public static GameObject areaIndicatorPrefab = EntityStates.Huntress.ArrowRain.areaIndicatorPrefab;
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
            if (cameraTargetParams)
            {
                cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
            }
            blastNum = 0;
            chargeDuration = .8f / attackSpeedStat;
            body = characterBody;
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

        private void UpdateAreaIndicator()
        {
            if ((bool)areaIndicatorInstance)
            {
                areaIndicatorInstance.transform.position = body.corePosition;
                areaIndicatorInstance.transform.up = body.transform.up;
                areaIndicatorInstance.transform.localScale = new Vector3(BlastRadius(), BlastRadius(), BlastRadius());
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority && inputBank.skill4.justReleased)
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
            if (isAuthority && inputBank.skill4.justPressed && specialReleasedOnce)
            {
                outer.SetNextState(new Recursion
                {
                    blastMax = blastNum + 1,
                });
            }
            if ((bool)skillLocator && inputBank.skill3.justPressed && skillLocator.utility.IsReady())
            {
                EntityState state = new EntityState();
                if (skillLocator.utility.stateMachine.state is Slipstream)
                {
                    state = new Slipstream();
                }
                if (skillLocator.utility.stateMachine.state is RiftRiderLocate)
                {
                    state = new RiftRiderLocate();
                }
                outer.SetNextState(new Recursion
                {
                    blastNum = 0,
                    blastMax = blastNum + 1,
                    setNextState = state
                });

            }
            if (isAuthority && blastNum > 4)
            {
                outer.SetNextState(new Recursion
                {
                    blastNum = 0,
                    blastMax = 5,
                });
            }

        }

        public override void OnExit()
        {
            if ((bool)areaIndicatorInstance)
            {
                Destroy(areaIndicatorInstance.gameObject);
            }
            if (cameraTargetParams)
            {
                cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Standard);
            }
            base.OnExit();
        }

        public override float BlastRadius()
        {
            return 10f * (float)Math.Pow(RifterStaticValues.overchargedCoefficient, blastNum);
        }

        public override float BlastDamage()
        {
            return characterBody.damage * RifterStaticValues.recursionCoefficient * (float)Math.Pow(RifterStaticValues.overchargedCoefficient, blastNum);
        }

    }
}


