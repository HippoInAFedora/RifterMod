using EntityStates;
using Newtonsoft.Json.Linq;
using RifterMod.Survivors.Rifter;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftSpecialStateSwap : BaseState
    {



        private int specialPrimary;

        private int specialSecondary;

        bool flag = false;
        bool flag2 = false;
        bool flag3 = false;

        private int specialUtility;

        float duration = 10f;

        public SkillDef primaryOverride;
        public SkillDef secondaryOverride;
        public SkillDef utilityOverride;

        public GenericSkill.SkillOverride SkillOverridePrimary;
        public GenericSkill.SkillOverride SkillOverrideSecondary;
        public GenericSkill.SkillOverride SkillOverrideUtility;

        GenericSkill utilitySkill;


        public override void OnEnter()
        {
            base.OnEnter();
            utilitySkill = base.skillLocator.utility;
            primaryOverride = SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("RiftPrimarySpecial"));
            secondaryOverride = SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("RiftSecondarySpecial"));
            utilityOverride = SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("RiftUtilitySpecial"));
            base.skillLocator.primary.SetSkillOverride(this, primaryOverride, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.secondary.SetSkillOverride(this, secondaryOverride, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.utility.SetSkillOverride(this, utilityOverride, GenericSkill.SkillOverridePriority.Contextual);
            specialPrimary = 1;
            specialSecondary = 1;
            specialUtility = 1;
            SkillOverridePrimary.Equals(primaryOverride);
            SkillOverrideSecondary.Equals(secondaryOverride);
            SkillOverrideUtility.Equals(utilityOverride);
        }

        public override void OnExit()
        {
            if (base.isAuthority)
            {
                Debug.Log(specialPrimary.ToString() + specialSecondary.ToString() + specialUtility.ToString());
                if((bool)base.skillLocator.primary)
                {
                    if (specialPrimary != 0)
                    {
                        base.skillLocator.primary.UnsetSkillOverride(this, primaryOverride, GenericSkill.SkillOverridePriority.Contextual);
                    }
                    else
                    {
                        base.skillLocator.primary.UnsetSkillOverride(SkillOverridePrimary, primaryOverride, GenericSkill.SkillOverridePriority.Contextual);
              
                    }
                }
                if ((bool)base.skillLocator.secondary)
                {
                    if (specialSecondary != 0)
                    {
                        base.skillLocator.secondary.UnsetSkillOverride(this, secondaryOverride, GenericSkill.SkillOverridePriority.Contextual);
                    }
                   
                }
                if ((bool)base.skillLocator.utility)
                {
                    if (specialUtility != 0)
                    {
                        base.skillLocator.utility.UnsetSkillOverride(this, utilityOverride, GenericSkill.SkillOverridePriority.Contextual);
                    }
                    else
                    {

                        base.skillLocator.utility.UnsetSkillOverride(this, utilityOverride, GenericSkill.SkillOverridePriority.Contextual);
                        
                        
                      
                    }
                }
                
            }

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority)
            {
                AuthorityFixedUpdate();
            }
            int num = 3;
            int num2 = specialPrimary + specialSecondary + specialUtility;
            if (num2 < num && base.isAuthority || base.fixedAge >= duration)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        private void AuthorityFixedUpdate()
        {

            if (base.inputBank.skill1.justPressed && flag == false)
            {
                flag = true;
            }
            if (base.inputBank.skill2.justPressed && flag2 == false)
            {
                flag2 = true;
            }
            if (base.inputBank.skill3.justPressed && utilitySkill.stock != 0 && flag3 == false)
            {
                flag3 = true;
            }

            if (flag)
            {
                outer.SetNextState(new RiftPrimarySpecial());
                base.skillLocator.special.stock -= 1;
                specialPrimary -= 1;
                return;
            }
            if (flag2)
            {
                outer.SetNextState(new RiftSecondarySpecial());
                base.skillLocator.special.stock -= 1;
                specialSecondary -= 1;
                return;
            }
            if(flag3)
            {
                outer.SetNextState(new RiftUtilitySpecialLocate());
                base.skillLocator.special.stock -= 1;
                specialUtility -= 1;
                return;
            }
        }
    }
}