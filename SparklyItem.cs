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
    public class SparklyItemPatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(SparklyItem), nameof(SparklyItem.Update))]
        [HarmonyPrefix]
        public static bool Update(SparklyItem __instance)
        {
            
                if (__instance.it.executed == 1 && !__instance.dead)
                {
                    __instance.it.executed = 0;
                    __instance.dead = true;
                    RandomizerItem item = __instance.gameObject.GetComponentsInParent<RandomizerItem>(true)[0];

                    if (__instance.SetWorldStateArray >= 0)
                    {
                        global.statstat.thedata.MiscWorldState[__instance.SetWorldStateArray].Value = __instance.SetWorldStateValue;
                    }
                    if (__instance.arrayNumber != -1)
                    {
                        global.statstat.thedata.ItemPickups[__instance.arrayNumber].Value = 1f;
                    }
                    __instance.it.RemoveSelf();
                    if (__instance.Module)
                    {
                        string moduletype = null;
                        if (__instance.moduleType == SparklyItem.modtype.Weapons)
                        {
                            moduletype = "weapon";
                        }
                        else if (__instance.moduleType == SparklyItem.modtype.Modifier)
                        {
                            moduletype = "mod";
                        }
                        else if (__instance.moduleType == SparklyItem.modtype.Special)
                        {
                            moduletype = "special";
                        }
                        global.statstat.GainModule(__instance.moduleNumber, moduletype, false);
                    }
                    if (__instance.Item)
                    {
                        global.statstat.GainItem(__instance.itemNumber);
                    }
                    __instance.StartCoroutine(__instance.unsuspend());
                    if ( item != null && item.name == "RandomizerItem") {
                        System.Console.WriteLine("Randomizer Item Grabbed");
                        RandomizerItem.saveToFile(true, item.Name);
                        UnityEngine.Object.Destroy(item);
                    }
                }
            return false;
        }
    }
}



