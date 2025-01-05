using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using R2API;
using UnityEngine.UIElements.StyleSheets;
using RifterMod.Survivors.Rifter;
using HG;
using EntityStates;
using Newtonsoft.Json.Utilities;
using RifterMod.Survivors.Rifter.SkillStates;
using R2API.Networking.Interfaces;
using R2API.Networking;
using RifterMod.Modules.Networking;
using System.Runtime.InteropServices;

namespace RifterMod.Characters.Survivors.Rifter.Components
{
    internal class DestroyOnRift : NetworkBehaviour
    {
        public bool destroying = false;

        ParticleSystem particleSystem;

        public Vector3 position;

        [SyncVar]
        public GameObject owner;

        public CharacterBody ownerBody;

        public RifterOverchargePassive rifterStep;

        public Collider hitTimelockCollider;
        public int hitEnemies;

        public static GameObject blastEffectPrefab = RifterAssets.riftExplosionEffect;
        public static GameObject overchargedEffectPrefab = RifterAssets.riftExplosionEffectOvercharged;

        private NetworkInstanceId ___ownerNetId;

        public static GameObject hitEffectPrefab = RifterAssets.hitEffect;

        public GameObject Networkowner
        {
            get
            {
                return owner;
            }
            [param: In]
            set
            {
                SetSyncVarGameObject(value, ref owner, 2u, ref ___ownerNetId);
            }
        }

        public void Awake()
        {
            particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        public void Start()
        {
            position = transform.position;
            if (owner != null)
            {
                ownerBody = owner.GetComponent<CharacterBody>();
            }
            if (owner != null && owner.TryGetComponent(out rifterStep))
            {
                //netIdOwner = ownerBody.netId;
                //netId = base.gameObject.GetComponent<NetworkIdentity>().netId;
                rifterStep.deployedList.Add(this.gameObject);
                //NetMessageExtensions.Send((INetMessage)(object)new AddGameObjectOnRequest( netIdOwner, netId, true ), NetworkDestination.Clients);
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
            if (!destroying && owner != null && NetworkServer.active)
            {
                Search2(base.transform.position);
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.attacker = gameObject;
                blastAttack.inflictor = gameObject;
                blastAttack.teamIndex = TeamIndex.Player;
                blastAttack.radius = RifterStaticValues.timelockRadius;
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseDamage = ownerBody.damage * RifterStaticValues.timelockBlast * RifterStaticValues.timelockEnemyMultiplier * hitEnemies;
                blastAttack.crit = ownerBody.RollCrit();
                blastAttack.baseForce = 0f;
                blastAttack.procCoefficient = 1f;
                blastAttack.canRejectForce = false;
                blastAttack.position = position;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                blastAttack.AddModdedDamageType(RifterDamage.riftSuperDamage);
                blastAttack.damageType.damageSource = DamageSource.Special;
                blastAttack.Fire();
                EffectData effectData = new EffectData();
                effectData.scale = blastAttack.radius / 10f;
                effectData.origin = position;
                EffectManager.SpawnEffect(overchargedEffectPrefab, effectData, transmit: true);
            }
            if (owner != null && owner.TryGetComponent(out rifterStep))
            {
                rifterStep.deployedList.Remove(this.gameObject);
                //NetMessageExtensions.Send((INetMessage)(object)new AddGameObjectOnRequest(netIdOwner, netId, false), NetworkDestination.Clients);
            }           
        }

        public void Search2(Vector3 position)
        {
            SphereSearch search = new SphereSearch();
            search.origin = position;
            search.radius = RifterStaticValues.timelockRadius;
            search.mask = LayerIndex.entityPrecise.mask;
            search.queryTriggerInteraction = QueryTriggerInteraction.Collide;
            List<HurtBox> listH = CollectionPool<HurtBox, List<HurtBox>>.RentCollection();
            search.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.AllExcept(TeamIndex.Player)).FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes(listH);
            hitEnemies = listH.Count;
            CollectionPool<HurtBox, List<HurtBox>>.ReturnCollection(listH);
        }
        private void UNetVersion()
        {
        }

        public override bool OnSerialize(NetworkWriter writer, bool forceAll)
        {
            if (forceAll)
            {
                writer.Write(owner);
                return true;
            }
            bool flag = false;
            if ((base.syncVarDirtyBits & 2u) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(owner);
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
                ___ownerNetId = reader.ReadNetworkId();
                return;
            }
            int num = (int)reader.ReadPackedUInt32();
            if (((uint)num & 2u) != 0)
            {
                owner = reader.ReadGameObject();
            }
        }
        public override void PreStartClient()
        {
            if (!___ownerNetId.IsEmpty())
            {
                Networkowner = ClientScene.FindLocalObject(___ownerNetId);
            }
        }

    } 
}
