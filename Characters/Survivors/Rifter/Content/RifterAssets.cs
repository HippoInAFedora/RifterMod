using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RifterMod.Modules;
using System;
using RoR2.Projectile;
using R2API;
using System.Collections.Generic;
using LeTai.Asset.TranslucentImage;
using UnityEngine.Networking;
using R2API.Utils;
using Rewired;
using UnityEngine.UIElements;
using UnityEngine.Events;

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
        public static GameObject distanceRenderer;
        public static GameObject fractureBeam;
        public static GameObject distanceOrb;

        public static Sprite shatterIcon;

        public static Material[] riftOverlayMat;
        public static Material riftMat;
        public static Material riftEnemyMaterial;
        public static Material fractureMaterial;
        public static Material matShatter;

        public static Color riftColor = new Color(66, 135, 245);
        public static Color overchargedColor = new Color(150, 66, 245);

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject bombProjectilePrefab;

        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Content.CreateAndAddNetworkSoundEventDef("RifterSwordHit");

            shatterIcon = _assetBundle.LoadAsset<Sprite>("Shatter_Debuff_ver1");

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
            CreateBombExplosionEffect();

            swordSwingEffect = _assetBundle.LoadEffect("RifterSwordSwingEffect", true);
            swordHitImpactEffect = _assetBundle.LoadEffect("ImpactRifterSlash");
            distanceRenderer = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Golem/LaserGolem.prefab").WaitForCompletion();
            var gradient = new Gradient();
            var colors = new GradientColorKey[2];
            colors[0] = new GradientColorKey(riftColor, 0.0f);
            colors[1] = new GradientColorKey(overchargedColor, 1.0f);
            var alphas = new GradientAlphaKey[1];
            alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
            gradient.SetKeys(colors, alphas);
            LineRenderer dLine = distanceRenderer.GetComponent<LineRenderer>();
            dLine.colorGradient = gradient;
            matShatter = _assetBundle.LoadMaterial("matShatter");

            distanceOrb = _assetBundle.LoadEffect("DistanceOrb");
            distanceOrb.AddComponent<EffectComponent>();

            //distanceRenderer = _assetBundle.LoadEffect("FractureTrail");
            distanceRenderer.AddComponent<EffectComponent>();
            Content.CreateAndAddEffectDef(distanceRenderer);
            Content.CreateAndAddEffectDef(distanceOrb);

            CreateRift();
            CreateFracture();
        }

        private static void CreateRift()
        {
            //riftEnemyMaterial = _assetBundle.LoadMaterial("matEnemyMask");
            riftExplosionEffect = _assetBundle.LoadEffect("riftObjectTypical");                   
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

            //riftExplosionEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/BubbleShieldEndEffect.prefab").WaitForCompletion().InstantiateClone("RiftExplosionEffect");
            //riftExplosionEffect = _assetBundle.LoadEffect("RiftObjectTypical");
            riftOverlayMat = new Material[2];
            riftOverlayMat[0] = Addressables.LoadAssetAsync<Material>("RoR2/Base/bazaar/matBazaarIceDistortion.mat").WaitForCompletion();
            riftOverlayMat[1] = Addressables.LoadAssetAsync<Material>("RoR2/Base/EliteIce/matAffixWhiteSphereExplosion.mat").WaitForCompletion();
            //riftExplosionEffect.GetComponent<DestroyOnTimer>().duration = .75f;
            //riftExplosionEffect.transform.GetChild(0).gameObject.GetComponent<ParticleSystemRenderer>().material = riftOverlayMat[0];
            //riftExplosionEffect.transform.GetChild(1).gameObject.GetComponent<ParticleSystemRenderer>().material = riftOverlayMat[1];
            //riftExplosionEffect.transform.GetChild(1).gameObject.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", riftColor);

            //riftExplosionEffect.transform.GetChild(2).gameObject.SetActive(false);

            riftExplosionEffectOvercharged = _assetBundle.LoadEffect("RiftOvercharged");
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



            //riftExplosionEffectOvercharged = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/BubbleShieldEndEffect.prefab").WaitForCompletion().InstantiateClone("RiftExplosionEffectOvercharged");
            //riftExplosionEffectOvercharged.AddComponent<NetworkIdentity>();
            //riftExplosionEffectOvercharged.GetComponent<DestroyOnTimer>().duration = .75f;
            //riftExplosionEffectOvercharged.transform.GetChild(0).gameObject.GetComponent<ParticleSystemRenderer>().material = riftOverlayMat[0];
            //riftExplosionEffectOvercharged.transform.GetChild(1).gameObject.GetComponent<ParticleSystemRenderer>().material = riftOverlayMat[1];
            //riftExplosionEffectOvercharged.transform.GetChild(1).gameObject.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", overchargedColor);
            //riftExplosionEffectOvercharged.transform.GetChild(2).gameObject.SetActive(false);
            //Content.CreateAndAddEffectDef(riftExplosionEffectOvercharged);


        }

        private static void CreateFracture()        
        {

            fractureLineTracer = _assetBundle.LoadEffect("FractureTrail");
            fractureLineTracer.AddComponent<EffectComponent>();
            fractureLineTracer.AddComponent<EventFunctions>();
            fractureLineTracer.AddComponent<Tracer>();
            fractureLineTracer.AddComponent<BeamPointsFromTransforms>();
            fractureLineTracer.AddComponent<DestroyOnTimer>().duration = .5f;

            if (fractureLineTracer.TryGetComponent(out BeamPointsFromTransforms beamPoints))
            {
                beamPoints.target = fractureLineTracer.GetComponent<LineRenderer>();
                Transform[] transforms = new Transform[2];
                transforms[0] = fractureLineTracer.transform.GetChild(0).transform;
                transforms[1] = fractureLineTracer.transform.GetChild(1).transform;

                beamPoints.pointTransforms = transforms;
            }

            fractureLineTracer.transform.GetChild(2).gameObject.AddComponent<BeamPointsFromTransforms>();
            if (fractureLineTracer.transform.GetChild(2).gameObject.TryGetComponent(out BeamPointsFromTransforms beamPoints2))
            {
                beamPoints2.target = fractureLineTracer.GetComponent<LineRenderer>();
                Transform[] transforms2 = new Transform[2];
                transforms2[0] = fractureLineTracer.transform.GetChild(2).transform;
                transforms2[1] = fractureLineTracer.transform.GetChild(2).GetChild(0).transform;

                beamPoints2.pointTransforms = transforms2;
            }

            if (fractureLineTracer.TryGetComponent(out Tracer tracer))
            {
                tracer.headTransform = fractureLineTracer.transform.GetChild(0).transform;
                tracer.tailTransform = fractureLineTracer.transform.GetChild(1).transform;
                tracer.startTransform = fractureLineTracer.transform.GetChild(2).GetChild(0).transform;
                tracer.beamObject = fractureLineTracer.transform.GetChild(2).gameObject;
                tracer.beamDensity = 1f;
                tracer.speed = 300f;
                tracer.length = 10f;
            }

            Content.CreateAndAddEffectDef(fractureLineTracer);



            //fractureLineTracer = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/GoldGat/TracerGoldGat.prefab").WaitForCompletion();
            //LineRenderer lineRenderer = fractureBeam.GetComponent<LineRenderer>();
            ///fractureLineTracer.GetComponent<LineRenderer>().material = lineRenderer.material;
            //fractureLineTracer.GetComponent<LineRenderer>().positionCount = lineRenderer.positionCount;
            //for (int i = 0; i < lineRenderer.positionCount; i++)
            //{
            //    fractureLineTracer.GetComponent<LineRenderer>().SetPosition(i, lineRenderer.GetPosition(i));
            //}
            //fractureLineTracer.GetComponent<LineRenderer>().startWidth = lineRenderer.startWidth;
            //fractureLineTracer.GetComponent<LineRenderer>().endWidth = lineRenderer.endWidth;



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
            CreateBombProjectile();
            Content.AddProjectilePrefab(bombProjectilePrefab);
        }

        private static void CreateBombProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            bombProjectilePrefab = Assets.CloneProjectilePrefab("CommandoGrenadeProjectile", "RifterBombProjectile");

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
