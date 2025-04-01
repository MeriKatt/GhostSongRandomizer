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
using System.Threading.Tasks;
using System.Linq;

namespace Randomizer
{
    public class SparklyItemPatch : MonoBehaviour
    {

        
        [HarmonyPatch(typeof(SparklyItem), nameof(SparklyItem.Update))]
        [HarmonyPrefix]
        public static bool Update(SparklyItem __instance)
        {
            if (__instance.it.executed == 1 && !__instance.dead)
            {
                RandomizerItem[] items = __instance.gameObject.GetComponentsInParent<RandomizerItem>(true);
                RandomizerItem item = null;
                if (items.Length > 0) {
                    item = items[0];
                }
                if ( item != null && item.name == "RandomizerItem") {
                    System.Console.WriteLine("Randomizer Item Grabbed");
                    RandomizerItem.saveToFile(true, item.Name);
                    UnityEngine.Object.Destroy(item);
                }
            }
            
            return true;
        }
    }
}



