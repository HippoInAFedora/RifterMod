using EntityStates;
using IL.RoR2.Skills;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftRiderLocate : BaseState
    {
        //This is deprecated. Still exists solely for EntanglementLocate to work correctly
        public static GameObject teleportLocatorPrefab = global::EntityStates.Huntress.ArrowRain.areaIndicatorPrefab;
        public static GameObject teleportLocatorInstance;

        public override void OnEnter()
        {
            base.OnEnter();
            if ((bool)teleportLocatorPrefab)
            {
                teleportLocatorInstance = Object.Instantiate(teleportLocatorPrefab);
                teleportLocatorInstance.transform.localScale = new Vector3(7f, 7f, 7f);
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(base.isAuthority && (bool)base.inputBank)
            {
                if (base.inputBank.skill3.justReleased)
                {
                    outer.SetNextState(new RiftRider());
                }
            }
            
        }

        public override void Update()
        {
            base.Update();
            UpdateAreaIndicator();
        }

        private void UpdateAreaIndicator()
        {
            if((bool)teleportLocatorInstance)
            {
                float maxDistance = RifterStaticValues.riftPrimaryDistance;
                teleportLocatorInstance.transform.position = base.GetAimRay().GetPoint(maxDistance);
                if (Physics.Raycast(base.GetAimRay(), out var hitInfo, maxDistance, LayerIndex.world.mask))
                {
                    teleportLocatorInstance.transform.position = hitInfo.point;
                }
            }
        }

        public override void OnExit()
        {
            if ((bool)teleportLocatorInstance)
            {
                EntityState.Destroy(teleportLocatorInstance);
            }
            base.OnExit();
        }

    }

}
    

