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
    public class BradPatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(brad), nameof(brad.Die))]
        [HarmonyPrefix]
        public static bool Die(brad __instance)
        {
            global.statstat.gsfx.PlayBossDeathChime(__instance.transform.position, 0f);
            __instance.et.GetComponent<enemytarget>().RemoveSelf();
            RandomizerItemBase loc = Plugin.layout.GetDialogueOrEnemy("brad", "enemy");
            CheckerComponent.spawnloot(__instance.gameObject, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, Plugin.layout.GetModtypeFromString(loc.modType));
 
            global.statstat.gsfx.PlaySplat(__instance.transform.position, 0.6f, 0.95f, 0.1f, 1f);
            __instance.dc.EnemyDeath();
            Collider2D[] array = __instance.thecols;
            for (int i = 0; i < array.Length; i++)
            {
                array[i].enabled = false;
            }
            __instance.anim.Play("die", 0);
            return false;
        }
    }
}