
using RoR2;
using RoR2.Skills;
using UnityEngine;
using JetBrains.Annotations;

namespace RifterMod.Modules
{
    public class RifterSkillDef : SkillDef
    {
        protected class InstanceData : BaseSkillInstanceData
        {
            public RifterStep step;
        }
    
        public GenericSkill skillSlot;

        public bool overcharges;

        public bool usesOvercharge;

        public Sprite overchargedIcon;

        public string overchargedNameToken;

        public string overchargedDescriptionToken;

        public bool lastChargeOvercharge;



        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData 
            { 
                step = skillSlot.GetComponent<RifterStep>()
            };
        }

        public override void OnUnassigned([NotNull] GenericSkill skillSlot)
        {
            base.OnUnassigned(skillSlot);
        }

        public override Sprite GetCurrentIcon([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (instanceData.step.rifterStep > 1 && usesOvercharge)
            {
                return overchargedIcon;
            }
            return base.GetCurrentIcon(skillSlot);
        }

        public override string GetCurrentNameToken([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (instanceData.step.rifterStep > 1 && usesOvercharge)
            {
                return overchargedNameToken;
            }
            return base.GetCurrentNameToken(skillSlot);
        }

        public override string GetCurrentDescriptionToken([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (instanceData.step.rifterStep > 1 && usesOvercharge)
            {
                return overchargedDescriptionToken;
            }
            return base.GetCurrentDescriptionToken(skillSlot);
        }



        public override void OnExecute([NotNull] GenericSkill skillSlot)
        {
            base.OnExecute(skillSlot);
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (overcharges)
            {
                if (instanceData.step.rifterStep < 1)
                {
                    instanceData.step.rifterStep = 2;
                }
                
            }
            if (usesOvercharge)
            {
                instanceData.step.rifterStep -= 1;
            }        
            if (instanceData.step.rifterStep >2)
            {
                instanceData.step.rifterStep = 2;
            }
            if (instanceData.step.rifterStep < 0)
            {
                instanceData.step.rifterStep = 0;
            }
        }


    }
}