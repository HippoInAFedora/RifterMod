

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using System;
using IL.RoR2.Skills;
using HG;
using R2API;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class Refraction : RiftGauntletBase
    {
        public float baseDuration = .7f;
        public bool isBlastOvercharge = true;

        private float isLeftHitGround;
        private float isRightHitGround;
        private float isMiddleHitGround;
        private Ray rightBlast;
        private Ray leftBlast;
        private Ray middleBlast;
        private Vector3 vectorRight;
        private Vector3 vectorLeft;
        private Vector3 vectorMiddle;
        public BlastAttack.Result result;

        List<GameObject> ignoreList2 = new List<GameObject>();

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / base.attackSpeedStat;
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, false);
            base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
            Util.PlaySound(FireBarrage.fireBarrageSoundString, base.gameObject);
            base.AddRecoil(-0.6f, 0.6f, -0.6f, 0.6f);
            if (FireBarrage.effectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(FireBarrage.effectPrefab, base.gameObject, "MuzzleRight", false);
            }
            if (base.isAuthority)
            {
                //Blast Attack Stuff
                List<GameObject> blasted2 = new List<GameObject>();
                vectorMiddle = aimRay.GetPoint(RiftDistance());

                Vector3 rhs1 = Vector3.Cross(Vector3.up, aimRay.direction);
                Vector3 axis = Vector3.Cross(aimRay.direction, rhs1);
                Quaternion quaternionRight = Quaternion.AngleAxis(30, axis);
                Quaternion quaternionLeft = Quaternion.AngleAxis(-30, axis);

                Vector3 rightAngle = quaternionRight * aimRay.direction.normalized;
                rightBlast = new Ray(aimRay.origin, rightAngle);
                vectorRight = rightBlast.GetPoint(RiftDistance());

                Vector3 leftAngle = quaternionLeft * aimRay.direction.normalized;
                leftBlast = new Ray(aimRay.origin, leftAngle);
                vectorLeft = leftBlast.GetPoint(RiftDistance());

                //if (Physics.Raycast(aimRay, out var endPoint, RiftDistance(), LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
                //{
                //    float hitMiddle = endPoint.distance;
                //    vectorMiddle = aimRay.GetPoint(hitMiddle);
                //}
                if (Physics.Raycast(leftBlast, out var leftPoint, RiftDistance(), LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
                {
                    float hitLeft = leftPoint.distance;
                    vectorLeft = leftBlast.GetPoint(hitLeft);
                }
                if (Physics.Raycast(rightBlast, out var rightPoint, RiftDistance(), LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
                {
                    float hitRight = rightPoint.distance;
                    vectorRight = rightBlast.GetPoint(hitRight);
                }

                float vectorDistanceLeft = Vector3.Distance(aimRay.origin, vectorLeft);
                if (vectorDistanceLeft < RiftDistance() * 2 / 3)
                {
                    float float1 = RiftDistance() - vectorDistanceLeft + 1.1f;
                    decimal dec = new decimal(float1);
                    double d = (double)dec;
                    double isRiftHitGroundDouble = 1 / Math.Log(d, 3.5);
                    isLeftHitGround = (float)isRiftHitGroundDouble;
                }
                else
                {
                    isLeftHitGround = 1f;
                }

                //float vectorDistanceMiddle = Vector3.Distance(aimRay.origin, vectorMiddle);
                //if (vectorDistanceMiddle + BlastRadius() < RiftDistance())
                //{
                //    float float1 = RiftDistance() - vectorDistanceMiddle + 1.1f;
                //    decimal dec = new decimal(float1);
                //    double d = (double)dec;
                //    double isRiftHitGroundDouble = 1 / Math.Log(d, 6.7);
                //    isMiddleHitGround = (float)isRiftHitGroundDouble;
                //}
                //else
                //{
                //    isMiddleHitGround = 1f;
                //}

                float vectorDistanceRight = Vector3.Distance(aimRay.origin, vectorRight);
                if (vectorDistanceRight < RiftDistance() * 2 / 3)
                {
                    float float1 = RiftDistance() - vectorDistanceRight + 1.1f;
                    decimal dec = new decimal(float1);
                    double d = (double)dec;
                    double isRiftHitGroundDouble = 1 / Math.Log(d, 3.5);
                    isRightHitGround = (float)isRiftHitGroundDouble;
                }
                else
                {
                    isRightHitGround = 1f;
                }

                BlastAttack.Result[] eResults = new BlastAttack.Result[2];
                SecondaryBlast(leftBlast, vectorLeft, isLeftHitGround, RiftDistance(), BlastRadius(), out eResults[0]);
                //SecondaryBlast(middleBlast, vectorMiddle, isMiddleHitGround, RiftDistance(), BlastRadius(), out eResults[1]);
                SecondaryBlast(rightBlast, vectorRight, isRightHitGround, RiftDistance(), BlastRadius(), out eResults[1]);

                for (int i = 0; i < eResults.Length; i++)
                {
                    foreach (var hit in eResults[i].hitPoints)
                    {
                        if (hit.hurtBox != null)
                        {
                            if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                            {
                                ignoreList2.AddDistinct(hurtBox.healthComponent.gameObject);
                                if (IsOvercharged() && hurtBox.healthComponent.alive)
                                {
                                    BlastOvercharge(result);
                                }
                            }

                        }
                    }

                }
                bool shotL = false;

                BulletAttack bulletAttackL = new BulletAttack();
                bulletAttackL.owner = base.gameObject;
                bulletAttackL.weapon = base.gameObject;
                bulletAttackL.origin = vectorLeft;
                bulletAttackL.aimVector = -leftBlast.direction;
                bulletAttackL.minSpread = 0f;
                bulletAttackL.maxSpread = base.characterBody.spreadBloomAngle;
                bulletAttackL.damage = base.characterBody.damage * 1.2f;
                bulletAttackL.bulletCount = 1U;
                bulletAttackL.procCoefficient = 0f;
                bulletAttackL.falloffModel = BulletAttack.FalloffModel.DefaultBullet;
                bulletAttackL.radius = .75f;
                bulletAttackL.tracerEffectPrefab = RiftGauntletBase.tracerEffectPrefab;
                bulletAttackL.muzzleName = "MuzzleRight";
                bulletAttackL.hitEffectPrefab = this.hitEffectPrefab;
                bulletAttackL.isCrit = false;
                bulletAttackL.HitEffectNormal = false;
                bulletAttackL.stopperMask = LayerIndex.playerBody.mask;
                bulletAttackL.smartCollision = true;
                bulletAttackL.maxDistance = vectorDistanceLeft;


                bulletAttackL.modifyOutgoingDamageCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo) //changed to _bulletAttack
                {
                    if (hitInfo.hitHurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        if (IsOvercharged() && hurtBox.healthComponent.alive)
                        {
                            Overcharge(hitInfo, hurtBox);
                        }
                    }

                };

                bulletAttackL.filterCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo)
                {
                    HealthComponent healthComponent = (hitInfo.hitHurtBox ? hitInfo.hitHurtBox.healthComponent : null);
                    if (healthComponent && healthComponent.alive && ignoreList2.Contains(healthComponent.gameObject))
                    {
                        return false;

                    }
                    return (!hitInfo.entityObject || (object)hitInfo.entityObject != _bulletAttack.owner) && BulletAttack.defaultFilterCallback(_bulletAttack, ref hitInfo);
                };

                bulletAttackL.Fire();

                BulletAttack bulletAttackR = new BulletAttack();
                bulletAttackR.owner = base.gameObject;
                bulletAttackR.weapon = base.gameObject;
                bulletAttackR.origin = vectorRight;
                bulletAttackR.aimVector = -rightBlast.direction;
                bulletAttackR.minSpread = 0f;
                bulletAttackR.maxSpread = base.characterBody.spreadBloomAngle;
                bulletAttackR.damage = base.characterBody.damage * 1f;
                bulletAttackR.bulletCount = 1U;
                bulletAttackR.procCoefficient = 0f;
                bulletAttackR.falloffModel = BulletAttack.FalloffModel.DefaultBullet;
                bulletAttackR.radius = .75f;
                bulletAttackR.tracerEffectPrefab = RiftGauntletBase.tracerEffectPrefab;
                bulletAttackR.muzzleName = "MuzzleRight";
                bulletAttackR.hitEffectPrefab = this.hitEffectPrefab;
                bulletAttackR.isCrit = false;
                bulletAttackR.HitEffectNormal = false;
                bulletAttackR.stopperMask = LayerIndex.playerBody.mask;
                bulletAttackR.smartCollision = true;
                bulletAttackR.maxDistance = vectorDistanceRight;


                bulletAttackR.modifyOutgoingDamageCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo) //changed to _bulletAttack
                {
                    if (hitInfo.hitHurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        if (IsOvercharged() && hurtBox.healthComponent.alive)
                        {
                            Overcharge(hitInfo, hurtBox);
                        }
                    }

                };

                bulletAttackR.filterCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo)
                {
                    HealthComponent healthComponent = (hitInfo.hitHurtBox ? hitInfo.hitHurtBox.healthComponent : null);
                    if (healthComponent && healthComponent.alive && ignoreList2.Contains(healthComponent.gameObject))
                    {
                        return false;

                    }
                    return (!hitInfo.entityObject || (object)hitInfo.entityObject != _bulletAttack.owner) && BulletAttack.defaultFilterCallback(_bulletAttack, ref hitInfo);
                };


                bulletAttackR.Fire();

            }
        }

        public override float RiftDistance()
        {
            return RifterStaticValues.riftSecondaryDistance;
        }

        public override float BlastRadius()
        {
            return 7.75f;
        }

        public override float BlastDamage()
        {
            return base.characterBody.damage * RifterStaticValues.secondaryRiftCoefficient + 1.0f;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public void SecondaryBlast(Ray aimRay, Vector3 position, float isHitGround, float riftPrimaryDistance2, float radius, out BlastAttack.Result result)
        {
            BlastAttack blastAttack = new BlastAttack();
            blastAttack.attacker = base.gameObject;
            blastAttack.inflictor = base.gameObject;
            blastAttack.teamIndex = TeamIndex.None;
            blastAttack.radius = radius;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseDamage = BlastDamage() * isHitGround;
            blastAttack.crit = base.RollCrit();
            blastAttack.procCoefficient = .8f;
            blastAttack.canRejectForce = false;
            blastAttack.position = position;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
            blastAttack.AddModdedDamageType(Damage.riftDamage);
            result = blastAttack.Fire();



        }

        public override void RunDistanceAssist(Vector3 vector, BlastAttack.Result result)
        {
            return;
        }
    }
}