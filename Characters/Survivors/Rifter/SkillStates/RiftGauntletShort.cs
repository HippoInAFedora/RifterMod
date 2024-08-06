

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using System;
using IL.RoR2.Skills;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftGauntletShort : RiftGauntletBase
    {
        public override float RiftDistance()
        {
            return RifterStaticValues.riftSecondaryDistance;
        }

        public override float BlastRadius()
        {
            return RifterStaticValues.blastRadius;
        }

        public override float BlastDamage()
        {
            return characterBody.damage * RifterStaticValues.secondaryRiftCoefficient;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void RunDistanceAssist(Vector3 vector, BlastAttack.Result result)
        {
            return;
        }
    }
}