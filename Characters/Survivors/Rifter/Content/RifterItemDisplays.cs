using RifterMod.Modules;
using RifterMod.Modules.Characters;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

/* for custom copy format in keb's helper
{childName},
                    {localPos}, 
                    {localAngles},
                    {localScale})
*/

namespace RifterMod.Survivors.Rifter
{
    public class RifterItemDisplays : ItemDisplaysBase
    {
        protected override void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules)
        {
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["AlienHead"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAlienHead"),
                    "Head",
                    new Vector3(-0.00824F, 0.27214F, 0.04635F),
                    new Vector3(325.3526F, 2.775F, 350.9309F),
                    new Vector3(1.2F, 1.2F, 1.2F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ArmorPlate"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
                    "UpperArmR",
                    new Vector3(-0.09613F, 0.03548F, -0.01962F),
                    new Vector3(83.08548F, 182.647F, 107.8737F),
                    new Vector3(0.175F, 0.175F, 0.175F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ArmorPlate"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
                    "UpperArmL",
                    new Vector3(0.08545F, 0.03075F, -0.0396F),
                    new Vector3(88.34436F, 71.1626F, 146.9115F),
                    new Vector3(0.175F, 0.175F, 0.175F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ArmorReductionOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarhammer"),
                   "HandL",
                    new Vector3(0.06629F, 0.04996F, 0.21467F),
                    new Vector3(5.28516F, 25.87513F, 264.2736F),
                    new Vector3(0.1F, 0.1F, 0.1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["AttackSpeedAndMoveSpeed"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCoffee"),
                    "Pelvis",
                    new Vector3(0.19092F, -0.04223F, -0.02907F),
                    new Vector3(0.9695F, 155.3193F, 176.9779F),
                    new Vector3(0.20507F, 0.20507F, 0.20507F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["AttackSpeedOnCrit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWolfPelt"),
                    "Head",
                    new Vector3(-0.01455F, 0.17139F, 0.0741F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.5F, 0.5F, 0.5F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["AutoCastEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFossil"),
                    "Backpack",
                    new Vector3(0.00168F, 0.00018F, 0.03728F),
                    new Vector3(0F, 90F, 0F),
                    new Vector3(0.5F, 0.5F, 0.5F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Bandolier"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBandolier"),
                    "Chest",
                    new Vector3(-0.00039F, 0.00074F, 0.00765F),
                    new Vector3(41.88574F, 91.5689F, 83.60735F),
                    new Vector3(0.7F, 0.7F, 0.7F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BarrierOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrooch"),
                    "Chest",
                    new Vector3(-0.12783F, 0.19045F, 0.15283F),
                    new Vector3(65.90182F, 343.907F, 0.22935F),
                    new Vector3(0.3F, 0.3F, 0.3F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BarrierOnOverHeal"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAegis"),
                    "Backpack",
                    new Vector3(-0.01221F, 0.09043F, 0.01304F),
                    new Vector3(89.43076F, 162.6091F, 345.9526F),
                    new Vector3(0.22265F, 0.22265F, 0.22265F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Bear"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBear"),
                    "Chest",
                    new Vector3(0.00773F, 0.09736F, 0.17525F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.22705F, 0.22705F, 0.22705F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BearVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBearVoid"),
                    "Chest",
                    new Vector3(0.00773F, 0.09736F, 0.17525F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.22705F, 0.22705F, 0.22705F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BeetleGland"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBeetleGland"),
                    "Backpack",
                    new Vector3(0.11143F, -0.06228F, -0.01523F),
                    new Vector3(32.23396F, 265.0125F, 158.9838F),
                    new Vector3(0.058F, 0.058F, 0.058F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Behemoth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBehemoth"),
                    "LowerArmL",
                    new Vector3(0.13581F, 0.15689F, 0.00357F),
                    new Vector3(0.82211F, 89.77238F, 175.1102F),
                    new Vector3(0.04793F, 0.04793F, 0.04793F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BleedOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTriTip"),
                    "HeadCenter",
                    new Vector3(0.00436F, 0.05479F, 0.17023F),
                    new Vector3(11.3919F, 0.5588F, 1.95877F),
                    new Vector3(0.68024F, 0.58102F, 0.36001F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BleedOnHitAndExplode"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBleedOnHitAndExplode"),
                    "UpperArmR",
                    new Vector3(-0.10787F, 0.22248F, 0.00286F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.036F, 0.036F, 0.036F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BleedOnHitAndExplode"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBleedOnHitAndExplode"),
                    "LowerArmL",
                    new Vector3(0.01755F, 0.11821F, -0.1186F),
                    new Vector3(305.9573F, 74.40313F, 242.4363F),
                    new Vector3(0.036F, 0.036F, 0.036F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BleedOnHitVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTriTipVoid"),
                    "HeadCenter",
                    new Vector3(-0.0088F, 0.07427F, 0.06714F),
                    new Vector3(9.39321F, 0.71117F, 1.71508F),
                    new Vector3(0.29214F, 0.25216F, 0.44661F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BonusGoldPackOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTome"),
                    "UpperArmR",
                    new Vector3(-0.07011F, 0.20712F, -0.00248F),
                    new Vector3(0.41606F, 263.9247F, 174.4318F),
                    new Vector3(0.02947F, 0.02947F, 0.02947F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BossDamageBonus"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAPRound"),
                    "HandL",
                    new Vector3(0.01725F, 0.14318F, -0.05625F),
                    new Vector3(80.15284F, 127.8336F, 26.88192F),
                    new Vector3(0.55568F, 0.55568F, 0.55568F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BounceNearby"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHook"),
                    "Backpack",
                    new Vector3(0.0043F, 0.03083F, 0.02763F),
                    new Vector3(0F, 180F, 0F),
                    new Vector3(0.60795F, 0.60795F, 0.60795F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ChainLightning"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayUkulele"),
                    "Backpack",
                    new Vector3(0.00062F, 0.02846F, 0.00822F),
                    new Vector3(-0.00001F, 0F, 180F),
                    new Vector3(0.75F, 0.75F, 0.75F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ChainLightningVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayUkuleleVoid"),
                    "Backpack",
                    new Vector3(0.01044F, 0.00935F, 0.00844F),
                    new Vector3(0F, 0F, 180F),
                    new Vector3(0.71375F, 0.71375F, 0.71375F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Clover"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayClover"),
                    "Head",
                    new Vector3(-0.08669F, 0.14455F, 0.19654F),
                    new Vector3(29.70302F, 18.64264F, 38.54863F),
                    new Vector3(0.17524F, 0.17524F, 0.17524F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CloverVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCloverVoid"),
                    "Head",
                    new Vector3(-0.08F, 0.144F, 0.196F),
                    new Vector3(29.7F, 18.64F, 38.55F),
                    new Vector3(0.17F, 0.17F, 0.17F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CooldownOnCrit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySkull"),
                    "ThighL",
                    new Vector3(-0.01431F, 0.48954F, 0.01157F),
                    new Vector3(0F, 0F, 180F),
                    new Vector3(0.25F, 0.2F, 0.25F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CritDamage"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLaserSight"),
                    "Head",
                    new Vector3(-0.04638F, -0.02379F, 0.05561F),
                    new Vector3(0F, 90F, 0F),
                    new Vector3(0.14501F, 0.14501F, 0.14501F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CritGlasses"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlasses"),
                    "Head",
                    new Vector3(0.00433F, 0.11434F, 0.22516F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.2227F, 0.2074F, 0.2074F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CritGlassesVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlassesVoid"),
                    "Head",
                    new Vector3(0.00268F, 0.09664F, 0.19611F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.19305F, 0.1787F, 0.1787F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Crowbar"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCrowbar"),
                    "CalfR",
                    new Vector3(0.00146F, 0.18401F, 0.00085F),
                    new Vector3(0F, 180F, 0F),
                    new Vector3(0.75F, 0.29966F, 0.5F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Dagger"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDagger"),
                    "UpperArmR",
                    new Vector3(-0.02815F, 0.00875F, -0.11115F),
                    new Vector3(328.752F, 178.3498F, 122.0625F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["DeathMark"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDeathMark"),
                    "Head",
                    new Vector3(-0.02658F, 0.153F, 0.01027F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(-0.04076F, -0.04076F, -0.04076F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ElementalRingVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVoidRing"),
                    "LowerArmR",
                    new Vector3(-0.00385F, 0.21718F, -0.00129F),
                    new Vector3(90F, 270F, 0F),
                    new Vector3(0.29175F, 0.29175F, 0.29175F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EmpowerAlways"],
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.Head),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySunHeadNeck"),
                    "Head",
                    new Vector3(-0.02911F, -0.06149F, 0.01041F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1F, 1F, 1F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySunHead"),
                    "Head",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.16375F, 0.16375F, 0.16375F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EnergizedOnEquipmentUse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarHorn"),
                    "Muzzle",
                    new Vector3(0.00856F, 0.03318F, 0.00959F),
                    new Vector3(358.1913F, 140.0009F, 340.6966F),
                    new Vector3(0.08171F, 0.08171F, 0.08171F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EquipmentMagazine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBattery"),
                    "Pelvis",
                    new Vector3(0.17851F, -0.06089F, 0.03727F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(-0.12489F, -0.12489F, -0.12489F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EquipmentMagazineVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFuelCellVoid"),
                    "Pelvis",
                    new Vector3(-0.21155F, -0.0136F, 0.03276F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.11563F, 0.11563F, 0.11563F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExecuteLowHealthElite"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGuillotine"),
                    "Head",
                    new Vector3(0.02395F, 0.23159F, 0.01842F),
                    new Vector3(276.4897F, 5.74668F, 266.1311F),
                    new Vector3(0.37697F, 0.30325F, 0.30325F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExplodeOnDeath"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWilloWisp"),
                    "FootL",
                    new Vector3(-0.04372F, 0.09934F, -0.11981F),
                    new Vector3(311.4641F, 153.1758F, 213.889F),
                    new Vector3(0.05254F, 0.05254F, 0.05254F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExplodeOnDeathVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWillowWispVoid"),
                    "Muzzle",
                    new Vector3(0F, 0.07077F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.02752F, 0.02752F, 0.02752F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExtraLife"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHippo"),
                    "FootL",
                    new Vector3(-0.00473F, -0.05692F, -0.0385F),
                    new Vector3(80.91624F, 329.1417F, 147.5451F),
                    new Vector3(0.32276F, 0.32276F, 0.32276F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExtraLifeVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHippoVoid"),
                    "Backpack",
                    new Vector3(0.00044F, 0.00023F, 0.01989F),
                    new Vector3(0F, 0F, 180F),
                    new Vector3(0.16238F, 0.16238F, 0.16238F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FallBoots"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravBoots"),
                    "CalfL",
                    new Vector3(0.00564F, 0.2421F, -0.00277F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.25F, 0.25F, 0.25F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravBoots"),
                    "CalfR",
                    new Vector3(-0.02842F, 0.23802F, 0.02543F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.25F, 0.25F, 0.25F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Feather"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFeather"),
                    "Head",
                    new Vector3(-0.05943F, 0.00348F, 0.01054F),
                    new Vector3(343.4253F, 335.9995F, 105.2724F),
                    new Vector3(0.01494F, 0.01494F, 0.01494F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FireballsOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFireballsOnHit"),
                    "ThighL",
                    new Vector3(-0.02019F, 0.46248F, -0.15399F),
                    new Vector3(38.92258F, 182.8324F, 16.75615F),
                    new Vector3(0.05786F, 0.05786F, 0.05786F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FireRing"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFireRing"),
                    "LowerArmL",
                    new Vector3(-0.00306F, 0.18361F, 0.00956F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(0.31277F, 0.31277F, 0.31277F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Firework"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFirework"),
                    "Backpack",
                    new Vector3(0.00407F, 0.44884F, 0.00728F),
                    new Vector3(280.7202F, 317.0967F, 38.02741F),
                    new Vector3(0.25366F, 0.25366F, 0.25366F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FlatHealth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySteakCurved"),
                    "Head",
                    new Vector3(-0.09811F, 0.07295F, 0.13702F),
                    new Vector3(344.644F, 297.9341F, 22.74812F),
                    new Vector3(0.04921F, 0.04921F, 0.04921F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FocusConvergence"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFocusedConvergence"),
                    "Muzzle",
                    new Vector3(0F, 0.03F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.005F, 0.005F, 0.005F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FragileDamageBonus"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDelicateWatch"),
                    "Pelvis",
                    new Vector3(-0.01095F, -0.02552F, -0.02861F),
                    new Vector3(279.7037F, 98.65726F, 261.3727F),
                    new Vector3(1.23047F, 2.48758F, 0.98964F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FreeChest"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShippingRequestForm"),
                    "HandL",
                    new Vector3(-0.11044F, 0.1781F, 0.07468F),
                    new Vector3(35.17252F, 91.83049F, 146.0241F),
                    new Vector3(0.70831F, 0.70831F, 0.70831F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GhostOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMask"),
                    "Chest",
                    new Vector3(0.10734F, 0.19018F, 0.15105F),
                    new Vector3(335.3398F, 3.83699F, 358.0405F),
                    new Vector3(0.10595F, 0.10595F, 0.10595F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GoldOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBoneCrown"),
                    "Head",
                    new Vector3(-0.04579F, 0.20708F, -0.04843F),
                    new Vector3(348.41F, 346.0524F, 54.41879F),
                    new Vector3(0.74982F, 0.74982F, 0.74982F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GoldOnHurt"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRollOfPennies"),
                    "Head",
                    new Vector3(-0.05417F, 0.11856F, 0.14657F),
                    new Vector3(77.66303F, 68.67515F, 79.53706F),
                    new Vector3(0.60661F, 0.60661F, 0.60661F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HalfAttackSpeedHalfCooldowns"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarShoulderNature"),
                    "UpperArmR",
                    new Vector3(-0.11099F, 0.01657F, -0.04417F),
                    new Vector3(356.6246F, 169.1163F, 242.495F),
                    new Vector3(0.48276F, 0.48276F, 0.48276F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HalfSpeedDoubleHealth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarShoulderStone"),
                    "UpperArmL",
                    new Vector3(0.10016F, -0.01351F, -0.03472F),
                    new Vector3(3.52241F, 18.26316F, 216.2745F),
                    new Vector3(0.45305F, 0.45305F, 0.45305F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HeadHunter"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySkullcrown"),
                    "Chest",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.26521F, 0.26521F, 0.26521F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HealingPotion"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHealingPotion"),
                    "Pelvis",
                    new Vector3(-0.20426F, 0.04578F, -0.09337F),
                    new Vector3(348.5571F, 3.20975F, 222.0132F),
                    new Vector3(0.03041F, 0.03041F, 0.03114F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HealOnCrit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScythe"),
                    "Head",
                    new Vector3(-0.00498F, 0.1327F, 0.01802F),
                    new Vector3(356.234F, 184.4841F, 354.5379F),
                    new Vector3(0.22959F, 0.22959F, 0.22959F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HealWhileSafe"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySnail"),
                    "Head",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.08191F, 0.08191F, 0.08191F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Hoof"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHoof"),
                    "CalfR",
                    new Vector3(-0.00349F, 0.27048F, -0.04728F),
                    new Vector3(69.11033F, 11.40189F, 25.46624F),
                    new Vector3(0.1F, 0.1F, 0.07965F)
                    ),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightCalf)
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IceRing"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayIceRing"),
                    "HandL",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(0.33666F, 0.33666F, 0.33666F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Icicle"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFrostRelic"),
                    "CalfR",
                    new Vector3(-0.00042F, 0.23235F, 0.00908F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IgniteOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGasoline"),
                    "Chest",
                    new Vector3(0.0618F, -0.1476F, 0.19313F),
                    new Vector3(287.9874F, 123.2461F, 170.1793F),
                    new Vector3(0.26872F, 0.26872F, 0.26872F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ImmuneToDebuff"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRainCoatBelt"),
                    "Pelvis",
                    new Vector3(-0.00814F, -0.03667F, 0.0012F),
                    new Vector3(350F, 180F, 180F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IncreaseHealing"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAntler"),
                    "Head",
                    new Vector3(-0.01017F, 0.31972F, -0.06037F),
                    new Vector3(9.60235F, 79.58936F, 351.0298F),
                    new Vector3(0.14602F, 0.14602F, 0.14602F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAntler"),
                    "Head",
                    new Vector3(-0.10171F, 0.19351F, -0.05414F),
                    new Vector3(10.03429F, 263.6601F, 11.53103F),
                    new Vector3(0.27278F, 0.27278F, 0.27278F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Incubator"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAncestralIncubator"),
                    "Backpack",
                    new Vector3(-0.07626F, -0.00218F, 0.00862F),
                    new Vector3(23.92321F, 1.9482F, 147.9577F),
                    new Vector3(0.02519F, 0.02519F, 0.02519F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Infusion"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayInfusion"),
                    "FootL",
                    new Vector3(0.09545F, -0.00248F, -0.03151F),
                    new Vector3(6.26724F, 279.19F, 108.4289F),
                    new Vector3(0.62314F, 0.62314F, 0.62314F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["JumpBoost"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWaxBird"),
                    "Head",
                    new Vector3(0.03879F, -0.34925F, -0.11032F),
                    new Vector3(358.8703F, 6.38548F, 11.95688F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["KillEliteFrenzy"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrainstalk"),
                    "Head",
                    new Vector3(0.00643F, 0.16751F, -0.03776F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.25978F, 0.25978F, 0.25978F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Knurl"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKnurl"),
                    "Head",
                    new Vector3(-0.0241F, 0.17366F, 0.00588F),
                    new Vector3(301.4142F, 274.5419F, 50.30404F),
                    new Vector3(0.08657F, 0.08657F, 0.08657F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LaserTurbine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLaserTurbine"),
                    "Muzzle",
                    new Vector3(0F, 0.1F, 0F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(0.12997F, 0.12997F, 0.12997F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LightningStrikeOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayChargedPerforator"),
                    "ThighR",
                    new Vector3(0.03182F, 0.49606F, -0.11051F),
                    new Vector3(303.856F, 176.7828F, 347.5674F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarDagger"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarDagger"),
                    "LowerArmR",
                    new Vector3(-0.00193F, -0.00446F, -0.00041F),
                    new Vector3(290.6319F, 3.98153F, 282.0215F),
                    new Vector3(0.27842F, 0.27842F, 0.27842F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarPrimaryReplacement"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdEye"),
                    "Head",
                    new Vector3(0.0728F, 0.12144F, 0.20657F),
                    new Vector3(282.5626F, 47.89578F, 302.8005F),
                    new Vector3(0.13399F, 0.13399F, 0.13399F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarSecondaryReplacement"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdClaw"),
                    "Head",
                    new Vector3(0.09873F, 0.05126F, 0.16479F),
                    new Vector3(359.8974F, 6.19717F, 24.38485F),
                    new Vector3(0.17429F, 0.17429F, 0.17586F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarSpecialReplacement"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdHeart"),
                    "CalfL",
                    new Vector3(-0.02742F, 0.15076F, 0.06666F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(0.09966F, 0.1036F, 0.09966F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarTrinket"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBeads"),
                    "Backpack",
                    new Vector3(0.21092F, -0.1399F, -0.09648F),
                    new Vector3(355.7524F, 321.2469F, 105.6219F),
                    new Vector3(0.583F, 0.583F, 0.583F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarUtilityReplacement"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdFoot"),
                    "FootR",
                    new Vector3(-0.01321F, 0.1035F, 0.09064F),
                    new Vector3(5.6378F, 74.0276F, 252.6239F),
                    new Vector3(0.50646F, 0.45818F, 0.67057F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Medkit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMedkit"),
                    "Pelvis",
                    new Vector3(0.00328F, -0.01213F, -0.17932F),
                    new Vector3(80.42115F, 307.7947F, 312.3329F),
                    new Vector3(0.54019F, 0.54019F, 0.54019F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MinorConstructOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDefenseNucleus"),
                    "Muzzle",
                    new Vector3(0F, 0.25F, 0F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(0.21913F, 0.21913F, 0.21913F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Missile"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileLauncher"),
                    "Chest",
                    new Vector3(-0.19148F, 0.48645F, -0.03801F),
                    new Vector3(351.2526F, 357.4162F, 17.99611F),
                    new Vector3(0.09451F, 0.09451F, 0.09451F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MissileVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileLauncherVoid"),
                    "Chest",
                    new Vector3(-0.19148F, 0.48645F, -0.03801F),
                    new Vector3(351.2526F, 357.4162F, 17.99611F),
                    new Vector3(0.09451F, 0.09451F, 0.09451F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MonstersOnShrineUse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMonstersOnShrineUse"),
                    "Backpack",
                    new Vector3(0.06007F, 0.11494F, 0.0622F),
                    new Vector3(302.7924F, 82.27598F, 193.8916F),
                    new Vector3(0.12773F, 0.12773F, 0.12773F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MoreMissile"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayICBM"),
                    "Backpack",
                    new Vector3(0.00565F, 0.00336F, 0.11622F),
                    new Vector3(84.26992F, 109.8211F, 106.2161F),
                    new Vector3(0.08513F, 0.08513F, 0.08513F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MoveSpeedOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGrappleHook"),
                    "LowerArmL",
                    new Vector3(0.0492F, 0.27278F, -0.01223F),
                    new Vector3(-0.00001F, 180F, 180F),
                    new Vector3(0.14984F, 0.14984F, 0.14984F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Mushroom"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMushroom"),
                    "CalfL",
                    new Vector3(-0.048F, 0.17409F, 0.00114F),
                    new Vector3(-0.00001F, 0.00001F, 180F),
                    new Vector3(0.08108F, 0.08108F, 0.08108F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MushroomVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMushroomVoid"),
                    "CalfL",
                    new Vector3(-0.07035F, 0.15708F, 0.0236F),
                    new Vector3(40.18388F, 42.80069F, 150.919F),
                    new Vector3(0.08F, 0.08F, 0.08F)

                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["NearbyDamageBonus"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDiamond"),
                    "Chest",
                    new Vector3(0.09465F, 0.04983F, 0.18597F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.04756F, 0.04756F, 0.04756F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["NovaOnHeal"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                    "Chest",
                    new Vector3(0.18553F, 0.10075F, 0.02427F),
                    new Vector3(359.6665F, 19.72731F, 318.9324F),
                    new Vector3(0.3F, 0.3F, 0.3F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                    "Chest",
                    new Vector3(-0.21564F, 0.09629F, -0.04306F),
                    new Vector3(339.6974F, 319.1538F, 342.7546F),
                    new Vector3(0.3F, 0.3F, 0.3F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["NovaOnLowHealth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayJellyGuts"),
                    "FootL",
                    new Vector3(-0.00607F, 0.0632F, -0.11923F),
                    new Vector3(359.7504F, 7.25587F, 10.13714F),
                    new Vector3(0.10153F, 0.10153F, 0.10153F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["OutOfCombatArmor"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayOddlyShapedOpal"),
                    "Chest",
                    new Vector3(0.22246F, -0.02501F, -0.05491F),
                    new Vector3(356.0901F, 290.7877F, 10.44807F),
                    new Vector3(0.31474F, 0.31474F, 0.31474F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ParentEgg"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayParentEgg"),
                    "Chest",
                    new Vector3(-0.10265F, 0.13939F, 0.20307F),
                    new Vector3(352.6181F, 312.3653F, 2.99483F),
                    new Vector3(0.04626F, 0.04626F, 0.04626F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Pearl"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPearl"),
                    "Pelvis",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(0.25F, 0.25F, 0.25F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["PermanentDebuffOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScorpion"),
                    "Pelvis",
                    new Vector3(0.00958F, 0.00721F, 0.06178F),
                    new Vector3(-0.00001F, 180F, 180F),
                    new Vector3(0.87897F, 0.87897F, 0.87897F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["PersonalShield"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldGenerator"),
                    "Head",
                    new Vector3(0.09034F, 0.03326F, 0.00222F),
                    new Vector3(66.25243F, 256.0576F, 349.0749F),
                    new Vector3(0.14027F, 0.14027F, 0.14027F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["PersonalShield"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldGenerator"),
                    "Head",
                    new Vector3(-0.09652F, 0.02131F, 0.00502F),
                    new Vector3(69.95217F, 52.58558F, 345.343F),
                    new Vector3(0.14027F, 0.14027F, 0.14027F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Phasing"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStealthkit"),
                    "UpperArmL",
                    new Vector3(0.08496F, 0.16501F, 0.00938F),
                    new Vector3(270F, 90F, 0F),
                    new Vector3(0.21021F, 0.37117F, 0.18675F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Plant"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayInterstellarDeskPlant"),
                    "Backpack",
                    new Vector3(0.00519F, -0.14119F, -0.01182F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(0.04683F, 0.04683F, 0.04683F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["PrimarySkillShuriken"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShuriken"),
                    "UpperArmR",
                    new Vector3(-0.0163F, 0.17982F, 0.02028F),
                    new Vector3(79.89165F, 36.60717F, 37.56866F),
                    new Vector3(0.61699F, 0.61699F, 0.61699F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RandomDamageZone"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRandomDamageZone"),
                    "Chest",
                    new Vector3(0.00742F, 0.22943F, -0.14073F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.07663F, 0.07663F, 0.07663F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RandomEquipmentTrigger"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBottledChaos"),
                    "Muzzle",
                    new Vector3(0F, 0.5F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.13898F, 0.13898F, 0.13898F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RandomlyLunar"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDomino"),
                    "FootL",
                    new Vector3(-0.02952F, -0.23763F, 0.10059F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RegeneratingScrap"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRegeneratingScrap"),
                    "HandR",
                    new Vector3(-0.03902F, 0.05124F, 0.02084F),
                    new Vector3(0F, 90F, 0F),
                    new Vector3(0.19966F, 0.19966F, 0.19966F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RepeatHeal"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCorpseflower"),
                    "Head",
                    new Vector3(0.00006F, 0.08443F, -0.00497F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(0.86688F, 0.86688F, 0.86688F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SecondarySkillMagazine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDoubleMag"),
                    "CalfL",
                    new Vector3(-0.10147F, 0.29794F, -0.01517F),
                    new Vector3(36.38993F, 164.1504F, 358.1621F),
                    new Vector3(0.04977F, 0.05066F, 0.05066F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Seed"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySeed"),
                    "Chest",
                    new Vector3(-0.19248F, -0.0411F, 0.02839F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.0442F, 0.0442F, 0.0442F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ShieldOnly"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldBug"),
                    "Head",
                    new Vector3(0.01119F, 0.17358F, 0.15472F),
                    new Vector3(19.87426F, 262.7456F, 10.46471F),
                    new Vector3(0.4345F, 0.4345F, 0.4345F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldBug"),
                    "Head",
                    new Vector3(0.01119F, 0.17358F, 0.15472F),
                    new Vector3(19.87426F, 262.7456F, 10.46471F),
                    new Vector3(0.4345F, 0.4345F, 0.4345F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ShinyPearl"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShinyPearl"),
                    "Pelvis",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(90F, 180F, 0F),
                    new Vector3(0.25F, 0.25F, 0.25F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ShockNearby"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTeslaCoil"),
                    "Chest",
                    new Vector3(0.17743F, 0.16236F, -0.01026F),
                    new Vector3(5.12137F, 10.60482F, 322.2942F),
                    new Vector3(0.33237F, 0.33237F, 0.33237F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SiphonOnLowHealth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySiphonOnLowHealth"),
                    "Head",
                    new Vector3(0F, 0F, -0.17449F),
                    new Vector3(67.02768F, 356.459F, 1.55168F),
                    new Vector3(0.07011F, 0.07011F, 0.07011F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SlowOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBauble"),
                    "Backpack",
                    new Vector3(-0.04768F, 0.64274F, 0.0244F),
                    new Vector3(16.59752F, 11.64821F, 131.8337F),
                    new Vector3(0.40708F, 0.40708F, 0.40708F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SlowOnHitVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBaubleVoid"),
                    "Backpack",
                    new Vector3(-0.26257F, 0.3611F, 0.09753F),
                    new Vector3(324.9962F, 176.6483F, 72.19518F),
                    new Vector3(0.40989F, 0.40989F, 0.40989F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SprintArmor"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBuckler"),
                    "LowerArmR",
                    new Vector3(0.06393F, 0.12537F, 0.01558F),
                    new Vector3(359.8887F, 206.807F, 83.38604F),
                    new Vector3(0.33369F, 0.33369F, 0.33369F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SprintBonus"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySoda"),
                    "CalfR",
                    new Vector3(-0.00827F, 0.07809F, 0.10695F),
                    new Vector3(341.3806F, 353.7826F, 355.1332F),
                    new Vector3(0.21348F, 0.21348F, 0.21348F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SprintOutOfCombat"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWhip"),
                    "Head",
                    new Vector3(0.00967F, -0.06753F, 0.20633F),
                    new Vector3(-0.00001F, 180F, 0F),
                    new Vector3(0.17988F, 0.17988F, 0.17988F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SprintWisp"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrokenMask"),
                    "LowerArmR",
                    new Vector3(0.02028F, 0.04072F, -0.04381F),
                    new Vector3(-0.00001F, 180F, 180F),
                    new Vector3(0.12375F, 0.12375F, 0.12375F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Squid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySquidTurret"),
                    "Head",
                    new Vector3(0.00486F, 0.22439F, -0.03971F),
                    new Vector3(76.10537F, 334.5558F, 242.5008F),
                    new Vector3(0.05759F, 0.05759F, 0.05759F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["StickyBomb"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStickyBomb"),
                    "Head",
                    new Vector3(-0.22342F, -0.02094F, 0.00524F),
                    new Vector3(336.1413F, 354.2385F, 115.8736F),
                    new Vector3(0.19084F, 0.19084F, 0.19084F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["StrengthenBurn"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGasTank"),
                    "Backpack",
                    new Vector3(0.17401F, 0.15506F, -0.01226F),
                    new Vector3(0.87234F, 357.8426F, 192.4398F),
                    new Vector3(0.2049F, 0.2049F, 0.2049F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["StunChanceOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStunGrenade"),
                    "UpperArmR",
                    new Vector3(-0.0542F, 0.0168F, -0.12179F),
                    new Vector3(90F, 90F, 0F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Syringe"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySyringeCluster"),
                    "Chest",
                    new Vector3(-0.13889F, 0.18793F, 0.07848F),
                    new Vector3(42.9411F, 340.438F, 3.25706F),
                    new Vector3(0.13772F, 0.13772F, 0.13772F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Talisman"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTalisman"),
                    "Head",
                    new Vector3(0.02926F, 0.17272F, 0.03042F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.25214F, 0.25214F, 0.25214F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Thorns"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRazorwireLeft"),
                    "UpperArmL",
                    new Vector3(-0.06278F, -0.00379F, 0.00704F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.80795F, 0.80795F, 0.80795F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TitanGoldDuringTP"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGoldHeart"),
                    "Chest",
                    new Vector3(-0.00075F, 0.01655F, 0.15645F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.3619F, 0.3619F, 0.3619F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Tooth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothNecklaceDecal"),
                    "Chest",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(8.07027F, 201.8752F, 59.71467F),
                    new Vector3(1F, 1F, 1F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshLarge"),
                    "Chest",
                    new Vector3(0.02749F, 0.22299F, 0.13861F),
                    new Vector3(333.8298F, 339.7349F, 3.84475F),
                    new Vector3(1F, 1.2043F, 1F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall1"),
                    "Chest",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(8.07027F, 201.8752F, 59.71467F),
                    new Vector3(1F, 1F, 1F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall2"),
                    "Chest",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(8.07027F, 201.8752F, 59.71467F),
                    new Vector3(1F, 1F, 1F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall2"),
                    "Chest",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(8.07027F, 201.8752F, 59.71467F),
                    new Vector3(1F, 1F, 1F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall1"),
                    "Chest",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(8.07027F, 201.8752F, 59.71467F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TPHealingNova"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlowFlower"),
                    "Chest",
                    new Vector3(-0.11199F, 0.20031F, 0.1498F),
                    new Vector3(336.8267F, 3.62942F, 357.9333F),
                    new Vector3(0.34443F, 0.34443F, 0.33575F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TreasureCache"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKey"),
                    "Chest",
                    new Vector3(-0.17728F, -0.21314F, -0.07577F),
                    new Vector3(357.6159F, 320.2462F, 80.72025F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TreasureCacheVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKeyVoid"),
                    "Chest",
                    new Vector3(-0.17728F, -0.21314F, -0.07577F),
                    new Vector3(357.6159F, 320.2462F, 80.72025F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["UtilitySkillMagazine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                    "Backpack",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(280.9884F, 263.6346F, 91.00021F),
                    new Vector3(0.50355F, 0.50355F, 0.50355F)
                    )
                //ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                //    "Chest",
                //    new Vector3(2, 2, 2),
                //    new Vector3(0, 0, 0),
                //    new Vector3(1, 1, 1)
                //    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["VoidMegaCrabItem"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMegaCrabItem"),
                    "Head",
                    new Vector3(0.00213F, 0.19104F, -0.18181F),
                    new Vector3(0F, 180F, 0F),
                    new Vector3(0.03454F, 0.03454F, 0.03454F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["WarCryOnMultiKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPauldron"),
                    "FootL",
                    new Vector3(0.0232F, 0.17983F, -0.02568F),
                    new Vector3(79.96291F, 350.1872F, 167.3891F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["WardOnLevel"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarbanner"),
                    "Chest",
                    new Vector3(-0.0065F, -0.06891F, -0.0954F),
                    new Vector3(0F, 180F, 0F),
                    new Vector3(0.41444F, 0.41444F, 0.41444F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BFG"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBFG"),
                    "Head",
                    new Vector3(0.02897F, 0.14627F, -0.1519F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.31153F, 0.31153F, 0.31153F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Blackhole"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravCube"),
                    "Muzzle",
                    new Vector3(0F, 0.25F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.23972F, 0.23972F, 0.23972F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BossHunter"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTricornGhost"),
                    "Head",
                    new Vector3(0.0037F, 0.22563F, -0.03863F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1F, 1F, 1F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBlunderbuss"),
                    "LowerArmL",
                    new Vector3(0.25F, 0.25F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BossHunterConsumed"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTricornUsed"),
                    "Head",
                    new Vector3(0.00821F, 0.24598F, -0.04513F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BurnNearby"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPotion"),
                    "Chest",
                    new Vector3(0.22443F, -0.40309F, 0.04404F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.04664F, 0.04664F, 0.04664F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Cleanse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWaterPack"),
                    "Chest",
                    new Vector3(-0.01238F, -0.15694F, -0.15666F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.07746F, 0.07746F, 0.07746F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CommandMissile"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileRack"),
                    "Chest",
                    new Vector3(0.00861F, 0.13853F, 0.16268F),
                    new Vector3(44.93786F, 353.6776F, 0.37769F),
                    new Vector3(0.37388F, 0.37388F, 0.37388F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CrippleWard"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEffigy"),
                    "Backpack",
                    new Vector3(-0.00626F, 0.42014F, -0.00479F),
                    new Vector3(0F, 180F, 0F),
                    new Vector3(0.48741F, 0.48741F, 0.48741F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CritOnUse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayNeuralImplant"),
                    "Head",
                    new Vector3(-0.10318F, 0.09175F, 0.49005F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.5F, 0.5F, 0.5F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["DeathProjectile"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDeathProjectile"),
                    "Head",
                    new Vector3(0.05957F, 0.21131F, -0.32499F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.08984F, 0.08984F, 0.08984F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["DroneBackup"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRadio"),
                    "Head",
                    new Vector3(-0.13054F, 0.15682F, 0.08437F),
                    new Vector3(14.19149F, 105.9529F, 351.8914F),
                    new Vector3(0.45063F, 0.45063F, 0.45063F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteEarthEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteMendingAntlers"),
                    "Head",
                    new Vector3(0.00139F, 0.09297F, -0.01189F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteFireEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                    "Chest",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.01F, 0.01F, 0.01F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                    "Head",
                    new Vector3(-0.07643F, 0.17331F, 0.01669F),
                    new Vector3(-0.00001F, 180F, 180F),
                    new Vector3(-0.1F, -0.1F, -0.1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteHauntedEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteStealthCrown"),
                    "Head",
                    new Vector3(0.0059F, 0.39479F, -0.05051F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.01842F, 0.01842F, 0.01842F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteIceEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteIceCrown"),
                    "Head",
                    new Vector3(0.00494F, 0.33051F, -0.04228F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(-0.0343F, -0.0343F, -0.0343F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteLightningEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                    "Head",
                    new Vector3(-0.07512F, 0.27328F, 0.04848F),
                    new Vector3(300.362F, 276.5477F, 103.568F),
                    new Vector3(0.24342F, 0.24342F, 0.24342F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                    "Chest",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.001F, 0.001F, 0.001F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteLunarEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteLunar,Eye"),
                    "Head",
                    new Vector3(0.00542F, 0.36306F, -0.04645F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(0.20421F, 0.20421F, 0.20421F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ElitePoisonEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteUrchinCrown"),
                    "Head",
                    new Vector3(-0.00778F, 0.15058F, 0.02341F),
                    new Vector3(270F, 0F, 0F),
                    new Vector3(0.04513F, 0.04513F, 0.04513F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteVoidEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAffixVoid"),
                    "Head",
                    new Vector3(-0.03416F, 0.12371F, 0.01533F),
                    new Vector3(69.05486F, 345.0612F, 77.7069F),
                    new Vector3(0.21977F, 0.21977F, 0.21977F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FireBallDash"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEgg"),
                    "Head",
                    new Vector3(0.0024F, 0.13929F, 0.0072F),
                    new Vector3(277.6984F, 352.9698F, 9.38205F),
                    new Vector3(0.45933F, 0.45933F, 0.45933F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Fruit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFruit"),
                    "Chest",
                    new Vector3(0.00341F, -0.00975F, -0.08058F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.20601F, 0.20601F, 0.20601F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GainArmor"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayElephantFigure"),
                    "Head",
                    new Vector3(0.03876F, 0.09046F, -0.16905F),
                    new Vector3(0F, 90F, 0F),
                    new Vector3(0.62568F, 0.62568F, 0.62568F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Gateway"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVase"),
                    "Muzzle",
                    new Vector3(0F, 0.15F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.16435F, 0.16435F, 0.16435F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GoldGat"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGoldGat"),
                    "Head",
                    new Vector3(-0.32336F, 0.20047F, 0.01694F),
                    new Vector3(8.14484F, 81.76649F, 303.4006F),
                    new Vector3(0.11577F, 0.11577F, 0.11577F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GummyClone"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGummyClone"),
                    "CalfR",
                    new Vector3(0.0382F, 0.2567F, 0.16915F),
                    new Vector3(338.2649F, 2.44504F, 180.2979F),
                    new Vector3(0.26691F, 0.26691F, 0.26286F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IrradiatingLaser"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayIrradiatingLaser"),
                    "Chest",
                    new Vector3(-0.01852F, 0.19251F, 0.01138F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.3106F, 0.19872F, 0.16904F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Jetpack"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBugWings"),
                    "Chest",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.26248F, 0.26248F, 0.26248F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LifestealOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLifestealOnHit"),
                    "Head",
                    new Vector3(0.09501F, 0.26747F, -0.06759F),
                    new Vector3(44.62922F, 301.6364F, 283.9519F),
                    new Vector3(0.09138F, 0.09138F, 0.09138F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Lightning"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLightningArmRight"),
                    "Chest",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(301.1211F, 140.1639F, 322.4987F),
                    new Vector3(0.50381F, 0.50381F, 0.50381F)
                    ),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightArm)
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarPortalOnUse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarPortalOnUse"),
                    "LowerArmR",
                    new Vector3(-0.12981F, 0.01761F, -0.15514F),
                    new Vector3(270F, 0F, 0F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Meteor"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMeteor"),
                    "Head",
                    new Vector3(-0.00231F, 0.15929F, 0.00465F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1.04298F, 1.04298F, 1.04298F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Molotov"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMolotov"),
                    "Chest",
                    new Vector3(0.1552F, -0.417F, -0.06415F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.25227F, 0.25227F, 0.25227F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MultiShopCard"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayExecutiveCard"),
                    "HandL",
                    new Vector3(-0.09607F, 0.13159F, 0.05615F),
                    new Vector3(326.2097F, 6.96006F, 6.16067F),
                    new Vector3(0.65101F, 0.65101F, 0.65101F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["QuestVolatileBattery"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBatteryArray"),
                    "Backpack",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.28323F, 0.28323F, 0.28323F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Recycle"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRecycler"),
                    "Root",
                    new Vector3(0.65457F, 0.19845F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.17351F, 0.17351F, 0.17351F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Saw"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySawmerangFollower"),
                    "Muzzle",
                    new Vector3(0F, 0.1F, 0F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(0.0809F, 0.0809F, 0.0809F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Scanner"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScanner"),
                    "Chest",
                    new Vector3(0.07524F, 0.18349F, -0.03891F),
                    new Vector3(301.5506F, 85.87163F, 282.4242F),
                    new Vector3(0.2205F, 0.2205F, 0.2205F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TeamWarCry"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTeamWarCry"),
                    "Chest",
                    new Vector3(0.00267F, 0.07182F, 0.20053F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.08609F, 0.08609F, 0.08609F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Tonic"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTonic"),
                    "Chest",
                    new Vector3(0.15999F, -0.3473F, 0.17756F),
                    new Vector3(335.5135F, 55.94006F, 355.8939F),
                    new Vector3(0.20476F, 0.21493F, 0.21493F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["VendingMachine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVendingMachine"),
                    "Head",
                    new Vector3(0.18263F, 0.16078F, 0.01271F),
                    new Vector3(357.1593F, 355.6719F, 344.7471F),
                    new Vector3(0.15899F, 0.15899F, 0.15899F)
                    )
                ));
        }
    }
}