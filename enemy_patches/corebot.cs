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
    public class CoreBotPatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(corebotpickup), nameof(corebotpickup.Update))]
        [HarmonyPrefix]
        public static bool Update(corebotpickup __instance)
        {
            RandomizerLocation loc = CheckerComponent.locations.Find((RandomizerLocation _loc) => {
                return _loc.name == "coreBot";
            });
            if (__instance.it.executed == 1 && __instance.alive)
            {
                __instance.alive = false;
                global.statstat.thedata.MiscWorldState[__instance.DataNumber].Value = 1f;
                global.statstat.gsfx.pickupitem();
                __instance.it.executed = 0;
                __instance.it.RemoveSelf();
                __instance.StartCoroutine(__instance.unsuspend());
                if(loc.Item) {
                    global.statstat.GainItem(loc.itemNumber);
                } else {
                    string modtype = "";
                    if (loc.modType == SparklyItem.modtype.Weapons) {
                        modtype = "weapon";
                    } else if (loc.modType == SparklyItem.modtype.Modifier) {
                        modtype = "mod";
                    }
                    else if(loc.modType == SparklyItem.modtype.Special) {
                        modtype = "special";
                    }
                    global.statstat.GainModule(loc.moduleNumber, modtype, false);
                }
                __instance.coreglow.Play("corebotglowanim2", 0);
            }
            
            return false;
        }
    }
}