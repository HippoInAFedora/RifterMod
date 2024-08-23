using RifterMod.Survivors.Rifter;
using RoR2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking.NetworkSystem;

namespace RifterMod.Modules
{
    internal class ShatterOverlay : MonoBehaviour
    {
        float _ClipVal;
        public float stacks;
        public float val;

        TemporaryOverlay temporaryOverlay;
        public Material matShatter = RifterAssets.matShatter;

        void Start()
        {
            ModelLocator locator = base.gameObject.GetComponent<ModelLocator>();
            if (!locator)
            {
                return;
            }
            Transform modelTransform = locator.modelTransform;
            if (modelTransform)
            {
                temporaryOverlay = base.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = float.PositiveInfinity;
                temporaryOverlay.originalMaterial = matShatter;
                //temporaryOverlay.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
               // Debug.Log("Temp Overlay Added");
            }
        }

        void Update()
        {
            CharacterBody body = base.gameObject.GetComponent<CharacterBody>();
            val = body.GetBuffCount(RifterBuffs.shatterDebuff);
            ChangeValue(val);
        }

        public void ChangeValue(float val) //enables changing the value of progress bar
        {
            _ClipVal = 1 - val / 20;
            if ((bool)temporaryOverlay)
            {
                temporaryOverlay.originalMaterial.SetFloat("_ClipVal", _ClipVal);
               // Debug.Log(_ClipVal);
                //Debug.Log("material has" + temporaryOverlay.originalMaterial.GetFloat("_ClipVal") + "for _ClipVal");

            }         

        }
    }
}
