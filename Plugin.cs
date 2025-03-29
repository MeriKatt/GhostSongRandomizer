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
using Il2CppInterop.Runtime.Injection;
using HarmonyLib;
using Il2CppSystem.Runtime.Remoting.Metadata;


namespace Randomizer;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{

    List<GameObject> objects = new List<GameObject>();
    internal static new ManualLogSource Log;

    private void onSceneLoaded(Scene scene, LoadSceneMode mode) {
        GameObject versionnumber = GameObject.Find("Camera Holder");
        if (versionnumber != null) {
            Transform child = versionnumber.transform.Find("VersionNumber");
            if (child != null) {
                GameObject obj = child.gameObject;
                if (obj != null ) {
                    obj.SetActive(true);
                    obj.GetComponent<TextMesh>().text = scene.name;
                }
            }
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
            objects.Add(obj);
        }
    }
    public override void Load()
    {
        // Plugin startup logic

        
        Log = base.Log;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        SceneManager.sceneLoaded += (UnityAction<Scene, LoadSceneMode>) onSceneLoaded;
        try
        {
            ClassInjector.RegisterTypeInIl2Cpp<CheckerComponent>();
            var go = new GameObject("CheckerObject");
            go.AddComponent<CheckerComponent>();
            UnityEngine.Object.DontDestroyOnLoad(go);
        }
        catch
        {
            Log.LogError("[RANDOMIZER] Failed to Register Il2Cpp Type: CheckerComponent");
        }
        try 
        {
            var harmony = new Harmony(Randomizer.MyPluginInfo.PLUGIN_GUID);
            try
            {
                harmony.PatchAll(typeof(Randomizer.CheckerComponent));
            }
            catch (System.Exception e)
            {
                Log.LogError(e);
            }


        }
        catch (System.Exception e)
        {
            Log.LogError($"[Randomizer] Harmony - FAILEd to Apply Patch's: {e}");
        }
    }
 
    
}

