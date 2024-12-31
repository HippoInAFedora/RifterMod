using EntityStates;
using RifterMod.Characters.Survivors.Rifter.Components;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    internal class PortalDrop : BaseSkillState
    {
        private float duration = .5f;

        public Vector3 position;
        public Vector3 portalMainPosition;


        public GameObject portalPrefab = RifterAssets.portal;
        public GameObject portalInstance;

        //public PortalInstanceTracker portalTracker;

        
        public override void OnEnter()
        {
            base.OnEnter();
            //portalTracker = base.GetComponent<PortalInstanceTracker>();
            //if (portalTracker.portalMain != null)
            //{
            //    Destroy(portalTracker.portalMain);
            //}
            //if (portalTracker.portalAux != null)
            //{
            //    Destroy(portalTracker.portalAux);
            //}
            float maxDistance = RifterStaticValues.riftSpecialDistance;
            position = GetAimRay().GetPoint(maxDistance);
            if (Physics.Raycast(GetAimRay(), out var hitInfo, maxDistance, LayerIndex.CommonMasks.bullet))
            {
                position = hitInfo.point + hitInfo.normal * 2f;
            }



            if (NetworkServer.active)
            {
                if (portalPrefab)
                {
                    portalInstance = UnityEngine.Object.Instantiate(portalPrefab);
                    PortalController portalController = portalInstance.GetComponent<PortalController>();
                    portalController.SetPointAPosition(portalMainPosition);
                    portalController.SetPointBPosition(position);
                    portalInstance.AddComponent<DestroyOnTimer>().duration = 15f;
                    portalInstance.GetComponent<GenericOwnership>().ownerObject = base.gameObject;
                }                
                NetworkServer.Spawn(portalInstance);
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge > duration && base.isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(portalMainPosition);
            writer.Write(position);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            portalMainPosition = reader.ReadVector3();
            position = reader.ReadVector3();
        }

    }
}
