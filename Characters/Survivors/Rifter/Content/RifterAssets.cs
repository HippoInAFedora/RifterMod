using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RifterMod.Modules;
using RifterMod.Characters.Survivors.Rifter.Components;
using RoR2.Projectile;
using R2API;
using System.Collections.Generic;
using UnityEngine.Networking;
using EntityStates;
using RifterMod.Characters.Survivors.Rifter.SkillStates;
using RifterMod.Survivors.Rifter.SkillStates;
using IL.RoR2.RemoteGameBrowser;

namespace RifterMod.Survivors.Rifter
{
    public static class RifterAssets
    {
        // particle effects

        internal static List<EffectDef> effectDefs = new List<EffectDef>();

        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;

        public static GameObject slipstreamInEffect;
        public static GameObject slipstreamOutEffect;



        public static GameObject riftExplosionEffect;
        public static GameObject riftExplosionEffectOvercharged;
        public static GameObject fractureLineTracer;
        public static GameObject fractureLineTracerOvercharged;
        public static GameObject fractureBeam;

        public static GameObject portal;
        public static GameObject portalZipline;
        public static GameObject portalBlast;

        public static GameObject shatterStackVisual;

        public static GameObject wanderingRiftProjectile;
        public static GameObject wanderingRiftExplosion;
        public static GameObject wanderingRiftExplosionSmall;
        public static GameObject wanderingRiftProjectileGhost;


        public static GameObject hitEffect;
        public static GameObject muzzleEffect;

        public static GameObject timelockMain;

        public static Sprite shatterIcon;

        public static GameObject riftIndicator;

        public static Material matTeleport;

        public static Material matBackVent;

        public static GameObject overchargeHUD;

        public static Color riftColor = new Color(11, 40, 77);
        public static Color overchargedColor = new Color(150, 66, 245);

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject bombProjectilePrefab;

        //emote
        public static GameObject emoteSkele;

        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            emoteSkele = _assetBundle.LoadAsset<GameObject>("Rifter_Emote");

            swordHitSoundEvent = Content.CreateAndAddNetworkSoundEventDef("RifterSwordHit");

            shatterIcon = _assetBundle.LoadAsset<Sprite>("texShatterDebuff");

            CreateEffects();

            CreateProjectiles();
        }

        private static void CleanChildren(Transform startingTrans)
        {
            for (int num = startingTrans.childCount - 1; num >= 0; num--)
            {
                if (startingTrans.GetChild(num).childCount > 0)
                {
                    CleanChildren(startingTrans.GetChild(num));
                }
                UnityEngine.Object.DestroyImmediate((UnityEngine.Object)(object)((Component)startingTrans.GetChild(num)).gameObject);
            }
        }

        #region effects
        private static void CreateEffects()
        {
            matTeleport = Addressables.LoadAssetAsync<Material>("RoR2/Base/Huntress/matHuntressFlashBright.mat").WaitForCompletion();
            matTeleport.SetColor("_TintColor", riftColor);
            matTeleport.SetFloat("_Boost", 0.125f);
            riftIndicator = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Huntress/HuntressTrackingIndicator.prefab").WaitForCompletion();

            hitEffect = _assetBundle.LoadEffect("RiftHitEffect");
            //hitEffect.AddComponent<NetworkIdentity>();
            //hitEffect.AddComponent<EffectComponent>();
            //hitEffect.AddComponent<DestroyOnParticleEnd>();
            //VFXAttributes hitEffectVFX = hitEffect.AddComponent<VFXAttributes>();
            //hitEffectVFX.vfxIntensity = VFXAttributes.VFXIntensity.Low;
            //hitEffectVFX.vfxPriority = VFXAttributes.VFXPriority.Low;
            //if (hitEffect.GetComponent<EffectComponent>())
            //{
            //    hitEffect.GetComponent<EffectComponent>().applyScale = true;
            //}
            
            muzzleEffect = _assetBundle.LoadEffect("RiftMuzzleEffect");
            //muzzleEffect.AddComponent<NetworkIdentity>();
            //muzzleEffect.AddComponent<EffectComponent>();
            //muzzleEffect.AddComponent<DestroyOnParticleEnd>();
            //VFXAttributes muzzleEffectVFX = muzzleEffect.AddComponent<VFXAttributes>();
            //muzzleEffectVFX.vfxIntensity = VFXAttributes.VFXIntensity.Low;
            //muzzleEffectVFX.vfxPriority = VFXAttributes.VFXPriority.Low;
            //if (muzzleEffect.GetComponent<EffectComponent>())
            //{
            //    muzzleEffect.GetComponent<EffectComponent>().applyScale = true;
            //}

            //PrefabAPI.RegisterNetworkPrefab(hitEffect); 
            //PrefabAPI.RegisterNetworkPrefab(muzzleEffect);

            shatterStackVisual = _assetBundle.LoadAsset<GameObject>("ShatterStack");
            TemporaryVisualEffect shatterVisual = shatterStackVisual.AddComponent<TemporaryVisualEffect>();
            shatterVisual.visualTransform = shatterStackVisual.transform.GetChild(0).transform;
            shatterVisual.radius = 1f;
            shatterStackVisual.AddComponent<DestroyOnTimer>();
            shatterStackVisual.GetComponent<DestroyOnTimer>().resetAgeOnDisable = true;
            shatterVisual.visualState = TemporaryVisualEffect.VisualState.Enter;
            shatterVisual.exitComponents = new MonoBehaviour[1];
            shatterVisual.exitComponents[0] = shatterStackVisual.GetComponent<DestroyOnTimer>();
            

            CreateRift();
            CreateFracture();
            CreatePortal();
            CreateTimelock();
            CreateHUD();
        }

        private static void CreateRift()
        {
            //riftEnemyMaterial = _assetBundle.LoadMaterial("matEnemyMask");
            riftExplosionEffect = _assetBundle.LoadEffect("riftObjectTypical");   
            riftExplosionEffect.transform.GetChild(2).gameObject.SetActive(false);
            riftExplosionEffect.AddComponent<NetworkIdentity>();
            riftExplosionEffect.AddComponent<DestroyOnTimer>().duration = .6f;
            riftExplosionEffect.AddComponent<EffectComponent>();
            if (riftExplosionEffect.GetComponent<EffectComponent>())
            {
                riftExplosionEffect.GetComponent<EffectComponent>().applyScale = true;
            }
            if (riftExplosionEffect == null)
            {
                Debug.Log("effect is not being loaded");
            }
            PrefabAPI.RegisterNetworkPrefab(riftExplosionEffect);

            riftExplosionEffectOvercharged = _assetBundle.LoadEffect("RiftOvercharged");
            riftExplosionEffectOvercharged.transform.GetChild(2).gameObject.SetActive(false);
            riftExplosionEffectOvercharged.AddComponent<NetworkIdentity>();
            riftExplosionEffectOvercharged.AddComponent<DestroyOnTimer>().duration = .6f;
            riftExplosionEffectOvercharged.AddComponent<EffectComponent>();
            if (riftExplosionEffectOvercharged.GetComponent<EffectComponent>())
            {
                riftExplosionEffectOvercharged.GetComponent<EffectComponent>().applyScale = true;
            }
            if (riftExplosionEffectOvercharged == null)
            {
                Debug.Log("effect is not being loaded");
            }
            PrefabAPI.RegisterNetworkPrefab(riftExplosionEffectOvercharged);

            slipstreamInEffect = _assetBundle.LoadEffect("SlipstreamIn");
            slipstreamInEffect.AddComponent<NetworkIdentity>();
            slipstreamInEffect.AddComponent<DestroyOnTimer>().duration = .5f;
            slipstreamInEffect.AddComponent<EffectComponent>();
            if (slipstreamInEffect.GetComponent<EffectComponent>())
            {
                slipstreamInEffect.GetComponent<EffectComponent>().applyScale = true;
            }
            if (slipstreamInEffect == null)
            {
                Debug.Log("effect is not being loaded");
            }
            PrefabAPI.RegisterNetworkPrefab(slipstreamInEffect);

            slipstreamOutEffect = _assetBundle.LoadEffect("SlipstreamOut");
            slipstreamOutEffect.AddComponent<NetworkIdentity>();
            slipstreamOutEffect.AddComponent<DestroyOnTimer>().duration = .5f;
            slipstreamOutEffect.AddComponent<EffectComponent>();
            if (slipstreamOutEffect.GetComponent<EffectComponent>())
            {
                slipstreamOutEffect.GetComponent<EffectComponent>().applyScale = true;
            }
            if (slipstreamOutEffect == null)
            {
                Debug.Log("effect is not being loaded");
            }
            PrefabAPI.RegisterNetworkPrefab(slipstreamOutEffect);

            wanderingRiftExplosion = _assetBundle.LoadEffect("WRiftExplosion");
            wanderingRiftExplosion.transform.GetChild(2).gameObject.SetActive(false);
            wanderingRiftExplosion.AddComponent<NetworkIdentity>();
            wanderingRiftExplosion.AddComponent<DestroyOnTimer>().duration = .6f;
            wanderingRiftExplosion.AddComponent<EffectComponent>();
            if (wanderingRiftExplosion.GetComponent<EffectComponent>())
            {
                wanderingRiftExplosion.GetComponent<EffectComponent>().applyScale = true;
            }
            if (wanderingRiftExplosion == null)
            {
                Debug.Log("effect is not being loaded");
            }
            PrefabAPI.RegisterNetworkPrefab(wanderingRiftExplosion);

            wanderingRiftExplosionSmall = _assetBundle.LoadEffect("WRiftExplosionSmall");
            wanderingRiftExplosionSmall.transform.GetChild(2).gameObject.SetActive(false);
            wanderingRiftExplosionSmall.AddComponent<NetworkIdentity>();
            wanderingRiftExplosionSmall.AddComponent<DestroyOnTimer>().duration = .6f;
            wanderingRiftExplosionSmall.AddComponent<EffectComponent>();
            if (wanderingRiftExplosionSmall.GetComponent<EffectComponent>())
            {
                wanderingRiftExplosionSmall.GetComponent<EffectComponent>().applyScale = true;
            }
            if (wanderingRiftExplosionSmall == null)
            {
                Debug.Log("effect is not being loaded");
            }
            PrefabAPI.RegisterNetworkPrefab(wanderingRiftExplosionSmall);


        }

        private static void CreateFracture()        
        {

            fractureLineTracer = _assetBundle.LoadEffect("FractureTrail");
            fractureLineTracer.AddComponent<EffectComponent>();
            fractureLineTracer.AddComponent<EventFunctions>();
            fractureLineTracer.AddComponent<Tracer>();

            fractureLineTracer.AddComponent<BeamPointsFromTransforms>();
            if (fractureLineTracer.TryGetComponent(out BeamPointsFromTransforms beam))
            {
                beam.target = fractureLineTracer.GetComponent<LineRenderer>();
                Transform[] transforms = new Transform[2];
                transforms[0] = fractureLineTracer.transform.GetChild(0).transform;
                transforms[1] = fractureLineTracer.transform.GetChild(1).transform;
                beam.pointTransforms = transforms;
            }


            if (fractureLineTracer.TryGetComponent(out Tracer tracer))
            {
                tracer.headTransform = fractureLineTracer.transform.GetChild(0).transform;
                tracer.tailTransform = fractureLineTracer.transform.GetChild(1).transform;
                tracer.startTransform = fractureLineTracer.transform.GetChild(2).transform;
                tracer.speed = 300f;
                tracer.length = 20f;
            }

            Content.CreateAndAddEffectDef(fractureLineTracer);



            fractureLineTracerOvercharged = _assetBundle.LoadEffect("FractureTrailOvercharged");
            fractureLineTracerOvercharged.AddComponent<EffectComponent>();
            fractureLineTracerOvercharged.AddComponent<EventFunctions>();
            fractureLineTracerOvercharged.AddComponent<Tracer>();
            fractureLineTracerOvercharged.AddComponent<BeamPointsFromTransforms>();
            //
            if (fractureLineTracerOvercharged.TryGetComponent(out BeamPointsFromTransforms beamPointsOvercharged))
            {
                beamPointsOvercharged.target = fractureLineTracerOvercharged.GetComponent<LineRenderer>();
                Transform[] transforms = new Transform[2];
                transforms[0] = fractureLineTracerOvercharged.transform.GetChild(0).transform;
                transforms[1] = fractureLineTracerOvercharged.transform.GetChild(1).transform;
                beamPointsOvercharged.pointTransforms = transforms;
            }

            if (fractureLineTracerOvercharged.TryGetComponent(out Tracer tracerOvercharged))
            {
                tracerOvercharged.headTransform = fractureLineTracerOvercharged.transform.GetChild(0).transform;
                tracerOvercharged.tailTransform = fractureLineTracerOvercharged.transform.GetChild(1).transform;
                tracerOvercharged.startTransform = fractureLineTracerOvercharged.transform.GetChild(2).transform;
                tracerOvercharged.speed = 1000f;
                tracerOvercharged.length = 100f;
            }

            Content.CreateAndAddEffectDef(fractureLineTracerOvercharged);
        }

        private static void CreateHUD()
        {
            overchargeHUD = _assetBundle.LoadAsset<GameObject>("HUDOvercharge");
        }

        private static void CreatePortal()
        {
            portal = _assetBundle.LoadAsset<GameObject>("Portal");

            var stateMachine = portal.AddComponent<EntityStateMachine>();
            stateMachine.customName = "Main";
            stateMachine.initialStateType = new EntityStates.SerializableEntityStateType(typeof(PortalBaseState));

            portal.AddComponent<NetworkStateMachine>();
            portal.AddComponent<PortalController>();
            portal.GetComponent<PortalController>().pointATransform = portal.transform.GetChild(0);
            portal.GetComponent<PortalController>().pointBTransform = portal.transform.GetChild(1);
            portal.AddComponent<GenericOwnership>();
            portal.AddComponent<EntityLocator>();
            portal.GetComponent<EntityLocator>().entity = portal;
            portal.AddComponent<NetworkIdentity>();
            portal.transform.GetChild(0).gameObject.AddComponent<NetworkIdentity>();
            portal.transform.GetChild(1).gameObject.AddComponent<NetworkIdentity>();

            PrefabAPI.RegisterNetworkPrefab(portal);

        }
        private static void CreateTimelock()
        {
            timelockMain = _assetBundle.LoadAsset<GameObject>("Timelock");
            timelockMain.layer = LayerIndex.defaultLayer.intVal;
            timelockMain.AddComponent<GenericOwnership>();
            timelockMain.AddComponent<DestroyOnRift>();
            timelockMain.AddComponent<EntityLocator>();
            timelockMain.GetComponent<EntityLocator>().entity = timelockMain;
            timelockMain.AddComponent<NetworkIdentity>();
            timelockMain.AddComponent<TeamFilter>();
            BuffWard nullifyBuff = timelockMain.AddComponent<BuffWard>();
            nullifyBuff.shape = BuffWard.BuffWardShape.Sphere;
            nullifyBuff.radius = 10f;
            nullifyBuff.interval = .2f;
            nullifyBuff.rangeIndicator = timelockMain.transform.GetChild(0).transform;
            nullifyBuff.buffDuration = 2f;
            nullifyBuff.floorWard = false;
            nullifyBuff.expires = true;
            nullifyBuff.invertTeamFilter = true;
            nullifyBuff.expireDuration = 10f;
            nullifyBuff.animateRadius = false;
            nullifyBuff.radiusCoefficientCurve = null;
            nullifyBuff.removalSoundString = null;
            nullifyBuff.removalTime = 10f;
            nullifyBuff.requireGrounded = false;
            timelockMain.AddComponent<Deployable>();
            timelockMain.AddComponent<EventFunctions>();
            timelockMain.GetComponent<SphereCollider>().radius = nullifyBuff.radius;
            //timelockMain.AddComponent<NoGravZone>(); infinite float??

            ParticleSystem.MainModule pmain = timelockMain.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            pmain.duration = nullifyBuff.expireDuration;

            PrefabAPI.RegisterNetworkPrefab(timelockMain);
        }
        private static void CreateBombExplosionEffect()
        {
            bombExplosionEffect = _assetBundle.LoadEffect("BombExplosionEffect", "RifterBombExplosion");

            if (!bombExplosionEffect)
                return;

            ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
            shakeEmitter.amplitudeTimeDecay = true;
            shakeEmitter.duration = 0.5f;
            shakeEmitter.radius = 200f;
            shakeEmitter.scaleShakeRadiusWithLocalScale = false;

            shakeEmitter.wave = new Wave
            {
                amplitude = 1f,
                frequency = 40f,
                cycleOffset = 0f
            };

        }
        #endregion effects

        #region projectiles
        private static void CreateProjectiles()
        {
            wanderingRiftProjectileGhost = _assetBundle.LoadAsset<GameObject>("WanderingRiftProjectileGhost");


            wanderingRiftProjectile = _assetBundle.LoadAsset<GameObject>("WanderingRiftProjectile");
            wanderingRiftProjectile.AddComponent<NetworkIdentity>();
            wanderingRiftProjectile.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
            ProjectileController wanderPC = wanderingRiftProjectile.AddComponent<ProjectileController>();
            wanderPC.procCoefficient = 1f;
            wanderPC.ghostPrefab = wanderingRiftProjectileGhost;
            wanderPC.startSound = "";

            
            wanderingRiftProjectileGhost.AddComponent<ProjectileGhostController>();
            //VFXAttributes vfx = wanderingRiftProjectileGhost.AddComponent<VFXAttributes>();
            //vfx.vfxPriority = VFXAttributes.VFXPriority.Always;
            //vfx.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            wanderPC.allowPrediction = true;
            wanderingRiftProjectile.AddComponent<ProjectileNetworkTransform>();
            ProjectileSimple wanderPSimple = wanderingRiftProjectile.AddComponent<ProjectileSimple>();
            wanderPSimple.updateAfterFiring = true;
            wanderPSimple.lifetime = 30f;
            wanderPSimple.desiredForwardSpeed = 10f;
            //pMain.startSpeed = wanderPSimple.desiredForwardSpeed;
            wanderingRiftProjectile.AddComponent<ProjectileDamage>();
            ProjectileImpactExplosionModify wanderPIE = wanderingRiftProjectile.AddComponent<ProjectileImpactExplosionModify>();
            wanderPIE.blastRadius = 8f;
            wanderPIE.blastDamageCoefficient = 1f;
            wanderPIE.falloffModel = BlastAttack.FalloffModel.None;
            wanderPIE.lifetime = 30f;
            wanderPIE.bonusBlastForce = Vector3.one * -10f;
            wanderPIE.lifetimeAfterImpact = .1f;
            wanderPIE.impactEffect = wanderingRiftExplosion;
            wanderPIE.destroyOnWorld = true;
            wanderPIE.destroyOnEnemy = true;
            wanderPIE.explodeOnLifeTimeExpiration = true;
            ProjectileDamage wanderDamage = wanderingRiftProjectile.AddComponent<ProjectileDamage>();
            wanderDamage.damageType = new DamageTypeCombo(DamageTypeCombo.Generic, DamageTypeExtended.Generic, DamageSource.NoneSpecified);
            wanderingRiftProjectile.AddComponent<TeamFilter>();

            BlastOnPrimary blast = wanderingRiftProjectile.AddComponent<BlastOnPrimary>();
            blast.blastDamageCoefficient = 1f;
            blast.blastRadius = RifterStaticValues.blastRadius * .75f;
            blast.explosionEffect = wanderingRiftExplosionSmall;
            blast.falloffModel = BlastAttack.FalloffModel.None;


            Content.AddProjectilePrefab(wanderingRiftProjectile);
            PrefabAPI.RegisterNetworkPrefab(wanderingRiftProjectile);
        }

        private static void CreateBombProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            bombProjectilePrefab = MyCharacterAssets.CloneProjectilePrefab("CommandoGrenadeProjectile", "RifterBombProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(bombProjectilePrefab.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();

            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.blastDamageCoefficient = 1f;
            bombImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = bombExplosionEffect;
            bombImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("RifterBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombProjectilePrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("RifterBombGhost") != null)
                bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("RifterBombGhost");

            bombController.startSound = "";
        }
        #endregion projectiles

        internal static void AddNewEffectDef(GameObject effectPrefab)
        {
            AddNewEffectDef(effectPrefab, "");
        }

        internal static void AddNewEffectDef(GameObject effectPrefab, string soundName)
        {
            EffectDef effectDef = new EffectDef();
            effectDef.prefab = effectPrefab;
            effectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            effectDef.prefabName = effectPrefab.name;
            effectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            effectDef.spawnSoundEventName = soundName;
            effectDefs.Add(effectDef);
        }
    }


}
