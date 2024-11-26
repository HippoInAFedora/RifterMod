using RoR2;
using EntityStates;
using UnityEngine;
using UnityEngine.Networking;
using R2API;
using System.Diagnostics;
using RifterMod.Survivors.Rifter;
using UnityEngine.UIElements;

public class ModifiedTeleportOld : BaseState
{
    public Vector3 initialPosition;
    public Vector3 targetFootPosition;

    public GameObject attackerAndInflictor;

    private Transform modelTransform;

    private CharacterModel characterModel;

    private HurtBoxGroup hurtboxGroup;

    GameObject inEffect = RifterAssets.slipstreamInEffect;
    GameObject outEffect = RifterAssets.slipstreamOutEffect;

    public static GameObject tracerEffectPrefabOvercharged = RifterAssets.fractureLineTracerOvercharged;

    public bool showEffect = false;

    public float stopwatch;
    public float teleportTimer;
    public float teleportWaitDuration;
    public bool teleportOut;

    public float damageOutput = 0f;

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

        if (base.characterBody.hasEffectiveAuthority)
        {
            TeleportHelper.TeleportBody(characterBody, targetFootPosition);
        }
       

        if (!showEffect)
        {
            EffectData inEffectData = new EffectData
            {
                origin = characterBody.corePosition,
                scale = base.characterBody.radius * 2.5f
            };
            EffectManager.SpawnEffect(inEffect, inEffectData, true);

            BulletAttack tracerAttack = new BulletAttack();
            tracerAttack.damage = 0f;
            tracerAttack.origin = initialPosition;
            tracerAttack.aimVector = targetFootPosition - initialPosition;
            tracerAttack.maxDistance = Vector3.Distance(initialPosition, targetFootPosition);
            tracerAttack.tracerEffectPrefab = tracerEffectPrefabOvercharged;
            tracerAttack.Fire();
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
        if ((bool)rigidbodyMotor)
        {
            rigidbodyMotor.moveVector = Vector3.zero;
        }
        //else
        //{
        //    transform.position = Vector3.zero;
        //}

        if (base.isAuthority && stopwatch >= teleportWaitDuration)
        {
            outer.SetNextStateToMain();
        }
    }

    public override void OnExit()
    {
        if (!outer.destroying)
        {
            modelTransform = GetModelTransform();
            if ((bool)modelTransform && showEffect)
            {
                TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(modelTransform.gameObject);
                temporaryOverlay.duration = 0.6f;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = RifterAssets.matTeleport;
                temporaryOverlay.AddToCharacterModel(modelTransform.GetComponent<CharacterModel>());
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

        if (base.isAuthority)
        {
            BlastAttack blastAttack = new BlastAttack();
            blastAttack.attacker = attackerAndInflictor;
            blastAttack.inflictor = attackerAndInflictor;
            blastAttack.teamIndex = TeamIndex.Player;
            blastAttack.radius = base.characterBody.radius;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseDamage = damageOutput;
            blastAttack.crit = RollCrit();
            blastAttack.procCoefficient = .8f;
            blastAttack.canRejectForce = false;
            blastAttack.position = base.transform.position;
            blastAttack.attackerFiltering = AttackerFiltering.AlwaysHitSelf;
            blastAttack.AddModdedDamageType(RifterDamage.riftDamage);
            BlastAttack.Result result = blastAttack.Fire();
        }

        if (!showEffect)
        {
            EffectData outEffectData = new EffectData
            {
                origin = targetFootPosition,
                scale = base.characterBody.radius * 5f
            };
            EffectManager.SpawnEffect(outEffect, outEffectData, true);
        }
        base.OnExit();
    }

    public override InterruptPriority GetMinimumInterruptPriority()
    {
        return InterruptPriority.Frozen;
    }
}

