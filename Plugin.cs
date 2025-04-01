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
using System.Collections;
using System.Threading;
using System.Net.NetworkInformation;
using Cpp2IL.Core.Extensions;
using Il2CppInterop.Runtime.Injection;
using HarmonyLib;
using Il2CppSystem.Runtime.Remoting.Metadata;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Immutable;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Il2CppInterop.Generator.Extensions;
using Il2CppSystem.Security.Util;
using System.Net;



namespace Randomizer;

[DataContract]
public class RandomizerLocations {
    [DataMember(IsRequired = true)]
    public List<RandomizerLocationInfo> locations { get; set; }
    public RandomizerLocations() {
        this.locations = new List<RandomizerLocationInfo>();
    }
}

[DataContract]
public class RandomizerLocationInfo
{
    [DataMember]
    public string scene_name { get; set; }
    [DataMember]
    public string item_number { get; set; }
    [DataMember]
    public string is_item { get; set; }
    [DataMember]
    public string is_module { get; set; }
    [DataMember]
    public string module_number { get; set; }
    [DataMember]
    public string module_type { get; set; }
    [DataMember]
    public string array_number { get; set; }
    [DataMember]
    public string item_position { get; set; }


    public static string toJson(RandomizerLocationInfo info) {

        return $$"""
            {
                "scene_name": "{{info.scene_name}}",
                "item_number": "{{info.item_number}}",
                "is_item": "{{info.is_item}}",
                "is_module": "{{info.is_module}}",
                "module_number": "{{info.module_number}}",
                "module_type": "{{info.module_type}}",
                "array_number": "{{info.array_number}}",
                "item_position": "{{info.item_position}}"
            
            }
        
        """;
    }

    /*public static void updateItemNum(string ObjName, string scene_name,
    int itemNumber,
    bool Item,
    bool Module,
    int moduleNumber,
    string moduleType,
    int arrayNumber,
    Vector2 position)
    {
        Plugin.location_number += 1;
        SaveDataManager.Set<int>(Plugin.nameOfVariable, Plugin.location_number );
        System.Console.WriteLine(Plugin.location_number);
        RandomizerLocationInfo info = new RandomizerLocationInfo();
        info.scene_name = scene_name;
        info.item_number = itemNumber;
        info.is_item = Item;
        info.is_module = Module;
        info.module_number = moduleNumber;
        info.module_type = moduleType;
        info.array_number = arrayNumber;
        info.item_position = position.ToString();
        System.Console.WriteLine(info.scene_name);
        string directory = "./locations/" + Plugin.location_number + "/";
        string fileName = directory + ObjName+".json";
        System.Console.WriteLine(fileName);
        Directory.CreateDirectory(directory);
        string json = RandomizerLocationInfo.toJson(info);
        System.Console.WriteLine(json);
        File.WriteAllText(@""+fileName, json);
        System.Console.WriteLine(File.ReadAllText(fileName));
    }*/
}


[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{

    public static RandomizerLocations AllLocations = new RandomizerLocations();
    List<GameObject> objects = new List<GameObject>();
    status status;
    internal static new ManualLogSource Log;
    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RandomizerLocations));

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
            item.scene = scene.name;
            item.wasCollected = RandomizerItem.loadFromFile(item.Name);
            item.item.dead = item.wasCollected;
            go.transform.position = pos;
            item.item.transform.position = pos;
            SceneManager.MoveGameObjectToScene(go, scene);
        }
    }
    public static bool isItemCloseEnough(Vector2 v1, Vector2 v2){
            return v1.x - v2.x < 1 && v1.x - v2.x > -1 && v1.y - v2.y < 1 && v1.y - v2.y > -1;
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
        SparklyItem[] items = GameObject.FindObjectsOfType<SparklyItem>();
        foreach (SparklyItem item in items) {
            if (item.isActiveAndEnabled) {
                foreach( RandomizerLocation loc in CheckerComponent.locations) {
                    if (isItemCloseEnough(loc.position, (Vector2)item.transform.position)){
                        spawnRandomizerItem(loc.name, scene, new Vector3(loc.position.x, loc.position.y, 0));
                        System.Console.WriteLine("Location found, spawning it");
                        UnityEngine.Object.Destroy(item.gameObject);
                        break;
                    }
                }
            }

        }

        /*if (scene.name == "doopy3") {
            spawnRandomizerItem("NewTestItem", scene, new Vector3(106.8151f, 8.6966f, 0));
        }*/
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
    public static Vector2 strToVec2(string text)
    {
        string[] words = text.Split(",");
        string first = words[0];
        first = first.Remove(0,1);
        string second = words[1];
        second = second.Remove(second.Length -1);
        Vector2 vec = new Vector2(float.Parse(first), float.Parse(second));
        return vec;
    }
    public override void Load()
    {
        Plugin.AllLocations = new RandomizerLocations();
        string input = File.ReadAllText("./locations/AllLocations.json");
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(input)))
        {
            Plugin.AllLocations = (RandomizerLocations)ser.ReadObject(ms);
        }
        foreach(RandomizerLocationInfo info in Plugin.AllLocations.locations) {
            CheckerComponent.locations.Add(new RandomizerLocation().setName(Plugin.AllLocations.locations.IndexOf(info).ToString() + "/"+info.scene_name)
            .setItem(info.is_item == "True")
            .setItemNum(int.Parse(info.item_number)).setArray(int.Parse(info.array_number)).setModule(info.is_module == "True")
            .setModType(int.Parse(info.module_type)).setModuleNum(int.Parse(info.module_number)).setPosition(strToVec2(info.item_position)));
            System.Console.WriteLine("Name of added:  " +CheckerComponent.locations.ElementAt(CheckerComponent.locations.Count - 1).name);
        }
        
        // Plugin startup logic
        Log = base.Log;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        SceneManager.sceneLoaded += (UnityAction<Scene, LoadSceneMode>) onSceneLoaded;
        try
        {
            ClassInjector.RegisterTypeInIl2Cpp<CheckerComponent>();
            ClassInjector.RegisterTypeInIl2Cpp<RandomizerItem>();
            ClassInjector.RegisterTypeInIl2Cpp<RandomizerLocation>();
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
                harmony.PatchAll(typeof(Randomizer.bigheadboss_Apply_Patch));
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

