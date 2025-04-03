using System;
using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Il2CppSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Threading;
using System.Net.NetworkInformation;
using Cpp2IL.Core.Extensions;
using HarmonyLib;
using System.Net;

namespace Randomizer
{
    public enum COST{
        minor = 50,
        small = 250,
        medium = 500,
        large = 1000,
        major = 2000,
        huge = 3000,
    }

    public class VendorCostItem
    {
        public int itemNum {get; set;}
        public int cost {get;set;}
    }
    public class VendorCostModule
    {
        public int modNum {get; set;}
        public string modType {get;set;}
        public int cost {get;set;}
    }

    public class VendorItemData
    {
        public int itemNum {get; set;}
        public string Class {get;set;}

        public string Name {get;set;}
        public string Desc {get;set;}

    }

    public class VendorModData
    {
        public int modNum {get; set;}
        public string modType {get;set;}
        public string Name {get;set;}
        public string Class {get;set;}
        public string Desc {get;set;}
        
    }

    public class VendorItemFuncs
    {
       public static List<VendorCostItem> ItemCosts = new List<VendorCostItem>() {
            new VendorCostItem() {itemNum = 1, cost = (int)COST.minor},
            new VendorCostItem() {itemNum = 4, cost = (int)COST.medium},
            new VendorCostItem() {itemNum = 7, cost = (int)COST.large},
            new VendorCostItem() {itemNum = 9, cost = (int)COST.medium},
            new VendorCostItem() {itemNum = 11, cost = (int)COST.medium},
            new VendorCostItem() {itemNum = 12, cost = (int)COST.large},
            new VendorCostItem() {itemNum = 14, cost = (int)COST.major},
            new VendorCostItem() {itemNum = 15, cost = (int)COST.major},
            new VendorCostItem() {itemNum = 16, cost = (int)COST.major},
            new VendorCostItem() {itemNum = 19, cost = (int)COST.large},
            new VendorCostItem() {itemNum = 20, cost = (int)COST.medium},
            new VendorCostItem() {itemNum = 26, cost = (int)COST.medium},
            new VendorCostItem() {itemNum = 29, cost = (int)COST.large},
            new VendorCostItem() {itemNum = 31, cost = (int)COST.major},
            new VendorCostItem() {itemNum = 32, cost = (int)COST.medium},
            new VendorCostItem() {itemNum = 33, cost = (int)COST.large},
            new VendorCostItem() {itemNum = 34, cost = (int)COST.major},
            new VendorCostItem() {itemNum = 35, cost = (int)COST.medium},
            new VendorCostItem() {itemNum = 37, cost = (int)COST.minor},
            new VendorCostItem() {itemNum = 39, cost = (int)COST.minor},
            new VendorCostItem() {itemNum = 40, cost = (int)COST.minor},
            new VendorCostItem() {itemNum = 41, cost = (int)COST.minor},
            new VendorCostItem() {itemNum = 43, cost = (int)COST.medium},
            new VendorCostItem() {itemNum = 46, cost = (int)COST.minor},
            new VendorCostItem() {itemNum = 48, cost = (int)COST.minor},
            new VendorCostItem() {itemNum = 53, cost = (int)COST.medium},
            new VendorCostItem() {itemNum = 67, cost = (int)COST.minor},
            new VendorCostItem() {itemNum = 64, cost = (int)COST.minor},
            new VendorCostItem() {itemNum = 65, cost = (int)COST.small},
            new VendorCostItem() {itemNum = 66, cost = (int)COST.medium},
            new VendorCostItem() {itemNum = 68, cost = (int)COST.minor},
            new VendorCostItem() {itemNum = 0, cost = (int)COST.medium}
       };
        
       public static int GetItemCost(int id) {
            return VendorItemFuncs.ItemCosts.Find((VendorCostItem item) => item.itemNum == id).cost;
       }

       public static List<VendorCostModule> ModCosts = new List<VendorCostModule>() {
        //Weapons
            new VendorCostModule() { modNum = 0, modType = "weapon", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 1, modType = "weapon", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 2, modType = "weapon", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 3, modType = "weapon", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 4, modType = "weapon", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 5, modType = "weapon", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 7, modType = "weapon", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 9, modType = "weapon", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 11, modType = "weapon", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 12, modType = "weapon", cost = (int)COST.large},
            new VendorCostModule() { modNum = 13, modType = "weapon", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 14, modType = "weapon", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 15, modType = "weapon", cost = (int)COST.medium},
        //Modifiers
            new VendorCostModule() { modNum = 0, modType = "mod", cost = (int)COST.large},
            new VendorCostModule() { modNum = 1, modType = "mod", cost = (int)COST.large},
            new VendorCostModule() { modNum = 2, modType = "mod", cost = (int)COST.large},
            new VendorCostModule() { modNum = 3, modType = "mod", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 4, modType = "mod", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 5, modType = "mod", cost = (int)COST.large},
            new VendorCostModule() { modNum = 7, modType = "mod", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 6, modType = "mod", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 8, modType = "mod", cost = (int)COST.minor},
            new VendorCostModule() { modNum = 9, modType = "mod", cost = (int)COST.small},
            new VendorCostModule() { modNum = 10, modType = "mod", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 11, modType = "mod", cost = (int)COST.large},
            new VendorCostModule() { modNum = 12, modType = "mod", cost = (int)COST.large},
            new VendorCostModule() { modNum = 13, modType = "mod", cost = (int)COST.large},
            new VendorCostModule() { modNum = 14, modType = "mod", cost = (int)COST.small},
            new VendorCostModule() { modNum = 15, modType = "mod", cost = (int)COST.small},
            new VendorCostModule() { modNum = 16, modType = "mod", cost = (int)COST.minor},
            new VendorCostModule() { modNum = 17, modType = "mod", cost = (int)COST.major},
            //Special
            new VendorCostModule() { modNum = 0, modType = "special", cost = (int)COST.small},
            new VendorCostModule() { modNum = 1, modType = "special", cost = (int)COST.small},
            new VendorCostModule() { modNum = 2, modType = "special", cost = (int)COST.small},
            new VendorCostModule() { modNum = 3, modType = "special", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 4, modType = "special", cost = (int)COST.small},
            new VendorCostModule() { modNum = 5, modType = "special", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 6, modType = "special", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 7, modType = "special", cost = (int)COST.medium},
            new VendorCostModule() { modNum = 8, modType = "special", cost = (int)COST.medium},
       };
    
        public static int GetModCost(int id, string type) {
            return VendorItemFuncs.ModCosts.Find((VendorCostModule item) => item.modNum == id && item.modType == type).cost;
       }

        public static List<VendorItemData> ItemDatas = new List<VendorItemData>() {
            new VendorItemData() {itemNum = 1, Desc = "The Buckler allows you to block an attack", Class = "equippable", Name = "Buckler"},
            new VendorItemData() {itemNum = 4, Desc = "Allows dashing", Class = "equippable", Name = "Dash"},
            new VendorItemData() {itemNum = 7, Desc = "Allows an additional healing injection", Class = "equippable", Name = "Healing Core"},
            new VendorItemData() {itemNum = 9, Desc = "a Large NanoGen cluster", Class = "consumable", Name = "Large Nanogen Cluster"},
            new VendorItemData() {itemNum = 11, Desc = "M.U.T.T's signature Hammer", Class = "equippable", Name = "HyperSledge"},
            new VendorItemData() {itemNum = 12, Desc = "The Fist! Allows for some additional vertical if used during a jump", Class = "equippable", Name = "The Fist"},
            new VendorItemData() {itemNum = 14, Desc = "The Red Godseed", Class = "consumable", Name = "Red Godseed"},
            new VendorItemData() {itemNum = 15, Desc = "The Orange Godseed", Class = "consumable", Name = "Orange Godseed"},
            new VendorItemData() {itemNum = 16, Desc = "The Violet Godseed", Class = "consumable", Name = "Violet Godseed"},
            new VendorItemData() {itemNum = 19, Desc = "The Wall Jump Item!", Class = "equippable", Name = "Ankle Spikes"},
            new VendorItemData() {itemNum = 20, Desc = "Water Movement, An unused item that significantly increases movement ability in water", Class = "equippable", Name = "Water Movement"},
            new VendorItemData() {itemNum = 26, Desc = "Dolly, the flying melee", Class = "equippable", Name = "Dolly"},
            new VendorItemData() {itemNum = 29, Desc = "Increases the power of your healing injections", Class = "equippable", Name = "Core Amplifier"},
            new VendorItemData() {itemNum = 31, Desc = "Increases Luck", Class = "consumable", Name = "Green Godseed"},
            new VendorItemData() {itemNum = 32, Desc = "The Yo-Yo melee of spinning death", Class = "equippable", Name = "PainWheel"},
            new VendorItemData() {itemNum = 33, Desc = "A Hazardous Melee weapon", Class = "equippable", Name = "Hazard Blade"},
            new VendorItemData() {itemNum = 34, Desc = "This Town Ain't Big enough for the both of us", Class = "equippable", Name = "Hudson Goodshot"},
            new VendorItemData() {itemNum = 35, Desc = "A Spear that cauterizes the wounds it makes", Class = "equippable", Name = "Magma Spear"},
            new VendorItemData() {itemNum = 37, Desc = "Yay! You can use the map now.", Class = "equippable", Name = "Mapping Unit"},
            new VendorItemData() {itemNum = 39, Desc = "Increases HEalth regeneration", Class = "consumable", Name = "Heart Fix"},
            new VendorItemData() {itemNum = 40, Desc = "Increases Energy regeneration", Class = "consumable", Name = "Energy Fix"},
            new VendorItemData() {itemNum = 41, Desc = "Increases Stamina Regeneration", Class = "consumable", Name = "Stamina Fix"},
            new VendorItemData() {itemNum = 43, Desc = "Tired of having to go to a statue to remove parasites?", Class = "equippable", Name = "Parasite Remover"},
            new VendorItemData() {itemNum = 46, Desc = "Molly was finally happy with one of her creations", Class = "equippable", Name = "Horse Sculpture"},
            new VendorItemData() {itemNum = 48, Desc = "The Cut Machine Awaits.", Class = "consumable", Name = "Pebble of Perspective"},
            new VendorItemData() {itemNum = 53, Desc = "Gambler Made this for you, Grants an additional Jump. Thank you Gambler!", Class = "equippable", Name = "Malformed Godseed"},
            new VendorItemData() {itemNum = 67, Desc = "How Curious...", Class = "equippable", Name = "Curious Gizmo"},
            new VendorItemData() {itemNum = 64, Desc = "A Small amount of Nanogen", Class = "consumable", Name = "Small Nanogen cluster"},
            new VendorItemData() {itemNum = 65, Desc = "A Medium amount of Nanogen", Class = "consumable", Name = "Medium Nanogen cluster"},
            new VendorItemData() {itemNum = 66, Desc = "A Large amount of Nanogen", Class = "consumable", Name = "Large Nanogen cluster"},
            new VendorItemData() {itemNum = 68, Desc = "An Axe type melee weapon", Class = "equippable", Name = "Kinzy Axe"},
            new VendorItemData() {itemNum = 0, Desc = "Grants an Additional Jump", Class = "equippable", Name = "Boost Boots"}
        };
    
        public static VendorItemData getItemData(int itemnum) {
            return ItemDatas.Find((VendorItemData item) => item.itemNum == itemnum);
        }
    
        public static List<VendorModData> ModDatas = new List<VendorModData>() {
            //Weapons
            new VendorModData() { modNum = 0, modType = "weapon", Desc = "A fine weapon, accurate and efficient", Class = "module", Name = "Cloudburt Rifle"},
            new VendorModData() { modNum = 1, modType = "weapon", Desc = "Belonged once to a prince", Class = "module", Name = "The Heartbreaker"},
            new VendorModData() { modNum = 2, modType = "weapon", Desc = "Fires a shortrange spray of pellets, effective at close-range", Class = "module", Name = "Scatter Burst"},
            new VendorModData() { modNum = 3, modType = "weapon", Desc = "Cold-based projectiles, cools your heated barrel", Class = "module", Name = "Chill Crumbler"},
            new VendorModData() { modNum = 4, modType = "weapon", Desc = "Fires Skel things", Class = "module", Name = "Skel Planter"},
            new VendorModData() { modNum = 5, modType = "weapon", Desc = "An electric weapon", Class = "module", Name = "Firefly"},
            new VendorModData() { modNum = 7, modType = "weapon", Desc = "Fires curious eggs.", Class = "module", Name = "Skab Device"},
            new VendorModData() { modNum = 9, modType = "weapon", Desc = "The Flower Power", Class = "module", Name = "Flower Song"},
            new VendorModData() { modNum = 11, modType = "weapon", Desc = "Fires superheated plasma", Class = "module", Name = "Plasma Burner"},
            new VendorModData() { modNum = 12, modType = "weapon", Desc = "Flying Explosive goes BOOM!", Class = "module", Name = "Coburn Launcher"},
            new VendorModData() { modNum = 13, modType = "weapon", Desc = "Green Slimes, theyre cute arent they?", Class = "module", Name = "Blob Bloom"},
            new VendorModData() { modNum = 14, modType = "weapon", Desc = "Red Slimes, careful theyre a bit warm", Class = "module", Name = "Blob Bloom H"},
            new VendorModData() { modNum = 15, modType = "weapon", Desc = "A Sprinkly that fires bullets!", Class = "module", Name = "Sprinkler"},
        //Modifiers
            new VendorModData() { modNum = 0, modType = "mod", Desc = "Gain a Bonus to Gunpower!", Class = "module", Name = "Heart of Fire"},
            new VendorModData() { modNum = 1, modType = "mod", Desc = "Gain a Bonus to Vigor!", Class = "module", Name = "Heart of Strength"},
            new VendorModData() { modNum = 2, modType = "mod", Desc = "Gain a Bonus to Resolve!", Class = "module", Name = "Heart of Steadiness"},
            new VendorModData() { modNum = 3, modType = "mod", Desc = "Reduces dropped nanogen", Class = "module", Name = "Clutcher"},
            new VendorModData() { modNum = 4, modType = "mod", Desc = "Tired of chasing the nanogen???", Class = "module", Name = "Five-Fingers"},
            new VendorModData() { modNum = 5, modType = "mod", Desc = "Increased Nano gain", Class = "module", Name = "Nano Boost"},
            new VendorModData() { modNum = 7, modType = "mod", Desc = "Increases range of your shots", Class = "module", Name = "Distant Star"},
            new VendorModData() { modNum = 6, modType = "mod", Desc = "Increases melee damage when at low hp", Class = "module", Name = "Berserker Device"},
            new VendorModData() { modNum = 8, modType = "mod", Desc = "So you want a hot barrel?", Class = "module", Name = "Mollys Tweak"},
            new VendorModData() { modNum = 9, modType = "mod", Desc = "Reduces heat build up", Class = "module", Name = "HeatHawk"},
            new VendorModData() { modNum = 10, modType = "mod", Desc = "Careful im electric", Class = "module", Name = "Backlash"},
            new VendorModData() { modNum = 11, modType = "mod", Desc = "HULK SMASH", Class = "module", Name = "Ogre Device"},
            new VendorModData() { modNum = 12, modType = "mod", Desc = "Gotta Go Fast!", Class = "module", Name = "Summer Step"},
            new VendorModData() { modNum = 13, modType = "mod", Desc = "More Energey per Energy", Class = "module", Name = "Gunner's Harmony"},
            new VendorModData() { modNum = 14, modType = "mod", Desc = "He Went that way!", Class = "module", Name = "Dash Decoy"},
            new VendorModData() { modNum = 15, modType = "mod", Desc = "You mean my syringe gives me enery too?", Class = "module", Name = "Fishmaster's Glow"},
            new VendorModData() { modNum = 16, modType = "mod", Desc = "I seeee you", Class = "module", Name = "Lord's Insight"},
            new VendorModData() { modNum = 17, modType = "mod", Desc = "So you want to Go higher?", Class = "module", Name = "Extra Jump"},
            //Special
            new VendorModData() { modNum = 0, modType = "special", Desc = "You mean running replenishes my energy?", Class = "module", Name = "Life Run"},
            new VendorModData() { modNum = 1, modType = "special", Desc = "Hello Flower", Class = "module", Name = "Flower Step"},
            new VendorModData() { modNum = 2, modType = "special", Desc = "Caddle PRod time", Class = "module", Name = "Whistle Shock"},
            new VendorModData() { modNum = 3, modType = "special", Desc = "Time to dish the hurt", Class = "module", Name = "Adams Tweak"},
            new VendorModData() { modNum = 4, modType = "special", Desc = "Your life is mine", Class = "module", Name = "Suntouch"},
            new VendorModData() { modNum = 5, modType = "special", Desc = "I am Robot", Class = "module", Name = "Robotic Heart"},
            new VendorModData() { modNum = 6, modType = "special", Desc = "A weird Roslock Shell", Class = "module", Name = "Frenzied Shell"},
            new VendorModData() { modNum = 7, modType = "special", Desc = "A weird Roslock Shell", Class = "module", Name = "Volatile Shell"},
            new VendorModData() { modNum = 8, modType = "special", Desc = "A weird Roslock Shell", Class = "module", Name = "Hardened Shell"},
        };

        public static VendorModData getModData(int modnum, string modtype) {
            return ModDatas.Find((VendorModData item) => item.modNum == modnum && item.modType == modtype);
        }

    }
}
