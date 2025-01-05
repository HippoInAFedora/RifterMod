
using EntityStates;
using Newtonsoft.Json.Utilities;
using R2API;
using RifterMod.Characters.Survivors.Rifter.Components;
using RifterMod.Modules;
using RifterMod.Survivors.Rifter;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class ChainedWorldsStartup : BaseState
    {
        public int blastNum = 0;
        public int blastMax = 5;

        public static GameObject areaIndicatorPrefab = EntityStates.Huntress.ArrowRain.areaIndicatorPrefab;
        public static GameObject[] areaIndicatorInstance = new GameObject[5];

        public Vector3 basePosition;
        public Vector3 baseDirection;

        public RifterOverchargePassive rifterStep;

        public override void OnEnter()
        {
            base.OnEnter();
            if ((bool)areaIndicatorPrefab)
            {
                for (int i = 0; i < areaIndicatorInstance.Length; i++)
                {
                    areaIndicatorInstance[i] = UnityEngine.Object.Instantiate(areaIndicatorPrefab);
                    areaIndicatorInstance[i].transform.localScale = new Vector3(BlastRadius(), BlastRadius(), BlastRadius());
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
            for (int i = 0;i < areaIndicatorInstance.Length;i++)
            {
                areaIndicatorInstance[i].transform.position = GetNumPosition(i);
            }
        }

        private Vector3 GetNumPosition(int num)
        {

            float num2 = RifterStaticValues.riftPrimaryDistance / 6 * (num) + 10f;
            Vector3 location = GetAimRay().GetPoint(num2);
            Vector3 position = location;
            if (Physics.SphereCast(basePosition, 0.05f, baseDirection, out var raycastHit, num2, LayerIndex.world.mask))
            {
                position = raycastHit.point;
            }
            return position;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && inputBank.skill4.justReleased)
            {
                basePosition = base.GetAimRay().origin;
                baseDirection = base.GetAimRay().direction.normalized;
                outer.SetNextState(new ChainedWorlds
                {
                    blastNum = blastNum,
                    blastMax = blastMax,
                    blastRadius = RifterStaticValues.blastRadius,
                    basePosition = basePosition,
                    baseDirection = baseDirection,
                });
            }
        }

        public override void OnExit()
        {
            for (int i = 0; i < areaIndicatorInstance.Length; i++) 
            { 
                if (areaIndicatorInstance[i] != null)
                {
                    Destroy(areaIndicatorInstance[i].gameObject);
                }
            }
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public float BlastRadius()
        {
            return RifterStaticValues.blastRadius;
        }
    }
}



