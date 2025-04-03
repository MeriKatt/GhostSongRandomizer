

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
    public class StatusPatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(status), nameof(status.LoadTheGame))]
        [HarmonyPostfix]
        public static void LoadTheGame()
        {
            if (Plugin.shouldRandomize)
            {
                Plugin.Randomize();
                Plugin.layout = new RandomzierLayout();
                string input = System.IO.File.ReadAllText("./locations/RandomizerSeed.json");
                using (var ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(input)))
                {
                    Plugin.layout = (RandomzierLayout)Plugin.ser.ReadObject(ms);
                }
            }
            List<VendorItem> mabecitems = Plugin.layout.Vendors.Find((Vendor v) => v.Name == "Mabec").Items;
            vendordata[] newMabecItems = new vendordata[mabecitems.Count];
            foreach (VendorItem _dat in mabecitems)
            {
                vendordata dat = global.statstat.npcvd.JuliaInventory[0];
                dat.Cost = _dat.Cost;
                dat.ItemName = _dat.Name;
                dat.Classification = _dat.Class;
                dat.Description = _dat.Desc;
                dat.ItemNumber = _dat.ItemNumber;
                dat.ModuleNumber = _dat.ModuleNumber;
                dat.ModuleType = _dat.ModuleType;
                dat.Stock = _dat.Stock;
                dat.ReqWorldState = _dat.WorldState;
                if (dat.ItemNumber != -1)
                {
                    dat.Icon = global.statstat.inventoryItems[dat.ItemNumber].Icon;
                }
                else
                {
                    GameObject[] array;
                    if (dat.ModuleType == "special")
                    {
                        array = global.statstat.allthemods.specialMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                    else if (dat.ModuleType == "mod")
                    {
                        array = global.statstat.allthemods.modMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                    else if (dat.ModuleType == "weapon")
                    {
                        array = global.statstat.allthemods.weaponMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                }
                newMabecItems.AddItem(dat);
            }
            global.statstat.npcvd.JuliaInventory = newMabecItems;
            List<VendorItem> billitems = Plugin.layout.Vendors.Find((Vendor v) => v.Name == "Bill").Items;
            vendordata[] newBillItems = new vendordata[billitems.Count];
            foreach (VendorItem _dat in billitems)

            {
                vendordata dat = global.statstat.npcvd.BillInventory[0];
                dat.Cost = _dat.Cost;
                dat.ItemName = _dat.Name;
                dat.Classification = _dat.Class;
                dat.Description = _dat.Desc;
                dat.ItemNumber = _dat.ItemNumber;
                dat.ModuleNumber = _dat.ModuleNumber;
                dat.ModuleType = _dat.ModuleType;
                dat.Stock = _dat.Stock;
                dat.ReqWorldState = _dat.WorldState;
                if (dat.ItemNumber != -1)
                {
                    dat.Icon = global.statstat.inventoryItems[dat.ItemNumber].Icon;
                }
                else
                {
                    GameObject[] array;
                    if (dat.ModuleType == "special")
                    {
                        array = global.statstat.allthemods.specialMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                    else if (dat.ModuleType == "mod")
                    {
                        array = global.statstat.allthemods.modMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                    else if (dat.ModuleType == "weapon")
                    {
                        array = global.statstat.allthemods.weaponMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                }
                newBillItems.AddItem(dat);
            }
            global.statstat.npcvd.BillInventory = newBillItems;
        }

        [HarmonyPatch(typeof(status), nameof(status.InitializeClass))]
        [HarmonyPostfix]
        public static void InitializeClass()
        {
            if (Plugin.shouldRandomize)
            {
                Plugin.Randomize();
                Plugin.layout = new RandomzierLayout();
                string input = System.IO.File.ReadAllText("./locations/RandomizerSeed.json");
                using (var ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(input)))
                {
                    Plugin.layout = (RandomzierLayout)Plugin.ser.ReadObject(ms);
                }
            }
            List<VendorItem> mabecitems = Plugin.layout.Vendors.Find((Vendor v) => v.Name == "Mabec").Items;
            vendordata[] newMabecItems = new vendordata[mabecitems.Count];
            foreach (VendorItem _dat in mabecitems)
            {
                vendordata dat = global.statstat.npcvd.JuliaInventory[0];
                dat.Cost = _dat.Cost;
                dat.ItemName = _dat.Name;
                dat.Classification = _dat.Class;
                dat.Description = _dat.Desc;
                dat.ItemNumber = _dat.ItemNumber;
                dat.ModuleNumber = _dat.ModuleNumber;
                dat.ModuleType = _dat.ModuleType;
                dat.Stock = _dat.Stock;
                dat.ReqWorldState = _dat.WorldState;
                if (dat.ItemNumber != -1)
                {
                    dat.Icon = global.statstat.inventoryItems[dat.ItemNumber].Icon;
                }
                else
                {
                    GameObject[] array;
                    if (dat.ModuleType == "special")
                    {
                        array = global.statstat.allthemods.specialMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                    else if (dat.ModuleType == "mod")
                    {
                        array = global.statstat.allthemods.modMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                    else if (dat.ModuleType == "weapon")
                    {
                        array = global.statstat.allthemods.weaponMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                }
                newMabecItems.AddItem(dat);
            }
            global.statstat.npcvd.JuliaInventory = newMabecItems;
            List<VendorItem> billitems = Plugin.layout.Vendors.Find((Vendor v) => v.Name == "Bill").Items;
            vendordata[] newBillItems = new vendordata[billitems.Count];
            foreach (VendorItem _dat in billitems)

            {
                vendordata dat = global.statstat.npcvd.BillInventory[0];
                dat.Cost = _dat.Cost;
                dat.ItemName = _dat.Name;
                dat.Classification = _dat.Class;
                dat.Description = _dat.Desc;
                dat.ItemNumber = _dat.ItemNumber;
                dat.ModuleNumber = _dat.ModuleNumber;
                dat.ModuleType = _dat.ModuleType;
                dat.Stock = _dat.Stock;
                dat.ReqWorldState = _dat.WorldState;
                if (dat.ItemNumber != -1)
                {
                    dat.Icon = global.statstat.inventoryItems[dat.ItemNumber].Icon;
                }
                else
                {
                    GameObject[] array;
                    if (dat.ModuleType == "special")
                    {
                        array = global.statstat.allthemods.specialMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                    else if (dat.ModuleType == "mod")
                    {
                        array = global.statstat.allthemods.modMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                    else if (dat.ModuleType == "weapon")
                    {
                        array = global.statstat.allthemods.weaponMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                }
                newBillItems.AddItem(dat);
            }
            global.statstat.npcvd.BillInventory = newBillItems;
        }
        [HarmonyPatch(typeof(status), nameof(status.InitializeClassPlus))]
        [HarmonyPostfix]
        public static void InitializeClassPlus()
        {
            if (Plugin.shouldRandomize)
            {
                Plugin.Randomize();
                Plugin.layout = new RandomzierLayout();
                string input = System.IO.File.ReadAllText("./locations/RandomizerSeed.json");
                using (var ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(input)))
                {
                    Plugin.layout = (RandomzierLayout)Plugin.ser.ReadObject(ms);
                }
            }
            List<VendorItem> mabecitems = Plugin.layout.Vendors.Find((Vendor v) => v.Name == "Mabec").Items;
            vendordata[] newMabecItems = new vendordata[mabecitems.Count];
            foreach (VendorItem _dat in mabecitems)
            {
                vendordata dat = global.statstat.npcvd.JuliaInventory[0];
                dat.Cost = _dat.Cost;
                dat.ItemName = _dat.Name;
                dat.Classification = _dat.Class;
                dat.Description = _dat.Desc;
                dat.ItemNumber = _dat.ItemNumber;
                dat.ModuleNumber = _dat.ModuleNumber;
                dat.ModuleType = _dat.ModuleType;
                dat.Stock = _dat.Stock;
                dat.ReqWorldState = _dat.WorldState;
                if (dat.ItemNumber != -1)
                {
                    dat.Icon = global.statstat.inventoryItems[dat.ItemNumber].Icon;
                }
                else
                {
                    GameObject[] array;
                    if (dat.ModuleType == "special")
                    {
                        array = global.statstat.allthemods.specialMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                    else if (dat.ModuleType == "mod")
                    {
                        array = global.statstat.allthemods.modMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                    else if (dat.ModuleType == "weapon")
                    {
                        array = global.statstat.allthemods.weaponMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                }
                newMabecItems.AddItem(dat);
            }
            global.statstat.npcvd.JuliaInventory = newMabecItems;
            List<VendorItem> billitems = Plugin.layout.Vendors.Find((Vendor v) => v.Name == "Bill").Items;
            vendordata[] newBillItems = new vendordata[billitems.Count];
            foreach (VendorItem _dat in billitems)

            {
                vendordata dat = global.statstat.npcvd.BillInventory[0];
                dat.Cost = _dat.Cost;
                dat.ItemName = _dat.Name;
                dat.Classification = _dat.Class;
                dat.Description = _dat.Desc;
                dat.ItemNumber = _dat.ItemNumber;
                dat.ModuleNumber = _dat.ModuleNumber;
                dat.ModuleType = _dat.ModuleType;
                dat.Stock = _dat.Stock;
                dat.ReqWorldState = _dat.WorldState;
                if (dat.ItemNumber != -1)
                {
                    dat.Icon = global.statstat.inventoryItems[dat.ItemNumber].Icon;
                }
                else
                {
                    GameObject[] array;
                    if (dat.ModuleType == "special")
                    {
                        array = global.statstat.allthemods.specialMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                    else if (dat.ModuleType == "mod")
                    {
                        array = global.statstat.allthemods.modMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                    else if (dat.ModuleType == "weapon")
                    {
                        array = global.statstat.allthemods.weaponMods;
                        for (int i = 0; i < array.Length; i++)
                        {
                            modulescript component2 = array[i].GetComponent<modulescript>();
                            if (component2.modnumber == dat.ModuleNumber)
                            {
                                dat.Icon = component2.active;
                            }
                        }
                    }
                }
                newBillItems.AddItem(dat);
            }
            global.statstat.npcvd.BillInventory = newBillItems;
        }
    }
}