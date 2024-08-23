using RifterMod.Survivors.Rifter;
using RifterMod.Survivors.Rifter.SkillStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RifterMod.Characters.Survivors.Rifter.SkillStates
{
    internal class RecursionScepter : Recursion
    {

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;

            if (stopwatch >= (duration / 5) && isAuthority)
            {
                if (blastNum < blastMax)
                {
                    outer.SetNextState(new RecursionScepter
                    {
                        blastNum = blastNum,
                        blastMax = blastMax,
                        basePosition = basePosition
                    });
                    return;
                }
                else
                {
                    outer.SetNextStateToMain();
                }

            }

        }
        public override float BlastRadius()
        {
            return 20f * (float)Math.Pow((double).95, (double)blastNum);
        }

        public override float BlastDamage()
        {
            return characterBody.damage * RifterStaticValues.recursionCoefficient * (float)Math.Pow((double)1.2, (double)blastNum);
        }
    }
}
