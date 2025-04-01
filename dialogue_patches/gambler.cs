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
    public class GamblerPatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(gamblerroom), nameof(gamblerroom.Update))]
        [HarmonyPrefix]
        public static bool Update(gamblerroom __instance)
        {
            if (global.statstat)
            {
                if (global.statstat.thedata.MiscWorldState[43].Value == 1f)
                {
                    global.statstat.thedata.MiscWorldState[43].Value = 2f;
                    Randomizer.RandomzierLayout.RandomizerGiveItem("Gambler", "dial");
                }
                if ( __instance.fgc.gatesclosed)
                {
                    if (global.statstat.thedata.MiscWorldState[21].Value == 1f && ! __instance.opentrg)
                    {
                        if (global.statstat.thedata.OtherData[27] != 1)
                        {
                            global.statstat.thedata.OtherData[27] = 1;
                        }
                         __instance.opentrg = true;
                        Sprite buttonicon = global.afx.particon[UnityEngine.Random.Range(0, global.afx.particon.Length)];
                        string text = global.statstat.ls.GetText("UI", "tut_partsonmap");
                        global.statstat.tpopu.displaytutpop(text, buttonicon, "none", 0, 800, true, false);
                         __instance.fgc.OpenGates();
                    }
                }
                else if (global.statstat.thedata.MiscWorldState[21].Value == 0f && global.elby.transform.position.x > 50f &&  __instance.uptime > 60)
                {
                     __instance.fgc.CloseGates();
                }
            }
            return false;
        }
    }
}