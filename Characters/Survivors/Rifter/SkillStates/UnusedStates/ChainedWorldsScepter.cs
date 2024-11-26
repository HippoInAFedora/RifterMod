
using EntityStates;
using Newtonsoft.Json.Utilities;
using R2API;
using RifterMod.Modules;
using RifterMod.Survivors.Rifter;
using RifterMod.Survivors.Rifter.SkillStates;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Characters.Survivors.Rifter.SkillStates.UnusedStates
{
    public class ChainedWorldsScepter : ChainedWorlds
    {

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;


            if (stopwatch >= duration / 5 && isAuthority)
            {
                if (blastNum < blastMax)
                {
                    outer.SetNextState(new ChainedWorldsScepter
                    {
                        blastNum = blastNum,
                        blastMax = blastMax,
                        blastRadius = blastRadius,
                        basePosition = basePosition,
                        baseDirection = baseDirection,
                    });
                    return;
                }
                else
                {
                    outer.SetNextStateToMain();
                }

            }

        }
        public override float BlastDamage()
        {
            return characterBody.damage * RifterStaticValues.secondaryRiftCoefficient * (float)Math.Pow((double)1.2, blastNum);
        }
    }
}


