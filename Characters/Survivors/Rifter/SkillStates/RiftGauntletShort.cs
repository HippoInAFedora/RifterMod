

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
    public class RiftGauntletShort : RiftBase
    {

        public override void OnEnter()
        {
            usesOvercharge = false;
            shouldBuckshot = true;
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

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge > duration)
            {
                if (inputBank.skill1.down && inputBank.skill2.wasDown)
                {
                    outer.SetNextState(new RiftGauntletShort());
                }
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public override float RiftDistance()
        {
            return RifterStaticValues.riftSecondaryDistance;
        }
    }
}