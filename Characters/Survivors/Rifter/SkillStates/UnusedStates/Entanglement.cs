using EntityStates;
using IL.RoR2.Skills;
using R2API;
using RifterMod.Survivors.Rifter;
using RifterMod.Survivors.Rifter.SkillStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;

namespace RifterMod.Characters.Survivors.Rifter.SkillStates.UnusedStates
{
    public class Entanglement : RiftBase
    {
        private Vector3 basePosition;

        public Vector3 targetFootPosition;

        private Transform modelTransform;

        private CharacterModel characterModel;

        private HurtBoxGroup hurtboxGroup;

        private float isRiftHitGround;

        float stopwatch;

        BlastAttack blastAttack;

        BlastAttack blastAttack2;

        float duration = .3f;

        public override void OnEnter()
        {
            base.OnEnter();
            basePosition = transform.position;
            modelTransform = GetModelTransform();
            if ((bool)modelTransform)
            {
                characterModel = modelTransform.GetComponent<CharacterModel>();
                hurtboxGroup = modelTransform.GetComponent<HurtBoxGroup>();
            }
            if ((bool)characterModel)
            {
                characterModel.invisibilityCount++;
            }
            if ((bool)hurtboxGroup)
            {
                HurtBoxGroup hurtBoxGroup = hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            }


            Ray aimRay = GetAimRay();
            targetFootPosition = aimRay.GetPoint(RiftDistance());

            if (Physics.Raycast(aimRay, out var endPoint, RiftDistance(), LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
            {
                float hit = endPoint.distance;
                targetFootPosition = aimRay.GetPoint(hit);
            }


            float vectorDistance = Vector3.Distance(aimRay.origin, targetFootPosition);
            if (vectorDistance + BlastRadius() < RiftDistance())
            {
                float float1 = RiftDistance() - vectorDistance + 1.1f;
                decimal dec = new decimal(float1);
                double d = (double)dec;
                double isRiftHitGroundDouble = 1 / Math.Log(d, 2.5);
                isRiftHitGround = (float)isRiftHitGroundDouble;
            }
            else
            {
                isRiftHitGround = 1f;
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.deltaTime;
            if (isAuthority)
            {
                if ((bool)characterMotor)
                {
                    characterMotor.velocity = Vector3.zero;
                }
                else if ((bool)rigidbodyMotor)
                {
                    rigidbodyMotor.moveVector = Vector3.zero;
                }
                else
                {
                    transform.position = Vector3.zero;
                }
                if ((bool)characterMotor && (bool)characterDirection)
                {
                    characterMotor.velocity = Vector3.zero;
                    characterMotor.rootMotion += GetAimRay().direction * (RifterStaticValues.riftPrimaryDistance / duration * Time.fixedDeltaTime);
                    characterDirection.forward = -characterDirection.forward;
                }

                if (modelTransform)
                {
                    modelTransform.SetPositionAndRotation(targetFootPosition, Util.QuaternionSafeLookRotation(basePosition - targetFootPosition));
                }

                if (stopwatch >= duration)
                {

                    outer.SetNextStateToMain();
                }
            }
        }

        public override void OnExit()
        {
            if (isAuthority)
            {

                blastAttack = new BlastAttack();
                blastAttack.attacker = gameObject;
                blastAttack.inflictor = gameObject;
                blastAttack.teamIndex = TeamIndex.None;
                blastAttack.radius = BlastRadius();
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseDamage = BlastDamage() * isRiftHitGround;
                blastAttack.crit = RollCrit();
                blastAttack.procCoefficient = 1f;
                blastAttack.canRejectForce = false;
                blastAttack.position = transform.position;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                blastAttack.AddModdedDamageType(RifterDamage.riftDamage);
                var result = blastAttack.Fire();

                blastAttack2 = new BlastAttack
                {
                    attacker = gameObject,
                    inflictor = gameObject,
                    teamIndex = TeamIndex.None,
                    radius = BlastRadius(),
                    falloffModel = BlastAttack.FalloffModel.None,
                    baseDamage = BlastDamage() * .75f,
                    crit = RollCrit(),
                    procCoefficient = 1f,
                    canRejectForce = false,
                    position = basePosition,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                };
                blastAttack2.AddModdedDamageType(RifterDamage.riftDamage);
                var result2 = blastAttack2.Fire();

                EffectData effectData2 = new EffectData();
                effectData2.origin = blastAttack.position;
                EffectManager.SpawnEffect(GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab, effectData2, transmit: false);

                foreach (var hit in result.hitPoints)
                {
                    if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        ModifyBlastOvercharge(result);
                    }
                };
                foreach (var hit in result2.hitPoints)
                {
                    if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        BlastOvercharge(result);
                    }
                };

                if (!outer.destroying)
                {
                    modelTransform = GetModelTransform();
                    if ((bool)modelTransform)
                    {
                        TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(modelTransform.gameObject);
                        temporaryOverlay.duration = 0.6f;
                        temporaryOverlay.animateShaderAlpha = true;
                        temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                        temporaryOverlay.destroyComponentOnEnd = true;
                        temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                        temporaryOverlay.AddToCharacterModel(modelTransform.GetComponent<CharacterModel>());
                        TemporaryOverlayInstance temporaryOverlay2 = TemporaryOverlayManager.AddOverlay(modelTransform.gameObject);
                        temporaryOverlay2.duration = 0.7f;
                        temporaryOverlay2.animateShaderAlpha = true;
                        temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                        temporaryOverlay2.destroyComponentOnEnd = true;
                        temporaryOverlay2.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
                        temporaryOverlay2.AddToCharacterModel(modelTransform.GetComponent<CharacterModel>());
                    }
                }
                if ((bool)characterModel)
                {
                    characterModel.invisibilityCount--;
                }
                if ((bool)hurtboxGroup)
                {
                    HurtBoxGroup hurtBoxGroup = hurtboxGroup;
                    int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
                    hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
                }
                if ((bool)characterMotor)
                {
                    characterMotor.disableAirControlUntilCollision = false;
                }
                base.OnExit();
            }

        }

        public override float RiftDistance()
        {
            return RifterStaticValues.riftPrimaryDistance;
        }

        public override float BlastRadius()
        {
            return 7f * RifterStaticValues.overchargedCoefficient;
        }

        public override float BlastDamage()
        {

            return characterBody.damage * RifterStaticValues.primaryRiftCoefficient * RifterStaticValues.overchargedCoefficient;

        }

        public override Vector3 GetTeleportLocation(CharacterBody body)
        {
            Vector3 baseDirection = (body.corePosition - characterBody.corePosition).normalized;
            Ray ray = new Ray(characterBody.corePosition, baseDirection);
            Vector3 location;
            if (body.isFlying || !body.characterMotor.isGrounded)
            {
                location = ray.GetPoint(RifterStaticValues.riftSecondaryDistance);
            }
            else
            {
                location = ray.GetPoint(RifterStaticValues.riftSecondaryDistance) + Vector3.up * 3f;
            }
            Vector3 direction = (location - characterBody.corePosition).normalized;
            RaycastHit raycastHit;
            Vector3 position = location;
            if (Physics.SphereCast(characterBody.corePosition, 0f, direction, out raycastHit, RifterStaticValues.riftSecondaryDistance, LayerIndex.world.mask, QueryTriggerInteraction.Collide))
            {
                position = raycastHit.point;
            }
            return position;
        }

        private void ModifyBlastOvercharge(BlastAttack.Result result)
        {
            foreach (var hit in result.hitPoints)
            {
                if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                {
                    HealthComponent enemyHit = hurtBox.healthComponent;
                    if (enemyHit == null)
                    {
                        Debug.Log("null");
                        return;
                    }
                    Vector3 enemyTeleportTo = basePosition + Vector3.up * .1f;
                    if (enemyHit.body && !enemyHit.body.isBoss)
                    {
                        TryTeleport(enemyHit.body, enemyTeleportTo);
                    }
                    if (enemyHit.body && enemyHit.body.isBoss)
                    {
                        TryTeleport(enemyHit.body, enemyTeleportTo);
                    }
                }

            };
        }

    }
}


