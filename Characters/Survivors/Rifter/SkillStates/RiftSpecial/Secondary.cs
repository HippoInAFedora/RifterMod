

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using System;
using RoR2.Skills;
using static RoR2.GenericSkill;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftSecondarySpecial : BaseSkillState
    {

    
        public static float damageCoefficient = RifterStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.5f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;

        //added vars for bulletAttack
        private Vector3 forwardBackwardAmount;
        public float riftPrimaryDistance1;
        public float riftPrimaryDistance2;

        //added vars for blastAttack

        //Interface stuff
        public int currentTeleportActive { get; set; }


        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;



        //OnEnter() runs once at the start of the skill
        //All we do here is create a BulletAttack and fire it
        public override void OnEnter()
        {
            base.OnEnter();

            this.duration = RiftGauntlet.baseDuration / base.attackSpeedStat;
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, false);
            base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
            Util.PlaySound(FireBarrage.fireBarrageSoundString, base.gameObject);
            base.AddRecoil(-0.6f, 0.6f, -0.6f, 0.6f);
            if (FireBarrage.effectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(FireBarrage.effectPrefab, base.gameObject, "MuzzleRight", false);
            }


            if (base.isAuthority)
            {

                Vector3 vector = aimRay.GetPoint(RifterStaticValues.riftSecondaryDistance);

                if (Physics.Raycast(aimRay, out var endPoint, RifterStaticValues.riftSecondaryDistance, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
                {
                    float hit = endPoint.distance;
                    vector = aimRay.GetPoint(hit);
                }
                float radius = 20f;
                RiftSpecial riftSpecial = base.gameObject.AddComponent<RiftSpecial>();
                riftSpecial.characterBody = base.characterBody;
                riftSpecial.vector = vector;
                riftSpecial.radius = radius;
                riftSpecial.enabled = false;
                if (!riftSpecial.enabled)
                {
                    riftSpecial.enabled = true;
                    if (riftSpecial.thirdShot == true && riftSpecial.enabled == false)
                    {
                        Destroy(riftSpecial);
                    }
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



    }
}