using RifterMod.Survivors.Rifter.SkillStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace RifterMod.Characters.Survivors.Rifter.SkillStates
{
    internal class RecursionLocateScepter : RecursionLocate
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority && (bool)inputBank)
            {
                if (inputBank.skill4.justReleased)
                {
                    outer.SetNextState(new RecursionScepter
                    {
                        blastMax = 10,
                        blastNum = 0,
                        basePosition = teleportLocatorInstance.transform.position
                    });
                }
            }
        }
    }
}
