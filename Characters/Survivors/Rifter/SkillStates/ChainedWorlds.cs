
using EntityStates;
using Newtonsoft.Json.Utilities;
using R2API;
using RifterMod.Characters.Survivors.Rifter.Components;
using RifterMod.Modules;
using RifterMod.Survivors.Rifter;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class ChainedWorlds : RiftBase
    {

        public Vector3 numPosition;
        public Vector3 basePosition;
        public Vector3 baseDirection;
        public Vector3 vector1;
        public Vector3 vector2;
        public float stopwatch;
        public float blastWatch;
        public float blastRadius;

        public float recursionRadius;
        public float recursionDamage;
        public int blastNum;
        public int blastMax;
        public RifterOverchargePassive riftPassive;
        public DamageAPI.ModdedDamageType damageType;

        Ray aimRay;

        public EntityStates.EntityState setNextState = null;


        BlastAttack blastAttack;

        public override void OnEnter()
        {
            shouldAnimate = blastNum == 0 ? true : false;
            damageType = blastNum == blastMax - 1 ? RifterDamage.riftSuperDamage : RifterDamage.riftDamage;
            base.OnEnter();
            usesOvercharge = true;
            riftPassive = base.GetComponent<RifterOverchargePassive>();
            aimRay = new Ray(basePosition, baseDirection);
            duration = 1.75f;         
            numPosition = GetNumPosition(blastNum);
            Fire();           
            TeleportEnemies();
            blastNum++;
        }

        private Vector3 GetNumPosition(int num)
        {
            
            float num2 = RiftDistance() / 6 * (num) + 10f ;
            Vector3 location = aimRay.GetPoint(num2);
            Vector3 position = location;
            if (Physics.SphereCast(basePosition, 0.05f, baseDirection, out var raycastHit, num2, LayerIndex.world.mask))
            {
                position = raycastHit.point;
            }
            if (base.isAuthority)
            {
                Search(position);
                if (hitTimelockCollider != null)
                {
                    position = hitTimelockCollider.transform.position;
                    Search2(position);
                }
            }
            
            return position;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            

            if (stopwatch >= (duration / 5) && isAuthority)
            {
                if (blastNum < blastMax && !hitTimelock)
                {
                    outer.SetNextState(new ChainedWorlds
                    {
                        blastNum = blastNum,
                        blastMax = blastMax,
                        blastRadius = blastRadius,
                        basePosition = basePosition,
                        baseDirection = baseDirection,
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

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        public override float RiftDistance()
        {
            return RifterStaticValues.riftPrimaryDistance;
        }


        public override Vector3 GetTeleportLocation(CharacterBody body)
        {
            if (rifterStep.deployedList.Count > 0 && blastNum == blastMax)
            {
                Vector3 position2 = rifterStep.deployedList.FirstOrDefault().transform.position;
                return position2;
            }
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
                        return;
                    }
                    if (RifterPlugin.blacklistBodyNames.Contains(enemyHit.name))
                    {
                        //Add Effect here later
                        return;
                    }
                    enemyBodies.AddDistinct(enemyHit);
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
            InvokeTeleportAction();
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
                blastAttack.damageType.damageSource = DamageSource.Utility;
                blastAttack.AddModdedDamageType(damageType);
                var result = blastAttack.Fire();

                EffectData effectData = new EffectData();
                effectData.scale = BlastRadius() / 10f;
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
        }

        public override bool IsOvercharged()
        {
            return true;
        }
        public override float BlastRadius()
        {
            if (hitTimelock)
            {
                return RifterStaticValues.timelockRadius;
            }
            return blastRadius; /// (float)Math.Pow((double)RifterStaticValues.overchargedCoefficient, (double)blastNum);
        }

        public override float BlastDamage()
        {
            if (hitTimelock)
            {
                return base.characterBody.damage * RifterStaticValues.timelockBlast + (base.characterBody.damage * RifterStaticValues.timelockEnemyMultiplier * hitEnemies);
            }
            return characterBody.damage * RifterStaticValues.secondaryRiftCoefficient; //* (float)Math.Pow((double)RifterStaticValues.overchargedCoefficient, (double)blastNum);
        }

        public virtual float ProcCoefficient()
        {
            return 1f;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write((byte)blastNum);
            writer.Write((byte)blastMax);
            writer.Write(enemyTeleportTo);
            writer.Write(basePosition);
            writer.Write(baseDirection);
            for (int i = 0; i < enemyBodies.Count; i++)
            {
                writer.Write(enemyBodies[i].netId);
            }

        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            blastNum = reader.ReadByte();
            blastMax = reader.ReadByte();            
            enemyTeleportTo = reader.ReadVector3();
            basePosition = reader.ReadVector3();
            baseDirection = reader.ReadVector3();
            while (reader.Position < reader.Length)
            {
                enemyBodies.Add(Util.FindNetworkObject(reader.ReadNetworkId()).GetComponent<CharacterBody>());
            }

        }
    }
}


