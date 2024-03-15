

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using System;
using IL.RoR2.Skills;
using RifterMod.Modules;
using static UnityEngine.SendMouseEvents;
using R2API;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftGauntlet : RiftBase
    {
        public float baseDuration = 0.7f;
        public bool blasted = false;
        public bool shot = false;

        public bool isBlastOvercharge = false;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        public float duration;

        //added vars for blastAttack

        //Interface stuff
        public int currentTeleportActive { get; set; }

        public RifterStep rifterStep;

        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;




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
                //Blast Attack Stuff
                Vector3 vector = aimRay.GetPoint(RiftDistance());

                if (Physics.Raycast(aimRay, out var endPoint, RiftDistance(), LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
                {
                    float hit = endPoint.distance;
                    vector = aimRay.GetPoint(hit);
                }


                float vectorDistance = Vector3.Distance(aimRay.origin, vector);
                float isRiftHitGround;
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


                HurtBox component1;

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
                blastAttack.AddModdedDamageType(Damage.overchargedDamageType);
                var result = blastAttack.Fire();

                EffectData effectData2 = new EffectData();
                effectData2.origin = blastAttack.position;
                EffectManager.SpawnEffect(GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab, effectData2, transmit: false);

                foreach (var hit in result.hitPoints)
                {
                    if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        blasted = true;

                        if (IsOvercharged())
                        {   
                            if (isBlastOvercharge || IsOvercharged())
                            {
                                BlastOvercharge(result);
                            }
                            
                        }
                    }

                };




                

                BulletAttack bulletAttack = new BulletAttack();
                bulletAttack.owner = base.gameObject;
                bulletAttack.weapon = base.gameObject;
                bulletAttack.origin = vector;
                bulletAttack.aimVector = -aimRay.direction;
                bulletAttack.minSpread = 0f;
                bulletAttack.maxSpread = base.characterBody.spreadBloomAngle;
                bulletAttack.damage = base.characterBody.damage * 1.2f;
                bulletAttack.bulletCount = 1U;
                bulletAttack.procCoefficient = 0f;
                bulletAttack.falloffModel = BulletAttack.FalloffModel.DefaultBullet;
                bulletAttack.radius = .75f;
                bulletAttack.tracerEffectPrefab = RiftGauntlet.tracerEffectPrefab;
                bulletAttack.muzzleName = "MuzzleRight";
                bulletAttack.hitEffectPrefab = this.hitEffectPrefab;
                bulletAttack.isCrit = false;
                bulletAttack.HitEffectNormal = false;
                bulletAttack.stopperMask = LayerIndex.playerBody.mask;
                bulletAttack.smartCollision = true;
                bulletAttack.maxDistance = vectorDistance;


                bulletAttack.modifyOutgoingDamageCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo) //changed to _bulletAttack
                {
                    if (hitInfo.hitHurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        shot = true;
                        if (IsOvercharged())
                        {                     
                            Overcharge(hitInfo, hurtBox);
                        }
                    }
                    
                };

                bulletAttack.filterCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo) //changed to _bulletAttack
                {
                    if (hitInfo.hitHurtBox.TryGetComponent(out HurtBox hurtBox) && blasted == true)
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
            if (enemyHit.body && !enemyHit.body.isBoss)
            {
                TryTeleport(enemyHit.body, enemyTeleportTo);
            }
            if (enemyHit.body && (enemyHit.body.isBoss || enemyHit.body.isChampion))
            {
                ;
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
                    if (enemyHit.body && !enemyHit.body.isBoss)
                    {
                        TryTeleport(enemyHit.body, enemyTeleportTo);
                    }
                    if (enemyHit.body && enemyHit.body.isBoss)
                    {
                        enemyTeleportTo /= 2f;
                        TryTeleport(enemyHit.body, enemyTeleportTo);
                    }
                }
                
            }          
        }

        public override Vector3 GetTeleportLocation(CharacterBody body)
        {
            Vector3 baseDirection = base.GetAimRay().direction;
            Ray ray = new Ray(base.characterBody.corePosition, baseDirection);
            Vector3 location;
            if (body.isFlying || !body.characterMotor.isGrounded)
            {
                location = ray.GetPoint(RifterStaticValues.riftPrimaryDistance);
            }
            else
            {
                location = ray.GetPoint(RifterStaticValues.riftPrimaryDistance) + (Vector3.up);
            }
            Vector3 direction = baseDirection;
            RaycastHit raycastHit;
            Vector3 position = location;
            if (Physics.SphereCast(base.characterBody.corePosition, 0.1f, direction, out raycastHit, RifterStaticValues.riftPrimaryDistance, LayerIndex.world.mask, QueryTriggerInteraction.Collide))
            {
                position = raycastHit.point;
            }           
            return position;
        }
    }
}