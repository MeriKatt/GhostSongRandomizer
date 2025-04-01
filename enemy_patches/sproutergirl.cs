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
    public class SprouterPatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(sproutergirl), nameof(sproutergirl.SpawnLoot))]
        [HarmonyPostfix]
        public static void SpawnLoot(sproutergirl __instance)
        {
            SparklyItem item = GameObject.FindObjectOfType<SparklyItem>();
            UnityEngine.Object.Destroy(item.gameObject);
            GameObject elby = global.elby;
            RandomizerLocation loc = CheckerComponent.locations.Find((RandomizerLocation _loc) => {
                return _loc.name == "sproutergirl";
            });
            CheckerComponent.spawnloot(elby, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, loc.modType);
        }
    }
}