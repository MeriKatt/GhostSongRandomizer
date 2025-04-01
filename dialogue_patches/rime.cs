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
    public class RimePatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(rimeworld1), nameof(rimeworld1.Update))]
        [HarmonyPrefix]
        public static bool Update(rimeworld1 __instance)
        {
            if (global.statstat.thedata)
            {
                if (global.statstat.thedata.MiscWorldState[17].Value == 1f && !__instance.trg)
                {
                    global.statstat.thedata.MiscWorldState[17].Value = 2f;
                    __instance.trg = true;
                    Randomizer.RandomzierLayout.RandomizerGiveItem("Rime","dial");
                }
                if (__instance.eyetimething > 50 && __instance.eyeglow.enabled)
                {
                    __instance.eyeglow.enabled = false;
                }
            }
            return false;
        }
    }
}