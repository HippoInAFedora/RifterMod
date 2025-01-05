

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RifterMod.Characters.Survivors.Rifter.Components;
using Newtonsoft.Json.Utilities;
using System;
using R2API;
using UnityEngine.Networking;
using System.Security.Cryptography;
using HG;
using UnityEngine.UIElements;
using System.Linq;
using System.Reflection;
using R2API.Networking.Interfaces;
using R2API.Networking;
using RifterMod.Modules.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftBase : BaseSkillState
    {
        public float duration;
        public float baseDuration = 0.7f;

        public bool usesOvercharge = false;
        public bool shot = false;
        public bool shouldDistanceAssist = false;
        float isRiftHitGround;
        public bool fractureHitCallback = true;
        public bool shouldBuckshot = false;
        public bool blast = true;
        public bool blastPulls = false;
        public Vector3 timelockInstancePosition;
        public GameObject timelockInstanceGameObject;
        public NetworkIdentity timelockInstanceGameObjectNetID;


        public bool hitTimelock;
        public Collider hitTimelockCollider;
        public Collider hitRiftProjectileCollider;
        public int hitEnemies;

        public RifterOverchargePassive rifterStep;

        public static GameObject blastEffectPrefab = RifterAssets.riftExplosionEffect;
        public static GameObject overchargedEffectPrefab = RifterAssets.riftExplosionEffectOvercharged;

        public static GameObject tracerEffectPrefab = RifterAssets.fractureLineTracer;
        public static GameObject tracerEffectPrefabOvercharged = RifterAssets.fractureLineTracerOvercharged;
        public static GameObject tracer;

        public static GameObject hitEffectPrefab = RifterAssets.hitEffect;
        public static GameObject muzzleEffectPrefab = RifterAssets.muzzleEffect;

        public CharacterBody enemyHit;
        public Vector3 originalPosition;
        public Vector3 enemyTeleportTo;

        public static event Action<List<CharacterBody>> onTeleportEnemies;

        public static event Action<GameObject> onPrimaryShot;

        List<GameObject> ignoreList1 = new List<GameObject>();
        //List<GameObject> ignoreList2 = new List<GameObject>();

        public List<CharacterBody> enemyBodies = new List<CharacterBody>();

        public float maxSlopeAngle = 90;

        SphereSearch search;

        public Animator modelAnimator;

        private int primaryRiftStateHash = Animator.StringToHash("Primary");
        private int primaryRiftPlaybackHash = Animator.StringToHash("ShootRift.playbackRate");

        private int secondaryFractureStateHash = Animator.StringToHash("Armature_Secondary");
        private int secondaryFracturePlaybackHash = Animator.StringToHash("Secondary.playbackRate");

        public bool isPrimary = true;
        public bool shouldAnimate = true;
        //public bool playAnim = true;

        public override void OnEnter()
        {
            base.OnEnter();
            tracer = IsOvercharged() ? tracerEffectPrefabOvercharged : tracerEffectPrefab;
            rifterStep = base.GetComponent<RifterOverchargePassive>();
            if (rifterStep.deployedList.Count > 0 )
            {
                timelockInstanceGameObject = rifterStep.deployedList.First();
                timelockInstancePosition = timelockInstanceGameObject.transform.position;
            }         
            if (NetworkServer.active && isPrimary)
            {
                onPrimaryShot?.Invoke(base.gameObject);
            }

            this.duration = baseDuration / attackSpeedStat;
            Ray aimRay = GetAimRay();
            StartAimMode(aimRay, 2f, false);
            modelAnimator = GetModelAnimator();
            if (modelAnimator && shouldAnimate)
            {
                PlayCrossfade("Gesture, Override", primaryRiftStateHash, primaryRiftPlaybackHash, .2f, .1f);
                //else
                //{
                //    PlayCrossfade("Gesture, Override", secondaryFractureStateHash, secondaryFracturePlaybackHash, .05f, .05f);
                //}
            }
            //base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
            Util.PlaySound(FireBarrage.fireBarrageSoundString, gameObject);
            AddRecoil(-0.6f, 0.6f, -0.6f, 0.6f);
            if ((bool)muzzleEffectPrefab && shouldAnimate)
            {
                EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, "Muzzle", false);
            }
           

        }

        //This method runs once at the end
        //Here, we are doing nothing
        public override void OnExit()
        {
            ignoreList1.RemoveAll(x => x != null);
            //ignoreList2.RemoveAll(x => x != null);
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

        public void GetPosition(Ray aimRay, out Vector3 position)
        {
            position = aimRay.GetPoint(RiftDistance());
            if (Physics.Raycast(aimRay, out var endPoint, RiftDistance(), LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
            {
                float hit = endPoint.distance;
                position = aimRay.GetPoint(hit);
            }
            Search(position);                                 
            if (hitTimelockCollider != null)
            {
                position = hitTimelockCollider.transform.position;
            }         
        }

        public void Search(Vector3 position)
        {
            search = new SphereSearch();
            search.origin = position;
            search.radius = BlastRadius();
            search.mask = LayerIndex.defaultLayer.mask;
            search.queryTriggerInteraction = QueryTriggerInteraction.Collide;
            List<Collider> list = CollectionPool<Collider, List<Collider>>.RentCollection();
            search.RefreshCandidates().FilterCandidatesByColliderEntities().FilterCandidatesByDistinctColliderEntities()
            .GetColliders(list);
            for (int i = 0; i < list.Count; i++)
            {
                DestroyOnRift component = list[i].GetComponent<EntityLocator>().entity.GetComponent<DestroyOnRift>();
                if (component)
                {
                    hitTimelock = true;
                    hitTimelockCollider = list[i];
                    component.destroying = true;
                    break;
                }
            }
            CollectionPool<Collider, List<Collider>>.ReturnCollection(list);

        }

        public void Search2(Vector3 position)
        {
            search = new SphereSearch();
            search.origin = position;
            search.radius = BlastRadius();
            search.mask = LayerIndex.entityPrecise.mask;
            search.queryTriggerInteraction = QueryTriggerInteraction.Collide;

            List<HurtBox> listH = CollectionPool<HurtBox, List<HurtBox>>.RentCollection();
            search.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.AllExcept(TeamIndex.Player)).FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes(listH);
            hitEnemies = listH.Count;
            CollectionPool<HurtBox, List<HurtBox>>.ReturnCollection(listH);
        }


        public void GetDistance(Ray aimRay, Vector3 position, out float vectorDistance)
        {
            vectorDistance = Vector3.Distance(aimRay.origin, position);
        }

        public void Blast(Ray aimRay, Vector3 position, float distance, DamageSource damageSource)
        {
            bool flag = false;

            isRiftHitGround = CalcDistanceFalloff(distance);

            //if (distance < RiftDistance() * 2 / 3)
            //{
            //    float float1 = RiftDistance() - distance + 1.1f;
            //    decimal dec = new decimal(float1);
            //    double d = (double)dec;
            //    double isRiftHitGroundDouble = 1 / Math.Log(d, 3.5);
            //    isRiftHitGround = (float)isRiftHitGroundDouble;
            //}
            //else
            //{
            //    isRiftHitGround = 1f;
            //}
            if (distance >= RiftDistance() - BlastRadius() && !hitTimelock)
            {
                flag = true;
            }

            if (hitTimelock)
            {
                Search2(position);
            }

            BlastAttack blastAttack = new BlastAttack();
            blastAttack.attacker = gameObject;
            blastAttack.inflictor = gameObject;
            blastAttack.teamIndex = TeamIndex.Player;
            blastAttack.radius = BlastRadius();
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseDamage = BlastDamage() * isRiftHitGround;
            blastAttack.crit = RollCrit();
            blastAttack.baseForce = blastPulls ? -250f : 0f;
            blastAttack.procCoefficient = 1f;
            blastAttack.canRejectForce = false;
            blastAttack.position = position;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
            blastAttack.AddModdedDamageType(RifterDamage.riftDamage);
            blastAttack.damageType.damageSource = damageSource;
            BlastAttack.Result result = blastAttack.Fire();

            EffectData effectData = new EffectData();
            effectData.scale = BlastRadius() / 10f;
            effectData.origin = position;
            if (!IsOvercharged() || !hitTimelock)
            {
                EffectManager.SpawnEffect(blastEffectPrefab, effectData, transmit: true);
            }
            else
            {
                EffectManager.SpawnEffect(overchargedEffectPrefab, effectData, transmit: true);
            }
            

            foreach (var hit in result.hitPoints)
            {
                if (hit.hurtBox != null)
                {
                    if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        ignoreList1.AddDistinct(hurtBox.healthComponent.gameObject);
                        //ignoreList2.AddDistinct(hurtBox.healthComponent.gameObject);
                        EffectData effectData2 = new EffectData();
                        effectData2.scale = hurtBox.healthComponent.body.radius;
                        effectData2.origin = hit.hitPosition;
                        if (hitEffectPrefab)
                        {
                            EffectManager.SpawnEffect(hitEffectPrefab, effectData2, transmit: true);
                        }

                        if (IsOvercharged() && hurtBox.healthComponent.alive || hitTimelock && hurtBox.healthComponent.alive || timelockInstanceGameObject != null)
                        {
                            BlastOvercharge(result);
                        }
                    }

                }
            };
        }

        public void Fracture(Ray aimRay, float vectorDistance, LayerIndex index, DamageSource damageSource)
        {
            BulletAttack bulletAttack = new BulletAttack();
            bulletAttack.owner = gameObject;
            bulletAttack.weapon = gameObject;
            bulletAttack.origin = aimRay.origin;
            bulletAttack.aimVector = aimRay.direction;
            bulletAttack.minSpread = 0f;
            bulletAttack.maxSpread = characterBody.spreadBloomAngle;
            bulletAttack.damage = base.characterBody.damage * RifterStaticValues.fractureCoefficient;
            bulletAttack.bulletCount = 1U;
            bulletAttack.procCoefficient = 1f;
            bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
            bulletAttack.radius = .75f;
            bulletAttack.tracerEffectPrefab = tracer;
            bulletAttack.muzzleName = "MuzzleRight";
            bulletAttack.hitEffectPrefab = hitEffectPrefab;
            bulletAttack.isCrit = false;
            bulletAttack.HitEffectNormal = false;
            bulletAttack.stopperMask = index.mask;
            bulletAttack.smartCollision = true;
            bulletAttack.maxDistance = vectorDistance;
            bulletAttack.damageType.damageSource = damageSource;

            bulletAttack.hitCallback = fractureHitCallback ? delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo)
            {
                float distance = Vector3.Distance(_bulletAttack.origin, hitInfo.point);
                float distanceMultiplier = CalcDistanceFalloff(distance);
                _bulletAttack.damage = base.characterBody.damage * RifterStaticValues.fractureCoefficientMax * distanceMultiplier;
                return BulletAttack.DefaultHitCallbackImplementation(_bulletAttack, ref hitInfo);
            } : BulletAttack.defaultHitCallback;

            bulletAttack.modifyOutgoingDamageCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo) //changed to _bulletAttack
            {
                if (hitInfo.hitHurtBox != null)
                {
                    if (hitInfo.hitHurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        if (IsOvercharged() && hurtBox.healthComponent.alive || timelockInstanceGameObject != null)
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


        private float CalcDistanceFalloff(float distance)
        {
            //if (distance < RiftDistance() * 2 / 3)
            //{
            //    float float1 = RiftDistance() - distance + 1.1f;
            //    decimal dec = new decimal(float1);
            //    double d = (double)dec;
            //    double isRiftHitGroundDouble = 1 / Math.Log(d, 3.5);
            //    isRiftHitGround = (float)isRiftHitGroundDouble;
            //}
            //else
            //{
            //    isRiftHitGround = 1f;
            //}
            float multiplier = 0.6f + Mathf.Clamp01(Mathf.InverseLerp(RiftDistance(), RiftDistance() * .3f, RiftDistance() - distance)) * .4f;
            return multiplier;
        }

        //private void RunDistanceAssist(Vector3 vector, BlastAttack.Result result)
        //{
        //    Ray aimRay = GetAimRay();

        //    BulletAttack bulletAttack = new BulletAttack();
        //    bulletAttack.owner = gameObject;
        //    bulletAttack.weapon = gameObject;
        //    bulletAttack.origin = vector;
        //    bulletAttack.aimVector = -aimRay.direction;
        //    bulletAttack.minSpread = 0f;
        //    bulletAttack.maxSpread = characterBody.spreadBloomAngle;
        //    bulletAttack.damage = BlastDamage() * isRiftHitGround;
        //    bulletAttack.bulletCount = 1U;
        //    bulletAttack.procCoefficient = .8f;
        //    bulletAttack.falloffModel = BulletAttack.FalloffModel.DefaultBullet;
        //    bulletAttack.radius = BlastRadius() / 2f;
        //    bulletAttack.muzzleName = "MuzzleRight";
        //    bulletAttack.hitEffectPrefab = this.hitEffectPrefab;
        //    bulletAttack.isCrit = false;
        //    bulletAttack.HitEffectNormal = false;
        //    bulletAttack.stopperMask = LayerIndex.playerBody.mask;
        //    bulletAttack.smartCollision = true;
        //    bulletAttack.maxDistance = BlastRadius() * .8f;
        //    bulletAttack.AddModdedDamageType(RifterDamage.riftDamage);


        //    bulletAttack.modifyOutgoingDamageCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo) //changed to _bulletAttack
        //    {
        //        if (hitInfo.hitHurtBox != null)
        //        {
        //            if (hitInfo.hitHurtBox.TryGetComponent(out HurtBox hurtBox))
        //            {
        //                ignoreList2.AddDistinct(hurtBox.healthComponent.gameObject);

        //                if (IsOvercharged())
        //                {
        //                    Overcharge(hitInfo, hurtBox);
        //                }
        //            }
        //        }
        //    };


        //    bulletAttack.filterCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo)
        //    {
        //        HealthComponent healthComponent = (hitInfo.hitHurtBox ? hitInfo.hitHurtBox.healthComponent : null);
        //        if (healthComponent && healthComponent.alive && ignoreList1.Contains(healthComponent.gameObject))
        //        {
        //            return false;

        //        }
        //        return (!hitInfo.entityObject || (object)hitInfo.entityObject != _bulletAttack.owner) && BulletAttack.defaultFilterCallback(_bulletAttack, ref hitInfo);
        //    };


        //    bulletAttack.Fire();
        //}

        public virtual void Buckshot(Vector3 origin)
        {
        }

        public virtual float RiftDistance()
        {
            return RifterStaticValues.riftPrimaryDistance;
        }

        public virtual float BlastRadius()
        {
            if (hitTimelock)
            {
                return RifterStaticValues.timelockRadius;
            }
            return RifterStaticValues.blastRadius;
        }

        public virtual float BlastDamage()
        {
            if (hitTimelock)
            {
                return base.characterBody.damage * RifterStaticValues.timelockBlast + (base.characterBody.damage * hitEnemies);
            }
            return characterBody.damage * RifterStaticValues.primaryRiftCoefficient;
        }

        public virtual bool IsOvercharged()
        {
            if (!usesOvercharge)
            {
                return false;
            }
            return true;
        }

        public virtual void Overcharge(BulletAttack.BulletHit hitInfo, HurtBox hurtBox)
        {
            HealthComponent enemyHitHealthbox = hurtBox.healthComponent;
            enemyHit = enemyHitHealthbox.body;
            if (RifterPlugin.blacklistBodyNames.Contains(enemyHit.name))
            {
                return;
            }
            enemyBodies.AddDistinct(enemyHit);
        }

        public virtual void BlastOvercharge(BlastAttack.Result result)
        {
            foreach (var hit in result.hitPoints)
            {
                if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                {
                    HealthComponent enemyHitHealthbox = hurtBox.healthComponent;
                    enemyHit = enemyHitHealthbox.body;
                    if (RifterPlugin.blacklistBodyNames.Contains(enemyHit.name))
                    {
                        //Add Effect here later
                        return;
                    }
                    enemyBodies.AddDistinct(enemyHit);
                }

            }
        }

        public virtual void TeleportEnemies()
        {
            for (int i = 0; i < enemyBodies.Count; i++)
            {
                if (enemyBodies[i].teamComponent.teamIndex == TeamIndex.Player && RifterConfig.teleportYourFriends.Value == false)
                {
                    return;
                }
                CharacterBody body = enemyBodies[i];
                originalPosition = body.gameObject.transform.position;
                enemyTeleportTo = GetTeleportLocation(body);
                //body.TryGetComponent(out RigidbodyMotor rbmotor);
                //body.TryGetComponent(out CharacterMotor motor);
                if (body)
                {
                    TryTeleport(body, enemyTeleportTo);
                }

            }

        }

        public virtual void TryTeleport(CharacterBody body, Vector3 teleportToPosition)
        {
            if (body.TryGetComponent(out SetStateOnHurt setStateOnHurt))
            {
                if (setStateOnHurt.targetStateMachine)
                {
                    ModifiedTeleport modifiedTeleport = new ModifiedTeleport();
                    modifiedTeleport.targetFootPosition = teleportToPosition;
                    modifiedTeleport.teleportWaitDuration = .25f;
                    modifiedTeleport.attackerAndInflictor = base.gameObject;
                    if (hitTimelock)
                    {
                        modifiedTeleport.damageOutput = base.characterBody.damage * RifterStaticValues.buckshotWeakRiftCoefficient;
                    }
                    setStateOnHurt.targetStateMachine.SetInterruptState(modifiedTeleport, InterruptPriority.Frozen);
                }
                EntityStateMachine[] array = setStateOnHurt.idleStateMachine;
                for (int i = 0; i < array.Length; i++)
                {
                    array[i].SetNextStateToMain();
                };
            }
            InvokeTeleportAction();
        }

        public void InvokeTeleportAction()
        {
            onTeleportEnemies?.Invoke(enemyBodies);
        }

        public virtual Vector3 GetTeleportLocation(CharacterBody body)
        {
            if (hitTimelock)
            {
                Ray aimRay = base.GetAimRay();
                GetPosition(aimRay, out Vector3 positionBase);
                Vector3 baseDirection1 = (body.corePosition - positionBase).normalized;
                Ray ray1 = new Ray(positionBase, baseDirection1);
                Vector3 location1;
                if (body.isFlying || !body.characterMotor.isGrounded)
                {
                    location1 = ray1.GetPoint(RifterStaticValues.riftPrimaryDistance);
                }
                else
                {
                    location1 = ray1.GetPoint(RifterStaticValues.riftPrimaryDistance) + (Vector3.up);
                }
                Vector3 direction1 = (location1 - characterBody.corePosition).normalized;
                RaycastHit raycastHit1;
                Vector3 position1 = location1;
                float distance1 = Vector3.Distance(body.corePosition, location1);
                if (Physics.SphereCast(body.corePosition, 0.05f, direction1, out raycastHit1, distance1, LayerIndex.world.mask, QueryTriggerInteraction.Collide))
                {
                    //bool normalPlacement = Vector3.Angle(Vector3.up, raycastHit1.normal) < maxSlopeAngle;
                        position1 = raycastHit1.point + raycastHit1.normal;
                }
                return position1;
            }

            if (timelockInstanceGameObject != null)
            {
                Vector3 position2 = timelockInstancePosition + Vector3.up * .5f;
                return position2;
            }

            Vector3 baseDirection = (body.corePosition - characterBody.corePosition).normalized;
            Ray ray = new Ray(characterBody.corePosition, baseDirection);
            Vector3 location;
            if (body.isFlying || !body.characterMotor.isGrounded)
            {
                location = ray.GetPoint(RifterStaticValues.riftPrimaryDistance);
            }
            else
            {
                location = ray.GetPoint(RifterStaticValues.riftPrimaryDistance) + (Vector3.up);
            }
            Vector3 direction = (location - base.GetAimRay().origin).normalized;
            RaycastHit raycastHit;
            Vector3 position = location;
            float distance = Vector3.Distance(body.corePosition, location);
            if (Physics.SphereCast(base.GetAimRay().origin, 0.05f, direction, out raycastHit, RifterStaticValues.riftPrimaryDistance, LayerIndex.world.mask, QueryTriggerInteraction.Collide))
            {

                position = raycastHit.point - raycastHit.normal;
                
            }
            return position;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            //writer.Write(timelockInstanceGameObjectNetID.netId);
            writer.Write(enemyTeleportTo);
            for (int i = 0; i < enemyBodies.Count; i++)
            {
                writer.Write(enemyBodies[i].netId);
            }

        }


        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            //Util.FindNetworkObject(reader.ReadNetworkId());
            enemyTeleportTo = reader.ReadVector3();
            while (reader.Position < reader.Length)
            {
                enemyBodies.Add(Util.FindNetworkObject(reader.ReadNetworkId()).GetComponent<CharacterBody>());
            }

        }
    }
}