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
    public class Walker5Patch : MonoBehaviour
    {
        [HarmonyPatch(typeof(walker5behavior), nameof(walker5behavior.SpawnLoot))]
        [HarmonyPostfix]
        public static void SpawnLoot(walker5behavior __instance)
        {
            SparklyItem item = GameObject.FindObjectOfType<SparklyItem>();
            UnityEngine.Object.Destroy(item.gameObject);
            GameObject elby = global.elby;
            RandomizerItemBase loc = Plugin.layout.GetDialogueOrEnemy("walker5", "enemy");
            CheckerComponent.spawnloot(__instance.gameObject, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, Plugin.layout.GetModtypeFromString(loc.modType));
         }
        [HarmonyPatch(typeof(walker5behavior), nameof(walker5behavior.SpawnRandomLoot))]
        [HarmonyPostfix]
        public static void SpawnRandomLoot(walker5behavior __instance)
        {
            SparklyItem item = GameObject.FindObjectOfType<SparklyItem>();
            UnityEngine.Object.Destroy(item.gameObject);
        }
    }
}