using EntityStates;
using RifterMod.Characters.Survivors.Rifter.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    internal class PortalAuxDrop : BaseSkillState
    {
        private float duration = .5f;

        public Vector3 position;
        public Quaternion rotation;

        public GameObject portalPrefab = RifterAssets.portal;
        private GameObject portalInstance;
        private GameObject portalInstance2;

        public PortalInstanceTracker portalTracker;

        public Vector3 portalMainPosition;

        public override void OnEnter()
        {
            base.OnEnter();
            portalTracker = base.GetComponent<PortalInstanceTracker>();
            if (portalTracker.portalMain != null)
            {
                Destroy(portalTracker.portalMain);
            }
            if (portalTracker.portalAux != null)
            {
                Destroy(portalTracker.portalAux);
            }
            float maxDistance = RifterStaticValues.riftSpecialDistance;
            position = GetAimRay().GetPoint(maxDistance);
            if (Physics.Raycast(GetAimRay(), out var hitInfo, maxDistance, LayerIndex.CommonMasks.bullet))
            {
                position = hitInfo.point;
            }
            if (NetworkServer.active)
            {
                portalInstance = Object.Instantiate(portalPrefab, portalMainPosition, base.transform.rotation);
                portalInstance.transform.localScale = Vector3.one * 2.5f;

                portalInstance2 = Object.Instantiate(portalPrefab, position, base.transform.rotation);
                portalInstance2.transform.localScale = Vector3.one * 2.5f;
            
                portalTracker.portalMain = portalInstance;
                portalTracker.portalAux = portalInstance2;

                if (portalInstance.TryGetComponent(out PortalController controllerForMain))
                {
                    controllerForMain.owner = base.gameObject;
                    controllerForMain.otherPortal = portalInstance2;
                    controllerForMain.isMain = true;
                }
                if (portalInstance2.TryGetComponent(out PortalController controllerForAux))
                {
                    controllerForAux.owner = base.gameObject;
                    controllerForAux.otherPortal = portalInstance;
                    controllerForAux.isMain = false;
                }


                Deployable component = portalInstance.GetComponent<Deployable>();
                if ((bool)component && (bool)base.characterBody.master)
                {
                    base.characterBody.master.AddDeployable(component, RifterSurvivor.portalSlot);
                }
                NetworkServer.Spawn(portalInstance);
                NetworkServer.Spawn(portalInstance2);
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

    }
}
