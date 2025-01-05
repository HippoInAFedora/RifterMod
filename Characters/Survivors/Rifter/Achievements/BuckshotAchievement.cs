using RifterMod.Survivors.Rifter.SkillStates;
using RoR2;
using RifterMod.Modules.Achievements;
using System.Collections.Generic;
using RoR2.Achievements;
using UnityEngine;
using RifterMod.Characters.Survivors.Rifter.Components;

namespace RifterMod.Survivors.Rifter.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 5, null)]
    public class BuckshotAchievement : BaseAchievement
    {
        public const string identifier = RifterSurvivor.Rifter_PREFIX + "BUCKSHOT_ACHIEVEMENT";
        public const string unlockableIdentifier = RifterSurvivor.Rifter_PREFIX + "BUCKSHOT_UNLOCKABLE";

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("RifterBody");
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RiftBase.onTeleportEnemies += onTeleportEnemies;
        }

        public override void OnUninstall()
        {
            RiftBase.onTeleportEnemies -= onTeleportEnemies;
            base.OnUninstall();
        }

        private void onTeleportEnemies(List<CharacterBody> list)
        {
            if (list.Count >= 10)
            {
                Grant();
            }
        }


    }
}