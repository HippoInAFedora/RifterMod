using RoR2;
using EntityStates;
using RifterMod.Characters.Survivors.Rifter.Components;
using RifterMod.Survivors.Rifter.SkillStates;
using UnityEngine;
using RifterMod.Survivors.Rifter;
using System.Linq;

public class FaultLine : RiftBase
{
    public float stopwatch;

    public override void OnEnter()
    {
        fractureHitCallback = false;
        isPrimary = false;
        usesOvercharge = true;
        shouldBuckshot = false;
        Ray aimRay = base.GetAimRay();
        baseDuration = .2f;
        base.OnEnter();
        if (base.isAuthority)
        {
            Fracture(aimRay, RiftDistance(), LayerIndex.defaultLayer, DamageSource.Secondary);
        }
        TeleportEnemies();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        stopwatch += Time.fixedDeltaTime;
        if (stopwatch >= duration && isAuthority)
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
        return InterruptPriority.Skill;
    }


    public override float RiftDistance()
    {
        return 500f;
    }

    public override bool IsOvercharged()
    {
        return true;
    }

    public override Vector3 GetTeleportLocation(CharacterBody body)
    {

        if (rifterStep.deployedList.Count > 0)
        {
            Vector3 position2 = rifterStep.deployedList.FirstOrDefault().transform.position + Vector3.up * .5f;
            return position2;
        }

        //Vector3 baseDirection = (body.corePosition - characterBody.corePosition).normalized;
        //Ray ray = new Ray(characterBody.corePosition, baseDirection);
        //Vector3 location;
        //if (body.isFlying || !body.characterMotor.isGrounded)
        //{
        //    location = ray.GetPoint(RifterStaticValues.riftPrimaryDistance);
        //}
        //else
        //{
        //    location = ray.GetPoint(RifterStaticValues.riftPrimaryDistance) + (Vector3.up);
        //}
        //Vector3 direction = (location - base.GetAimRay().origin).normalized;
        RaycastHit raycastHit;
        Vector3 position = base.GetAimRay().GetPoint(RifterStaticValues.riftPrimaryDistance);
        //Vector3 position = location;
        //float distance = Vector3.Distance(body.corePosition, location);
        if (Physics.SphereCast(base.GetAimRay().origin, 0.05f, base.GetAimRay().direction, out raycastHit, RifterStaticValues.riftPrimaryDistance, LayerIndex.world.mask, QueryTriggerInteraction.Collide))
        {
            bool normalPlacement = Vector3.Angle(Vector3.up, raycastHit.normal) < maxSlopeAngle;
            if (normalPlacement)
            {
                position = raycastHit.point + Vector3.up * .5f;
            }
            if (!normalPlacement)
            {
                position = raycastHit.point - base.GetAimRay().direction.normalized;
            }
        }
        return position;
    }
}

