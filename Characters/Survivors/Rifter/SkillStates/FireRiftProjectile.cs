

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using System;
using UnityEngine.Networking;
using R2API;
using RifterMod.Characters.Survivors.Rifter.Components;
using RoR2.Projectile;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class FireRiftProjectile : BaseState
    {
        public GameObject projectilePrefab = RifterAssets.wanderingRiftProjectile;

        public float baseDuration = .8f;

        private float duration;

        private bool hasShot = false;

        private Ray aimRay;

        private int fireStateHash = Animator.StringToHash("Armature_Secondary");
        private int firePlaybackHash = Animator.StringToHash("Secondary.playbackRate");

        public override void OnEnter()
        {
            base.OnEnter();
            aimRay = base.GetAimRay();
            duration = baseDuration / attackSpeedStat;
            StartAimMode(duration);
            PlayAnimation("Gesture, Override", fireStateHash, firePlaybackHash, duration);
            AddRecoil(-1f, -1.5f, -0.25f, 0.25f);
            base.characterBody.AddSpreadBloom(1);
            if (base.isAuthority) 
            {
                FireProjectileInfo fireProjectileInfo = new FireProjectileInfo();
                fireProjectileInfo.projectilePrefab = projectilePrefab;
                fireProjectileInfo.position = aimRay.origin;
                fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                fireProjectileInfo.owner = base.gameObject;
                fireProjectileInfo.damage = base.characterBody.damage * RifterStaticValues.wanderingRiftCoefficient;
                fireProjectileInfo.force = 0f;
                fireProjectileInfo.crit = Util.CheckRoll(critStat, base.characterBody.master);
                fireProjectileInfo.speedOverride = base.attackSpeedStat * 10f + 5f;
                fireProjectileInfo.damageTypeOverride = DamageSource.Secondary;
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge > duration / 2 && !hasShot)
            {
                
            }
            if (base.isAuthority && base.fixedAge > duration)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}