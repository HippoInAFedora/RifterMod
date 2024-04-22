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

namespace RifterMod.Survivors.Rifter
{
    public static class RifterAssets
    {
        // particle effects

        internal static List<EffectDef> effectDefs = new List<EffectDef>();

        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;

        public static GameObject riftExplosionEffect;
        public static GameObject riftExplosionEffectOvercharged;
        public static GameObject fractureLineEffect;

        public static Material[] riftOverlayMat;

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
            CreateRift();
            CreateFracture();
        }

        private static void CreateRift()
        {

            riftOverlayMat = new Material[2];
            riftOverlayMat[0] = Addressables.LoadAssetAsync<Material>("RoR2/Base/bazaar/matBazaarIceDistortion.mat").WaitForCompletion();
            riftOverlayMat[1] = Addressables.LoadAssetAsync<Material>("RoR2/Base/EliteIce/matAffixWhiteSphereExplosion.mat").WaitForCompletion();

            riftExplosionEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/BubbleShieldEndEffect.prefab").WaitForCompletion().InstantiateClone("RiftExplosionEffect");
            riftExplosionEffect.AddComponent<NetworkIdentity>();
            riftExplosionEffect.GetComponent<DestroyOnTimer>().duration = .75f;
            riftExplosionEffect.transform.GetChild(0).gameObject.GetComponent<ParticleSystemRenderer>().material = riftOverlayMat[0];
            riftExplosionEffect.transform.GetChild(1).gameObject.GetComponent<ParticleSystemRenderer>().material = riftOverlayMat[1];
            riftExplosionEffect.transform.GetChild(1).gameObject.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", riftColor);

            riftExplosionEffect.transform.GetChild(2).gameObject.SetActive(false);
            Content.CreateAndAddEffectDef(riftExplosionEffect);


            riftExplosionEffectOvercharged = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/BubbleShieldEndEffect.prefab").WaitForCompletion().InstantiateClone("RiftExplosionEffectOvercharged");
            riftExplosionEffectOvercharged.AddComponent<NetworkIdentity>();
            riftExplosionEffectOvercharged.GetComponent<DestroyOnTimer>().duration = .75f;
            riftExplosionEffectOvercharged.transform.GetChild(0).gameObject.GetComponent<ParticleSystemRenderer>().material = riftOverlayMat[0];
            riftExplosionEffectOvercharged.transform.GetChild(1).gameObject.GetComponent<ParticleSystemRenderer>().material = riftOverlayMat[1];
            riftExplosionEffectOvercharged.transform.GetChild(1).gameObject.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", overchargedColor);
            riftExplosionEffectOvercharged.transform.GetChild(2).gameObject.SetActive(false);
            Content.CreateAndAddEffectDef(riftExplosionEffectOvercharged);


        }

        private static void CreateFracture()
        {
            fractureLineEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/TracerRailgunLight.prefab").WaitForCompletion().InstantiateClone("FractureLineEffect");
            fractureLineEffect.transform.GetChild(4).gameObject.SetActive(false);
            fractureLineEffect.transform.GetChild(3).GetChild(0).GetChild(0).gameObject.GetComponent<LineRenderer>().materials[0] = riftOverlayMat[1];
            fractureLineEffect.transform.GetChild(3).GetChild(0).GetChild(1).gameObject.GetComponent<LineRenderer>().materials[0] = riftOverlayMat[1];
            fractureLineEffect.transform.GetChild(3).GetChild(1).GetChild(0).gameObject.GetComponent<LineRenderer>().materials[0].SetFloat("_DisableRemapOn", 1);

            //Color colorMax = fractureLineEffect.transform.GetChild(4).gameObject.GetComponent<ParticleSystem>().main.startColor.colorMax;
            //colorMin = riftColor;
            //colorMax = overchargedColor;
            Content.CreateAndAddEffectDef(fractureLineEffect);
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
