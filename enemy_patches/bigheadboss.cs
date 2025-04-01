
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
using HarmonyLib.Tools;
using System.Net;
using System.Collections;
using AsmResolver.PE.DotNet.ReadyToRun;
using System.Reflection.Emit;
using Il2CppSystem.Runtime.Remoting.Messaging;
using MonoMod.Utils;
using System.Reflection;

namespace Randomizer
{
    
    public class bigheadboss_Apply_Patch : MonoBehaviour
    {

        static void MySpawnLoot() {
            GameObject elby = global.elby;
            RandomizerLocation loc = CheckerComponent.locations.Find((RandomizerLocation _loc) => {
                return _loc.name == "bigheadboss";
            });
            CheckerComponent.spawnloot(elby, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, loc.modType);
        }

        [HarmonyPatch(typeof(bigheadboss), nameof(bigheadboss.SpawnLoot))]
        [HarmonyPostfix]
        static void SpawnLoot()
        {
           SparklyItem item = GameObject.FindObjectOfType<SparklyItem>();
           UnityEngine.Object.Destroy(item.gameObject);
           bigheadboss_Apply_Patch.MySpawnLoot();
        } 
    }
}