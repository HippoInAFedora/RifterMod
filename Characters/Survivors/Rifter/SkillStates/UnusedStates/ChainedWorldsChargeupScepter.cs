using EntityStates;
using IL.RoR2.Skills;
using R2API;
using RifterMod.Modules;
using RifterMod.Survivors.Rifter;
using RifterMod.Survivors.Rifter.SkillStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Characters.Survivors.Rifter.SkillStates.UnusedStates
{
    public class ChainedWorldsChargeupScepter : ChainedWorldsChargeup
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            blastRadius = BlastRadius();
            if (isAuthority && !IsKeyDownAuthority() || fixedAge > chargeDuration + 1f)
            {
                outer.SetNextState(new ChainedWorlds
                {
                    blastNum = 0,
                    blastMax = 7,
                    blastRadius = blastRadius,
                    basePosition = characterBody.corePosition,
                    baseDirection = GetAimRay().direction
                });
            }
        }
    }
}


