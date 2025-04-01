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
    public class MollyPatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(mollycamp3special), nameof(mollycamp3special.Update))]
        [HarmonyPrefix]
        public static bool Update(mollycamp3special __instance)
        {
            if (global.statstat.thedata && global.statstat.thedata.MiscWorldState[22].Value == 1f && !__instance.trg)
            {
                global.statstat.thedata.MiscWorldState[22].Value = 2f;
                __instance.trg = true;
                Randomizer.RandomzierLayout.RandomizerGiveItem("MollyCamp3", "dial");
            }
            return false;
        }

        [HarmonyPatch(typeof(mollygeneral), nameof(mollygeneral.Update))]
        [HarmonyPrefix]
        public static bool Update(mollygeneral __instance)
        {
            if ( __instance.turnsHead)
            {
                 __instance.managehead();
            }
            if ( __instance.miscChatter)
            {
                 __instance.chatter();
            }
            if ( __instance.gestureTalk)
            {
                if ( __instance.npcg.currentconvo != null)
                {
                    if (! __instance.trg2)
                    {
                         __instance.trg2 = true;
                        if (UnityEngine.Random.value > 0.5f)
                        {
                             __instance.anim.Play("idletalk", 0);
                        }
                        else
                        {
                             __instance.anim.Play("idletalk2", 0);
                        }
                    }
                }
                else
                {
                     __instance.trg2 = false;
                }
            }
            else if ( __instance.mollyBally)
            {
                if ( __instance.npcg.currentconvo != null && ! __instance.trg)
                {
                     __instance.trg = true;
                     __instance.anim.Play("idlehold4", 0);
                }
                if (global.statstat.thedata.MiscWorldState[22].Value == 1f && ! __instance.trgx)
                {
                     __instance.anim.Play("idleholdidle", 0);
                    global.statstat.thedata.MiscWorldState[22].Value = 2f;
                     __instance.trgx = true;
                    Randomizer.RandomzierLayout.RandomizerGiveItem("MollyGen", "dial");
                }
            }
            if ( __instance.horseversion && global.statstat.thedata.MiscWorldState[44].Value == 1f && ! __instance.trgx2)
            {
                 __instance.anim.Play("idleholdidle", 0);
                global.statstat.thedata.MiscWorldState[44].Value = 2f;
                 __instance.trgx2 = true;
                Randomizer.RandomzierLayout.RandomizerGiveItem("MollyGen", "dial");
                AchievementsManager.UnlockAchievement("npc08");
                global.statstat.achman.achievementsList[29] = 1;
                SaveDataManager.SetList<int>("achievementslist", global.statstat.achman.achievementsList);
                SaveDataManager.Write(true);
            }
            return false;
        }
    }
}