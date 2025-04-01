

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
        public static void LoadTheGame(status __instance)
        {
            int ind = 0;
            int vendorItemInd = 0;
            List<VendorItem> mabecitems = Plugin.layout.Vendors.Find((Vendor v) => v.Name == "Mabec").Items;
            foreach (vendordata dat in __instance.npcvd.JuliaInventory) {
                if (ind > 2 || ind == 3  || ind == 4 || ind == 5 || ind == 8 || vendorItemInd > mabecitems.Count - 1) {
                    ind += 1;
                } else {
                    dat.Cost = mabecitems[vendorItemInd].Cost;
                    dat.ItemName = mabecitems[vendorItemInd].Name;
                    dat.Classification = mabecitems[vendorItemInd].Class;
                    dat.Description = mabecitems[vendorItemInd].Desc;
                    dat.ItemNumber = mabecitems[vendorItemInd].ItemNumber;
                    dat.ModuleNumber = mabecitems[vendorItemInd].ModuleNumber;
                    dat.ModuleType = mabecitems[vendorItemInd].ModuleType;
                    dat.Stock = mabecitems[vendorItemInd].Stock;
                    dat.ReqWorldState = mabecitems[vendorItemInd].WorldState;
                    __instance.npcvd.JuliaInventory[ind] = dat;
                    vendorItemInd += 1;
                }
            }
            ind = 0;
            vendorItemInd = 0;
            List<VendorItem> billitems = Plugin.layout.Vendors.Find((Vendor v) => v.Name == "Bill").Items;
            foreach (vendordata dat in __instance.npcvd.BillInventory) {
                if ( ind == 2  || ind == 3 || ind == 5 || vendorItemInd > billitems.Count - 1) {
                    ind += 1;
                } else {
                    dat.Cost = billitems[vendorItemInd].Cost;
                    dat.ItemName = billitems[vendorItemInd].Name;
                    dat.Classification = billitems[vendorItemInd].Class;
                    dat.Description = billitems[vendorItemInd].Desc;
                    dat.ItemNumber = billitems[vendorItemInd].ItemNumber;
                    dat.ModuleNumber = billitems[vendorItemInd].ModuleNumber;
                    dat.ModuleType = billitems[vendorItemInd].ModuleType;
                    dat.Stock = billitems[vendorItemInd].Stock;
                    dat.ReqWorldState = billitems[vendorItemInd].WorldState;
                    __instance.npcvd.JuliaInventory[ind] = dat;
                    vendorItemInd += 1;
                }
            }
        }
    }
}