using BepInEx.Configuration;
using EntityStates;
using RifterMod.Characters.Survivors.Rifter.Components;
using RifterMod.Characters.Survivors.Rifter.SkillStates;
using RifterMod.Modules;
using RifterMod.Modules.Characters;
using RifterMod.Survivors.Rifter.Components;
using RifterMod.Survivors.Rifter.SkillStates;
using RoR2;
using AncientScepter;
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
            bodyColor = new Color(0.329f, 0.42f, 0.651f),
            sortPosition = 100,

            crosshair = RifterMod.Modules.Assets.LoadCrosshair("SimpleDot"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1.5f,
            armor = 0f,

            jumpCount = 1,

            damage = 14f, //changed from 12
            damageGrowth = 14f * 0.2f, //changed 12f to 14f
    };

        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "HeadPiece"
                },
                 new CustomRendererInfo
                {
                    childName = "GlassArm",
                },
                  new CustomRendererInfo
                {
                    childName = "GlassLeg",
                },
                new CustomRendererInfo
                {
                    childName = "MainBody",
                },
                new CustomRendererInfo
                {
                    childName = "BackVentTransparent", material = assetBundle.LoadMaterial("matBacklight")
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
            RifterDamage.SetupModdedDamage();

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
            bodyPrefab.AddComponent<RifterOverchargePassive>();
            bodyPrefab.AddComponent<RiftAndFracture>();
            bodyPrefab.AddComponent<OverchargeMeter>();
            bodyPrefab.GetComponent<OverchargeMeter>().passive = bodyPrefab.GetComponent<RifterOverchargePassive>();
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
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(RifterMain), typeof(EntityStates.SpawnTeleporterState));

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }

        #region skills
        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(bodyPrefab);
            //add our own
            AddPassiveSkills();
            Skills.CreateSkillFamilies(bodyPrefab);

            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddSpecialSkills();
            if (RifterPlugin.ScepterInstalled)
            {
                InitializeScepter();
            }
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPassiveSkills()
        {
            SkillLocator skillLocator = bodyPrefab.GetComponent<SkillLocator>();
            skillLocator.passiveSkill.enabled = true;
            skillLocator.passiveSkill.skillNameToken = Rifter_PREFIX + "PASSIVE_RIFT_BOOST";
            skillLocator.passiveSkill.skillDescriptionToken = Rifter_PREFIX + "PASSIVE_RIFT_BOOST_DESCRIPTION";
            skillLocator.passiveSkill.icon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon");


            //RiftAndFracture component = bodyPrefab.GetComponent<RiftAndFracture>();
            // RoR2.SkillLocator component2 = bodyPrefab.GetComponent<RoR2.SkillLocator>();
            //component2.passiveSkill.enabled = false;
            //component.riftAndFracturePassive = Skills.CreateSkillDef(new SkillDefInfo
            //{
            //    skillName = Rifter_PREFIX + "PASSIVE_RIFT_FRACTURE",
            //    skillNameToken = Rifter_PREFIX + "PASSIVE_RIFT_FRACTURE",
            //    skillDescriptionToken = Rifter_PREFIX + "PASSIVE_RIFT_FRACTURE_DESCRIPTION",
            //    skillIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
            //     activationState = new EntityStates.SerializableEntityStateType(typeof(Idle)),              
            //});
            //Skills.AddPassiveSkills(component.passiveSkillSlot.skillFamily, component.riftAndFracturePassive);
            //
            // component.riftOvercharge = Skills.CreateSkillDef(new SkillDefInfo
            // {
            //     skillName = Rifter_PREFIX + "PASSIVE_OVERCHARGED",
            //     skillNameToken = Rifter_PREFIX + "PASSIVE_OVERCHARGED",
            //     skillDescriptionToken = Rifter_PREFIX + "PASSIVE_OVERCHARGED_DESCRIPTION",
            //     skillIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
            //     activationState = new EntityStates.SerializableEntityStateType(typeof(Idle)),
            // });
            //Skills.AddPassiveSkills(component.overchargeSkillSlot.skillFamily, component.riftOvercharge);

        }

        private void AddPrimarySkills()
        {
            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            RifterSkillDef primarySkillDef1 = Skills.CreateSkillDef<RifterSkillDef>(new SkillDefInfo
                (
                    "Rift Gauntlet Scope",
                    Rifter_PREFIX + "PRIMARY_GAUNTLET_RANGED",
                    Rifter_PREFIX + "PRIMARY_GAUNTLET_RANGED_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                    new EntityStates.SerializableEntityStateType(typeof(SkillStates.RiftGauntletBase)),
                    "Weapon",
                    false
                ));
            primarySkillDef1.keywordTokens = new[] { Tokens.overchargedKeyword, Tokens.fractureKeyword };
            primarySkillDef1.overchargedIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon");
            primarySkillDef1.overchargedNameToken = Rifter_PREFIX + "SECONDARY_GUN_NAME";
            primarySkillDef1.overchargedDescriptionToken = Rifter_PREFIX + "SECONDARY_GUN_DESCRIPTION";
            primarySkillDef1.usesOvercharge = true;
            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);


            RifterSkillDef primarySkillDef2 = Skills.CreateSkillDef<RifterSkillDef>(new SkillDefInfo
               (
                   "Rift Gauntlet Scope",
                   Rifter_PREFIX + "PRIMARY_BUCKSHOT",
                   Rifter_PREFIX + "PRIMARY_BUCKSHOT_DESCRIPTION",
                   assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                   new EntityStates.SerializableEntityStateType(typeof(SkillStates.RiftBuckshot)),
                   "Weapon",
                   false
               ));
            primarySkillDef2.keywordTokens = new[] { Tokens.overchargedKeyword, Tokens.fractureKeyword };
            primarySkillDef2.overchargedIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon");
            primarySkillDef2.overchargedNameToken = Rifter_PREFIX + "SECONDARY_GUN_NAME";
            primarySkillDef2.overchargedDescriptionToken = Rifter_PREFIX + "SECONDARY_GUN_DESCRIPTION";
            primarySkillDef2.usesOvercharge = true;
            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef2);
        }

        private void AddSecondarySkills()
        {
            //here is a basic skill def with all fields accounted for
            RifterSkillDef secondarySkillDef1 = Skills.CreateSkillDef<RifterSkillDef>(new SkillDefInfo
            {
                skillName = "Rift Gauntlet",
                skillNameToken = Rifter_PREFIX + "SECONDARY_GAUNTLET",
                skillDescriptionToken = Rifter_PREFIX + "SECONDARY_GAUNTLET_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                keywordTokens = new[] { Tokens.overchargedKeyword, Tokens.fractureKeyword },
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.RiftGauntletShort)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Any,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,

            });
            secondarySkillDef1.lastChargeOvercharge = true;
            secondarySkillDef1.overchargedIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon");
            secondarySkillDef1.overchargedNameToken = Rifter_PREFIX + "PRIMARY_SLASH_NAME";
            secondarySkillDef1.overchargedDescriptionToken = Rifter_PREFIX + "PRIMARY_SLASH_DESCRIPTION";
            secondarySkillDef1.usesOvercharge = true;
            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1);

            RapidfireSkillDef secondarySkillDef2 = Skills.CreateSkillDef<RapidfireSkillDef>(new SkillDefInfo
            {
                skillName = "Rapidfire Gauntlet",
                skillNameToken = Rifter_PREFIX + "SECONDARY_RAPIDFIRE",
                skillDescriptionToken = Rifter_PREFIX + "SECONDARY_RAPIDFIRE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                keywordTokens = new[] { Tokens.overchargedKeyword, Tokens.fractureKeyword },
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.RapidfireGauntlet)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Any,

                baseMaxStock = 3,
                rechargeStock = 1,
                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,

            });
            secondarySkillDef2.newRecharge = 3.5f;
            secondarySkillDef2.baseRechargeInterval = 2f;
            secondarySkillDef2.lastChargeOvercharge = true;
            secondarySkillDef2.resetCooldownTimerOnUse = true;
            secondarySkillDef2.overchargedIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon");
            secondarySkillDef2.overchargedNameToken = Rifter_PREFIX + "PRIMARY_SLASH_NAME";
            secondarySkillDef2.overchargedDescriptionToken = Rifter_PREFIX + "PRIMARY_SLASH_DESCRIPTION";
            secondarySkillDef2.usesOvercharge = true;

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef2);

            RifterSkillDef secondarySkillDef3 = Skills.CreateSkillDef<RifterSkillDef>(new SkillDefInfo
            {
                skillName = "Refraction",
                skillNameToken = Rifter_PREFIX + "SECONDARY_REFRACTION",
                skillDescriptionToken = Rifter_PREFIX + "SECONDARY_REFRACTION_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                keywordTokens = new[] { Tokens.overchargedKeyword, Tokens.fractureKeyword },
                activationState = new EntityStates.SerializableEntityStateType(typeof(Refraction)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Any,

                baseMaxStock = 1,
                baseRechargeInterval = 3f,
                resetCooldownTimerOnUse = true,
                fullRestockOnAssign = false,
                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,

            });
            secondarySkillDef3.lastChargeOvercharge = false;
            secondarySkillDef3.overchargedIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon");
            secondarySkillDef3.overchargedNameToken = Rifter_PREFIX + "PRIMARY_GAUNTLET_RANGED";
            secondarySkillDef3.overchargedDescriptionToken = Rifter_PREFIX + "PRIMARY_GAUNTLET_RANGED_DESCRIPTION";
            secondarySkillDef3.usesOvercharge = false;
            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef3);


        }

        private void AddUtiitySkills()
        {

            RifterSkillDef utilitySkillDef1 = Skills.CreateSkillDef<RifterSkillDef>(new SkillDefInfo
            {
                skillName = "Slipstream",
                skillNameToken = Rifter_PREFIX + "UTILITY_SLIPSTREAM",
                skillDescriptionToken = Rifter_PREFIX + "UTILITY_SLIPSTREAM_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Slipstream)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Body",
                interruptPriority = InterruptPriority.PrioritySkill,
                keywordTokens = new[] { Tokens.overchargedKeyword },
                baseMaxStock = 3,
                baseRechargeInterval = 4f,

                stockToConsume = 1,

                forceSprintDuringState = true,
                canceledFromSprinting = false,
                isCombatSkill = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true
            });
            utilitySkillDef1.overchargedIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon");
            utilitySkillDef1.overcharges = true;
            utilitySkillDef1.usesOvercharge = false;
            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1);
            //here's a skilldef of a typical movement skill. some fields are omitted and will just have default values

            RifterSkillDef utilitySkillDef2 = Skills.CreateSkillDef<RifterSkillDef>(new SkillDefInfo
            {
                skillName = "Rift Rider",
                skillNameToken = Rifter_PREFIX + "UTILITY_RIFT_RIDER",
                skillDescriptionToken = Rifter_PREFIX + "UTILITY_RIFT_RIDER_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(RiftRiderLocate)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.PrioritySkill,

                baseMaxStock = 1,
                baseRechargeInterval = 10f,

                stockToConsume = 1,

                cancelSprintingOnActivation = true,
                canceledFromSprinting = false,
                isCombatSkill = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true
            });
            utilitySkillDef2.overchargedIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon");
            utilitySkillDef2.overcharges = false;
            utilitySkillDef2.usesOvercharge = false;
            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef2);

        }

        private void AddSpecialSkills()
        {
            //a basic skill
            RifterSkillDef specialSkillDef1 = Skills.CreateSkillDef<RifterSkillDef>(new SkillDefInfo
            {
                skillName = "Recursion",
                skillNameToken = Rifter_PREFIX + "SPECIAL_RECURSION",
                skillDescriptionToken = Rifter_PREFIX + "SPECIAL_RECURSION_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                keywordTokens = new[] { Tokens.overchargedKeyword },
                activationState = new EntityStates.SerializableEntityStateType(typeof(RecursionLocate)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.PrioritySkill,

                baseMaxStock = 1,
                baseRechargeInterval = 12f,

                beginSkillCooldownOnSkillEnd = true,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
            });
            specialSkillDef1.usesOvercharge = false;
            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);

            RifterSkillDef specialSkillDef2 = Skills.CreateSkillDef<RifterSkillDef>(new SkillDefInfo
            {
                skillName = "Chained Worlds",
                skillNameToken = Rifter_PREFIX + "SPECIAL_CHAINED_WORLDS",
                skillDescriptionToken = Rifter_PREFIX + "SPECIAL_CHAINED_WORLDS_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(ChainedWorldsChargeup)),
                keywordTokens = new[] { Tokens.overchargedChainedKeyword },
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.PrioritySkill,

                baseMaxStock = 1,
                baseRechargeInterval = 15f,

                beginSkillCooldownOnSkillEnd = true,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
            });
            specialSkillDef2.usesOvercharge = false;
            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef2);
        }


        private void InitializeScepter()
        {
            RifterSkillDef scepterSkillDef1 = Skills.CreateSkillDef<RifterSkillDef>(new SkillDefInfo
            {
                skillName = "To Singularity",
                skillNameToken = Rifter_PREFIX + "SPECIAL_RECURSION_SCEPTER",
                skillDescriptionToken = Rifter_PREFIX + "SPECIAL_RECURSION_DESCRIPTION_SCEPTER",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                keywordTokens = new[] { Tokens.overchargedKeyword },
                activationState = new EntityStates.SerializableEntityStateType(typeof(RecursionLocateScepter)),
                activationStateMachineName = "Weapon2",
                interruptPriority = InterruptPriority.PrioritySkill,

                baseMaxStock = 1,
                baseRechargeInterval = 12f,

                beginSkillCooldownOnSkillEnd = true,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
            });
            scepterSkillDef1.usesOvercharge = false;
            ItemBase<AncientScepterItem>.instance.RegisterScepterSkill(scepterSkillDef1, bodyName, SkillSlot.Special, 0);

            RifterSkillDef scepterSkillDef2 = Skills.CreateSkillDef<RifterSkillDef>(new SkillDefInfo
            {
                skillName = "Colliding Worlds",
                skillNameToken = Rifter_PREFIX + "SPECIAL_CHAINED_WORLDS_SCEPTER",
                skillDescriptionToken = Rifter_PREFIX + "SPECIAL_CHAINED_WORLDS_DESCRIPTION_SCEPTER",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(ChainedWorldsChargeupScepter)),
                keywordTokens = new[] { Tokens.overchargedChainedKeyword },
                activationStateMachineName = "Weapon2",
                interruptPriority = InterruptPriority.PrioritySkill,

                baseMaxStock = 1,
                baseRechargeInterval = 15f,

                beginSkillCooldownOnSkillEnd = true,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
            });
            scepterSkillDef2.usesOvercharge = false;
            ItemBase<AncientScepterItem>.instance.RegisterScepterSkill(scepterSkillDef2, bodyName, SkillSlot.Special, 1);
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