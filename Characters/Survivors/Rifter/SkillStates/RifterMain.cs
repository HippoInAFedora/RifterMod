using RoR2;
using EntityStates;
using UnityEngine;
using RifterMod.Survivors.Rifter.Components;
using JetBrains.Annotations;
using UnityEngine.Experimental.GlobalIllumination;
using RifterMod.Characters.Survivors.Rifter.Components;
using RifterMod.Survivors.Rifter;
using System;
using UnityEngine.UIElements;

public class RifterMain : GenericCharacterMain
{

    private RifterTracker rifterTracker;


    public override void OnEnter()
    {
        base.OnEnter();
        rifterTracker = GetComponent<RifterTracker>();   
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override InterruptPriority GetMinimumInterruptPriority()
    {
        return InterruptPriority.Any;
    }
}

