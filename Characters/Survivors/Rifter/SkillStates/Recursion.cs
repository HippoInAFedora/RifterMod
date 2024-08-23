
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
        public float stopwatch;
        public float blastWatch;

        public float recursionRadius;
        public float recursionDamage;
        public int blastNum;
        public int blastMax = 5;

        public EntityStates.EntityState setNextState = null;

        BlastAttack blastAttack;
        BlastAttack.Result result;
        public float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = 1.5f;
            blastNum++;
            Fire();
            Debug.Log(blastNum);
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
                blastAttack.AddModdedDamageType(RifterDamage.riftDamage);
                result = blastAttack.Fire();

                EffectData effectData = new EffectData();
                effectData.scale = BlastRadius() / 10f;
                effectData.origin = basePosition;
                if (blastNum < blastMax)
                {
                    //blastEffectPrefab.transform.GetChild(0).localScale = Vector3.one * BlastRadius() / 7.5f;    
                    //blastEffectPrefab.transform.GetChild(1).localScale = Vector3.one * BlastRadius() / 7.5f;
                    //blastEffectPrefab.transform.GetChild(2).localScale = Vector3.one * BlastRadius() / 7.5f;
                    EffectManager.SpawnEffect(blastEffectPrefab, effectData, transmit: true);
                    
                }
                else
                {

                    //overchargedEffectPrefab.transform.GetChild(0).localScale = Vector3.one * BlastRadius() / 7.5f;
                    //overchargedEffectPrefab.transform.GetChild(1).localScale = Vector3.one * BlastRadius() / 7.5f;
                    //overchargedEffectPrefab.transform.GetChild(2).localScale = Vector3.one * BlastRadius() / 7.5f;
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
            return 7.5f * (float)Math.Pow((double)RifterStaticValues.overchargedCoefficient, (double)blastNum);
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
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            blastNum = reader.ReadByte();
        }
    }
}


