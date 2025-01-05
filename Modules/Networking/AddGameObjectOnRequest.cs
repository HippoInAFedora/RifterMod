using R2API.Networking.Interfaces;
using System;
using System.Collections.Generic;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;
using EntityStates;
using static UnityEngine.UI.GridLayoutGroup;
using RifterMod.Characters.Survivors.Rifter.Components;

namespace RifterMod.Modules.Networking
{
    internal class AddGameObjectOnRequest : INetMessage, ISerializableObject
    {

        private NetworkInstanceId netIdOwner;

        private NetworkInstanceId netId;

        private bool adding;

        public AddGameObjectOnRequest()
        {

        }

        public AddGameObjectOnRequest(NetworkInstanceId netIdOwner, NetworkInstanceId netId, bool adding)
        {
            this.netId = netIdOwner;
            this.netId = netId;
            this.adding = adding;
            
        }

        public void Deserialize(NetworkReader reader)
        {
            netIdOwner = reader.ReadNetworkId();
            netId = reader.ReadNetworkId();
            adding = reader.ReadBoolean();
        }

        public void OnReceived()
        {
            ForceAdd();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netIdOwner);
            writer.Write(netId);
            writer.Write(adding);
        }

        public void ForceAdd()
        {
            GameObject gameObjectOwner = Util.FindNetworkObject(netIdOwner);
            GameObject gameObject = Util.FindNetworkObject(netId);
            if (!gameObjectOwner)
            {
                Debug.Log("Specified Owner GameObject not found!");
                return;
            }
            if (!gameObject)
            {
                Debug.Log("Specified GameObject not found!");
                return;
            }
            if (NetworkServer.localClientActive)
            {
                if (gameObjectOwner.TryGetComponent(out RifterOverchargePassive component))
                {
                    if (adding == true)
                    {
                        component.deployedList.Add(gameObject);
                    }

                    if (adding == false)
                    {
                        component.deployedList.Remove(gameObject);
                    }
                    
                }
            }
        }
    }
}
