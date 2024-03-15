

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using System;
using IL.RoR2.Skills;
using RifterMod.Modules;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RapidfireGauntlet : RiftGauntlet
    {
        public float baseDuration = .4f;
        public bool isBlastOvercharge = true;


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / base.attackSpeedStat;

        }
        public override float RiftDistance()
        {
            return RifterStaticValues.riftSecondaryDistance;
        }

        public override bool IsOvercharged()
        {
            if (rifterStep.rapidfireShot == true)
            {
                return true;
            }
            return base.IsOvercharged();
        }

        public override float BlastRadius()
        {
            return 7.75f;
        }

        public override float BlastDamage()
        {
            return base.characterBody.damage * RifterStaticValues.secondaryRiftCoefficient;
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