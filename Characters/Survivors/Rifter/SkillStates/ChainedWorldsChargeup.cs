﻿using EntityStates;
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

        public float blastRadius;

        public override void OnEnter()
        {
            base.OnEnter();
            if (cameraTargetParams)
            {
                cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
            }
            blastNum = 0;
            chargeDuration = 2f / attackSpeedStat;
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

        //private Vector3 GetNumPosition(int num)
        //{
        //    float num2 = RiftDistance() / 5 * (num + 1);
        ////    Vector3 location = GetAimRay().GetPoint(num2);
        //    Vector3 position = location;
        //    if (Physics.SphereCast(characterBody.corePosition, 0.05f, GetAimRay().direction, out var raycastHit, num2, LayerIndex.world.mask, QueryTriggerInteraction.Collide))
        //    {
        //        position = raycastHit.point;
        //    }
        //    return position;
        //}

        private void UpdateAreaIndicator()
        {
            if ((bool)areaIndicatorInstance)
            {
                areaIndicatorInstance.transform.position = GetAimRay().GetPoint(RifterStaticValues.riftPrimaryDistance);
                if (Physics.SphereCast(characterBody.corePosition, 0.05f, GetAimRay().direction, out var raycastHit, RifterStaticValues.riftPrimaryDistance, LayerIndex.world.mask))
                        {
                            areaIndicatorInstance.transform.position = raycastHit.point;
                        }
                    areaIndicatorInstance.transform.localScale = new Vector3(blastRadius, blastRadius, blastRadius);
            }
        }

        public void OLDFixedUpdate()
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
            if (isAuthority && ((inputBank.skill4.justPressed && specialReleasedOnce) || inputBank.skill1.justPressed || inputBank.skill2.justPressed))
            {
                outer.SetNextState(new ChainedWorlds
                {
                    blastNum = 0,
                    blastMax = blastNum + 1,
                });
            }
            if (isAuthority && blastNum > 4)
            {
                outer.SetNextState(new ChainedWorlds
                {
                    blastNum = 0,
                    blastMax = 5,
                });
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            blastRadius = BlastRadius(); 
            Debug.Log(blastRadius);
            if (isAuthority && !IsKeyDownAuthority() || base.fixedAge > chargeDuration + 1f)
            {
                outer.SetNextState(new ChainedWorlds
                {
                    blastNum = 0,
                    blastMax = 5,
                    blastRadius = blastRadius,      
                    basePosition = base.characterBody.corePosition,
                    baseDirection = base.GetAimRay().direction
                }) ;
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
            float radius = Mathf.Clamp01(base.fixedAge/chargeDuration);
            if (base.fixedAge > chargeDuration)
            {
                radius = 1f;
            }
            return radius * 7f + 5f;
        }

        public override float BlastDamage()
        {
            return characterBody.damage * RifterStaticValues.chainedWorldsCoefficient;
        }

    }
}


