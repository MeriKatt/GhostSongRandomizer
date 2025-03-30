
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
    public class AlienBlobPath : MonoBehaviour
    {

        [HarmonyPatch(typeof(AlienBlob), nameof(AlienBlob.Die))]
        [HarmonyPrefix]
        public static bool Die(AlienBlob __instance)
        {
            AchievementsManager.UnlockAchievement("en08");
            global.statstat.achman.achievementsList[16] = 1;
            SaveDataManager.SetList<int>("achievementslist", global.statstat.achman.achievementsList);
            SaveDataManager.Write(true);
            RandomizerLocation loc = CheckerComponent.locations.Find((RandomizerLocation _loc) => {
                return _loc.name == "alienblob";
            });
            CheckerComponent.spawnloot(__instance.gameObject, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, loc.modType);
 
            global.statstat.gsfx.PlayBossDeathChime(__instance.transform.position, 0f);
            __instance.beanhit(170f, 6);
            __instance.beanhit(10f, 6);
            global.statstat.shake(0.4f, 15);
            global.statstat.buzz(0.4f, 0.4f, 20);
            for (int i = 2; i > 0; i--)
            {
                __instance.spawncloud();
            }
            allfx afx = global.afx;
            __instance.dc.EnemyDeath();
            gibholder[] array = __instance.gibholders;
            for (int j = 0; j < array.Length; j++)
            {
                array[j].Go(__instance.dc.DeathForce);
            }
            global.statstat.gsfx.PlaySplat(__instance.transform.position, 0.55f, 0.95f, 0.1f, 1f);
            global.statstat.gsfx.PlayAnySound(__instance.mutate, __instance.transform.position, 1f, 1f, 0f, 0.5f);
            __instance.spawndoll();
            UnityEngine.Object.Instantiate<GameObject>(afx.newsplosion, new Vector2(__instance.dc.bulletpos.x + 0f, __instance.dc.bulletpos.y), Quaternion.identity);
            global.statstat.bulletPools.SparkBurstAt(new Vector2(__instance.dc.bulletpos.x, __instance.dc.bulletpos.y), 0.1f, 0.7f, 4f, 30f, 35f, 0f);
            __instance.dc.SpawnBots(5, "small", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 5f), 0f, 0.8f);
            __instance.dc.SpawnBots(15, "medium", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 3f), 0f, 0.8f);
            for (int k = 15; k > 0; k--)
            {
                __instance.spawnblood();
            }
            UnityEngine.Object.Destroy(__instance.gameObject, 0f);
            return false;
        }
    }
}