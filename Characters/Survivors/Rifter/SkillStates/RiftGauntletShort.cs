

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using System;
using IL.RoR2.Skills;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftGauntletShort : BaseSkillState
    {
        public static float damageCoefficient = RifterStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1.0f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;

        //added vars for bulletAttack
        private Vector3 forwardBackwardAmount;
        public float riftPrimaryDistance1;
        public float riftPrimaryDistance2;

        //added vars for blastAttack

        //Interface stuff
        public int currentTeleportActive { get; set; }


        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;




        //OnEnter() runs once at the start of the skill
        //All we do here is create a BulletAttack and fire it
        public override void OnEnter()
        {
            base.OnEnter();

            this.duration = RiftGauntlet.baseDuration / base.attackSpeedStat;
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


                BulletAttack bulletAttack = new BulletAttack();
                bulletAttack.owner = base.gameObject;
                bulletAttack.weapon = base.gameObject;
                bulletAttack.origin = aimRay.origin;
                bulletAttack.aimVector = aimRay.direction;
                bulletAttack.minSpread = 0f;
                bulletAttack.maxSpread = base.characterBody.spreadBloomAngle;
                bulletAttack.damage = base.characterBody.damage * .8f;
                bulletAttack.bulletCount = 1U;
                bulletAttack.procCoefficient = .5f;
                bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
                bulletAttack.radius = 1f;
                bulletAttack.tracerEffectPrefab = RiftGauntlet.tracerEffectPrefab;
                bulletAttack.muzzleName = "MuzzleRight";
                bulletAttack.hitEffectPrefab = this.hitEffectPrefab;
                bulletAttack.isCrit = false;
                bulletAttack.HitEffectNormal = false;
                bulletAttack.stopperMask = LayerIndex.world.mask;
                bulletAttack.smartCollision = true;
                bulletAttack.maxDistance = RifterStaticValues.riftSecondaryDistance;




                //Blast Attack Stuff
                Vector3 vector = aimRay.GetPoint(RifterStaticValues.riftSecondaryDistance);

                if (Physics.Raycast(aimRay, out var endPoint, RifterStaticValues.riftSecondaryDistance, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
                {
                    float hit = endPoint.distance;
                    vector = aimRay.GetPoint(hit);
                }
                float radius = 6.5f;
                float vectorDistance = Vector3.Distance(aimRay.origin, vector);
                float isRiftHitGround;
                if (vectorDistance + radius < RifterStaticValues.riftSecondaryDistance)
                {
                    float float1 = RifterStaticValues.riftSecondaryDistance - vectorDistance + 1.1f;
                    decimal dec = new decimal(float1);
                    double d = (double)dec;
                    double isRiftHitGroundDouble = 1 / Math.Log(d, 4.5);
                    isRiftHitGround = (float)isRiftHitGroundDouble;
                }
                else
                {
                    isRiftHitGround = 1f;
                }

                bool blasted = false;
                HurtBox component1;

                BlastAttack blastAttack = new BlastAttack();
                blastAttack.attacker = base.gameObject;
                blastAttack.inflictor = base.gameObject;
                blastAttack.teamIndex = TeamIndex.None;
                blastAttack.radius = radius;
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseDamage = base.characterBody.damage * 4f * isRiftHitGround;
                blastAttack.crit = base.RollCrit();
                blastAttack.procCoefficient = 1f;
                blastAttack.canRejectForce = false;
                blastAttack.position = vector;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                var result = blastAttack.Fire();

                EffectData effectData2 = new EffectData();
                effectData2.origin = blastAttack.position;
                EffectManager.SpawnEffect(GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab, effectData2, transmit: false);


                foreach (var hit in result.hitPoints)
                {
                    if (hit.hurtBox != null)
                    {
                        blasted = true;
                    }
                };

                bulletAttack.modifyOutgoingDamageCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo) //changed to _bulletAttack
                {
                    float componentWeight;
                    damageInfo.canRejectForce = false;
                    Rigidbody rigidbody = hitInfo.entityObject.GetComponent<Rigidbody>();
                    componentWeight = rigidbody.mass;


                    if (Math.Abs(hitInfo.distance) < Math.Abs(RifterStaticValues.riftSecondaryDistance)/2)
                    {
                        damageInfo.force *= 28 * componentWeight / (1 + Math.Abs(hitInfo.distance) / 2);
                    }


                    ModelLocator modelLocator = hitInfo.entityObject.GetComponent<ModelLocator>();
                            if (modelLocator == null)
                            {
                                return;
                            }
                            if (modelLocator != null && base.characterBody.GetBuffCount(Rifter.RifterBuffs.riftTeleportableBuff) > 0)
                            {
                                HealthComponent enemyHit = modelLocator.gameObject.GetComponent<HealthComponent>();
                                if (enemyHit == null)
                                {
                                    UnityEngine.Debug.Log("null");
                                    return;
                                }
                                Vector3 enemyAngleVector = base.GetAimRay().direction * Vector3.Angle(base.GetAimRay().origin, hitInfo.point);
                                Ray enemyRayHit = new Ray(base.GetAimRay().origin, enemyAngleVector);
                                Vector3 enemyTeleportTo = enemyRayHit.GetPoint(RifterStaticValues.riftSecondaryDistance);
                        if (Physics.Raycast(enemyRayHit, out var hitPoint, RifterStaticValues.riftPrimaryDistance, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
                        {
                            float hit = endPoint.distance;
                            enemyTeleportTo = aimRay.GetPoint(hit);
                        }

                        ModifiedTeleport teleport = enemyHit.gameObject.AddComponent<ModifiedTeleport>(); ;
                                teleport.body = enemyHit.body;
                                teleport.targetFootPosition = enemyTeleportTo;
                            }
                        
                    
                };

                bulletAttack.filterCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo) //changed to _bulletAttack
                {
                    component1 = hitInfo.collider.GetComponent<HurtBox>();
                    if (component1 && blasted == true)
                    {
                        return false;
                    }
                    return (!hitInfo.entityObject || (object)hitInfo.entityObject != bulletAttack.owner) && BulletAttack.defaultFilterCallback(bulletAttack, ref hitInfo);
                };

                bulletAttack.Fire();
                
                
            }
        }

        //This method runs once at the end
        //Here, we are doing nothing
        public override void OnExit()
        {
            if (base.characterBody.GetBuffCount(Rifter.RifterBuffs.riftTeleportableBuff) > 0)
            {
                base.characterBody.RemoveBuff(Rifter.RifterBuffs.riftTeleportableBuff);
            }
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
            return InterruptPriority.PrioritySkill;
        }




    }
}