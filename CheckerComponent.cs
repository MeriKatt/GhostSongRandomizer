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

namespace Randomizer
{
    public class CheckerComponent : MonoBehaviour
    {

        [HarmonyPatch(typeof(surfacehuntergirl), nameof(surfacehuntergirl.Die))]
        [HarmonyPrefix]
        public static bool Die(surfacehuntergirl __instance)
        {
            global.statstat.gsfx.PlayBossDeathChime(__instance.transform.position, 0f);
            __instance.dead = true;

            Vector2 v = new Vector2(__instance.transform.position.x, __instance.transform.position.y + 3f);
            SparklyItem sparklyItem = UnityEngine.Object.Instantiate<SparklyItem>(global.afx.itemdrop, v, Quaternion.identity);
            sparklyItem.Item = false;
            sparklyItem.Module = true;
            sparklyItem.moduleNumber = 12;
            sparklyItem.arrayNumber = 0;
            sparklyItem.moduleType = SparklyItem.modtype.Weapons;


            __instance.gibHolder.Go(__instance.dc.DeathForce);
            float num = 0.2f;
            if (__instance.transform.localScale.x < 0f)
            {
                num *= -1f;
            }
            UnityEngine.Object.Instantiate<GameObject>(__instance.fx.newsplosion, new Vector2(__instance.dc.bulletpos.x + -num, __instance.dc.bulletpos.y), Quaternion.identity);
            global.statstat.bulletPools.SparkBurstAt(new Vector2(__instance.dc.bulletpos.x, __instance.dc.bulletpos.y), 0.1f, 0.7f, 4f, 30f, 35f, 0f);
            if (__instance.dc.elbydistance.y < 5f && __instance.dc.elbydistance.x < 5f)
            {
                global.elby.GetComponent<playermovement>().Blooded(0.9f, __instance.dc.bloodColor2);
                global.elby.GetComponent<lbflash>().RollFlash(__instance.dc.bloodColor2, 0.5f, 0.05f);
            }
            UnityEngine.Object.Instantiate<GameObject>(__instance.deathglow, new Vector2(__instance.transform.position.x + 0.2f, __instance.transform.position.y + 0.14f), Quaternion.identity).GetComponent<deathglow>().LetsGo(new Color(0.7f, 0.04f, 0.04f, 1f));
            global.statstat.gsfx.PlaySplat(__instance.transform.position, 0.6f, 0.95f, 0.1f, 1f);
            global.statstat.gsfx.PlayAnySoundDelayed(__instance.mutationsounds[0], __instance.transform.position, 0.4f, 1f, 0f, 1f, 0.1f);
            __instance.dc.EnemyDeath();
            __instance.SpawnDoll();
            __instance.SpawnHeadDoll();
            __instance.SpawnArmDoll();
            __instance.SpawnSpineDoll();
            for (int i = 12; i > 0; i--)
            {
                __instance.spawnblood();
            }
            UnityEngine.Object.Destroy(__instance.gameObject, 0f);
            return false;
        }
    
    }

}
