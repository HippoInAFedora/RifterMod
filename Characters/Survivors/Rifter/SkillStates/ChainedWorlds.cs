
using EntityStates;
using Newtonsoft.Json.Utilities;
using R2API;
using RifterMod.Modules;
using RifterMod.Survivors.Rifter;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class ChainedWorlds : RiftBase
    {

        public Vector3 numPosition;
        float stopwatch;
        float blastWatch;

        public float recursionRadius;
        public float recursionDamage;
        public int blastNum;
        public int blastMax;

        Ray aimRay;

        public EntityStates.EntityState setNextState = null;

        BlastAttack blastAttack;
        float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = 1.5f;
            blastNum = 0;
            aimRay = base.GetAimRay();
        }

        private Vector3 GetNumPosition(int num)
        {
            float num2 = RiftDistance() / 5 * (num + 1);
            Vector3 location = aimRay.GetPoint(num2);
            Vector3 position = location;
            if (Physics.SphereCast(base.characterBody.corePosition, 0.05f, aimRay.direction, out var raycastHit, num2, LayerIndex.world.mask, QueryTriggerInteraction.Collide))
            {
                position = raycastHit.point;
            }
            Debug.Log(position + "position and num2 is " + num2);
            return position;
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority)
            {
                stopwatch += Time.fixedDeltaTime;
                if (blastNum == 0)
                {
                    numPosition = GetNumPosition(blastNum);
                    Fire();
                    blastNum++;
                }

                if (blastNum != blastMax)
                {
                    blastWatch += Time.fixedDeltaTime;
                    if (blastWatch > duration / 5)
                    {
                        numPosition = GetNumPosition(blastNum);
                        Fire();
                        blastNum++;
                        blastWatch = 0;
                    }

                }

                if (stopwatch >= duration && base.isAuthority || base.isAuthority && blastNum >= blastMax)
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
            Vector3 position = GetNumPosition(blastNum + 1);
            return position;
        }

        protected virtual void ModifyBlastOvercharge(BlastAttack.Result result)
        {
            foreach (var hit in result.hitPoints)
            {
                if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                {
                    enemyHit = hurtBox.healthComponent.body;
                    if (enemyHit == null)
                    {
                        UnityEngine.Debug.Log("null");
                        return;
                    }
                    if (RifterPlugin.blacklistBodyNames.Contains(enemyHit.name))
                    {
                        Debug.Log("notgettingteleported");
                        //Add Effect here later
                        return;
                    }
                    //enemyBodies.AddDistinct(enemyHit);
                    enemyTeleportTo = GetTeleportLocation(enemyHit);
                    enemyHit.TryGetComponent(out CharacterMotor motor);
                    enemyHit.TryGetComponent(out RigidbodyMotor rbmotor);
                    if (motor || rbmotor)
                    {
                        TryTeleport(enemyHit, enemyTeleportTo);
                    }

                }

            }
        }

        public override void TryTeleport(CharacterBody body, Vector3 teleportToPosition)
        {
            if (base.isAuthority)
            {
                if (body.TryGetComponent(out SetStateOnHurt setStateOnHurt))
                {
                    if (setStateOnHurt.targetStateMachine)
                    {
                        ModifiedTeleport modifiedTeleport = new ModifiedTeleport();
                        modifiedTeleport.targetFootPosition = teleportToPosition;
                        modifiedTeleport.teleportWaitDuration = .05f;
                        setStateOnHurt.targetStateMachine.SetInterruptState(modifiedTeleport, InterruptPriority.Frozen);
                    }
                    EntityStateMachine[] array = setStateOnHurt.idleStateMachine;
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i].SetNextStateToMain();
                    };
                }
            }



        }

        private void Fire()
        {
            blastAttack = new BlastAttack();
            blastAttack.attacker = base.gameObject;
            blastAttack.inflictor = base.gameObject;
            blastAttack.teamIndex = TeamIndex.Player;
            blastAttack.radius = BlastRadius();
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseDamage = BlastDamage();
            blastAttack.crit = base.RollCrit();
            blastAttack.procCoefficient = ProcCoefficient();
            blastAttack.canRejectForce = false;
            blastAttack.position = GetNumPosition(blastNum);
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
            blastAttack.AddModdedDamageType(Damage.riftDamage);
            var result = blastAttack.Fire();

            EffectData effectData = new EffectData();
            blastEffectPrefab.transform.localScale = Vector3.one;
            effectData.scale = BlastRadius() * 1.5f;
            effectData.origin = blastAttack.position;
            EffectManager.SpawnEffect(overchargedEffectPrefab, effectData, transmit: true);

            foreach (var hit in result.hitPoints)
            {
                if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                {
                    ModifyBlastOvercharge(result);
                }
            };
            //TeleportEnemies();
        }

        public override float BlastRadius()
        {
            return 12f / (float)Math.Pow((double)RifterStaticValues.overchargedCoefficient, (double)blastNum);
        }

        public override float BlastDamage()
        {
            return base.characterBody.damage * RifterStaticValues.chainedWorldsCoefficient * (float)Math.Pow((double)RifterStaticValues.overchargedCoefficient, (double)blastNum);
        }

        public virtual float ProcCoefficient()
        {
            return blastNum / (blastMax + 1);
        }

    }
}


