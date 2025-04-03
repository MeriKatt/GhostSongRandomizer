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
using System.Runtime.Serialization;

namespace Randomizer
{

    [DataContract]
    public class RandomizerItemBase
    {
        [DataMember]
        public int arrayNumber {get; set;}
        [DataMember]
        public bool Item {get; set;}
        [DataMember]
        public int itemNumber {get; set;}
        [DataMember]
        public bool Module {get; set;}
        [DataMember]
        public int moduleNumber {get; set;}
        [DataMember]
        public string modType {get; set;}

        public static RandomizerItemBase From(RandomizerItemInfo info) {
            RandomizerItemBase item = new RandomizerItemBase();
            item.arrayNumber = -1;
            item.Item = info.Item;
            item.itemNumber = info.itemNumber;
            item.Module = !item.Item;
            if (item.Module) {
                item.itemNumber =  -1;
            }
            item.moduleNumber = info.moduleNumber;
            item.modType = info.modType;
            if (item.itemNumber == 0 && item.Module == false) item.Item = true;
            return item;
        }
        public static RandomizerItemBase From(VendorItem info) {
            RandomizerItemBase item = new RandomizerItemBase();
            item.arrayNumber = -1;
            item.Item = info.ItemNumber > -1;
            item.itemNumber = info.ItemNumber;
            item.Module = !item.Item;
            if (item.itemNumber == 0 && item.Module == false) item.Item = true;
            item.moduleNumber = info.ModuleNumber;
            if (info.ModuleType == "weapon"){
                item.modType = "Weapons";
            } else if (info.ModuleType == "mod") {
                item.modType = "Modifier";
            } else if (info.ModuleType == "special") {
                item.modType = "Special";
            }
            return item;
        }
        public static RandomizerItemBase From(RandomizerItemBase info) {
            RandomizerItemBase item = new RandomizerItemBase();
            item.arrayNumber = -1;
            item.Item = info.Item;
            item.itemNumber = info.itemNumber;
            item.Module = !item.Item;
            if (item.Module) {
                item.itemNumber =  -1;
            }
            if (item.itemNumber == 0 && item.Module == false) item.Item = true;
            item.moduleNumber = info.moduleNumber;
            item.modType = info.modType;
            return item;
        }
    }

    [DataContract]
    public class RandomizerItemInfo : RandomizerItemBase
    {
        [DataMember]
        public string Name {get; set;}
        [DataMember]
        public string position {get; set;}

         public static RandomizerItemInfo From(RandomizerItemBase info, string Name, string position) {
            RandomizerItemInfo item = new RandomizerItemInfo();
            item.arrayNumber = -1;
            item.Item = info.Item;
            item.itemNumber = info.itemNumber;
            item.Module = !item.Item;
            if (item.Module) {
                item.itemNumber =  -1;
            }
            item.moduleNumber = info.moduleNumber;
            item.modType = info.modType;
            item.Name = Name;
            item.position = position;
            return item;
        }
        public static RandomizerItemInfo From(VendorItem info,  string Name, string position) {
            RandomizerItemInfo item = new RandomizerItemInfo();
            item.arrayNumber = -1;
            item.Item = info.ItemNumber > -1;
            item.itemNumber = info.ItemNumber;
            item.Module = info.ItemNumber == -1;
            if (item.Module) {
                item.itemNumber =  -1;
            }
            item.moduleNumber = info.ModuleNumber;
            if (info.ModuleType == "weapon"){
                item.modType = "Weapons";
            } else if (info.ModuleType == "mod") {
                item.modType = "Modifier";
            } else if (info.ModuleType == "special") {
                item.modType = "Special";
            }
            item.Name = Name;
            item.position = position;
            return item;
        }
    }


    [DataContract]
    public class Room
    {
        [DataMember]
        public string Name {get; set;}
        [DataMember]
        public List<RandomizerItemInfo> Items {get; set;}
        [DataMember]
        public string Enemies {get; set;}

    }

    [DataContract]
    public class VendorItem {
        [DataMember]
        public string Name {get;set;}
        [DataMember]
        public string Desc {get;set;}
        [DataMember]
        public string Class {get;set;}
        [DataMember]
        public int Stock {get;set;}
        [DataMember]
        public int Cost {get;set;}
        [DataMember]
        public int WorldState {get;set;}
        [DataMember]
        public int ItemNumber {get;set;} // -1 means its a module instead
        [DataMember]
        public int ModuleNumber {get;set;}
        [DataMember]
        public string ModuleType {get;set;}

        public static VendorItem From(RandomizerItemBase info) {
            VendorItem item = new VendorItem();
            if (info.Module)
            {
                item.ItemNumber = -1;
            } else {
                item.ItemNumber = info.itemNumber;
            }
            item.ModuleNumber = info.moduleNumber;
            if (item.ItemNumber != -1) {
                item.ModuleNumber = -1;
            }
            if (info.modType == "Weapons"){
                item.ModuleType = "weapon";
            } else if (info.modType == "Modifier") {
                item.ModuleType = "mod";
            } else if (info.modType == "Special") {
                item.ModuleType = "special";
            }
            if (item.ItemNumber > -1) {
                item.Cost = VendorItemFuncs.GetItemCost(item.ItemNumber);
            } else {
                item.Cost = VendorItemFuncs.GetModCost(item.ModuleNumber, item.ModuleType);
            }
            item.Stock = 1;
            if (item.ItemNumber > -1) {
                VendorItemData dat = VendorItemFuncs.getItemData(item.ItemNumber);
                item.Class = dat.Class;
                item.Desc = dat.Desc;
                item.Name = dat.Name;
            } else {
                VendorModData dat = VendorItemFuncs.getModData(item.ModuleNumber, item.ModuleType);
                item.Class = dat.Class;
                item.Desc = dat.Desc;
                item.Name = dat.Name;
            }
            return item;
        }
    }

    [DataContract]
    public class Vendor {
        [DataMember]
        public string Name {get;set;}
        [DataMember]
        public List<VendorItem> Items {get;set;}
        
    }

    [DataContract]
    public class Enemy {
        [DataMember]
        public string Name {get;set;}
        [DataMember]
        public RandomizerItemBase Item {get;set;}
    }

    
    [DataContract]
    public class Dialogue{
        [DataMember]
        public string Name {get; set;}

        [DataMember]
        public RandomizerItemBase Item {get; set;}
    }

    [DataContract]
    public class RandomzierLayout
    {
        [DataMember]
        public List<Room> Rooms{get; set;}
        [DataMember]
        public List<Vendor> Vendors{get; set;}
        [DataMember]
        public List<Enemy> EnemyDrops;
        [DataMember]
        public List<Dialogue> DialogueItems;

        public SparklyItem.modtype GetModtypeFromString(string type) {
            if (type == SparklyItem.modtype.Modifier.ToString()) return SparklyItem.modtype.Modifier;
            if (type == SparklyItem.modtype.Special.ToString()) return SparklyItem.modtype.Special;
            return SparklyItem.modtype.Weapons;
        }

        public RandomizerItemBase GetDialogueOrEnemy(string name, string type) {
            if (type == "enemy"){
                return this.EnemyDrops.Find((Enemy enemy) => enemy.Name == name).Item;
            } else if (type == "dial") {
                return this.DialogueItems.Find((Dialogue dial) => dial.Name == name).Item;
            }
            return null;
        }
        public VendorItem GetVendorItem(string name, int index) {
            return this.Vendors.Find((Vendor ven) => ven.Name == name).Items[index];
        }
        public List<RandomizerItemInfo> GetRoomItems(string name) {
            if (this.Rooms.Find((Room room) => room.Name == name) != null ){
                return this.Rooms.Find((Room room) => room.Name == name).Items;
            }
            return null;
        }
        public static void RandomizerGiveItem(string name, string type) {
            RandomizerItemBase item = Plugin.layout.GetDialogueOrEnemy(name, type);
            if (item.Item)
            {
                global.statstat.GainItem(item.itemNumber);
            }
            else
            {
                string modtype = "";
                if (item.modType == SparklyItem.modtype.Weapons.ToString())
                {
                    modtype = "weapon";
                }
                else if (item.modType == SparklyItem.modtype.Modifier.ToString())
                {
                    modtype = "mod";
                }
                else if (item.modType == SparklyItem.modtype.Special.ToString())
                {
                    modtype = "special";
                }
                global.statstat.GainModule(item.moduleNumber, modtype, false);
            }
        }

    }
}