using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;

namespace RifterMod.Characters.Survivors.Rifter.Components
{
    internal class DestroyOnRift : MonoBehaviour
    {
        public bool destroying = false;

        ParticleSystem particleSystem;

        public Vector3 position;

        public GameObject owner;

        public RifterOverchargePassive rifterStep;

        public void Awake()
        {
            particleSystem = GetComponentInChildren<ParticleSystem>();        
        }

        public void Start()
        {
            position = transform.position;
            if (owner != null && owner.TryGetComponent(out rifterStep))
            {
                rifterStep.deployedList.Add(position);
            }
        }

        public void FixedUpdate()
        {
            if (destroying)
            {
                Destroy(base.gameObject);
            }
        }

        public void OnDestroy()
        {
            Destroy(particleSystem);
            if (owner != null && owner.TryGetComponent(out rifterStep))
            {
                rifterStep.deployedList.Remove(position);
            }           
        }

    }
}
