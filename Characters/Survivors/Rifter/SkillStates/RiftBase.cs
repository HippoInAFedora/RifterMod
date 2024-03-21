

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
    public class RiftBase : BaseSkillState
    {
        private float duration;

        public float isBlastOvercharge;

        RifterStep rifterStep;

        //OnEnter() runs once at the start of the skill
        //All we do here is create a BulletAttack and fire it
        public override void OnEnter()
        {
            base.OnEnter();
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
            return 7f;
        }

        public virtual float BlastDamage()
        {
            return base.characterBody.damage * RifterStaticValues.primaryRiftCoefficient;
        }

        public virtual bool IsOvercharged()
        {
            if (rifterStep.rifterStep <= 0 || !rifterStep.rapidfireShot)
            {
                return false;
            }
            return true;
        }


        public virtual void Overcharge(BulletAttack.BulletHit hitInfo, HurtBox hurtBox)
        {
            HealthComponent enemyHit = hurtBox.healthComponent;
            enemyHit.body.TryGetComponent(out CharacterMotor motor);
            enemyHit.body.TryGetComponent(out RigidbodyMotor rbmotor);

            Vector3 enemyTeleportTo = GetTeleportLocation(enemyHit.body);
            if (motor || rbmotor)
            {
                TryTeleport(enemyHit.body, enemyTeleportTo);
            }
        }

        public virtual void BlastOvercharge(BlastAttack.Result result)
        {
            foreach (var hit in result.hitPoints)
            {
                if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                {
                    HealthComponent enemyHit = hurtBox.healthComponent;
                    if (!enemyHit.body.hasEffectiveAuthority)
                    {
                        return;
                    }
                    Vector3 enemyTeleportTo = GetTeleportLocation(enemyHit.body);
                    enemyHit.body.TryGetComponent(out CharacterMotor motor);
                    enemyHit.body.TryGetComponent(out RigidbodyMotor rbmotor);
                    if (motor || rbmotor)
                    {
                        TryTeleport(enemyHit.body, enemyTeleportTo);
                    }
                }
                
            }          
        }

        public void TryTeleport(CharacterBody body, Vector3 teleportToPosition)
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
            Vector3 baseDirection = (body.corePosition - base.characterBody.corePosition).normalized;
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
            Vector3 direction = (location - base.characterBody.corePosition).normalized;           
            RaycastHit raycastHit;
            Vector3 position = location;
            float distance = Vector3.Distance(body.corePosition, location);
            if (Physics.SphereCast(body.corePosition, 0.05f, direction, out raycastHit, distance, LayerIndex.world.mask, QueryTriggerInteraction.Collide))
            {
                position = raycastHit.point;
            }
            return position;
        }
    }
}