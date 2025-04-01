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
    public class Gili5Patch : MonoBehaviour
    {
        [HarmonyPatch(typeof(giliworld5script), nameof(giliworld5script.Update))]
        [HarmonyPrefix]
        public static bool Update(giliworld5script __instance)
        {
            if (global.statstat.thedata && global.statstat.thedata.MiscWorldState[35].Value == 1f && !__instance.trg)
            {
                global.statstat.thedata.MiscWorldState[35].Value = 2f;
                __instance.trg = true;
                Randomizer.RandomzierLayout.RandomizerGiveItem("GiliWorld5","dial");
                AchievementsManager.UnlockAchievement("npc07");
                global.statstat.achman.achievementsList[28] = 1;
                SaveDataManager.SetList<int>("achievementslist", global.statstat.achman.achievementsList);
                SaveDataManager.Write(true);
            }
            return false;
        }
    }
}