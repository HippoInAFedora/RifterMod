

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RoR2;
using UnityEngine;
using System;
using RifterMod.Modules;
using R2API;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftGauntletBase : RiftBase
    {
        public float baseDuration = 0.7f;
        public bool shot = false;
        public bool shouldDistanceAssist;
        float isRiftHitGround;

        public bool isBlastOvercharge = false;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        public float duration;


        //added vars for blastAttack

        //Interface stuff
        public int currentTeleportActive { get; set; }

        public RifterStep rifterStep;

        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;

        List<GameObject> ignoreList1 = new List<GameObject>();
        List<GameObject> ignoreList2 = new List<GameObject>();


        //OnEnter() runs once at the start of the skill
        //All we do here is create a BulletAttack and fire it
        public override void OnEnter()
        {
            base.OnEnter();


            rifterStep = base.GetComponent<RifterStep>();

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
                RiftAndFracture();                           
            }
        }

        //This method runs once at the end
        //Here, we are doing nothing
        public override void OnExit()
        {
            ignoreList1.RemoveAll(x => x != null);
            ignoreList2.RemoveAll(x => x != null);
            base.OnExit(); 
        }


        //FixedUpdate() runs almost every frame of the skill
        //Here, we end the skill once it exceeds its intended duration
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
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
            Ray aimRay = base.GetAimRay();
            //Blast Attack Stuff
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

            BlastAttack blastAttack = new BlastAttack();
            blastAttack.attacker = base.gameObject;
            blastAttack.inflictor = base.gameObject;
            blastAttack.teamIndex = TeamIndex.None;
            blastAttack.radius = BlastRadius();
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseDamage = BlastDamage() * isRiftHitGround;
            blastAttack.crit = base.RollCrit();
            blastAttack.procCoefficient = .8f;
            blastAttack.canRejectForce = false;
            blastAttack.position = vector;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
            blastAttack.AddModdedDamageType(Damage.riftDamage);
            var result = blastAttack.Fire();

            EffectData effectData2 = new EffectData();
            effectData2.origin = blastAttack.position;
            EffectManager.SpawnEffect(GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab, effectData2, transmit: false);

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


            BulletAttack bulletAttack = new BulletAttack();
            bulletAttack.owner = base.gameObject;
            bulletAttack.weapon = base.gameObject;
            bulletAttack.origin = vector;
            bulletAttack.aimVector = -aimRay.direction;
            bulletAttack.minSpread = 0f;
            bulletAttack.maxSpread = base.characterBody.spreadBloomAngle;
            bulletAttack.damage = base.characterBody.damage * 1f;
            bulletAttack.bulletCount = 1U;
            bulletAttack.procCoefficient = 0f;
            bulletAttack.falloffModel = BulletAttack.FalloffModel.DefaultBullet;
            bulletAttack.radius = .75f;
            bulletAttack.tracerEffectPrefab = RiftGauntletBase.tracerEffectPrefab;
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

        public override float RiftDistance()
        {
            return RifterStaticValues.riftPrimaryDistance;
        }

        public override float BlastRadius()
        {
            return 7f;
        }

        public override float BlastDamage()
        {
            return base.characterBody.damage * RifterStaticValues.primaryRiftCoefficient;
        }

        public override bool IsOvercharged()
        {
            if (rifterStep.rifterStep <= 0)
            {
                return false;
            }
            return true;
        }

        


        public override void Overcharge(BulletAttack.BulletHit hitInfo, HurtBox hurtBox)
        {
            HealthComponent enemyHit = hurtBox.healthComponent;
            Vector3 enemyTeleportTo = GetTeleportLocation(enemyHit.body);
            if (enemyHit.body && enemyHit.alive)
            {
                TryTeleport(enemyHit.body, enemyTeleportTo);
            }
        }

        public override void BlastOvercharge(BlastAttack.Result result)
        {
            foreach (var hit in result.hitPoints)
            {
                if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                {
                    HealthComponent enemyHit = hurtBox.healthComponent;
                    if (enemyHit == null)
                    {
                        UnityEngine.Debug.Log("null");
                        return;
                    }
                    Vector3 enemyTeleportTo = GetTeleportLocation(enemyHit.body);
                    if (enemyHit.body)
                    {
                        TryTeleport(enemyHit.body, enemyTeleportTo);
                    }
                }
                
            }          
        }

        public virtual void RunDistanceAssist(Vector3 vector, BlastAttack.Result result)
        {
            Ray aimRay = base.GetAimRay();

            BulletAttack bulletAttack = new BulletAttack();
            bulletAttack.owner = base.gameObject;
            bulletAttack.weapon = base.gameObject;
            bulletAttack.origin = vector;
            bulletAttack.aimVector = -aimRay.direction;
            bulletAttack.minSpread = 0f;
            bulletAttack.maxSpread = base.characterBody.spreadBloomAngle;
            bulletAttack.damage = BlastDamage() * isRiftHitGround;
            bulletAttack.bulletCount = 1U;
            bulletAttack.procCoefficient = .8f;
            bulletAttack.falloffModel = BulletAttack.FalloffModel.DefaultBullet;
            bulletAttack.radius = BlastRadius() / 2;
            bulletAttack.tracerEffectPrefab = RiftGauntletBase.tracerEffectPrefab;
            bulletAttack.muzzleName = "MuzzleRight";
            bulletAttack.hitEffectPrefab = this.hitEffectPrefab;
            bulletAttack.isCrit = false;
            bulletAttack.HitEffectNormal = false;
            bulletAttack.stopperMask = LayerIndex.playerBody.mask;
            bulletAttack.smartCollision = true;
            bulletAttack.maxDistance = BlastRadius();
            bulletAttack.AddModdedDamageType(Damage.riftAssistDamage);


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


    }
}