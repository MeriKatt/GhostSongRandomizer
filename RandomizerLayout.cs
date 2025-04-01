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
    }

    [DataContract]
    public class RandomizerItemInfo : RandomizerItemBase
    {
        [DataMember]
        public string Name {get; set;}
        [DataMember]
        public string position {get; set;}
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