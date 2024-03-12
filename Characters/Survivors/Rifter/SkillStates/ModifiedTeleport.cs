using RoR2;
using EntityStates;
using UnityEngine;
using UnityEngine.Networking;

public class ModifiedTeleport : BaseState
{
    public Vector3 targetFootPosition;

    private Transform modelTransform;

    private CharacterModel characterModel;

    private HurtBoxGroup hurtboxGroup;

    public EntityState setNextState = null;

    private float stopwatch;
    public float teleportTimer;
    public float teleportWaitDuration = .5f;
    public bool teleportOut;

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("began teleport");
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
            if ((bool)characterBody)
            {
                TeleportHelper.TeleportBody(base.characterBody, targetFootPosition);
            }
        }       
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        stopwatch += Time.deltaTime;
        if ((bool)base.characterMotor)
        {
            base.characterMotor.velocity = Vector3.zero;
        }
        else if ((bool)base.rigidbodyMotor)
        {
            base.rigidbodyMotor.moveVector = Vector3.zero;
        }
        else
        {
            base.transform.position = Vector3.zero;
        }

        if (stopwatch >= teleportWaitDuration && base.isAuthority)
        {
            if (characterBody.modelLocator.name == "FlyingVermin(Clone)")
            {
                outer.SetNextState(new EntityStates.FlyingVermin.Mode.GrantFlight());
            }
            else 
            {
                outer.SetNextStateToMain();
            }
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
        Debug.Log("got out of teleport");
        base.OnExit();
    }

    public override InterruptPriority GetMinimumInterruptPriority()
    {
        return InterruptPriority.Frozen;
    }
}

