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
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;

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
        public UnityEngine.Vector2 position;

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

        public RandomizerLocation setModType(int mod) {
            this.modType = (SparklyItem.modtype)mod;
            return this;
        }

        public RandomizerLocation setPosition(Vector2 vec) {
            this.position = vec;
            return this;
        }

    }

    public class CheckerComponent : MonoBehaviour
    {

        public static List<RandomizerLocation> locations = new List<RandomizerLocation>() {
            /*new RandomizerLocation().setName("surfacehuntergirl").setItem(true).setItemNum(0).setArray(0)
            .setModule(false).setModuleNum(0).setModType(2),
            new RandomizerLocation().setName("devilwink").setItem(false).setItemNum(0).setArray(94)
            .setModule(true).setModuleNum(12).setModType(1),
            new RandomizerLocation().setName("brad").setItem(false).setItemNum(0).setArray(88).setModule(true)
            .setModType(0).setModuleNum(3),
            new RandomizerLocation().setName("alienblob").setItem(false).setItemNum(0).setArray(92).setModule(true)
            .setModType(0).setModuleNum(14),
            new RandomizerLocation().setName("bigheadboss").setItem(false).setItemNum(0).setArray(84).setModule(true)
            .setModType(1).setModuleNum(16),
            new RandomizerLocation().setName("fistomini").setItem(false).setItemNum(0).setArray(84).setModule(true)
            .setModType(1).setModuleNum(4),
            new RandomizerLocation().setName("fistorandom1").setItem(false).setItemNum(0).setArray(84).setModule(true)
            .setModType(1).setModuleNum(15),
            new RandomizerLocation().setName("fistorandom2").setItem(false).setItemNum(0).setArray(84).setModule(true)
            .setModType(2).setModuleNum(1),
            new RandomizerLocation().setName("flailboss").setItem(false).setItemNum(0).setArray(15).setModule(true)
            .setModType(1).setModuleNum(11),
            new RandomizerLocation().setName("henriettmutated").setItem(false).setItemNum(0).setArray(90).setModule(true)
            .setModType(2).setModuleNum(4),
            new RandomizerLocation().setName("newmama").setItem(false).setItemNum(0).setArray(34).setModule(true)
            .setModType(2).setModuleNum(2),
            new RandomizerLocation().setName("newspikemouth1").setItem(false).setItemNum(0).setArray(34).setModule(true)
            .setModType(1).setModuleNum(15),
            new RandomizerLocation().setName("newspikemouth2").setItem(false).setItemNum(0).setArray(34).setModule(true)
            .setModType(2).setModuleNum(1),
            new RandomizerLocation().setName("phz").setItem(false).setItemNum(0).setArray(93).setModule(true)
            .setModType(0).setModuleNum(11),
            new RandomizerLocation().setName("warrior1").setItem(false).setItemNum(0).setArray(93).setModule(true)
            .setModType(2).setModuleNum(7),
            new RandomizerLocation().setName("warrior2").setItem(false).setItemNum(0).setArray(93).setModule(true)
            .setModType(2).setModuleNum(8),
            new RandomizerLocation().setName("worker").setItem(false).setItemNum(0).setArray(93).setModule(true)
            .setModType(2).setModuleNum(6),
            new RandomizerLocation().setName("slimedude").setItem(false).setItemNum(0).setArray(91).setModule(true)
            .setModType(0).setModuleNum(13),
            new RandomizerLocation().setName("spikerunner").setItem(false).setItemNum(0).setArray(22).setModule(true)
            .setModType(0).setModuleNum(12),
            new RandomizerLocation().setName("Mabec0").setItem(false).setItemNum(0).setArray(22).setModule(true)
            .setModType(1).setModuleNum(2),
            new RandomizerLocation().setName("Mabec1").setItem(false).setItemNum(0).setArray(22).setModule(true)
            .setModType(1).setModuleNum(13),
            new RandomizerLocation().setName("Mabec2").setItem(false).setItemNum(0).setArray(22).setModule(true)
            .setModType(1).setModuleNum(17),
            new RandomizerLocation().setName("Mabec3").setItem(false).setItemNum(0).setArray(22).setModule(true)
            .setModType(0).setModuleNum(5),
            new RandomizerLocation().setName("Mabec4").setItem(true).setItemNum(26).setArray(22).setModule(false)
            .setModType(1).setModuleNum(2),
            new RandomizerLocation().setName("Mabec5").setItem(true).setItemNum(67).setArray(22).setModule(false)
            .setModType(1).setModuleNum(2),
            new RandomizerLocation().setName("Mabec6").setItem(false).setItemNum(26).setArray(22).setModule(true)
            .setModType(0).setModuleNum(4),
            new RandomizerLocation().setName("Bill0").setItem(true).setItemNum(1).setArray(22).setModule(false)
            .setModType(1).setModuleNum(2),
            new RandomizerLocation().setName("Bill1").setItem(true).setItemNum(31).setArray(22).setModule(false)
            .setModType(1).setModuleNum(13),
            new RandomizerLocation().setName("Bill2").setItem(false).setItemNum(0).setArray(22).setModule(true)
            .setModType(1).setModuleNum(16),
            new RandomizerLocation().setName("Bill3").setItem(true).setItemNum(39).setArray(22).setModule(false)
            .setModType(0).setModuleNum(5),
            new RandomizerLocation().setName("Bill4").setItem(true).setItemNum(40).setArray(22).setModule(false)
            .setModType(1).setModuleNum(2),
            new RandomizerLocation().setName("Bill5").setItem(true).setItemNum(41).setArray(22).setModule(false)
            .setModType(1).setModuleNum(2),
            new RandomizerLocation().setName("Bill6").setItem(true).setItemNum(7).setArray(22).setModule(false)
            .setModType(0).setModuleNum(4),
            new RandomizerLocation().setName("Bill7").setItem(true).setItemNum(29).setArray(22).setModule(false)
            .setModType(1).setModuleNum(2),
            new RandomizerLocation().setName("Bill8").setItem(false).setItemNum(67).setArray(22).setModule(true)
            .setModType(0).setModuleNum(2),
            new RandomizerLocation().setName("Bill9").setItem(false).setItemNum(26).setArray(22).setModule(true)
            .setModType(1).setModuleNum(7),
            new RandomizerLocation().setName("Gambler").setItem(true).setItemNum(53).setArray(22).setModule(false)
            .setModType(1).setModuleNum(7),
            new RandomizerLocation().setName("GiliWorld5").setItem(true).setItemNum(34).setArray(22).setModule(false)
            .setModType(1).setModuleNum(7),
            new RandomizerLocation().setName("MollyCamp3").setItem(false).setItemNum(53).setArray(22).setModule(true)
            .setModType(1).setModuleNum(8),
            new RandomizerLocation().setName("MollyGen").setItem(true).setItemNum(46).setArray(22).setModule(false)
            .setModType(1).setModuleNum(8),
            new RandomizerLocation().setName("Relic").setItem(true).setItemNum(48).setArray(22).setModule(false)
            .setModType(1).setModuleNum(8),
            new RandomizerLocation().setName("Rime").setItem(false).setItemNum(53).setArray(22).setModule(true)
            .setModType(1).setModuleNum(14),*/
        };

        public static void newSpawnLoot(string name, GameObject instance) {
            RandomizerItemBase loc = Plugin.layout.GetDialogueOrEnemy(name, "enemy");
            CheckerComponent.spawnloot(instance.gameObject, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, Plugin.layout.GetModtypeFromString(loc.modType));
 
        }

        public static void spawnloot(GameObject inst, bool Module, bool Item, int arrayNumber, int itemNumber, int moduleNumber, SparklyItem.modtype moduleType)
        {
            Vector2 v2 = new UnityEngine.Vector2(inst.transform.position.x, inst.transform.position.y + 3f);
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
