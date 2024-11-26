using EntityStates;
using IL.RoR2.Skills;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Characters.Survivors.Rifter.SkillStates.UnusedStates
{
    public class EntanglementLocate : RiftRiderLocate
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority && (bool)inputBank)
            {
                if ((bool)skillLocator && skillLocator.special.IsReady() && inputBank.skill4.justPressed)
                {
                    outer.SetNextStateToMain();
                }
                else if (inputBank.skill1.justPressed || inputBank.skill3.justReleased)
                {
                    outer.SetNextState(new Entanglement());
                }
            }

        }
    }

}


