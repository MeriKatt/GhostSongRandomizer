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

            if (__instance.it.executed == 1 && __instance.alive)
            {
                __instance.alive = false;
                global.statstat.thedata.MiscWorldState[__instance.DataNumber].Value = 1f;
                global.statstat.gsfx.pickupitem();
                __instance.it.executed = 0;
                __instance.it.RemoveSelf();
                __instance.StartCoroutine(__instance.unsuspend());
                Randomizer.RandomzierLayout.RandomizerGiveItem("corebot","dial");
                __instance.coreglow.Play("corebotglowanim2", 0);
            }
            
            return false;
        }
    }
}