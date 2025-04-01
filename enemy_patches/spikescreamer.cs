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
    public class SpikeScreamerPatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(spikescreamer), nameof(spikescreamer.SpawnRandomLoot))]
        [HarmonyPostfix]
        public static void SpawnRandomLoot(spikescreamer __instance)
        {
            SparklyItem item = GameObject.FindObjectOfType<SparklyItem>();
            UnityEngine.Object.Destroy(item.gameObject);
        }
    }
}