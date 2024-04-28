

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using System;
using System.Collections.Generic;
using IL.RoR2.Skills;
using static UnityEngine.SendMouseEvents;
using R2API;
using UnityEngine.AddressableAssets;
using static RoR2.Skills.SkillFamily;
using RifterMod.Characters.Survivors.Rifter.Components;
using UnityEngine.Networking;
using Newtonsoft.Json.Utilities;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftBase : BaseSkillState
    {
        private float duration;

        public float isBlastOvercharge;
        RifterOverchargePassive rifterStep;

        public static GameObject blastEffectPrefab = RifterAssets.riftExplosionEffect;
        public static GameObject overchargedEffectPrefab = RifterAssets.riftExplosionEffectOvercharged;

        public CharacterBody enemyHit;
        public Vector3 originalPosition;
        public Vector3 enemyTeleportTo;

 

        public List<CharacterBody> enemyBodies = new List<CharacterBody>();

        public float maxSlopeAngle = 90;




        //OnEnter() runs once at the start of the skill
        //All we do here is create a BulletAttack and fire it
        public override void OnEnter()
        {
            base.OnEnter();

            //TeleportEnemies();
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
        }

        //GetMinimumInterruptPriority() returns the InterruptPriority required to interrupt this skill
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public virtual float RiftDistance()
        {
            return RifterStaticValues.riftPrimaryDistance;
        }

        public virtual float BlastRadius()
        {
            return 7.5f;
        }

        public virtual float BlastDamage()
        {
            return characterBody.damage * RifterStaticValues.primaryRiftCoefficient;
        }

        public virtual bool IsOvercharged()
        {
            if (rifterStep.rifterOverchargePassive <= 0 || !rifterStep.rapidfireShot)
            {
                return false;
            }
            return true;
        }


        public virtual void Overcharge(BulletAttack.BulletHit hitInfo, HurtBox hurtBox)
        {
            HealthComponent enemyHitHealthbox = hurtBox.healthComponent;
            enemyHit = enemyHitHealthbox.body;
            //enemyHit.TryGetComponent(out CharacterMotor motor);
            //enemyHit.TryGetComponent(out RigidbodyMotor rbmotor);
            if (RifterPlugin.blacklistBodyNames.Contains(enemyHit.name))
            {
                Debug.Log("notgettingteleported");
                //Add Effect here later
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
                        Debug.Log("notgettingteleported");
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
                CharacterBody body = enemyBodies[i];
                originalPosition = body.gameObject.transform.position;
                enemyTeleportTo = GetTeleportLocation(body);
                body.TryGetComponent(out RigidbodyMotor rbmotor);
                body.TryGetComponent(out CharacterMotor motor);
                if (motor || rbmotor)
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
                    setStateOnHurt.targetStateMachine.SetInterruptState(modifiedTeleport, InterruptPriority.Frozen);
                }
                EntityStateMachine[] array = setStateOnHurt.idleStateMachine;
                for (int i = 0; i < array.Length; i++)
                {
                    array[i].SetNextStateToMain();
                };

            }

        }

        public virtual Vector3 GetTeleportLocation(CharacterBody body)
        {
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
            Vector3 direction = (location - characterBody.corePosition).normalized;
            RaycastHit raycastHit;
            Vector3 position = location;
            float distance = Vector3.Distance(body.corePosition, location);
            if (Physics.SphereCast(body.corePosition, 0.05f, direction, out raycastHit, distance, LayerIndex.world.mask, QueryTriggerInteraction.Collide))
            {
                bool normalPlacement = Vector3.Angle(Vector3.up, raycastHit.normal) < maxSlopeAngle;
                if (normalPlacement)
                {
                    position = raycastHit.point;
                }
                if (!normalPlacement)
                {
                    position = raycastHit.point - direction.normalized;
                }
            }
            return position;
        }

        
    }
}