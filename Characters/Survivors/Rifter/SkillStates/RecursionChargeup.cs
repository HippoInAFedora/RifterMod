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
    public class RecursionChargeup : RiftBase
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
            chargeDuration = .5f / base.attackSpeedStat;
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
            if (base.isAuthority && base.inputBank.skill4.justReleased)
            {
                specialReleasedOnce = true;
            }

            stopwatch += Time.fixedDeltaTime;
            if (blastNum <= 3)
            {
                blastWatch += Time.fixedDeltaTime;
                if (blastWatch > chargeDuration)
                {
                    blastNum++;
                    blastWatch = 0;
                }
            }
            if (base.isAuthority && base.inputBank.skill4.justPressed && specialReleasedOnce || blastNum > 3 && base.isAuthority)
                {
                outer.SetNextState(new Recursion
                {
                    blastMax = blastNum + 1,
                }) ;
                }
            if((bool)base.skillLocator && base.inputBank.skill3.justPressed && base.skillLocator.utility.IsReady())
            {
                base.skillLocator.utility.stock--;
                outer.SetNextState(new Recursion
                {
                    blastMax = blastNum + 1,
                    setNextState = new Slipstream(),
                }) ;
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
            return 10f * (float)Math.Pow((double)RifterStaticValues.overchargedCoefficient, (double)blastNum);
        }

        public override float BlastDamage()
        {
          return base.characterBody.damage * RifterStaticValues.recursionCoefficient * (float)Math.Pow((double)RifterStaticValues.overchargedCoefficient, (double)blastNum);
        }

    }
}
    

