using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Modules.BaseStates;
using RoR2;
using System;
using UnityEngine;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RefractedReality : BaseSkillState
    {

        public float baseDuration = .5f;
        private float duration;
        //added vars for bulletAttack
        private Vector3 forwardBackwardAmount;
        private float riftSecondaryDistance;

        //added vars for delay
        public float timerStagger;
        private float timer;
        public float maxTimer;
        public BlastAttack.Result result;
        public bool isBlastShot;
        public float teleportTimer;
        public bool isTeleportTimerStarted;

        //added vars for blastAttack
        private float radius = 7f;
        private float isLeftHitGround;
        private float isRightHitGround;
        private float isMiddleHitGround;
        private float riftPrimaryDistance2;
        private Ray rightBlast;
        private Ray leftBlast;
        private Ray middleBlast;
        private Vector3 vectorRight;
        private Vector3 vectorLeft;
        private Vector3 vectorMiddle;

        // added vars for teleporting
        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;
        public GameObject tracerEffectPrefab = FireBarrage.tracerEffectPrefab;





        //OnEnter() runs once at the start of the skill
        //All we do here is create a BulletAttack and fire it
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / base.attackSpeedStat;
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, false);
            base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
            Util.PlaySound(FireBarrage.fireBarrageSoundString, base.gameObject);
            base.AddRecoil(-0.6f, 0.6f, -0.6f, 0.6f);
            if (FireBarrage.effectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(FireBarrage.effectPrefab, base.gameObject, "MuzzleRight", false);
            }

            Vector3 rhs = (base.characterDirection ? base.characterDirection.forward : forwardBackwardAmount);

            if ((bool)base.inputBank && (bool)base.characterDirection)
            {
                forwardBackwardAmount = (base.inputBank.moveVector).normalized;
            }
            riftSecondaryDistance = (Vector3.Dot(forwardBackwardAmount, rhs) + 1f) * 5 + 10f;
            riftPrimaryDistance2 = (Vector3.Dot(forwardBackwardAmount, rhs) + 1f) * 20f + 30f;
            if (Physics.Raycast(aimRay, out var riftPoint, riftPrimaryDistance2, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
            {
                float hit = riftPoint.distance;
                riftPrimaryDistance2 = Vector3.Distance(aimRay.origin, aimRay.GetPoint(hit));
            }

            if (base.isAuthority)

            {
                //Blast Attack Stuff
                vectorMiddle = aimRay.GetPoint(riftSecondaryDistance);

                Vector3 rhs1 = Vector3.Cross(Vector3.up, aimRay.direction);
                Vector3 axis = Vector3.Cross(aimRay.direction, rhs1);
                Quaternion quaternionRight = Quaternion.AngleAxis(30, axis);
                Quaternion quaternionLeft = Quaternion.AngleAxis(-30, axis);

                Vector3 rightAngle = quaternionRight * aimRay.direction.normalized;
                rightBlast = new Ray(aimRay.origin, rightAngle);
                vectorRight = rightBlast.GetPoint(riftSecondaryDistance);

                Vector3 leftAngle = quaternionLeft * aimRay.direction.normalized;
                leftBlast = new Ray(aimRay.origin, leftAngle);
                vectorLeft = leftBlast.GetPoint(riftSecondaryDistance);

                if (Physics.Raycast(aimRay, out var endPoint, riftSecondaryDistance, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
                {
                    float hitMiddle = endPoint.distance;
                    vectorMiddle = aimRay.GetPoint(hitMiddle);
                }
                if (Physics.Raycast(leftBlast, out var leftPoint, riftSecondaryDistance, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
                {
                    float hitLeft = leftPoint.distance;
                    vectorLeft = leftBlast.GetPoint(hitLeft);
                }
                if (Physics.Raycast(rightBlast, out var rightPoint, riftSecondaryDistance, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
                {
                    float hitRight = rightPoint.distance;
                    vectorRight = rightBlast.GetPoint(hitRight);
                }

                float vectorDistanceLeft = Vector3.Distance(aimRay.origin, vectorLeft);
                if (vectorDistanceLeft + radius < riftSecondaryDistance)
                {
                    isLeftHitGround = .25f;
                }
                else
                {
                    isLeftHitGround = 1f;
                }

                float vectorDistanceMiddle = Vector3.Distance(aimRay.origin, vectorMiddle);
                if (vectorDistanceMiddle + radius < riftSecondaryDistance)
                {
                    isMiddleHitGround = .25f;
                }
                else
                {
                    isMiddleHitGround = 1f;
                }

                float vectorDistanceRight = Vector3.Distance(aimRay.origin, vectorRight);
                if (vectorDistanceRight + radius < riftSecondaryDistance)
                {
                    isRightHitGround = .25f;
                }
                else
                {
                    isRightHitGround = 1f;
                }

            }


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

            timer += Time.fixedDeltaTime;
            if (timer >= this.duration)

            {
                if (!isBlastShot)
                {
                    BlastAttack.Result[] eResults = new BlastAttack.Result[3];
                    SecondaryBlast(leftBlast, vectorLeft, isLeftHitGround, riftPrimaryDistance2, radius, out eResults[0]);
                    SecondaryBlast(middleBlast, vectorMiddle, isMiddleHitGround, riftPrimaryDistance2, radius, out eResults[1]);
                    SecondaryBlast(rightBlast, vectorRight, isRightHitGround, riftPrimaryDistance2, radius, out eResults[2]);

                    if (eResults != null)
                    {
                        isBlastShot = true;

                    }
                    for (int i = 0; i < eResults.Length; i++)
                    {
                        foreach (var hit in eResults[i].hitPoints)
                        {
                            if (hit.hurtBox == null)
                            {

                                break;
                            }
                            if (hit.hurtBox != null)
                            {
                                HealthComponent enemyHit = hit.hurtBox.healthComponent;
                                if (enemyHit == null)
                                {
                                    UnityEngine.Debug.Log("null");
                                    break;
                                }
                                Vector3 enemyAngleVector = base.GetAimRay().direction * Vector3.Angle(base.GetAimRay().origin, hit.hitPosition);
                                Ray enemyRayHit = new Ray(base.GetAimRay().origin, enemyAngleVector);
                                Vector3 enemyTeleportTo = enemyRayHit.GetPoint(riftPrimaryDistance2);

                                ModifiedTeleport teleport = enemyHit.gameObject.AddComponent<ModifiedTeleport>(); ;
                                teleport.body = enemyHit.body;
                                teleport.targetFootPosition = enemyTeleportTo;
                                teleport.enabled = false;
                                if (teleport.teleportOut == false)
                                {
                                    teleport.enabled = true;
                                    if (teleport.teleportOut == true && teleport.enabled == false)
                                    {
                                        Destroy(teleport);
                                    }
                                }


                            }
                        }
                    }
                }
            }


            if (base.fixedAge >= this.duration && base.isAuthority)
            {

                outer.SetNextStateToMain();
                return;
            }
        }

        //GetMinimumInterruptPriority() returns the InterruptPriority required to interrupt this skill
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public void SecondaryBlast(Ray aimRay, Vector3 position, float isHitGround, float riftPrimaryDistance2, float radius, out BlastAttack.Result result)
        {

            BlastAttack blastAttack = new BlastAttack();
            blastAttack.attacker = base.gameObject;
            blastAttack.inflictor = base.gameObject;
            blastAttack.teamIndex = TeamIndex.None;
            blastAttack.radius = radius;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseDamage = base.characterBody.damage * 4f * isHitGround;
            blastAttack.crit = base.RollCrit();
            blastAttack.procCoefficient = 1f;
            blastAttack.canRejectForce = false;
            blastAttack.position = position;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
            result = blastAttack.Fire();
            EffectData effectData2 = new EffectData();
            effectData2.origin = blastAttack.position;
            EffectManager.SpawnEffect(GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab, effectData2, transmit: false);

        }


    }



}