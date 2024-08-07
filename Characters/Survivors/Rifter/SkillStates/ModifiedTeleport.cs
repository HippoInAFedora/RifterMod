﻿using RoR2;
using EntityStates;
using UnityEngine;
using UnityEngine.Networking;
using R2API;
using System.Diagnostics;
using RifterMod.Survivors.Rifter;

public class ModifiedTeleport : BaseState
{
    public Vector3 initialPosition;
    public Vector3 targetFootPosition;

    private Transform modelTransform;

    private CharacterModel characterModel;

    private HurtBoxGroup hurtboxGroup;

    GameObject trailObject = RifterAssets.fractureLineTracer;
    GameObject inEffect = RifterAssets.slipstreamInEffect;
    GameObject outEffect = RifterAssets.slipstreamOutEffect;

    TrailRenderer teleportTrail;

    private float stopwatch;
    public float teleportTimer;
    public float teleportWaitDuration;
    public bool teleportOut;

    public override void OnEnter()
    {
        base.OnEnter();
        initialPosition = characterBody.corePosition;
        modelTransform = GetModelTransform();
        if ((bool)modelTransform)
        {
            characterModel = modelTransform.GetComponent<CharacterModel>();
            hurtboxGroup = modelTransform.GetComponent<HurtBoxGroup>();
        }
        if ((bool)characterModel)
        {
            characterModel.invisibilityCount++;
        }
        if ((bool)hurtboxGroup)
        {
            HurtBoxGroup hurtBoxGroup = hurtboxGroup;
            int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
            hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
        }

        if (base.isAuthority)
        {
            UnityEngine.Debug.Log("base authority check");
        }

        if (NetworkServer.active)
        {
            UnityEngine.Debug.Log("network server active check");
        }

        TeleportHelper.TeleportBody(characterBody, targetFootPosition);

        EffectData inEffectData = new EffectData
        {
            origin = characterBody.corePosition,
            scale = base.characterBody.radius * 2.5f
        };
        EffectManager.SpawnEffect(inEffect, inEffectData, true);

        //if (trailObject)
        //{
        //    EffectData effectData = new EffectData
        //    {
        //        origin = targetFootPosition,
        //        start = characterBody.corePosition,
        //    };
        //    effectData.SetHurtBoxReference(base.characterBody.mainHurtBox);
        //    EffectManager.SpawnEffect(trailObject, effectData, true);
        //}
        

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        stopwatch += Time.fixedDeltaTime;
        //if ((bool)trailObject)
        //{
        //    trailObject.transform.position += trailObject.transform.forward * (Vector3.Distance(targetFootPosition, initialPosition) / teleportWaitDuration * Time.fixedDeltaTime);
        //    teleportTrail.AddPosition((Vector3)trailObject.transform.position);
        //}

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

        if (base.isAuthority && stopwatch >= teleportWaitDuration)
        {
            if (RifterConfig.cursed.Value == true)
            {
                outer.SetNextState(new Idle());
            }
            outer.SetNextStateToMain();
        }
    }

    public override void OnExit()
    {
        if (!outer.destroying)
        {
            modelTransform = GetModelTransform();
            if ((bool)modelTransform)
            {
                TemporaryOverlay temporaryOverlay = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = 0.6f;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                temporaryOverlay.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
                TemporaryOverlay temporaryOverlay2 = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay2.duration = 0.7f;
                temporaryOverlay2.animateShaderAlpha = true;
                temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay2.destroyComponentOnEnd = true;
                temporaryOverlay2.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
                temporaryOverlay2.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
            }
        }
        if (trailObject)
        {
            Destroy(trailObject);
        }
        if ((bool)characterModel)
        {
            characterModel.invisibilityCount--;
        }
        if ((bool)hurtboxGroup)
        {
            HurtBoxGroup hurtBoxGroup = hurtboxGroup;
            int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
            hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
        }

        EffectData outEffectData = new EffectData
        {
            origin = targetFootPosition,
            scale = base.characterBody.radius * 5f
        };
        EffectManager.SpawnEffect(outEffect, outEffectData, true);

        base.OnExit();
    }

    public override InterruptPriority GetMinimumInterruptPriority()
    {
        return InterruptPriority.Frozen;
    }

    //public override void OnSerialize(NetworkWriter writer)
    //{
    //    base.OnSerialize(writer);
    //    writer.Write(targetFootPosition);
    //}

    //public override void OnDeserialize(NetworkReader reader)
    //{
    //    base.OnDeserialize(reader);
    //    targetFootPosition = reader.ReadVector3();
    //}
}

