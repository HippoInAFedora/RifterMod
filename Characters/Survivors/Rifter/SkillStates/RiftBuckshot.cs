

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
    public class RiftBuckshot : RiftGauntletBase
    {
        private float buckshotMax = 8.5f;

        public float duration = .5f;

        public bool hasOvercharge;
        public int overchargedBuckshots = 3;

        //public float buckshotNum;

        public override void OnEnter()
        {
            shouldBuckshot = true;
            //buckshotNum = 1;
            base.OnEnter();

           
        }

        public override float RiftDistance()
        {
            return RifterStaticValues.riftPrimaryDistance;
        }

        public override float BlastRadius()
        {
            return RifterStaticValues.blastRadius * .6f;
        }

        public override float BlastDamage()
        {
            return characterBody.damage * RifterStaticValues.buckshotRiftCoefficient;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void RunDistanceAssist(Vector3 vector, BlastAttack.Result result)
        {
            return;
        }

        public override void Buckshot(Vector3 origin)
        {
            base.Buckshot(origin);
            Ray aimRay = base.GetAimRay();
            float[] floats = new float[5 + (IsOvercharged()? overchargedBuckshots : 0)];
            Vector3[] angles = new Vector3[5 + (IsOvercharged() ? overchargedBuckshots : 0)];

            for (int i = 0; i < floats.Length - 1; i++)
            {
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


                BlastAttack buckshotBlast = new BlastAttack();
                buckshotBlast.attacker = gameObject;
                buckshotBlast.inflictor = gameObject;
                buckshotBlast.teamIndex = TeamIndex.Player;
                buckshotBlast.radius = BlastRadius() * .5f;
                buckshotBlast.falloffModel = BlastAttack.FalloffModel.None;
                buckshotBlast.baseDamage = base.characterBody.damage * RifterStaticValues.buckshotWeakRiftCoefficient;
                buckshotBlast.crit = RollCrit();
                buckshotBlast.procCoefficient = .8f;
                buckshotBlast.canRejectForce = false;
                buckshotBlast.position = vector;
                buckshotBlast.attackerFiltering = AttackerFiltering.NeverHitSelf;
                //buckshotBlast.AddModdedDamageType(RifterDamage.riftDamage);
                var result = buckshotBlast.Fire();

                

                EffectData effectData = new EffectData();
                effectData.origin = vector;
                effectData.scale = BlastRadius() / 10f * .5f ;
                if (!IsOvercharged())
                {
                    //blastEffectPrefab.transform.GetChild(0).localScale = Vector3.one * BlastRadius() / 7.5f;
                    //blastEffectPrefab.transform.GetChild(1).localScale = Vector3.one * BlastRadius() / 7.5f;
                    //blastEffectPrefab.transform.GetChild(2).localScale = Vector3.one * BlastRadius() / 7.5f;
                    EffectManager.SpawnEffect(blastEffectPrefab, effectData, transmit: true);
                }
                else
                {
                    //overchargedEffectPrefab.transform.GetChild(0).localScale = Vector3.one * BlastRadius() / 7.5f;
                    //overchargedEffectPrefab.transform.GetChild(1).localScale = Vector3.one * BlastRadius() / 7.5f;
                    //overchargedEffectPrefab.transform.GetChild(2).localScale = Vector3.one * BlastRadius() / 7.5f;
                    EffectManager.SpawnEffect(overchargedEffectPrefab, effectData, transmit: true);
                }

                foreach (var hit in result.hitPoints)
                {
                    if (hit.hurtBox != null)
                    {
                        if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                        {
                            if (IsOvercharged() && hurtBox.healthComponent.alive)
                            {
                                BlastOvercharge(result);
                                Debug.Log("buckshot overcharge");
                            }
                        }

                    }
                };
            }
        }
    }
}