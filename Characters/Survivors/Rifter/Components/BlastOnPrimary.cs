using RifterMod.Survivors.Rifter;
using RifterMod.Survivors.Rifter.SkillStates;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Characters.Survivors.Rifter.Components
{
    public class BlastOnPrimary: ProjectileExplosion
    {
        public override void Awake()
        {
            base.Awake();
            RiftBase.onPrimaryShot += RiftBase_onPrimaryShot;
        }

        private void RiftBase_onPrimaryShot(GameObject obj)
        {
            if (base.gameObject == null)
            {
                return;
            }
            if (projectileController.owner == obj)
            {
                projectileDamage.damage = projectileController.owner.GetComponent<CharacterBody>().damage * RifterStaticValues.wRiftOnPrimaryBlastCoefficient;
                ModifiedDetonate();
            }
        }

        public void ModifiedDetonate()
        {
            if (NetworkServer.active)
            {
                DetonateServer();
            }
        }

        public override void OnBlastAttackResult(BlastAttack blastAttack, BlastAttack.Result result)
        {
            base.OnBlastAttackResult(blastAttack, result);
        }

        public void OnDestroy()
        {
            RiftBase.onPrimaryShot -= RiftBase_onPrimaryShot;
        }

    }
}
