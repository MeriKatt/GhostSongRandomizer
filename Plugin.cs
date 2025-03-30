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
using System.Linq;


namespace Randomizer;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{

    List<GameObject> objects = new List<GameObject>();
    status status;
    internal static new ManualLogSource Log;


    private void spawnRandomizerItem(string Name, Scene scene, Vector3 pos) {
        if (RandomizerItem.loadFromFile(Name) == false)
        {
            var loc = CheckerComponent.locations.Find((RandomizerLocation loc) => {
                return loc.name == Name;
            });
            var go = new GameObject("RandomizerItem");
            go.AddComponent<RandomizerItem>();
            RandomizerItem item = go.GetComponent<RandomizerItem>();
                    //new Vector3(106.8151f, 10.6966f, 0);
            item.Name = Name;
            item.item = UnityEngine.Object.Instantiate<SparklyItem>(global.afx.itemdrop);
            item.item.transform.parent = go.transform;
            item.item.itemNumber = loc.itemNumber;
            item.item.Item = loc.Item;
            item.item.Module = loc.Module;
            item.item.moduleNumber = loc.moduleNumber;
            item.item.arrayNumber = loc.arrayNumber;
            item.item.moduleType = loc.modType;
            item.pos = pos;
            item.pos.y += 3f;
            item.scene = scene.name;
            item.wasCollected = RandomizerItem.loadFromFile(item.Name);
            item.item.dead = item.wasCollected;
            go.transform.position = pos;
            item.item.transform.position = pos;
        }
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode) {
        status = Component.FindObjectOfType<status>();
        if (status is not null) {
            if (status.npcvd is not null ) {
                vendordata item = status.npcvd.BillInventory[1];
                if (item.ItemName != "Water Movement") {
                    item.ItemName = "Water Movement";
                    item.ItemNumber = 20;
                    item.ReqWorldState = 0;
                    item.Cost = 1;
                    item.Stock = 1;
                }
                status.npcvd.BillInventory[1] = item;
            }
        }        
        if (scene.name == "doopy3") {
            spawnRandomizerItem("NewTestItem", scene, new Vector3(106.8151f, 8.6966f, 0));
        }
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
            ClassInjector.RegisterTypeInIl2Cpp<RandomizerItem>();
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
                harmony.PatchAll(typeof(Randomizer.AlienBlobPath));
                //harmony.PatchAll(typeof(Randomizer.BigHeadBossPatch));
                harmony.PatchAll(typeof(Randomizer.BradPatch));
                harmony.PatchAll(typeof(Randomizer.CheckerComponent));
                harmony.PatchAll(typeof(Randomizer.DevilWinkPatch));
                harmony.PatchAll(typeof(Randomizer.FistoPatch));
                harmony.PatchAll(typeof(Randomizer.FlailBossPatch));
                harmony.PatchAll(typeof(Randomizer.HenriettMutatedPatch));
                harmony.PatchAll(typeof(Randomizer.NewMamaPatch));
                harmony.PatchAll(typeof(Randomizer.NewSpikeMouthPatch));
                harmony.PatchAll(typeof(Randomizer.PhzPatch));
                harmony.PatchAll(typeof(Randomizer.Plugin));
                harmony.PatchAll(typeof(Randomizer.SlimeDudePatch));
                harmony.PatchAll(typeof(Randomizer.SpikeRunnerPatch));
                harmony.PatchAll(typeof(Randomizer.SurfaceHunterPatch));
                harmony.PatchAll(typeof(Randomizer.WarriorPatch));
                harmony.PatchAll(typeof(Randomizer.WorkerPatch));
                harmony.PatchAll(typeof(Randomizer.SparklyItemPatch));
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

