using RoR2;
using EntityStates;
using UnityEngine;
using UnityEngine.Networking;
using RifterMod.Survivors.Rifter;

public class ModifiedTeleport : BaseState
{
    public Vector3 initialPosition;
    public Vector3 currentPosition;
    public Vector3 targetFootPosition;

    public GameObject attackerAndInflictor;

    private Transform modelTransform;

    private CharacterModel characterModel;

    private HurtBoxGroup hurtboxGroup;

    private float duration;

    private bool hasSnapped;

    public bool isPortalTeleport;

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
        CalculateSnapDestination();
        if (isPortalTeleport && NetworkServer.active)
        {
            base.characterBody.AddTimedBuff(RifterBuffs.postTeleport, 2f);
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
        if ((bool)base.characterMotor)
        {
            base.characterMotor.velocity = Vector3.zero;
        }
        if (!hasSnapped)
        {
            SetPosition(Vector3.Lerp(currentPosition, targetFootPosition, base.fixedAge / duration));
        }
        if (base.fixedAge >= duration && base.isAuthority)
        {
            hasSnapped = true;
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

        //if (base.isAuthority)
        //{
        //    BlastAttack blastAttack = new BlastAttack();
        //    blastAttack.attacker = attackerAndInflictor;
        //    blastAttack.inflictor = attackerAndInflictor;
        //    blastAttack.teamIndex = TeamIndex.Player;
        //    blastAttack.radius = base.characterBody.radius;
        //    blastAttack.falloffModel = BlastAttack.FalloffModel.None;
        //    blastAttack.baseDamage = damageOutput;
        //    blastAttack.crit = RollCrit();
        //    blastAttack.procCoefficient = .8f;
        //    blastAttack.canRejectForce = false;
        //    blastAttack.position = base.transform.position;
        //    blastAttack.attackerFiltering = AttackerFiltering.AlwaysHitSelf;
        //    blastAttack.AddModdedDamageType(RifterDamage.riftDamage);
        //    BlastAttack.Result result = blastAttack.Fire();
        //}

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

    private void SetPosition(Vector3 newPosition)
    {
        if ((bool)base.characterMotor)
        {
            base.characterMotor.Motor.SetPositionAndRotation(newPosition, Quaternion.identity);
        }
        if ((bool)base.rigidbodyMotor)
        {
            base.rigidbodyMotor.transform.SetPositionAndRotation(newPosition, Quaternion.identity);
        }
    }

    private void CalculateSnapDestination()
    {
        if (base.characterDirection != null)
        {
            base.characterDirection.forward = targetFootPosition;
        }
        else
        {
            base.transform.rotation = Util.QuaternionSafeLookRotation(targetFootPosition);
        }

        if (base.characterBody.footPosition != null)
        {
            currentPosition = base.characterBody.footPosition;
        }
        else
        {
            currentPosition = base.characterBody.corePosition;
        }
        duration = (currentPosition - targetFootPosition).magnitude;
        duration = Util.Remap(duration, 0f, 100f, 0.1f, .25f);
    }

    public override void OnSerialize(NetworkWriter writer)
    {
        base.OnSerialize(writer);
        writer.Write(targetFootPosition);
        writer.Write(currentPosition);
    }

    public override void OnDeserialize(NetworkReader reader)
    {
        base.OnDeserialize(reader);
        targetFootPosition = reader.ReadVector3();
        currentPosition = reader.ReadVector3();
    }
}

