using BepInEx.Configuration;
using RifterMod.Modules;
using RifterMod.Modules.Characters;
using RifterMod.Survivors.Rifter.Components;
using RifterMod.Survivors.Rifter.SkillStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RifterMod.Survivors.Rifter
{
    public class RifterSurvivor : SurvivorBase<RifterSurvivor>
    {
        //used to load the assetbundle for this character. must be unique
        public override string assetBundleName => "rifterassetbundle"; //if you do not change this, you are giving permission to deprecate the mod

        //the name of the prefab we will create. conventionally ending in "Body". must be unique
        public override string bodyName => "RifterBody"; //if you do not change this, you get the point by now

        //name of the ai master for vengeance and goobo. must be unique
        public override string masterName => "RifterMonsterMaster"; //if you do not

        //the names of the prefabs you set up in unity that we will use to build your character
        public override string modelPrefabName => "mdlRifter";
        public override string displayPrefabName => "RifterDisplay";

        public const string Rifter_PREFIX = RifterPlugin.DEVELOPER_PREFIX + "_Rifter_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => Rifter_PREFIX;
        
        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = Rifter_PREFIX + "NAME",
            subtitleNameToken = Rifter_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("texRifterIcon"),
            bodyColor = Color.white,
            sortPosition = 100,

            crosshair = Assets.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1.5f,
            armor = 0f,

            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "SwordModel",
                    material = assetBundle.LoadMaterial("matHenry"),
                },
                new CustomRendererInfo
                {
                    childName = "GunModel",
                },
                new CustomRendererInfo
                {
                    childName = "Model",
                }
        };

        public override UnlockableDef characterUnlockableDef => RifterUnlockables.characterUnlockableDef;
        
        public override ItemDisplaysBase itemDisplays => new RifterItemDisplays();

        //set in base classes
        public override AssetBundle assetBundle { get; protected set; }

        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public override void Initialize()
        {
            //uncomment if you have multiple characters
            //ConfigEntry<bool> characterEnabled = Config.CharacterEnableConfig("Survivors", "Rifter");

            //if (!characterEnabled.Value)
            //    return;

            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            RifterUnlockables.Init();

            base.InitializeCharacter();

            RifterConfig.Init();
            RifterStates.Init();
            RifterTokens.Init();

            RifterAssets.Init(assetBundle);
            RifterBuffs.Init(assetBundle);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();
        }

        private void AdditionalBodySetup()
        {
            AddHitboxes();
            bodyPrefab.AddComponent<RifterWeaponComponent>();
            //bodyPrefab.AddComponent<HuntressTrackerComopnent>();
            //anything else here
        }

        public void AddHitboxes()
        {
            ChildLocator childLocator = characterModelObject.GetComponent<ChildLocator>();

            //example of how to create a hitbox
            Transform hitBoxTransform = childLocator.FindChild("SwordHitbox");
            Prefabs.SetupHitBoxGroup(characterModelObject, "SwordGroup", hitBoxTransform);
        }

        public override void InitializeEntityStateMachines() 
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //if you set up a custom main characterstate, set it up here
                //don't forget to register custom entitystates in your RifterStates.cs
            //the main "body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }

        #region skills
        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(bodyPrefab);
            //add our own
            Skills.CreateSkillFamilies(bodyPrefab);
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddSpecialSkills();
            AddSpecialSkillsPlus();
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPrimarySkills()
        {
            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            SkillDef primarySkillDef1 = Skills.CreateSkillDef<SkillDef>(new SkillDefInfo
                (
                    "Rift Gauntlet Scope",
                    Rifter_PREFIX + "PRIMARY_SLASH_NAME",
                    Rifter_PREFIX + "PRIMARY_SLASH_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                    new EntityStates.SerializableEntityStateType(typeof(SkillStates.RiftGauntlet)),
                    "Weapon",
                    false
                )) ;
            primarySkillDef1.interruptPriority = EntityStates.InterruptPriority.Any;
            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
        }

        private void AddSecondarySkills()
        {
            //here is a basic skill def with all fields accounted for
            SkillDef secondarySkillDef1 = Skills.CreateSkillDef<SkillDef>(new SkillDefInfo
            {
                skillName = "Gauntlet Scatter",
                skillNameToken = Rifter_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = Rifter_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.RiftGauntletShort)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Any,


                baseRechargeInterval = .8f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });
            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1);

            
        }

        private void AddUtiitySkills()
        {
            //here's a skilldef of a typical movement skill. some fields are omitted and will just have default values
            SkillDef utilitySkillDef1 = Skills.CreateSkillDef<SkillDef>(new SkillDefInfo
            {
                skillName = "Slipstream",
                skillNameToken = Rifter_PREFIX + "UTILITY_ROLL_NAME",
                skillDescriptionToken = Rifter_PREFIX + "UTILITY_ROLL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Slipstream)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseMaxStock = 1,
                baseRechargeInterval = 4f,

                isCombatSkill = false,
                mustKeyPress = false,
                forceSprintDuringState = true,
                cancelSprintingOnActivation = false,
            });
            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1);
        }

        private void AddSpecialSkills()
        {
            //a basic skill
            SkillDef specialSkilLDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "RifterSpecial",
                skillNameToken = Rifter_PREFIX + "SPECIAL_BOMB_NAME",
                skillDescriptionToken = Rifter_PREFIX + "SPECIAL_BOMB_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(RiftSpecialStateSwap)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2", interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseMaxStock = 1,
                baseRechargeInterval = 15f,

                stockToConsume = 0,

                isCombatSkill = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true
            });
            Skills.AddSpecialSkills(bodyPrefab, specialSkilLDef1);
        }

        private void AddSpecialSkillsPlus()
        {
            SkillDef primarySkillDefSpecial = Skills.CreateSkillDef<SkillDef>(new SkillDefInfo
            {
                skillName = "RiftPrimarySpecial",
                skillNameToken = Rifter_PREFIX + "Primary Special",
                skillDescriptionToken = Rifter_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(RiftPrimarySpecial)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = true,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });
            RifterMod.Modules.Content.AddSkillDef(primarySkillDefSpecial);

            SkillDef secondarySkillDefSpecial = Skills.CreateSkillDef<SkillDef>(new SkillDefInfo
            {
                skillName = "RiftSecondarySpecial",
                skillNameToken = Rifter_PREFIX + "Secondary Special",
                skillDescriptionToken = Rifter_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(RiftSecondarySpecial)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = true,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,


            });
            RifterMod.Modules.Content.AddSkillDef(secondarySkillDefSpecial);


            SkillDef utilitySkillDefSpecial = Skills.CreateSkillDef<SkillDef>(new SkillDefInfo
            {
                skillName = "RiftUtilitySpecial",
                skillNameToken = Rifter_PREFIX + "Utility Special",
                skillDescriptionToken = Rifter_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(RiftUtilitySpecialLocate)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 10f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = true,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = true,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });
            RifterMod.Modules.Content.AddSkillDef(utilitySkillDefSpecial);
        }
        #endregion skills
        
        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Skins.CreateSkinDef("DEFAULT_SKIN",
                assetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
                //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
                //uncomment this when you have another skin
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshRifterSword",
            //    "meshRifterGun",
            //    "meshRifter");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin
            
            ////creating a new skindef as we did before
            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(Rifter_PREFIX + "MASTERY_SKIN_NAME",
            //    assetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
            //    defaultRendererinfos,
            //    prefabCharacterModel.gameObject,
            //    RifterUnlockables.masterySkinUnlockableDef);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshRifterSwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshRifterAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            //masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matRifterAlt");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matRifterAlt");
            //masterySkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matRifterAlt");

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            //skins.Add(masterySkin);
            
            #endregion

            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            RifterAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {

            //if (sender.HasBuff(RifterBuffs.fractureDebuff))
            //{
            //    args.armorAdd += 300;
            //}
        }
    }
}