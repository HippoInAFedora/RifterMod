﻿using RoR2;
using EntityStates;
using UnityEngine;

public class RiftRiderOut : ModifiedTeleport
{
    float stopwatch;

    public bool isResults;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        stopwatch += Time.fixedDeltaTime;
        if (stopwatch > teleportWaitDuration && base.isAuthority)
        {

            if (!base.characterMotor.isGrounded && isResults)
            {
                outer.SetNextState(new AirBourneOut());
            }
            else
            {
                outer.SetNextStateToMain();
            }
        }
        
    }
}

