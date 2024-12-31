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
    public class ProjectileImpactExplosionModify: ProjectileImpactExplosion
    {
        public override void OnBlastAttackResult(BlastAttack blastAttack, BlastAttack.Result result)
        {
            base.OnBlastAttackResult(blastAttack, result);
            if (NetworkServer.active)
            {
                foreach (var item in result.hitPoints)
                {
                    item.hurtBox.healthComponent.body.AddTimedBuff(RifterBuffs.superShatterDebuff, 1f);
                }
            }
           
        }
    }
}
