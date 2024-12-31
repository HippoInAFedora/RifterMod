

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using System;
using IL.RoR2.Skills;
using UnityEngine.Networking;
using R2API;
using RifterMod.Characters.Survivors.Rifter.Components;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftFocus : RiftBase
    {
        public override void OnEnter()
        {
            usesOvercharge = false;
            shouldBuckshot = true;
            blastPulls = true;
            Ray aimRay = base.GetAimRay();
            base.OnEnter();
            GetPosition(aimRay, out Vector3 position);
            GetDistance(aimRay, position, out float distance);
            if (base.isAuthority)
            {
                if (blast)
                {
                    Blast(aimRay, position, distance, DamageSource.Primary);
                    if (shouldBuckshot)
                    {
                        Buckshot(position);
                    }
                }
                Fracture(aimRay, distance, LayerIndex.noCollision, DamageSource.Primary);
            }
            TeleportEnemies();
        }

        public override float RiftDistance()
        {
            if (inputBank.skill2.down)
            {
                return RifterStaticValues.riftSecondaryDistance;
            }
            return RifterStaticValues.riftPrimaryDistance;
        }

        public override float BlastRadius()
        {
            if (hitTimelock)
            {
                return RifterStaticValues.timelockRadius;
            }
            if (inputBank.skill2.down)
            {
                return RifterStaticValues.blastRadius * .8f;
            }
            return RifterStaticValues.blastRadius;
        }

        public override float BlastDamage()
        {
            if (hitTimelock)
            {
                return base.characterBody.damage * RifterStaticValues.timelockBlast + (base.characterBody.damage * RifterStaticValues.timelockEnemyMultiplier * hitEnemies);
            }
            if (inputBank.skill2.down)
            {
                return characterBody.damage * RifterStaticValues.primaryRiftCoefficient * .8f;
            }
            return characterBody.damage * RifterStaticValues.primaryRiftCoefficient;
        }

    }
}