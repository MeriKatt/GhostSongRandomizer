
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
using System.Collections;

namespace Randomizer
{
    public class BigHeadBossPatch : MonoBehaviour
    {

        [HarmonyPatch(typeof(bigheadboss), nameof(bigheadboss.Die))]
        [HarmonyPrefix]
        public static IEnumerator Die(bigheadboss __instance)
        {
            AchievementsManager.UnlockAchievement("en06");
            global.statstat.achman.achievementsList[14] = 1;
            SaveDataManager.SetList<int>("achievementslist", global.statstat.achman.achievementsList);
            SaveDataManager.Write(true);
            global.statstat.cameraholder.GetComponent<smoothcamerafollow>().specialTarget = null;
            global.statstat.gsfx.PlayBossDeathChime(__instance.transform.position, 0f);
            __instance.jp3.endmusic();
            UnityEngine.Object.Instantiate<GameObject>(__instance.sparkparts, new Vector2(__instance.transform.position.x, __instance.transform.position.y + 6f), Quaternion.identity);
            UnityEngine.Object.Instantiate<GameObject>(__instance.redpartsdebris, new Vector2(__instance.transform.position.x, __instance.transform.position.y + 6f), Quaternion.identity);
            __instance.StartCoroutine(__instance.chainsplode(0.2f));
            __instance.brakes = true;
            Collider2D[] array = __instance.thecols;
            for (int i = 0; i < array.Length; i++)
            {
                array[i].enabled = false;
            }
            __instance.dc.EnemyDeath();
            __instance.push3 = false;
            __instance.push2 = false;
            __instance.push1 = false;
            __instance.StartCoroutine(__instance.chainsplode(0.35f));
            yield return new WaitForSeconds(0.7f);
            UnityEngine.Object.Instantiate<GameObject>(__instance.redpartsdebris, new Vector2(__instance.transform.position.x, __instance.transform.position.y + 6f), Quaternion.identity);
            __instance.StartCoroutine(__instance.chainsplode(0.2f));
            __instance.StartCoroutine(__instance.chainsplode(0.25f));
            yield return new WaitForSeconds(0.7f);
            UnityEngine.Object.Instantiate<GameObject>(__instance.redpartsdebris, new Vector2(__instance.transform.position.x, __instance.transform.position.y + 6f), Quaternion.identity);
            __instance.StartCoroutine(__instance.chainsplode(0.2f));
            UnityEngine.Object.Instantiate<GameObject>(__instance.sparkparts, new Vector2(__instance.transform.position.x, __instance.transform.position.y + 6f), Quaternion.identity);
            UnityEngine.Object.Instantiate<GameObject>(__instance.thejunkbotdebris, new Vector2(__instance.transform.position.x, __instance.transform.position.y + 4f), Quaternion.identity);
            __instance.thecorpse.transform.parent = null;
            __instance.thecorpse.SetActive(true);
            __instance.fgc.OpenGates();
            __instance.bhfr.fightover = true;
            global.statstat.thedata.OtherData[52] = 1;
            RandomizerLocation loc = CheckerComponent.locations.Find((RandomizerLocation _loc) => {
                return _loc.name == "bigheadboss";
            });
            CheckerComponent.spawnloot(__instance.gameObject, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, loc.modType);
 
            UnityEngine.Object.Destroy(__instance.gameObject, 0f);
            yield break;
        }
    }
}