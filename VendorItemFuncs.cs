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

        public Sprite sprite {get;set;}

    }

    public class VendorModData
    {
        public int modNum {get; set;}
        public string modType {get;set;}
        public string Name {get;set;}
        public string Class {get;set;}
        public string Desc {get;set;}

        public Sprite sprite {get;set;}
        
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
    
        public static VendorItemData getItemData(int itemnum) {
            var item = global.statstat.inventoryItems[itemnum];
            var type = "";
            if (item.Consumable > 0 && item.Consumable < 21) {
                type = "consumable";
            } else {
                type = "equippable";
            }
            return new VendorItemData(){
                Class = type,
                Name = item.Name,
                itemNum = itemnum,
                Desc = item.Desc
            };
        }
        public static VendorModData getModData(int modnum, string modtype) {
            GameObject[] array;
            if (modtype == "special") {
                array = global.statstat.allthemods.specialMods;
                 for (int i = 0; i < array.Length; i++)
				{
					modulescript component2 = array[i].GetComponent<modulescript>();
					if (component2.modnumber == modnum)
					{
						var itemname = global.statstat.ls.GetText("ui", component2.Name);
						var icon = component2.active;
                        return new VendorModData(){ Class = "module", Desc = component2.Desc, modNum = modnum, modType = modtype, Name = itemname, sprite = icon};
					}
				}
            }else if (modtype == "mod") {
                array = global.statstat.allthemods.modMods;
                for (int i = 0; i < array.Length; i++)
				{
					modulescript component2 = array[i].GetComponent<modulescript>();
					if (component2.modnumber == modnum)
					{
						var itemname = global.statstat.ls.GetText("ui", component2.Name);
						var icon = component2.active;
                        return new VendorModData(){ Class = "module", Desc = component2.Desc, modNum = modnum, modType = modtype, Name = itemname, sprite = icon};
					}
				}
            } else if (modtype == "weapon") {
                array = global.statstat.allthemods.weaponMods;
                for (int i = 0; i < array.Length; i++)
				{
					modulescript component2 = array[i].GetComponent<modulescript>();
					if (component2.modnumber == modnum)
					{
						var itemname = global.statstat.ls.GetText("ui", component2.Name);
						var icon = component2.active;
                        return new VendorModData(){ Class = "module", Desc = component2.Desc, modNum = modnum, modType = modtype, Name = itemname, sprite = icon};
					}
				}
            }
            return null;
        }

    }
}
