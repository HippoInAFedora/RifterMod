

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using System;
using IL.RoR2.Skills;
using static UnityEngine.SendMouseEvents;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftUtilitySpecialTeleport : BaseSkillState
    {
        public static float damageCoefficient = RifterStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 3f;
        public static float range = 1000f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration = .3f;
        float radius = 20f;
        float stopwatch;

        Vector3 startPosition;
        Vector3 finalPosition;

        bool flag = false;
        bool flag2 = false;

        public Vector3 teleportVector;
        public Vector3 teleportDirection;

        RiftSpecial riftSpecial;


        private bool teleportFinished = false;

        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;

        private enum State
        {
            None= -1,
            Init,
            Waiting,
            Execute
        }

        private State state;
        //OnEnter() runs once at the start of the skill
        //All we do here is create a BulletAttack and fire it
        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            RaycastHit hitInfo;
            Physics.Raycast(aimRay, out hitInfo, range, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal);
            teleportVector = hitInfo.point;
            teleportDirection = aimRay.direction;
            state = State.None;
            startPosition = base.transform.position;

            riftSpecial = base.gameObject.AddComponent<RiftSpecial>();
            riftSpecial.characterBody = base.characterBody;
            riftSpecial.vector = base.transform.position;
            riftSpecial.radius = radius;
        }

        //This method runs once at the end
        //Here, we are doing nothing
        public override void OnExit()
        {
            if ((bool)base.characterMotor)
            {
                base.characterMotor.disableAirControlUntilCollision = false;
            }

            base.OnExit();
            
        }


        //FixedUpdate() runs almost every frame of the skill
        //Here, we end the skill once it exceeds its intended duration
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority)
            {
                    riftSpecial.enabled = true;
                    if (riftSpecial.thirdShot == true)
                    {
                        state = State.Execute;
                    }


                if (state == State.Execute)
                {
                    stopwatch += Time.fixedDeltaTime;
                    if ((bool)base.characterMotor && (bool)base.characterDirection)
                    {
                        base.characterMotor.velocity = Vector3.zero;
                        base.characterMotor.rootMotion += teleportDirection * (teleportVector.magnitude/duration * Time.fixedDeltaTime);
                    }
                    if (base.transform.position == teleportVector)
                    {
                        BlastAttack blastAttackOut = new BlastAttack();
                        blastAttackOut.attacker = base.gameObject;
                        blastAttackOut.inflictor = base.gameObject;
                        blastAttackOut.teamIndex = TeamIndex.None;
                        blastAttackOut.radius = radius;
                        blastAttackOut.falloffModel = BlastAttack.FalloffModel.None;
                        blastAttackOut.baseDamage = base.characterBody.damage * 15f;
                        blastAttackOut.crit = base.RollCrit();
                        blastAttackOut.procCoefficient = 1f;
                        blastAttackOut.canRejectForce = false;
                        blastAttackOut.position = base.transform.position;
                        blastAttackOut.attackerFiltering = AttackerFiltering.NeverHitSelf;
                        blastAttackOut.Fire();
                        teleportFinished = true;
                        state = State.None;
                    }
                }
                if (teleportFinished || stopwatch >= duration + 1f)
                {
                    outer.SetNextStateToMain();
                }
            }
           

        }
    
        //GetMinimumInterruptPriority() returns the InterruptPriority required to interrupt this skill
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(teleportVector);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            teleportVector = reader.ReadVector3();
        }


    }
}