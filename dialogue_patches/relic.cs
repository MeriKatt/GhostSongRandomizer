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
    public class RelicPatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(relictalks), nameof(relictalks.Update))]
        [HarmonyPrefix]
        public static bool Update(relictalks __instance)
        {
            if ( __instance.npcg.currentconvo)
            {
                 __instance.thecd = 120;
            }
            if (global.statstat.npcd.RimeProgress[7] >= 6 && global.statstat.thedata.OtherData[57] == 0)
            {
                global.statstat.thedata.OtherData[57] = 1;
            }
            if ( __instance.rfc.FlowerHit > 0 && ! __instance.trg && !charcon.lockinputs && !charcon.stopstatusfromcomingup &&  __instance.thecd <= 0 && ! __instance.talksource.isPlaying)
            {
                 __instance.trg = true;
                 __instance.it.RemoveSelf();
                 __instance.npcg.interactionpup.GetComponent<InteractionPopUp>().suspend = true;
                 __instance.npcg.interactionpup.GetComponent<InteractionPopUp>().RemoveTarget();
                 __instance.talksource.Stop();
                 __instance.StartCoroutine( __instance.flowercomment());
            }
            if (! __instance.stoplook)
            {
                 __instance.managelook();
            }
            if (global.statstat && global.statstat.thedata.MiscWorldState[46].Value == 1f)
            {
                global.statstat.thedata.MiscWorldState[46].Value = 2f;
                Randomizer.RandomzierLayout.RandomizerGiveItem("Relic","dial");
                AchievementsManager.UnlockAchievement("npc06");
                global.statstat.achman.achievementsList[27] = 1;
                SaveDataManager.SetList<int>("achievementslist", global.statstat.achman.achievementsList);
                SaveDataManager.Write(true);
            }
            return false;
        }
    }
}