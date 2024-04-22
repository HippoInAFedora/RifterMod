
using RoR2;
using RoR2.Skills;
using UnityEngine;
using JetBrains.Annotations;
using RifterMod.Characters.Survivors.Rifter.Components;

namespace RifterMod.Modules
{
    public class RifterSkillDef : SkillDef
    {
        protected class InstanceData : BaseSkillInstanceData
        {
            public RifterOverchargePassive step;
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
                step = skillSlot.GetComponent<RifterOverchargePassive>()
            };
        }

        public override void OnUnassigned([NotNull] GenericSkill skillSlot)
        {
            base.OnUnassigned(skillSlot);
        }

        public override Sprite GetCurrentIcon([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (instanceData.step.rifterOverchargePassive > 1 && usesOvercharge)
            {
                return overchargedIcon;
            }
            return base.GetCurrentIcon(skillSlot);
        }

        public override string GetCurrentNameToken([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (instanceData.step.rifterOverchargePassive > 1 && usesOvercharge)
            {
                return overchargedNameToken;
            }
            return base.GetCurrentNameToken(skillSlot);
        }

        public override string GetCurrentDescriptionToken([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (instanceData.step.rifterOverchargePassive > 1 && usesOvercharge)
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
                if (instanceData.step.rifterOverchargePassive < 1)
                {
                    instanceData.step.rifterOverchargePassive = 1;
                }
                instanceData.step.rifterOverchargePassive++;

            }
            if (usesOvercharge)
            {
                instanceData.step.rifterOverchargePassive -= 1;
            }
            //if (instanceData.step.rifterOverchargePassive >2)
            //{
            //    instanceData.step.rifterOverchargePassive = 2;
            //}
            if (instanceData.step.rifterOverchargePassive < 0)
            {
                instanceData.step.rifterOverchargePassive = 0;
            }
        }


    }
}