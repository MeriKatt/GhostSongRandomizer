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
    public class BaseEnemyPatch : MonoBehaviour
    {

        [HarmonyPatch(typeof(devilwink), nameof(devilwink.die))]
        [HarmonyPrefix]
        public static bool die(devilwink __instance)
        {
        
            return false;
        }
    }
}