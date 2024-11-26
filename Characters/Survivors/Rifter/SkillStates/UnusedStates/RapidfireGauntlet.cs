

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using System;
using IL.RoR2.Skills;
using RifterMod.Modules;
using UnityEngine.Networking;
using RifterMod.Survivors.Rifter.SkillStates;

namespace RifterMod.Characters.Survivors.Rifter.SkillStates.UnusedStates
{
    public class RapidfireGauntlet : RiftBase
    {

        public override void OnEnter()
        {
            base.OnEnter();
            baseDuration = .4f;

        }
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