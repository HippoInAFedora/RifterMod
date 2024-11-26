
using RoR2;
using RoR2.Skills;
using UnityEngine;
using JetBrains.Annotations;
using RifterMod.Characters.Survivors.Rifter.Components;
using R2API.Utils;

namespace RifterMod.Modules
{
    public class RifterSkillDef : SkillDef
    {
        protected class InstanceData : BaseSkillInstanceData
        {
            public RifterOverchargePassive step;
        }

        public GenericSkill skillSlot;

        public int maxStockUses;

        public bool overcharges;

        public bool usesOvercharge;

        public int usedOvercharge = 0;





        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData
            {
                step = skillSlot.GetComponent<RifterOverchargePassive>(),
            };
        }

        public override void OnUnassigned([NotNull] GenericSkill skillSlot)
        {
            base.OnUnassigned(skillSlot);
        }

        public override void OnExecute([NotNull] GenericSkill skillSlot)
        {
            base.OnExecute(skillSlot);
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (overcharges)
            {
                //instanceData.step.rifterOverchargePassive++;
            }
            if (instanceData.step.rifterOverchargePassive < 0)
            {
                instanceData.step.rifterOverchargePassive = 0;
            }
            if (usesOvercharge)
            {
                usedOvercharge -= stockToConsume;
                if (usedOvercharge < 0)
                {
                    usedOvercharge = 0;
                }
            }
        }

        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot, float deltaTime)
        {
            base.OnFixedUpdate(skillSlot, deltaTime);
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (instanceData.step.rifterOverchargePassive > 0)
            {
                if (usesOvercharge && usedOvercharge < 6)
                {
                    skillSlot.AddOneStock();
                    instanceData.step.rifterOverchargePassive--;
                    usedOvercharge++;
                }
            }
            if (usesOvercharge)
            {
                stockToConsume = (skillSlot.stock >= maxStockUses) ? maxStockUses : skillSlot.stock;
                instanceData.step.stocksConsumed = stockToConsume;
            }
        }
    }
}