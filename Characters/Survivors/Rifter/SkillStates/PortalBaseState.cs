

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
using static UnityEngine.UI.GridLayoutGroup;
using RoR2.Projectile;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;
using UnityEngine.UIElements;
using System.Linq;
using R2API.Networking.Interfaces;
using R2API.Networking;
using RifterMod.Modules.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    internal class PortalBaseState : EntityState
    {
        public static GameObject tracerEffectPrefabOvercharged = RifterAssets.fractureLineTracerOvercharged;

        public GameObject hitEffectPrefab = RifterAssets.hitEffect;

        public List<CharacterBody> teleportBodies = new List<CharacterBody>();
        public List<CharacterBody> teleportA = new List<CharacterBody>();

        public GameObject owner;

        public PortalController portalController;

        public Vector3 pointAPosition;
        public Vector3 pointBPosition;

        public GameObject blastEffectPrefab = RifterAssets.slipstreamOutEffect;

        public static float stopwatch;

        public static float tickRate = .2f;

        public int fireChecker = 0;


        public override void OnEnter()
        {
            base.OnEnter();

            fireChecker++;
            portalController = GetComponent<PortalController>();
            pointAPosition = portalController.pointATransform.position;
            pointBPosition = portalController.pointBTransform.position;
            owner = GetComponent<GenericOwnership>().ownerObject;
            if (NetworkServer.active)
            {
                CheckPortal(pointAPosition, pointBPosition);
                CheckPortal(pointBPosition, pointAPosition);
                if (fireChecker >= 5)
                {
                    fireChecker = 0;
                    Fracture(pointAPosition, true);
                    Fracture(pointBPosition, false);
                }
            }         
            Teleport();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (Util.HasEffectiveAuthority(base.gameObject) && base.fixedAge > tickRate)
            {
                outer.SetNextState(new PortalBaseState
                {
                    fireChecker = fireChecker,
                });
            }
        }

        private void CheckPortal(Vector3 pointPosition, Vector3 otherPosition)
        {
            SphereSearch sphereSearch = new SphereSearch
            {
                origin = pointPosition,
                radius = 5f,
                mask = LayerIndex.entityPrecise.mask
            };
            List<HurtBox> list = new List<HurtBox>();
            sphereSearch.RefreshCandidates().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes(list);
            foreach (HurtBox item in list)
            {
                if (!(item.healthComponent?.body != null))
                {
                    continue;
                }
                if (item.healthComponent.body.HasBuff(RifterBuffs.postTeleport))
                {
                    break;
                }
                if (item.healthComponent.body.teamComponent.teamIndex == TeamIndex.Player)
                {
                    EntityStateMachine[] components = item.healthComponent.body.GetComponents<EntityStateMachine>();
                    EntityStateMachine[] array = components;
                    foreach (EntityStateMachine entityStateMachine in array)
                    {
                        if (entityStateMachine.state.GetType() != typeof(Idle))
                        {
                            entityStateMachine.SetNextState(new Idle());
                        }
                    }
                    NetMessageExtensions.Send((INetMessage)(object)new TeleportOnBodyRequest(item.healthComponent.body.masterObjectId, item.healthComponent.body.corePosition, otherPosition, true), NetworkDestination.Clients);
                };
            }
        }

        public void Fracture(Vector3 pointPosition, bool shouldFracture)
        {
            CharacterBody body = owner.GetComponent<CharacterBody>();
            if (shouldFracture)
            {
                BulletAttack bulletAttack = new BulletAttack();
                bulletAttack.owner = owner;
                bulletAttack.weapon = portalController.pointATransform.gameObject;
                bulletAttack.muzzleName = portalController.pointATransform.name;
                bulletAttack.origin = pointPosition;
                bulletAttack.aimVector = pointBPosition - pointAPosition;
                bulletAttack.minSpread = 0f;
                bulletAttack.damage = body.damage * RifterStaticValues.fractureCoefficient;
                bulletAttack.bulletCount = 1U;
                bulletAttack.procCoefficient = 0f;
                bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
                bulletAttack.radius = .75f;
                bulletAttack.tracerEffectPrefab = tracerEffectPrefabOvercharged;
                bulletAttack.hitEffectPrefab = hitEffectPrefab;
                bulletAttack.isCrit = false;
                bulletAttack.HitEffectNormal = false;
                bulletAttack.stopperMask = LayerIndex.noCollision.mask;
                bulletAttack.smartCollision = false;
                bulletAttack.maxDistance = Vector3.Distance(pointAPosition, pointBPosition);
                bulletAttack.Fire();

                bulletAttack.modifyOutgoingDamageCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo) //changed to _bulletAttack
                {
                    if (hitInfo.hitHurtBox != null)
                    {
                        if (hitInfo.hitHurtBox.TryGetComponent(out HurtBox hurtBox))
                        {
                            CharacterBody bulletBody = hurtBox.gameObject.GetComponent<CharacterBody>();
                            if (hurtBox.healthComponent.alive)
                            {
                                teleportBodies.AddDistinct(bulletBody);
                            }
                        }
                    }
                };

            }

            BlastAttack blastAttack = new BlastAttack();
            blastAttack.attacker = owner;
            blastAttack.teamIndex = TeamIndex.Player;
            blastAttack.radius = 10f;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseDamage = body.damage * RifterStaticValues.portalBlast;
            blastAttack.procCoefficient = .8f;
            blastAttack.canRejectForce = false;
            blastAttack.position = pointPosition;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
            BlastAttack.Result result = blastAttack.Fire();
            foreach (var hit in result.hitPoints)
            {
                if (hit.hurtBox != null)
                {
                    if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        CharacterBody body2 = hurtBox.healthComponent.body;
                        if (!body2.HasBuff(RifterBuffs.postTeleport) && body2.teamComponent.teamIndex != TeamIndex.Player)
                        {
                            teleportBodies.AddDistinct(body2);
                        }
                        if (shouldFracture)
                        {
                            teleportA.AddDistinct(body2);
                        }
                    }
                }
            }

            EffectData effectData = new EffectData();
            effectData.scale = blastAttack.radius / 2;
            effectData.origin = blastAttack.position;
            EffectManager.SpawnEffect(blastEffectPrefab, effectData, transmit: true);

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
                    modifiedTeleport.attackerAndInflictor = owner;
                    modifiedTeleport.isPortalTeleport = true;
                    setStateOnHurt.targetStateMachine.SetInterruptState(modifiedTeleport, InterruptPriority.Frozen);
                }
                EntityStateMachine[] array = setStateOnHurt.idleStateMachine;
                for (int i = 0; i < array.Length; i++)
                {
                    array[i].SetNextStateToMain();
                };
            }
        }

        public virtual void Teleport()
        {
            for (int i = 0; i < teleportBodies.Count; i++)
            {               
                CharacterBody body = teleportBodies[i];
                Vector3 position = teleportA.Contains(body) ? portalController.pointBTransform.position : portalController.pointATransform.position;
                if (body)
                {
                    TryTeleport(body,position);
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }


        public override void OnExit()
        {
            base.OnExit();
            teleportBodies.RemoveAll(x => x != null);
            teleportA.RemoveAll(x => x != null);
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            for (int i = 0; i < teleportBodies.Count; i++)
            {
                writer.Write(teleportBodies[i].netId);
                writer.Write(teleportA[i].netId);
            }

        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            while (reader.Position < reader.Length)
            {
                teleportBodies.Add(Util.FindNetworkObject(reader.ReadNetworkId()).GetComponent<CharacterBody>());
                teleportA.Add(Util.FindNetworkObject(reader.ReadNetworkId()).GetComponent<CharacterBody>());
            }

        }
    }
}