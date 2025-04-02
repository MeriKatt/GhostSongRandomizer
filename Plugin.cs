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
using System.Reflection;
using MonoMod.Utils;
using Random = System.Random;



namespace Randomizer;

[DataContract]
public class RandomizerLocations {
    [DataMember(IsRequired = true)]
    public List<BRandomizerLocationInfo> locations { get; set; }
    public RandomizerLocations() {
        this.locations = new List<BRandomizerLocationInfo>();
    }
}


[DataContract]
public class BRandomizerLocationInfo
{
    [DataMember]
    public string Name { get; set; }
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
}

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    public static bool shouldRandomize = false;

    public static Randomizer.RandomzierLayout layout = new RandomzierLayout();
    internal static new ManualLogSource Log;
    public static DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RandomzierLayout));

    public static void spawnRandomizerItem(string Name, Scene scene, Vector3 pos, RandomizerItemInfo loc) {
        if (RandomizerItem.loadFromFile(Name) == false)
        {
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
            item.item.moduleType = layout.GetModtypeFromString(loc.modType);
            item.pos = pos;
            item.scene = scene.name;
            item.wasCollected = RandomizerItem.loadFromFile(item.Name);
            item.item.dead = item.wasCollected;
            go.transform.position = pos;
            item.item.transform.position = pos;
            SceneManager.MoveGameObjectToScene(go, scene);
        }
    }
    private static  void onSceneLoaded(Scene scene, LoadSceneMode mode) {
        SparklyItem[] items = GameObject.FindObjectsOfType<SparklyItem>();
        foreach (SparklyItem item in items) {
            if (item.gameObject.GetComponentsInParent<RandomizerItem>(false).Length == 0){
                UnityEngine.Object.Destroy(item.gameObject);
            }
        }
        List<RandomizerItemInfo> RandomizerItems = layout.GetRoomItems(scene.name);
        if (RandomizerItems != null) {
            foreach( RandomizerItemInfo loc in RandomizerItems) 
            {
                Vector2 pos = Plugin.strToVec2(loc.position);
                spawnRandomizerItem(loc.Name, scene, new Vector3(pos.x, pos.y +1.5f, 0), loc);
                System.Console.WriteLine("Location found, spawning it"); 
            }
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
    public static System.Random rng = new Random();  

    public static List<RandomizerItemBase> Shuffle(List<RandomizerItemBase> list)  
    {  
        for (int i = list.Count - 1; i > 0; i --){
            var k =  rng.Next(i+1);
            var value = list[k];
            list[k] = list[i];
            list[i] = value;
        }
        return list;
    }
    public static void Randomize() 
    {
        Randomizer.RandomzierLayout New = new RandomzierLayout() {
            Rooms = new List<Room>(),
            DialogueItems = new List<Dialogue>(),
            EnemyDrops = new List<Enemy>(),
            Vendors = new List<Vendor>()
        };
        List<RandomizerItemBase> items = new List<RandomizerItemBase>();
        foreach(Room room in Plugin.layout.Rooms) {
            foreach(RandomizerItemInfo item in Plugin.layout.GetRoomItems(room.Name)){
                items.Add(RandomizerItemBase.From(item));
            }
        }
        foreach(Vendor ven in Plugin.layout.Vendors) {
            foreach (VendorItem item in ven.Items) {
                items.Add(RandomizerItemBase.From(item));
            }
        }
        foreach(Enemy enmy in Plugin.layout.EnemyDrops) {
            items.Add(enmy.Item);
        }
        foreach(Dialogue dial in Plugin.layout.DialogueItems) {
            items.Add(dial.Item);
        }


        items = Shuffle(items);


        int i = 0;
        int roomInd = 0;
        foreach(Room room in Plugin.layout.Rooms) {
            New.Rooms.Add(new Room() {Name = room.Name, Items = new List<RandomizerItemInfo>()});
            foreach(RandomizerItemInfo item in Plugin.layout.GetRoomItems(room.Name)){
                RandomizerItemInfo itm = room.Items.Find((RandomizerItemInfo inf) => inf.Name == item.Name); 
                itm = RandomizerItemInfo.From(items[i], item.Name, item.position);
                New.Rooms[roomInd].Items.Add(itm);
                i +=1;
            }
            roomInd += 1;
        }
        int venInd = 0;
        foreach(Vendor ven in Plugin.layout.Vendors) {
            New.Vendors.Add(new Vendor() {Name = ven.Name, Items = new List<VendorItem>()});
            foreach (VendorItem item in ven.Items) {
                VendorItem itm = new VendorItem();
                itm = VendorItem.From(items[i]);
                New.Vendors[venInd].Items.Add(itm);
                i += 1;
            }
            venInd += 1;
        }
        foreach(Enemy enmy in Plugin.layout.EnemyDrops) {
            New.EnemyDrops.Add(new Enemy(){Name = enmy.Name, Item = RandomizerItemBase.From(items[i])});
            i+=1;
        }
        foreach(Dialogue dial in Plugin.layout.DialogueItems) {
            New.DialogueItems.Add(new Dialogue(){Name = dial.Name, Item = RandomizerItemBase.From(items[i])});
            i+=1;
        }
        
        var output = string.Empty;
            using (var ms = new MemoryStream())
                {
                    ser.WriteObject(ms, New);
                    output = Encoding.UTF8.GetString(ms.ToArray());
    
                    // {"BirthDate":"\/Date(1468591293120+0300)\/","Name":"Fluffy","Tags":["black tail","green eyes"]}
                }
                File.WriteAllText("./locations/RandomizerSeed.json", output);

    }
    public override void Load()
    {
        Plugin.layout = new RandomzierLayout();
        if (File.Exists("./locations/RandomizerSeed.json")) {
            shouldRandomize = false;
            string input = File.ReadAllText("./locations/RandomizerSeed.json");
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(input)))
            {
                Plugin.layout = (RandomzierLayout)ser.ReadObject(ms);
            }
        } else {
            shouldRandomize = true;
            string input = File.ReadAllText("./locations/DefaultLayout.json");
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(input)))
            {
                Plugin.layout = (RandomzierLayout)ser.ReadObject(ms);
            }
        }
        if (shouldRandomize) {
            Plugin.Randomize();
            Plugin.layout = new RandomzierLayout();
            string input = File.ReadAllText("./locations/RandomizerSeed.json");
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(input)))
            {
                Plugin.layout = (RandomzierLayout)ser.ReadObject(ms);
            }
        }
        /*
        var output = string.Empty;
        using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, Plugin.layout);
                output = Encoding.UTF8.GetString(ms.ToArray());
 
                // {"BirthDate":"\/Date(1468591293120+0300)\/","Name":"Fluffy","Tags":["black tail","green eyes"]}
            }
            File.WriteAllText("./locations/TestReadWrite.json", output);
        */

        /*foreach(vendordata data in global.statstat.npcvd.BillInventory)
        {
            int index = global.statstat.npcvd.BillInventory.IndexOf(data);
            VendorItem item = Plugin.layout.GetVendorItem("Bill", index);
            data.Cost = item.Cost;
            data.Classification = item.Class;
            data.Description = item.Desc;
            data.ItemName = item.Name;
            data.ItemNumber = item.ItemNumber;
            data.ModuleNumber = item.ModuleNumber;
            data.ModuleType = item.ModuleType;
            data.Stock = item.Stock;
            data.ReqWorldState = item.WorldState;
            global.statstat.npcvd.BillInventory[index] = data;
        }
        foreach(vendordata data in global.statstat.npcvd.JuliaInventory)
        {
            int index = global.statstat.npcvd.JuliaInventory.IndexOf(data);
            VendorItem item = Plugin.layout.GetVendorItem("Mabec", index);
            data.Cost = item.Cost;
            data.Classification = item.Class;
            data.Description = item.Desc;
            data.ItemName = item.Name;
            data.ItemNumber = item.ItemNumber;
            data.ModuleNumber = item.ModuleNumber;
            data.ModuleType = item.ModuleType;
            data.Stock = item.Stock;
            data.ReqWorldState = item.WorldState;
            global.statstat.npcvd.JuliaInventory[index] = data;
        }*/
        
        /*var ser_ = new DataContractJsonSerializer(typeof(Randomzier_Layout));
            var output = string.Empty;
 
            using (var ms = new MemoryStream())
            {
                ser_.WriteObject(ms, layout);
                output = Encoding.UTF8.GetString(ms.ToArray());
 
                // {"BirthDate":"\/Date(1468591293120+0300)\/","Name":"Fluffy","Tags":["black tail","green eyes"]}
            }
            File.WriteAllText("./locations/RoomsWithItems.json", output);
        */

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
                harmony.PatchAll(typeof(Randomizer.CoreBotPatch));
                harmony.PatchAll(typeof(Randomizer.SpikeScreamerPatch));
                harmony.PatchAll(typeof(Randomizer.SprouterPatch));
                harmony.PatchAll(typeof(Randomizer.Walker5Patch));
                harmony.PatchAll(typeof(Randomizer.LeafPatch));
                harmony.PatchAll(typeof(Randomizer.GamblerPatch));
                harmony.PatchAll(typeof(Randomizer.Gili5Patch));
                harmony.PatchAll(typeof(Randomizer.MollyPatch));
                harmony.PatchAll(typeof(Randomizer.RelicPatch));
                harmony.PatchAll(typeof(Randomizer.RimePatch));
                harmony.PatchAll(typeof(Randomizer.SavePatch));
                harmony.PatchAll(typeof(Randomizer.StatusPatch));
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

