
using R2API;
using RifterMod.Modules;
using RifterMod.Survivors.Rifter;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class Recursion : RiftBase
    {
        

        public Vector3 basePosition;
        float stopwatch;
        float blastWatch;

        public float recursionRadius;
        public float recursionDamage;
        public int blastNum;
        public int blastMax;

        public EntityStates.EntityState setNextState = null;

        BlastAttack blastAttack;
        float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = 1f;
            blastNum = 0;
            if (base.isAuthority)
            {
                basePosition = base.transform.position;              
            }
            
        }

        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (base.isAuthority)
            {
                stopwatch += Time.fixedDeltaTime;
                if (blastNum == 0)
                {
                    Fire();
                    blastNum++;
                }
                
                if (blastNum != blastMax)
                {
                    blastWatch += Time.fixedDeltaTime;
                    if (blastWatch > duration / 5)
                    {
                        Fire();
                        blastNum++;
                        blastWatch = 0;
                    }

                }

                if (stopwatch >= duration && base.isAuthority)
                {
                    if (cameraTargetParams)
                    {
                        cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Standard);
                    }
                    if (setNextState != null)
                    {
                        outer.SetNextState(setNextState);
                    }
                    outer.SetNextStateToMain();
                }
            }
        }

        public override void OnExit()
        {

            base.OnExit();
        }

        public override float RiftDistance()
        {
            return RifterStaticValues.riftPrimaryDistance;
        }


        public override Vector3 GetTeleportLocation(CharacterBody body)
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
            if (Physics.SphereCast(base.characterBody.corePosition, 0.05f, direction, out raycastHit, RifterStaticValues.riftPrimaryDistance, LayerIndex.world.mask, QueryTriggerInteraction.Collide))
            {
                position = raycastHit.point;
            }
            return position;
        }

        protected void ModifyBlastOvercharge(BlastAttack.Result result)
        {
            foreach (var hit in result.hitPoints)
            {
                if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                {
                    CharacterBody enemyHit = hurtBox.healthComponent.body;
                    if (enemyHit == null)
                    {
                        UnityEngine.Debug.Log("null");
                        return;
                    }

                    enemyHit.TryGetComponent(out CharacterMotor motor);
                    enemyHit.TryGetComponent(out RigidbodyMotor rbmotor);

                    Vector3 enemyTeleportTo = GetTeleportLocation(enemyHit);
                    if (motor || rbmotor)
                    {
                        TryTeleport(enemyHit, enemyTeleportTo);
                    }
                }

            }
        }

        private void Fire()
        {
            blastAttack = new BlastAttack();
            blastAttack.attacker = base.gameObject;
            blastAttack.inflictor = base.gameObject;
            blastAttack.teamIndex = TeamIndex.None;
            blastAttack.radius = BlastRadius();
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseDamage = BlastDamage();
            blastAttack.crit = base.RollCrit();
            blastAttack.procCoefficient = ProcCoefficient();
            blastAttack.canRejectForce = false;
            blastAttack.position = basePosition;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
            blastAttack.AddModdedDamageType(Damage.riftDamage);
            var result = blastAttack.Fire();

            EffectData effectData2 = new EffectData();
            effectData2.origin = blastAttack.position;
            EffectManager.SpawnEffect(GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab, effectData2, transmit: false);

            foreach (var hit in result.hitPoints)
            {
                if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                {
                    if (blastNum == blastMax - 1 && hurtBox.healthComponent.alive)
                    {
                        ModifyBlastOvercharge(result);
                    }                 
                }
            };
        }

        public override float BlastRadius()
        {
            return 10f * (float)Math.Pow((double)RifterStaticValues.overchargedCoefficient, (double)blastNum);
        }

        public override float BlastDamage()
        {
            return base.characterBody.damage * RifterStaticValues.recursionCoefficient * (float)Math.Pow((double)RifterStaticValues.overchargedCoefficient, (double)blastNum);
        }

        public virtual float ProcCoefficient()
        {
            return blastNum / (blastMax + 1);
        }
    }
}
    

