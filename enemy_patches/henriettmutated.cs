
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
    public class HenriettMutatedPatch : MonoBehaviour
    {

        [HarmonyPatch(typeof(HenriettMutated), nameof(HenriettMutated.Die))]
        [HarmonyPrefix]
        public static bool Die(HenriettMutated __instance)
        {
            AchievementsManager.UnlockAchievement("en03");
            global.statstat.achman.achievementsList[11] = 1;
            SaveDataManager.SetList<int>("achievementslist", global.statstat.achman.achievementsList);
            SaveDataManager.Write(true);
            global.statstat.cameraholder.GetComponent<smoothcamerafollow>().specialTarget = null;
            global.statstat.gsfx.PlayBossDeathChime(__instance.transform.position, 0f);
            __instance.fgc.OpenGates();
            __instance.fightsource.Stop();
            __instance.fightsource.clip = __instance.fightsong[1];
            __instance.fightsource.volume = 0.5f;
            __instance.fightsource.loop = false;
            __instance.fightsource.Play();
            __instance.SpawnDoll();
            RandomizerLocation loc = CheckerComponent.locations.Find((RandomizerLocation _loc) => {
                return _loc.name == "henriettmutated";
            });
            CheckerComponent.spawnloot(__instance.gameObject, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, loc.modType);
 
            allfx afx = global.afx;
            float num = 0f;
            UnityEngine.Object.Instantiate<GameObject>(afx.newsplosion, new Vector2(__instance.dc.bulletpos.x + num, __instance.dc.bulletpos.y), Quaternion.identity);
            global.statstat.bulletPools.SparkBurstAt(new Vector2(__instance.dc.bulletpos.x, __instance.dc.bulletpos.y), 0.1f, 0.7f, 4f, 30f, 35f, 0f);
            if (__instance.dc.elbydistance.y < 5f && __instance.dc.elbydistance.x < 5f)
            {
                global.elby.GetComponent<playermovement>().Blooded(0.9f, __instance.dc.bloodColor2);
                global.elby.GetComponent<lbflash>().RollFlash(__instance.dc.bloodColor2, 0.5f, 0.05f);
            }
            UnityEngine.Object.Instantiate<GameObject>(__instance.deathglow, new Vector2(__instance.transform.position.x + 0.2f, __instance.transform.position.y + 0.14f), Quaternion.identity).GetComponent<deathglow>().LetsGo(new Color(0.7f, 0.04f, 0.04f, 1f));
            global.statstat.gsfx.PlaySplat(__instance.transform.position, 0.6f, 0.95f, 0.1f, 1f);
            global.statstat.gsfx.PlayAnySoundDelayed(__instance.mutationsounds[0], __instance.transform.position, 0.6f, 1f, 0f, 1f, 0.1f);
            __instance.dc.EnemyDeath();
            for (int i = 12; i > 0; i--)
            {
                __instance.spawnblood();
            }
            __instance.gibs_Chest.Go(__instance.dc.DeathForce);
            __instance.gibs_Chest2.Go(__instance.dc.DeathForce);
            UnityEngine.Object.Destroy(__instance.gameObject, 0f);
            return false;
        }
    }
}