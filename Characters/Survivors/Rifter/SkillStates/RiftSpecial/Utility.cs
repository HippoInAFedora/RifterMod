

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
using AK.Wwise;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftUtilitySpecialLocate : BaseSkillState
    {

        public Vector3 teleportVector;


        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;

        public static float baseDuration;


        public static float maxDistance = 1000f;

        public static string fireSoundString;



        //OnEnter() runs once at the start of the skill
        //All we do here is create a BulletAttack and fire it
        public override void OnEnter()
        {
            base.OnEnter();
            UpdateAreaIndicator();
        }

        public override void Update()
        {
            base.Update();
            UpdateAreaIndicator();
        }
        //This method runs once at the end
        //Here, we are doing nothing
        public override void OnExit()
        {
            //if (!goodPlacement)
            //{
            //    base.skillLocator.special.AddOneStock();
            //    base.skillLocator.utility.AddOneStock();
            //}
            //EntityState.Destroy(areaIndicatorInstance.gameObject);
            //crosshairOverrideRequest?.Dispose();
            base.OnExit();
        }


        //FixedUpdate() runs almost every frame of the skill
        //Here, we end the skill once it exceeds its intended duration
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.inputBank.skill3.justReleased)
            {        
                outer.SetNextState(InstantiateNextState());
            }
        }             

        //GetMinimumInterruptPriority() returns the InterruptPriority required to interrupt this skill
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        private void UpdateAreaIndicator()
        {
             float num = maxDistance;
                float extraRaycastDistance = 0f;
                if (Physics.Raycast(CameraRigController.ModifyAimRayIfApplicable(GetAimRay(), base.gameObject, out extraRaycastDistance), out var hitInfo, num + extraRaycastDistance, LayerIndex.world.mask))
                {
                    Vector3 vector = hitInfo.point;
                }
        }

        protected EntityState InstantiateNextState()
        {
            return new RiftUtilitySpecialTeleport();
        }
    }
}