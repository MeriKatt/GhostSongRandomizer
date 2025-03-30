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

    public class RandomizerLocation: MonoBehaviour
    {
        public new string name;
        public bool Module;
        public bool Item;
        public int arrayNumber;
        public int itemNumber;
        public int moduleNumber;
        public SparklyItem.modtype modType;

        public RandomizerLocation setName(string name){
            this.name = name;
            return this;
        }
        public RandomizerLocation setModule(bool mod) {
            this.Module = mod;
            return this;
        }

        public RandomizerLocation setItem(bool item) {
            this.Item = item;
            return this;
        }

        public RandomizerLocation setArray(int arry) {
            this.arrayNumber = arry;
            return this;
        }

        public RandomizerLocation setItemNum(int num) {
            this.itemNumber = num;
            return this;
        }

        public RandomizerLocation setModuleNum(int num) {
            this.moduleNumber = num;
            return this;
        }

        public RandomizerLocation setModType(SparklyItem.modtype mod) {
            this.modType = mod;
            return this;
        }

    }

    public class CheckerComponent : MonoBehaviour
    {

        public static List<RandomizerLocation> locations = new List<RandomizerLocation>() {
            new RandomizerLocation().setName("surfacehuntergirl").setItem(true).setItemNum(0).setArray(0)
            .setModule(false).setModuleNum(0).setModType(SparklyItem.modtype.Special),
            new RandomizerLocation().setName("devilwink").setItem(false).setItemNum(0).setArray(94)
            .setModule(true).setModuleNum(12).setModType(SparklyItem.modtype.Modifier),
            new RandomizerLocation().setName("brad").setItem(false).setItemNum(0).setArray(88).setModule(true)
            .setModType(SparklyItem.modtype.Weapons).setModuleNum(3),
            new RandomizerLocation().setName("alienblob").setItem(false).setItemNum(0).setArray(92).setModule(true)
            .setModType(SparklyItem.modtype.Weapons).setModuleNum(14),
            new RandomizerLocation().setName("bigheadboss").setItem(false).setItemNum(0).setArray(84).setModule(true)
            .setModType(SparklyItem.modtype.Modifier).setModuleNum(6),
            new RandomizerLocation().setName("fistomini").setItem(false).setItemNum(0).setArray(84).setModule(true)
            .setModType(SparklyItem.modtype.Modifier).setModuleNum(4),
            new RandomizerLocation().setName("fistorandom1").setItem(false).setItemNum(0).setArray(84).setModule(true)
            .setModType(SparklyItem.modtype.Modifier).setModuleNum(15),
            new RandomizerLocation().setName("fistorandom2").setItem(false).setItemNum(0).setArray(84).setModule(true)
            .setModType(SparklyItem.modtype.Special).setModuleNum(1),
            new RandomizerLocation().setName("flailboss").setItem(false).setItemNum(0).setArray(15).setModule(true)
            .setModType(SparklyItem.modtype.Modifier).setModuleNum(11),
            new RandomizerLocation().setName("henriettmutated").setItem(false).setItemNum(0).setArray(90).setModule(true)
            .setModType(SparklyItem.modtype.Special).setModuleNum(4),
            new RandomizerLocation().setName("newmama").setItem(false).setItemNum(0).setArray(34).setModule(true)
            .setModType(SparklyItem.modtype.Special).setModuleNum(2),
            new RandomizerLocation().setName("newspikemouth1").setItem(false).setItemNum(0).setArray(34).setModule(true)
            .setModType(SparklyItem.modtype.Modifier).setModuleNum(15),
            new RandomizerLocation().setName("newspikemouth2").setItem(false).setItemNum(0).setArray(34).setModule(true)
            .setModType(SparklyItem.modtype.Special).setModuleNum(1),
            new RandomizerLocation().setName("phz").setItem(false).setItemNum(0).setArray(93).setModule(true)
            .setModType(SparklyItem.modtype.Weapons).setModuleNum(11),
            new RandomizerLocation().setName("warrior1").setItem(false).setItemNum(0).setArray(93).setModule(true)
            .setModType(SparklyItem.modtype.Special).setModuleNum(7),
            new RandomizerLocation().setName("warrior2").setItem(false).setItemNum(0).setArray(93).setModule(true)
            .setModType(SparklyItem.modtype.Special).setModuleNum(8),
            new RandomizerLocation().setName("worker").setItem(false).setItemNum(0).setArray(93).setModule(true)
            .setModType(SparklyItem.modtype.Special).setModuleNum(6),
            new RandomizerLocation().setName("slimedude").setItem(false).setItemNum(0).setArray(91).setModule(true)
            .setModType(SparklyItem.modtype.Weapons).setModuleNum(13),
            new RandomizerLocation().setName("spikerunner").setItem(false).setItemNum(0).setArray(22).setModule(true)
            .setModType(SparklyItem.modtype.Weapons).setModuleNum(12),
        };

        public static void newSpawnLoot(string name, GameObject instance) {
            RandomizerLocation loc = CheckerComponent.locations.Find((RandomizerLocation _loc) => {
                return _loc.name == name;
            });
            CheckerComponent.spawnloot(instance, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, loc.modType);

        }

        public static void spawnloot(GameObject inst, bool Module, bool Item, int arrayNumber, int itemNumber, int moduleNumber, SparklyItem.modtype moduleType)
        {
            Vector2 v2 = new Vector2(inst.transform.position.x, inst.transform.position.y + 3f);
            SparklyItem sparklyItem2 = UnityEngine.Object.Instantiate<SparklyItem>(global.afx.itemdrop, v2, Quaternion.identity);
            sparklyItem2.wasdropped = true;
            sparklyItem2.Item = Item;
            sparklyItem2.Module = Module;
            sparklyItem2.arrayNumber = arrayNumber;
            sparklyItem2.moduleNumber = moduleNumber;
            sparklyItem2.moduleType = moduleType;
            sparklyItem2.itemNumber = itemNumber;
        }
    }

}
