

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RoR2;
using UnityEngine;
using System;
using R2API;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;
using RifterMod.Characters.Survivors.Rifter.Components;
using RifterMod.Modules;
using UnityEngine.Networking;
using System.Linq;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftGauntletBase : RiftBase
    {
        public float baseDuration = 0.7f;
        public bool shot = false;
        public bool shouldDistanceAssist;
        float isRiftHitGround;
        public bool shouldBuckshot = false;

        public bool isBlastOvercharge = false;
        //public static GameObject tracerEffectPrefabOld = RifterAssets.fractureLineEffect;

        public static GameObject tracerEffectPrefab = RifterAssets.fractureLineTracer;
        public static GameObject tracerEffectPrefabOvercharged = RifterAssets.fractureLineTracerOvercharged;
        public GameObject tracer;

        public float duration;

        public RifterOverchargePassive rifterStep;

        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;

        List<GameObject> ignoreList1 = new List<GameObject>();
        List<GameObject> ignoreList2 = new List<GameObject>();

        //OnEnter() runs once at the start of the skill
        //All we do here is create a BulletAttack and fire it
        public override void OnEnter()
        {
            base.OnEnter();

            rifterStep = GetComponent<RifterOverchargePassive>();

            this.duration = baseDuration / attackSpeedStat;
            Ray aimRay = GetAimRay();
            StartAimMode(aimRay, 2f, false);
            base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
            Util.PlaySound(FireBarrage.fireBarrageSoundString, gameObject);
            AddRecoil(-0.6f, 0.6f, -0.6f, 0.6f);
            if (FireBarrage.effectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(FireBarrage.effectPrefab, gameObject, "MuzzleRight", false);
            }

            RiftAndFracture();         
            TeleportEnemies();
        }

        //This method runs once at the end
        //Here, we are doing nothing
        public override void OnExit()
        {
            ignoreList1.RemoveAll(x => x != null);
            ignoreList2.RemoveAll(x => x != null);
            enemyBodies.RemoveAll(x => x != null);
            base.OnExit();
        }


        //FixedUpdate() runs almost every frame of the skill
        //Here, we end the skill once it exceeds its intended duration
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= this.duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        //GetMinimumInterruptPriority() returns the InterruptPriority required to interrupt this skill
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public virtual void RiftAndFracture()
        {
            Ray aimRay = GetAimRay();
            Vector3 vector = aimRay.GetPoint(RiftDistance());

            if (Physics.Raycast(aimRay, out var endPoint, RiftDistance(), LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
            {
                float hit = endPoint.distance;
                vector = aimRay.GetPoint(hit);
            }

            float vectorDistance = Vector3.Distance(aimRay.origin, vector);
            if (vectorDistance < RiftDistance() * 2 / 3)
            {
                float float1 = RiftDistance() - vectorDistance + 1.1f;
                decimal dec = new decimal(float1);
                double d = (double)dec;
                double isRiftHitGroundDouble = 1 / Math.Log(d, 3.5);
                isRiftHitGround = (float)isRiftHitGroundDouble;
            }
            else
            {
                isRiftHitGround = 1f;
            }
            if (vectorDistance >= RiftDistance() - BlastRadius())
            {
                shouldDistanceAssist = true;
            }

            if (isAuthority)
            {
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.attacker = gameObject;
                blastAttack.inflictor = gameObject;
                blastAttack.teamIndex = TeamIndex.Player;
                blastAttack.radius = BlastRadius();
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseDamage = BlastDamage() * isRiftHitGround;
                blastAttack.crit = RollCrit();
                blastAttack.procCoefficient = .8f;
                blastAttack.canRejectForce = false;
                blastAttack.position = vector;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                blastAttack.AddModdedDamageType(RifterDamage.riftDamage);
                var result = blastAttack.Fire();

                ImpactEffect(vector);

                foreach (var hit in result.hitPoints)
                {
                    if (hit.hurtBox != null)
                    {
                        if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                        {
                            ignoreList1.AddDistinct(hurtBox.healthComponent.gameObject);
                            ignoreList2.AddDistinct(hurtBox.healthComponent.gameObject);

                            if (IsOvercharged() && hurtBox.healthComponent.alive)
                            {
                                BlastOvercharge(result);
                            }
                        }

                    }
                };

                for (int i = 0; i < result.hitPoints.Length; i++)
                {
                    var hit = result.hitPoints[i];
                }

                if (shouldDistanceAssist)
                {
                    RunDistanceAssist(vector, result);
                }

                if (shouldBuckshot)
                {
                    Buckshot(vector);
                }

                tracer = IsOvercharged() ? tracerEffectPrefabOvercharged : tracerEffectPrefab;

                BulletAttack bulletAttack = new BulletAttack();
                bulletAttack.owner = gameObject;
                bulletAttack.weapon = gameObject;
                bulletAttack.origin = aimRay.origin;
                bulletAttack.aimVector = aimRay.direction;
                bulletAttack.minSpread = 0f;
                bulletAttack.maxSpread = characterBody.spreadBloomAngle;
                bulletAttack.damage = characterBody.damage * RifterStaticValues.fractureCoefficient;
                bulletAttack.bulletCount = 1U;
                bulletAttack.procCoefficient = 0f;
                bulletAttack.falloffModel = BulletAttack.FalloffModel.DefaultBullet;
                bulletAttack.radius = .75f;
                bulletAttack.tracerEffectPrefab = tracer;
                bulletAttack.muzzleName = "MuzzleRight";
                bulletAttack.hitEffectPrefab = this.hitEffectPrefab;
                bulletAttack.isCrit = false;
                bulletAttack.HitEffectNormal = false;
                bulletAttack.stopperMask = LayerIndex.playerBody.mask;
                bulletAttack.smartCollision = true;
                bulletAttack.maxDistance = vectorDistance;

                bulletAttack.modifyOutgoingDamageCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo) //changed to _bulletAttack
                {
                    if (hitInfo.hitHurtBox != null)
                    {
                        if (hitInfo.hitHurtBox.TryGetComponent(out HurtBox hurtBox))
                        {

                            if (IsOvercharged() && hurtBox.healthComponent.alive)
                            {
                                Overcharge(hitInfo, hurtBox);
                            }
                        }

                    }
                };

                bulletAttack.filterCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo)
                {
                    HealthComponent healthComponent = (hitInfo.hitHurtBox ? hitInfo.hitHurtBox.healthComponent : null);
                    if (healthComponent && healthComponent.alive && ignoreList2.Contains(healthComponent.gameObject))
                    {
                        return false;

                    }
                    return (!hitInfo.entityObject || (object)hitInfo.entityObject != _bulletAttack.owner) && BulletAttack.defaultFilterCallback(_bulletAttack, ref hitInfo);
                };

                bulletAttack.Fire();
            }

        }

        public override float RiftDistance()
        {
            return RifterStaticValues.riftPrimaryDistance;
        }

        public override float BlastRadius()
        {
            return RifterStaticValues.blastRadius;
        }

        private void ImpactEffect(Vector3 vector)
        {
            EffectData effectData = new EffectData();
            effectData.origin = vector;
            effectData.scale = BlastRadius() / 10f;
            if (!IsOvercharged())
            {
                EffectManager.SpawnEffect(blastEffectPrefab, effectData, transmit: true);
            }
            else
            {
                EffectManager.SpawnEffect(overchargedEffectPrefab, effectData, transmit: true);
            }
        }
        public override float BlastDamage()
        {
            return characterBody.damage * RifterStaticValues.primaryRiftCoefficient;
        }

        public override bool IsOvercharged()
        {
            if (rifterStep.rifterOverchargePassive <= 0)
            {
                return false;
            }
            return true;
        }



        public virtual void RunDistanceAssist(Vector3 vector, BlastAttack.Result result)
        {
            Ray aimRay = GetAimRay();

            BulletAttack bulletAttack = new BulletAttack();
            bulletAttack.owner = gameObject;
            bulletAttack.weapon = gameObject;
            bulletAttack.origin = vector;
            bulletAttack.aimVector = -aimRay.direction;
            bulletAttack.minSpread = 0f;
            bulletAttack.maxSpread = characterBody.spreadBloomAngle;
            bulletAttack.damage = BlastDamage() * isRiftHitGround;
            bulletAttack.bulletCount = 1U;
            bulletAttack.procCoefficient = .8f;
            bulletAttack.falloffModel = BulletAttack.FalloffModel.DefaultBullet;
            bulletAttack.radius = BlastRadius() / 2f;
            bulletAttack.muzzleName = "MuzzleRight";
            bulletAttack.hitEffectPrefab = this.hitEffectPrefab;
            bulletAttack.isCrit = false;
            bulletAttack.HitEffectNormal = false;
            bulletAttack.stopperMask = LayerIndex.playerBody.mask;
            bulletAttack.smartCollision = true;
            bulletAttack.maxDistance = BlastRadius() * .8f;
            bulletAttack.AddModdedDamageType(RifterDamage.riftDamage);


            bulletAttack.modifyOutgoingDamageCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo) //changed to _bulletAttack
            {
                if (hitInfo.hitHurtBox != null)
                {
                    if (hitInfo.hitHurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        ignoreList2.AddDistinct(hurtBox.healthComponent.gameObject);

                        if (IsOvercharged())
                        {
                            Overcharge(hitInfo, hurtBox);
                        }
                    }
                }
            };


            bulletAttack.filterCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo)
            {
                HealthComponent healthComponent = (hitInfo.hitHurtBox ? hitInfo.hitHurtBox.healthComponent : null);
                if (healthComponent && healthComponent.alive && ignoreList1.Contains(healthComponent.gameObject))
                {
                    return false;

                }
                return (!hitInfo.entityObject || (object)hitInfo.entityObject != _bulletAttack.owner) && BulletAttack.defaultFilterCallback(_bulletAttack, ref hitInfo);
            };


            bulletAttack.Fire();
        }


        public virtual void Buckshot(Vector3 origin)
        {
        }
        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            //writer.Write(base.gameObject.transform.position);
            writer.Write(enemyTeleportTo);
            for (int i = 0; i < enemyBodies.Count; i++)
            {
                writer.Write(enemyBodies[i].netId);
            }

        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            //originalPosition = reader.ReadVector3();
            enemyTeleportTo = reader.ReadVector3();
            while (reader.Position < reader.Length)
            {
                enemyBodies.Add(Util.FindNetworkObject(reader.ReadNetworkId()).GetComponent<CharacterBody>());
            }

        }

    }
}