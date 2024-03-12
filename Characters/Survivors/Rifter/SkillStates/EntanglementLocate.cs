using EntityStates;
using IL.RoR2.Skills;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class EntanglementLocate : RiftRiderLocate
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(base.isAuthority && (bool)base.inputBank)
            {
                if ((bool)base.skillLocator && base.skillLocator.special.IsReady() && base.inputBank.skill4.justPressed)
                {
                    outer.SetNextStateToMain();
                }
                else if (base.inputBank.skill1.justPressed || base.inputBank.skill3.justReleased)
                {
                    outer.SetNextState(new Entanglement());
                }
            }
            
        }
    }

}
    

