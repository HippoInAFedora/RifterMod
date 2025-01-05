

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
    public class RiftBuckshot : RiftBase
    {
        private float buckshotMax = 5f;

        public BlastAttack[] buckshotBlasts;
        public float[] durations;
        public EffectData[] effectData;
        public bool[] flag;

        public override void OnEnter()
        {
            usesOvercharge = false;
            shouldBuckshot = true;
            baseDuration = .5f;
            base.OnEnter();
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
            if (base.isAuthority)
            {
                for (int i = 0; i < durations.Length - 1; i++)
                {
                    if (base.fixedAge > durations[i] && !flag[i])
                    {
                        buckshotBlasts[i].Fire();
                        effectData[i] = new EffectData();
                        effectData[i].origin = buckshotBlasts[i].position;
                        effectData[i].scale = BlastRadius() / 10f * .5f;
                        EffectManager.SpawnEffect(blastEffectPrefab, effectData[i], transmit: true);
                        flag[i] = true;
                    }
                }
            }
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
                return RifterStaticValues.blastRadius * .5f;
            }
            return RifterStaticValues.blastRadius * .6f;
        }

        public override float BlastDamage()
        {
            if (hitTimelock)
            {
                return base.characterBody.damage * RifterStaticValues.timelockBlast + (base.characterBody.damage * RifterStaticValues.timelockEnemyMultiplier * hitEnemies);
            }
            if (inputBank.skill2.down)
            {
                return characterBody.damage * RifterStaticValues.buckshotRiftCoefficient * .8f;
            }
            return characterBody.damage * RifterStaticValues.buckshotRiftCoefficient;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void Buckshot(Vector3 origin)
        {
            base.Buckshot(origin);
            Ray aimRay = base.GetAimRay();
            float[] floats = new float[5];
            Vector3[] angles = new Vector3[5];
            buckshotBlasts = new BlastAttack[5];
            durations = new float[5];
            effectData = new EffectData[5];
            flag = new bool[5];

            for (int i = 0; i < floats.Length - 1; i++)
            {
                flag[i] = false;
                durations[i] = UnityEngine.Random.Range(0, duration * .5f);
                floats[i] = UnityEngine.Random.Range(5f, buckshotMax);
                angles[i] = UnityEngine.Random.onUnitSphere;
                Ray newRay = new Ray();
                newRay.origin = origin;
                newRay.direction = angles[i];
                Vector3 vector = newRay.GetPoint(floats[i]);

                if (Physics.Raycast(newRay, out var endPoint, floats[i], LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
                {
                    float hit = endPoint.distance;
                    vector = newRay.GetPoint(hit);
                }


                buckshotBlasts[i] = new BlastAttack();
                buckshotBlasts[i].attacker = gameObject;
                buckshotBlasts[i].inflictor = gameObject;
                buckshotBlasts[i].teamIndex = TeamIndex.Player;
                buckshotBlasts[i].radius = BlastRadius() * .5f;
                buckshotBlasts[i].falloffModel = BlastAttack.FalloffModel.None;
                buckshotBlasts[i].baseDamage = base.characterBody.damage * RifterStaticValues.buckshotWeakRiftCoefficient;
                buckshotBlasts[i].crit = RollCrit();
                buckshotBlasts[i].procCoefficient = .8f;
                buckshotBlasts[i].canRejectForce = false;
                buckshotBlasts[i].position = vector;
                buckshotBlasts[i].attackerFiltering = AttackerFiltering.NeverHitSelf;
            }
        }
    }
}