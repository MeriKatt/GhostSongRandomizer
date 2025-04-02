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
    public class RandomizerItem : MonoBehaviour
    {
        public bool wasCollected;
        public SparklyItem item;

        public string scene;

        public Vector2 pos;

        public string Name;

        public static void saveToFile(bool wasCollected, string Name) {
            SaveDataManager.Set<bool>(Name + "_wasCollected_"+global.statstat.SaveSlot, wasCollected);
        }
        public static bool loadFromFile(string Name) {
            return SaveDataManager.Get<bool>(Name + "_wasCollected_"+global.statstat.SaveSlot);
        }
    }
}
