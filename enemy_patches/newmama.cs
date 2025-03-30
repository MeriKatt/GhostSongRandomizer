
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
    public class NewMamaPatch : MonoBehaviour
    {

        [HarmonyPatch(typeof(NewMama), nameof(NewMama.Die))]
        [HarmonyPrefix]
        public static bool Die(NewMama __instance)
        {
            AchievementsManager.UnlockAchievement("en05");
            global.statstat.achman.achievementsList[13] = 1;
            SaveDataManager.SetList<int>("achievementslist", global.statstat.achman.achievementsList);
            SaveDataManager.Write(true);
            global.statstat.gsfx.PlayBossDeathChime(__instance.transform.position, 0f);
            UnityEngine.Object.Instantiate<GameObject>(__instance.bloodycloud, new Vector2(__instance.transform.position.x, __instance.transform.position.y + 5f), Quaternion.identity);
            global.statstat.buzz(0.7f, 0.7f, 60);
            global.statstat.shake(0.18f, 60);
            __instance.gibshead.Go(2f);
            if (__instance.dc.elbydistance.y < 5f && __instance.dc.elbydistance.x < 5f)
            {
                float num = 0.5f - __instance.dc.elbydistance.x / 10f;
                if (num < 0.05f)
                {
                    num = 0.05f;
                }
                global.elby.GetComponent<playermovement>().Blooded(num, __instance.dc.bloodColor2);
                global.elby.GetComponent<lbflash>().RollFlash(__instance.dc.bloodColor2, 0.5f, 0.05f);
            }
            int i = 14;
            if (global.reducedDetailMode)
            {
                i = 5;
            }
            while (i > 0)
            {
                __instance.spawnblood();
                i--;
            }
            CheckerComponent.newSpawnLoot("newmama", __instance.gameObject);
            __instance.spawndoll();
            UnityEngine.Object.Instantiate<GameObject>(__instance.debrisparts1, new Vector2(__instance.transform.position.x, __instance.transform.position.y + 5f), Quaternion.identity);
            __instance.dc.EnemyDeath();
            UnityEngine.Object.Destroy(__instance.gameObject, 0f);
            return false;
        }
    }
}