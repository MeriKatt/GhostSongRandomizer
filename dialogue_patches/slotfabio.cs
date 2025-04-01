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
    public class SavePatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(slotsfabio), nameof(slotsfabio.fullydelete))]
        [HarmonyPrefix]
        public static bool fullydelete(slotsfabio __instance)
        {
            if (Plugin.layout.Rooms == null) return true;
            foreach(Room room in Plugin.layout.Rooms) {
                foreach(RandomizerItemInfo info in room.Items) {
                    if (info == null) return true;
                    if (SaveDataManager.Get<bool>(info.Name + "_wasCollected_"+__instance.row.ToString(), false)) {
                        SaveDataManager.Set<bool>(info.Name + "_wasCollected_"+__instance.row.ToString(), false);
                    }
                    
                }
            }
            return true;
        }
    }
}