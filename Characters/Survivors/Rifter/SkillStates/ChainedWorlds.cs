
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
        public Vector3 basePosition;
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
            duration = 1.75f;
            aimRay = GetAimRay();

            if (blastNum == 0)
            {
                basePosition = base.transform.position;              
            }
            blastNum++;
            numPosition = GetNumPosition(blastNum);
            Debug.Log(blastNum);
            Fire();
            TeleportEnemies();
            
            
        }

        private Vector3 GetNumPosition(int num)
        {
            float num2 = RiftDistance() / 5 * (num);
            Vector3 location = aimRay.GetPoint(num2);
            Vector3 position = location;
            if (Physics.SphereCast(basePosition, 0.05f, aimRay.direction, out var raycastHit, num2, LayerIndex.world.mask, QueryTriggerInteraction.Collide))
            {
                position = raycastHit.point;
            }
            Debug.Log(position + "position and num2 is " + num2);
            return position;
        }


        public void OldFixedUpdate()
        {
            base.FixedUpdate();
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

            if (stopwatch >= duration && isAuthority || isAuthority && blastNum >= blastMax)
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

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            

            //if (blastNum != blastMax)
            //{
            //    blastWatch += Time.fixedDeltaTime;
            //    if (blastWatch > duration / 5)
            //    {
            //        numPosition = GetNumPosition(blastNum);
            //        Fire();
            //        blastNum++;
            //        blastWatch = 0;
            //    }

            //}

            if (stopwatch >= (duration / 5) && isAuthority)
            {
                if (cameraTargetParams)
                {
                    cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Standard);
                }
                if (blastNum < blastMax)
                {
                    outer.SetNextState(new ChainedWorlds
                    {
                        blastNum = blastNum,
                        blastMax = blastMax,
                        basePosition = basePosition,
                    });
                    return;
                }
                else
                {
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
                        Debug.Log("null");
                        return;
                    }
                    if (RifterPlugin.blacklistBodyNames.Contains(enemyHit.name))
                    {
                        Debug.Log("notgettingteleported");
                        //Add Effect here later
                        return;
                    }
                    enemyBodies.AddDistinct(enemyHit);
                    //enemyTeleportTo = GetTeleportLocation(enemyHit);
                    //enemyHit.TryGetComponent(out CharacterMotor motor);
                    //enemyHit.TryGetComponent(out RigidbodyMotor rbmotor);
                    //if (motor || rbmotor)
                    //{
                    //    TryTeleport(enemyHit, enemyTeleportTo);
                    //}

                }

            }
        }

        public override void TryTeleport(CharacterBody body, Vector3 teleportToPosition)
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

        private void Fire()
        {
            if (base.isAuthority)
            {
                blastAttack = new BlastAttack();
                blastAttack.attacker = gameObject;
                blastAttack.inflictor = gameObject;
                blastAttack.teamIndex = TeamIndex.Player;
                blastAttack.radius = BlastRadius();
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseDamage = BlastDamage();
                blastAttack.crit = RollCrit();
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
                    if (hit.hurtBox != null)
                    {
                        if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                        {
                            if (IsOvercharged() && hurtBox.healthComponent.alive)
                            {
                                BlastOvercharge(result);
                            }
                        }
                    }
                }
            }          
            //TeleportEnemies();
        }

        public override bool IsOvercharged()
        {
            return true;
        }
        public override float BlastRadius()

        {
            return 12f; /// (float)Math.Pow((double)RifterStaticValues.overchargedCoefficient, (double)blastNum);
        }

        public override float BlastDamage()
        {
            return characterBody.damage * RifterStaticValues.chainedWorldsCoefficient * (float)Math.Pow((double)RifterStaticValues.overchargedCoefficient, (double)blastNum);
        }

        public virtual float ProcCoefficient()
        {
            return blastNum / (blastMax + 1);
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write((byte)blastNum);
            writer.Write((byte)blastMax);
            writer.Write(enemyTeleportTo);
            for (int i = 0; i < enemyBodies.Count; i++)
            {
                writer.Write(enemyBodies[i].netId);
                Debug.Log("serialized enemy body count " + enemyBodies.Count);
            }

        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            blastNum = reader.ReadByte();
            blastMax = reader.ReadByte();
            enemyTeleportTo = reader.ReadVector3();
            while (reader.Position < reader.Length)
            {
                enemyBodies.Add(Util.FindNetworkObject(reader.ReadNetworkId()).GetComponent<CharacterBody>());
                Debug.Log("enemy body count " + enemyBodies.Count);
            }

        }
    }
}


