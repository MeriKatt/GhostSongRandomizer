
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
    public class FlailBossPatch : MonoBehaviour
    {

        [HarmonyPatch(typeof(FlailBoss), nameof(FlailBoss.Die))]
        [HarmonyPrefix]
        public static bool Die(FlailBoss __instance)
        {
            AchievementsManager.UnlockAchievement("en04");
            global.statstat.achman.achievementsList[12] = 1;
            SaveDataManager.SetList<int>("achievementslist", global.statstat.achman.achievementsList);
            SaveDataManager.Write(true);
            global.statstat.cameraholder.GetComponent<smoothcamerafollow>().specialTarget = null;
            global.statstat.gsfx.PlayBossDeathChime(__instance.transform.position, 0f);
            __instance.StartCoroutine(__instance.endmusic());
            Vector2 vector = __instance.smokesystems[0].transform.position;
            UnityEngine.Object.Instantiate<GameObject>(__instance.coolsplosion2, new Vector2(vector.x + 0f, vector.y + 0f), Quaternion.identity);
            __instance.StartCoroutine(__instance.csplode());
            AudioClip clip = global.statstat.gsfx.headpop[0];
            AudioClip clip2 = global.statstat.gsfx.shattersplode[0];
            global.statstat.gsfx.PlayAnySoundDelayed(clip2, __instance.transform.position, 1f, 1f, 0.2f, 0.6f, 0.25f);
            global.statstat.gsfx.PlayAnySoundDelayed(clip2, __instance.transform.position, 1f, 1f, 0.2f, 0.6f, 0.55f);
            global.statstat.gsfx.PlayAnySoundDelayed(clip2, __instance.transform.position, 1f, 1f, 0.2f, 0.6f, 0.66f);
            global.statstat.gsfx.PlayAnySound(clip, __instance.transform.position, 0.5f, 0.9f, 0f, 1f);
            global.statstat.gsfx.PlayAnySoundDelayed(clip2, __instance.transform.position, 0.75f, 1f, 0f, 1f, 0.1f);
            __instance.StartCoroutine(__instance.flipsmokems(false, 5f));
            RandomizerItemBase loc = Plugin.layout.GetDialogueOrEnemy("flailboss", "enemy");
            CheckerComponent.spawnloot(__instance.gameObject, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, Plugin.layout.GetModtypeFromString(loc.modType));
 
            __instance.dc.EnemyDeath();
            Collider2D[] array = __instance.allcols;
            for (int i = 0; i < array.Length; i++)
            {
                array[i].enabled = false;
            }
            __instance.anim.Play("die", 0, 0f);
            return false;
        }
    }
}