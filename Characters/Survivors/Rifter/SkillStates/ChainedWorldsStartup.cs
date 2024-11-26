
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
        public int blastMax;

        public Vector3 basePosition;
        public Vector3 baseDirection;

        public RifterOverchargePassive rifterStep;

        public override void OnEnter()
        {
            base.OnEnter();
            basePosition = base.GetAimRay().origin;
            baseDirection = base.GetAimRay().direction.normalized;
            rifterStep = base.GetComponent<RifterOverchargePassive>();
            if (rifterStep != null )
            {
                blastMax = rifterStep.stocksConsumed;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority)
            {
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
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }
}


