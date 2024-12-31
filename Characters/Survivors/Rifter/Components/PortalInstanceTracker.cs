using RoR2;
using RifterMod.Survivors.Rifter;
using System.Collections.Generic;
using UnityEngine;
using HG;
using UnityEngine.Networking;
using EntityStates;
using R2API;
using UnityEngine.UIElements;

namespace RifterMod.Characters.Survivors.Rifter.Components
{
    internal class PortalInstanceTracker : NetworkBehaviour
    { 

        public GameObject owner;

        public GameObject portalMain;

        public GameObject portalAux;

        public static float duration;

        public void Awake()
        {
            owner = GetComponent<GameObject>();
        }

        public void Start()
        {

        }


        public void FixedUpdate()
        {
            if (!base.hasAuthority)
            {
                return;
            }
            if (portalMain == null)
            {
                duration = 0;
            }
            if (portalMain != null)
            {
                duration += Time.fixedDeltaTime;
            }
            if (duration > 15f)
            {
                if (portalMain != null)
                {
                    Destroy(portalMain);
                }
                if (portalAux != null)
                {
                    Destroy(portalAux);
                }
                duration = 0;
            }           
        }
    }
}
