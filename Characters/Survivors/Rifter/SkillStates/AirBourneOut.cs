using RoR2;
using EntityStates;
using UnityEngine;

public class AirBourneOut : BaseState
{
    private HurtBoxGroup hurtboxGroup;

    private float duration = .5f;

    private float stopwatch;

    public override void OnEnter()
    {
        base.OnEnter();
        if ((bool)hurtboxGroup)
        {
            HurtBoxGroup hurtBoxGroup = hurtboxGroup;
            int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
            hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        stopwatch += Time.fixedDeltaTime;
        if ((bool)characterMotor)
        {
            characterMotor.velocity = Vector3.zero;
        }
        else if ((bool)rigidbodyMotor)
        {
            rigidbodyMotor.moveVector = Vector3.zero;
        }
        else
        {
            transform.position = Vector3.zero;
        }

        if (stopwatch >= duration && isAuthority)
        {
            outer.SetNextStateToMain();
        }
    }

    public override void OnExit()
    {
        if ((bool)hurtboxGroup)
        {
            HurtBoxGroup hurtBoxGroup = hurtboxGroup;
            int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
            hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
        }
        base.OnExit();
    }

    public override InterruptPriority GetMinimumInterruptPriority()
    {
        return InterruptPriority.Skill;
    }
}

