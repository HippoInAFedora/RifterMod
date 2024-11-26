

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

namespace RifterMod.Survivors.Rifter.SkillStates
{
    internal class PortalBaseState : EntityState
    {
        public static GameObject tracerEffectPrefabOvercharged = RifterAssets.fractureLineTracerOvercharged;

        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;

        public List<CharacterBody> teleportBodies = new List<CharacterBody>();

        public GameObject owner;

        public PortalController portalController;
        public GameObject blastEffectPrefab = RifterAssets.slipstreamOutEffect;

        public GameObject otherPortal;

        public static float stopwatch;

        public static float tickRate = .2f;

        public int fireChecker = 0;


        public override void OnEnter()
        {
            base.OnEnter();

            fireChecker++;
            portalController = GetComponent<PortalController>();
            otherPortal = portalController.otherPortal;
            owner = portalController.owner;
            if (Util.HasEffectiveAuthority(base.gameObject))
            {
                CheckPortal();
                if (fireChecker >= 5)
                {
                    fireChecker = 0;
                    Fracture();
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

        private void CheckPortal()
        {
            bool flag = false;
            float distance = Vector3.Distance(owner.transform.position, base.transform.position);
            if (distance < 5f)
            {
                flag = true;
            }

            BlastAttack blastAttack = new BlastAttack();
            blastAttack.attacker = portalController.owner;
            blastAttack.teamIndex = TeamIndex.None;
            blastAttack.radius = 5f;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseDamage = 0f;
            blastAttack.procCoefficient = 0f;
            blastAttack.canRejectForce = false;
            blastAttack.position = base.transform.position;
            blastAttack.attackerFiltering = flag? AttackerFiltering.AlwaysHitSelf : AttackerFiltering.NeverHitSelf;
            BlastAttack.Result result = blastAttack.Fire();
            foreach (var hit in result.hitPoints)
            {
                if (hit.hurtBox != null)
                {
                    if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        CharacterBody body2 = hurtBox.healthComponent.body;
                        if (!body2.HasBuff(RifterBuffs.postTeleport))
                        {
                            teleportBodies.AddDistinct(body2);
                        }

                    }
                }
            }
        }

        public void Fracture()
        {
            CharacterBody body = owner.GetComponent<CharacterBody>();
            if (portalController.isMain)
            {
                BulletAttack bulletAttack = new BulletAttack();
                bulletAttack.owner = portalController.owner;
                bulletAttack.weapon = base.gameObject;
                bulletAttack.origin = base.transform.position;
                bulletAttack.aimVector = (portalController.otherPortal.transform.position - base.transform.position).normalized;
                bulletAttack.minSpread = 0f;
                bulletAttack.damage = body.damage * RifterStaticValues.fractureCoefficient;
                bulletAttack.bulletCount = 1U;
                bulletAttack.procCoefficient = 0f;
                bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
                bulletAttack.radius = .75f;
                bulletAttack.tracerEffectPrefab = tracerEffectPrefabOvercharged;
                bulletAttack.hitEffectPrefab = this.hitEffectPrefab;
                bulletAttack.isCrit = false;
                bulletAttack.HitEffectNormal = false;
                bulletAttack.stopperMask = LayerIndex.noCollision.mask;
                bulletAttack.smartCollision = true;
                bulletAttack.maxDistance = Vector3.Distance(base.transform.position, portalController.otherPortal.transform.position);
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
            blastAttack.attacker = portalController.owner;
            blastAttack.teamIndex = TeamIndex.Player;
            blastAttack.radius = 10f;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseDamage = body.damage * RifterStaticValues.portalBlast;
            blastAttack.procCoefficient = .8f;
            blastAttack.canRejectForce = false;
            blastAttack.position = base.transform.position;
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
                    modifiedTeleport.attackerAndInflictor = portalController.owner;
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
                Vector3 position = otherPortal.transform.position + Vector3.up * .5f;
                CharacterBody body = teleportBodies[i];
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
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            for (int i = 0; i < teleportBodies.Count; i++)
            {
                writer.Write(teleportBodies[i].netId);
            }

        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            while (reader.Position < reader.Length)
            {
                teleportBodies.Add(Util.FindNetworkObject(reader.ReadNetworkId()).GetComponent<CharacterBody>());
            }

        }
    }
}