using RifterMod.Survivors.Rifter;
using RoR2;
using System;
using UnityEngine;


namespace RifterMod.Modules
{
    internal class FractureBeamComponent : MonoBehaviour
    {
        public Transform trans1;
        public Transform trans2;

        public bool trans1isOffset;
        public bool trans2isOffset;

        public Vector3 offsetVector;
        Vector3 positionMain;

        private float duration = .1f;

        void Start()
        {
            offsetVector = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        }

        public void FixedUpdate() 
        {
            float durationBetweenOffsets = Time.fixedDeltaTime;
            if (durationBetweenOffsets > duration)
            {
                offsetVector = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
                durationBetweenOffsets = 0f;
            }
        }
        void Update()
        {
            positionMain = (trans1.position + trans2.position) / 2;
            base.transform.position = positionMain + offsetVector;
        }
    }
}
