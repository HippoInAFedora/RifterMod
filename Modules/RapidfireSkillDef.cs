
using RoR2;
using RoR2.Skills;
using UnityEngine;
using JetBrains.Annotations;
using HG;
using RifterMod.Survivors.Rifter;
using System.Collections.Generic;
using System;

namespace RifterMod.Modules
{
    public class RapidfireSkillDef : RifterSkillDef
    {
        public float baseRecharge;
        public float newRecharge;

        private bool runRecharge;
        public override void OnExecute([NotNull] GenericSkill skillSlot)
        {
            base.OnExecute(skillSlot);
            if (skillSlot.stock == 0)
            {
                skillSlot.RunRecharge(-newRecharge + skillSlot.baseRechargeInterval);
            }
        }

        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            base.OnFixedUpdate(skillSlot);
            if (skillSlot.stock == 1)
            {
                instanceData.step.rapidfireShot = true;
            }
            else
            {
                instanceData.step.rapidfireShot = false;
            }
        }
        public override Sprite GetCurrentIcon([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (instanceData.step.rapidfireShot)
            {
                return overchargedIcon;
            }
            return base.GetCurrentIcon(skillSlot);
        }

        public override string GetCurrentNameToken([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (instanceData.step.rapidfireShot)
            {
                return overchargedNameToken;
            }
            return base.GetCurrentNameToken(skillSlot);
        }

        public override string GetCurrentDescriptionToken([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (instanceData.step.rapidfireShot)
            {
                return overchargedDescriptionToken;
            }
            return base.GetCurrentDescriptionToken(skillSlot);
        }

        public override int GetRechargeStock([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (skillSlot.stock == 0)
            {
                return base.GetMaxStock(skillSlot);
            }
            return base.rechargeStock;
        }
    }
}