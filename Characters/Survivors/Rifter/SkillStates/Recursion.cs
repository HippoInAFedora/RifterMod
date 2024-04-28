
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
        BlastAttack.Result result;
        float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = 1f;
            if (blastNum == 0)
            {
                basePosition = transform.position;
            }
            blastNum++;
            Fire();
            Debug.Log(blastNum);
            Debug.Log(blastMax);
            TeleportEnemies();
            
        }


        public void OldFixedUpdate()
        {
            base.FixedUpdate();
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

            if (stopwatch >= duration && isAuthority)
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
                    outer.SetNextState(new Recursion
                    {
                        blastNum = blastNum,
                        blastMax = blastMax,
                        basePosition = basePosition
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
            if (Physics.SphereCast(base.transform.position, 0.05f, direction, out raycastHit, RifterStaticValues.riftPrimaryDistance, LayerIndex.world.mask, QueryTriggerInteraction.Collide))
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

        //protected virtual void ModifyBlastOvercharge(BlastAttack.Result result)
        //{
        //    foreach (var hit in result.hitPoints)
        //    {
        //        if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
        //        {
        //            enemyHit = hurtBox.healthComponent.body;
        //            if (enemyHit == null)
        //            {
        //                Debug.Log("null");
         //               return;
        //            }
        //            if (RifterPlugin.blacklistBodyNames.Contains(enemyHit.name))
        //            {
        //                Debug.Log("notgettingteleported");
        //                //Add Effect here later
        //                return;
        //            }
        //            enemyBodies.AddDistinct(enemyHit);
        //        }
//
        //    }
        //}

        public void Fire()
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
                blastAttack.position = basePosition;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                blastAttack.AddModdedDamageType(Damage.riftDamage);
                result = blastAttack.Fire();

                EffectData effectData = new EffectData();
                blastEffectPrefab.transform.localScale = Vector3.one;
                effectData.scale = BlastRadius() * 1.5f;
                effectData.origin = basePosition;
                if (blastNum < blastMax)
                {
                    EffectManager.SpawnEffect(blastEffectPrefab, effectData, transmit: true);
                }
                else
                {
                    EffectManager.SpawnEffect(overchargedEffectPrefab, effectData, transmit: true);
                }

                foreach (var hit in result.hitPoints)
                {
                    if (hit.hurtBox.TryGetComponent(out HurtBox hurtBox))
                    {
                        if (IsOvercharged() && hurtBox.healthComponent.alive)
                        {
                            BlastOvercharge(result);
                        }
                    }
                };
            }

            
            //if (blastNum == blastMax - 1)
            //{
            //    TeleportEnemies();
            //}
           
        }

        public override bool IsOvercharged()
        {
            if (blastNum < blastMax)
            {
                return false;
            }
            return true;
        }

        public override float BlastRadius()
        {
            return 10f * (float)Math.Pow((double)RifterStaticValues.overchargedCoefficient, (double)blastNum);
        }

        public override float BlastDamage()
        {
            return characterBody.damage * RifterStaticValues.recursionCoefficient * (float)Math.Pow((double)RifterStaticValues.overchargedCoefficient, (double)blastNum);
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


