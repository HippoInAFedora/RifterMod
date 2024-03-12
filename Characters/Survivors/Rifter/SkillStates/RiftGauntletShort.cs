﻿

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using System;
using IL.RoR2.Skills;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftGauntletShort : RiftGauntlet
    {
        public bool isBlastOvercharge = true;
       public override float RiftDistance()
        {
            return RifterStaticValues.riftSecondaryDistance;
        }

        public override float BlastRadius()
        {
            if (IsOvercharged())
            {
                return 7.75f * RifterStaticValues.overchargedCoefficient;
            }
            return 7.75f;
        }

        public override float BlastDamage()
        {
            if (IsOvercharged())
            {
                return (base.characterBody.damage * RifterStaticValues.secondaryRiftCoefficient) * RifterStaticValues.overchargedCoefficient;
            }
            return base.characterBody.damage * RifterStaticValues.secondaryRiftCoefficient;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}