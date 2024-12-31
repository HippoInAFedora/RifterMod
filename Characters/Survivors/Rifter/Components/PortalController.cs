using RoR2;
using RifterMod.Survivors.Rifter;
using System.Collections.Generic;
using UnityEngine;
using HG;
using UnityEngine.Networking;
using EntityStates;
using R2API;
using UnityEngine.UIElements;
using RoR2.Projectile;
using System.Runtime.InteropServices;

namespace RifterMod.Characters.Survivors.Rifter.Components
{
    internal class PortalController : NetworkBehaviour
    {
        public Transform pointATransform;
        public Transform pointBTransform;

        [SyncVar(hook = "SetPointAPosition")]
        private Vector3 pointAPosition;

        [SyncVar(hook = "SetPointBPosition")]
        private Vector3 pointBPosition;


        public Vector3 NetworkpointAPosition
        {
            get
            {
                return pointAPosition;
            }
            [param: In]
            set
            {
                if (NetworkServer.localClientActive && !base.syncVarHookGuard)
                {
                    base.syncVarHookGuard = true;
                    SetPointAPosition(value);
                    base.syncVarHookGuard = false;
                }
                SetSyncVar(value, ref pointAPosition, 1u);
            }
        }

        public Vector3 NetworkpointBPosition
        {
            get
            {
                return pointBPosition;
            }
            [param: In]
            set
            {
                if (NetworkServer.localClientActive && !base.syncVarHookGuard)
                {
                    base.syncVarHookGuard = true;
                    SetPointBPosition(value);
                    base.syncVarHookGuard = false;
                }
                SetSyncVar(value, ref pointBPosition, 2u);
            }
        }

        public void SetPointAPosition(Vector3 position)
        {
            NetworkpointAPosition = position;
            pointATransform.position = pointAPosition;
            pointATransform.LookAt(pointBTransform);
            pointBTransform.LookAt(pointATransform);
        }

        public void SetPointBPosition(Vector3 position)
        {
            NetworkpointBPosition = position;
            pointBTransform.position = pointBPosition;
            pointATransform.LookAt(pointBTransform);
            pointBTransform.LookAt(pointATransform);
        }

        private void OnDestroy()
        {
            if (NetworkServer.active)
            {
                Destroy(gameObject);
            }
        }

        public override bool OnSerialize(NetworkWriter writer, bool forceAll)
        {
            if (forceAll)
            {
                writer.Write(pointAPosition);
                writer.Write(pointBPosition);
                return true;
            }
            bool flag = false;
            if ((base.syncVarDirtyBits & (true ? 1u : 0u)) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(pointAPosition);
            }
            if ((base.syncVarDirtyBits & 2u) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(pointBPosition);
            }
            if (!flag)
            {
                writer.WritePackedUInt32(base.syncVarDirtyBits);
            }
            return flag;
        }

        public override void OnDeserialize(NetworkReader reader, bool initialState)
        {
            if (initialState)
            {
                pointAPosition = reader.ReadVector3();
                pointBPosition = reader.ReadVector3();
                return;
            }
            int num = (int)reader.ReadPackedUInt32();
            if (((uint)num & (true ? 1u : 0u)) != 0)
            {
                SetPointAPosition(reader.ReadVector3());
            }
            if (((uint)num & 2u) != 0)
            {
                SetPointBPosition(reader.ReadVector3());
            }
        }
    }
       
}
