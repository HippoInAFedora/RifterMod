using R2API.Networking.Interfaces;
using System;
using System.Collections.Generic;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;
using EntityStates;
using static UnityEngine.UI.GridLayoutGroup;

namespace RifterMod.Modules.Networking
{
    internal class TeleportOnBodyRequest : INetMessage, ISerializableObject
    {

        private NetworkInstanceId netId;

        private Vector3 currentPosition;

        private Vector3 targetFootPosition;

        private bool isPortalTeleport;


        public TeleportOnBodyRequest()
        {

        }

        public TeleportOnBodyRequest(NetworkInstanceId netId, Vector3 currentPosition, Vector3 targetFootPosition, bool isPortalTeleport)
        {
            this.netId = netId;
            this.currentPosition = currentPosition;
            this.targetFootPosition = targetFootPosition;
            this.isPortalTeleport = isPortalTeleport;
        }

        public void Deserialize(NetworkReader reader)
        {
            netId = reader.ReadNetworkId();
            currentPosition = reader.ReadVector3();
            targetFootPosition = reader.ReadVector3();
            isPortalTeleport = reader.ReadBoolean();
        }

        public void OnReceived()
        {
            ForceTeleport();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netId);
            writer.Write(currentPosition);
            writer.Write(targetFootPosition);
            writer.Write(isPortalTeleport);
        }

        public void ForceTeleport()
        {
            GameObject gameObject = Util.FindNetworkObject(netId);
            if (!gameObject)
            {
                Debug.Log("Specified GameObject not found!");
                return;
            }
            CharacterMaster component = gameObject.GetComponent<CharacterMaster>();
            if (!component)
            {
                Debug.Log("CharacterMaster not found!");
                return;
            }
            GameObject bodyObject = component.GetBodyObject();
            if (!bodyObject)
            {
                return;
            }
            HealthComponent component2 = bodyObject.GetComponent<HealthComponent>();
            if (!(component2 == null) && !(component2.health <= 0f))
            {
                EntityStateMachine entityStateMachine = EntityStateMachine.FindByCustomName(bodyObject, "Body");
                if (entityStateMachine != null)
                {
                    entityStateMachine.SetInterruptState(new ModifiedTeleport
                    {
                        currentPosition = currentPosition,
                        targetFootPosition = targetFootPosition,
                        teleportWaitDuration = .25f,
                        isPortalTeleport = isPortalTeleport,
                    }, InterruptPriority.Frozen);
                }
            }
        }
    }
}
