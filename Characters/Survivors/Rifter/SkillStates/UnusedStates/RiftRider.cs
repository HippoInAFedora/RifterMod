

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using System;
using IL.RoR2.Skills;
using R2API;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;
using UnityEngine.Networking;
using RifterMod.Characters.Survivors.Rifter.Components;
using RifterMod.Survivors.Rifter.SkillStates;

namespace RifterMod.Characters.Survivors.Rifter.SkillStates.UnusedStates
{
    public class RiftRider : RiftBase
    {
        public bool isBlastOvercharge = true;

        public Vector3 initialPosition;

        public Vector3 targetFootPosition;

        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;

        public GameObject tracerEffectPrefab = RifterAssets.fractureLineTracer;

        private GameObject riftIn = RifterAssets.slipstreamInEffect;

        public bool isResults;

        List<GameObject> ignoreList1 = new List<GameObject>();

        public RifterOverchargePassive step;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = GetAimRay();
            step = GetComponent<RifterOverchargePassive>();
            initialPosition = characterBody.corePosition;
            if (NetworkServer.active)
            {
                Util.CleanseBody(characterBody, removeDebuffs: true, removeBuffs: false, removeCooldownBuffs: false, removeDots: true, removeStun: true, removeNearbyProjectiles: false);
            }
            if (isAuthority)
            {
                targetFootPosition = aimRay.GetPoint(RiftDistance());

                if (Physics.Raycast(aimRay, out var endPoint, RiftDistance(), LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
                {
                    float hit = endPoint.distance;
                    targetFootPosition = aimRay.GetPoint(hit);
                }


                float vectorDistance = Vector3.Distance(aimRay.origin, targetFootPosition);
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

                bool blasted = false;
                HurtBox component1;

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
                blastAttack.position = targetFootPosition;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                blastAttack.AddModdedDamageType(RifterDamage.riftDamage);
                var result = blastAttack.Fire();

                EffectData effectData = new EffectData();
                effectData.scale = BlastRadius() / 10f;
                effectData.origin = blastAttack.position;

                if (result.hitCount > 0)
                {
                    EffectManager.SpawnEffect(overchargedEffectPrefab, effectData, transmit: true);
                    step.rifterOverchargePassive++;
                }
                else
                {
                    EffectManager.SpawnEffect(blastEffectPrefab, effectData, transmit: true);
                }
                foreach (var hit in result.hitPoints)
                {
                    if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        ignoreList1.AddDistinct(hurtBox.healthComponent.gameObject);
                        isResults = true;

                        if (IsOvercharged() && hurtBox.healthComponent.alive && isBlastOvercharge)
                        {
                            BlastOvercharge(result);


                        }
                    }
                };






                BulletAttack bulletAttack = new BulletAttack();
                bulletAttack.owner = gameObject;
                bulletAttack.weapon = gameObject;
                bulletAttack.origin = targetFootPosition;
                bulletAttack.aimVector = -aimRay.direction;
                bulletAttack.minSpread = 0f;
                bulletAttack.maxSpread = characterBody.spreadBloomAngle;
                bulletAttack.damage = characterBody.damage * 1.2f;
                bulletAttack.bulletCount = 1U;
                bulletAttack.procCoefficient = 0f;
                bulletAttack.falloffModel = BulletAttack.FalloffModel.DefaultBullet;
                bulletAttack.radius = 4f;
                bulletAttack.tracerEffectPrefab = tracerEffectPrefab;
                bulletAttack.muzzleName = "MuzzleRight";
                bulletAttack.hitEffectPrefab = hitEffectPrefab;
                bulletAttack.isCrit = false;
                bulletAttack.HitEffectNormal = false;
                bulletAttack.stopperMask = LayerIndex.playerBody.mask;
                bulletAttack.smartCollision = true;
                bulletAttack.maxDistance = vectorDistance;


                bulletAttack.modifyOutgoingDamageCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo) //changed to _bulletAttack
                {
                    if (hitInfo.hitHurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        isResults = true;
                        if (IsOvercharged() && hurtBox.healthComponent.alive)
                        {

                            Overcharge(hitInfo, hurtBox);
                        }
                    }

                };

                bulletAttack.filterCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo)
                {
                    HealthComponent healthComponent = hitInfo.hitHurtBox ? hitInfo.hitHurtBox.healthComponent : null;
                    if (healthComponent && healthComponent.alive && ignoreList1.Contains(healthComponent.gameObject))
                    {
                        return false;
                    }
                    step.rifterOverchargePassive++;
                    return (!hitInfo.entityObject || (object)hitInfo.entityObject != _bulletAttack.owner) && BulletAttack.defaultFilterCallback(_bulletAttack, ref hitInfo);
                };

                bulletAttack.Fire();



                BlastAttack blastAttack2 = new BlastAttack();
                blastAttack2.attacker = gameObject;
                blastAttack2.inflictor = gameObject;

                blastAttack2.teamIndex = TeamIndex.Player;
                blastAttack2.radius = 10f;
                blastAttack2.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack2.baseDamage = RifterStaticValues.riftPrimaryDistance;
                blastAttack2.crit = RollCrit();
                blastAttack2.procCoefficient = .8f;
                blastAttack2.canRejectForce = false;
                blastAttack2.position = initialPosition;
                blastAttack2.attackerFiltering = AttackerFiltering.NeverHitSelf;
                blastAttack2.AddModdedDamageType(RifterDamage.riftDamage);
                var result2 = blastAttack.Fire();

                EffectData effectData3 = new EffectData();
                effectData3.origin = blastAttack2.position;
                effectData3.scale = blastAttack2.radius;
                EffectManager.SpawnEffect(riftIn, effectData3, transmit: false);

                foreach (var hit in result2.hitPoints)
                {
                    if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        ignoreList1.AddDistinct(hurtBox.healthComponent.gameObject);
                        isResults = true;
                        step.rifterOverchargePassive++;

                        if (IsOvercharged() && hurtBox.healthComponent.alive && isBlastOvercharge)
                        {

                            BlastOvercharge(result);

                        }
                    }

                };
            }
            TeleportEnemies();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority)
            {
                outer.SetNextState(new RiftRiderOut
                {
                    initialPosition = initialPosition,
                    targetFootPosition = targetFootPosition,
                    teleportWaitDuration = .5f,
                    isResults = isResults,
                    showEffect = true
                });
            }

        }

        public override void OnExit()
        {
            ignoreList1.RemoveAll(x => x != null);
            base.OnExit();
        }

        public override bool IsOvercharged()
        {
            return true;
        }

        public override Vector3 GetTeleportLocation(CharacterBody body)
        {
            return initialPosition;
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