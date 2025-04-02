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
    public class SpikeScreamerPatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(spikescreamer), nameof(spikescreamer.Die))]
        [HarmonyPrefix]
        public static bool Die(spikescreamer __instance)
        {
            if (charcon.InNewGamePlus)
            {
                bool flag = false;
                int i = UnityEngine.Random.Range(-10, 2);
                if (__instance.ng_customParasitesBig > -1)
                {
                    i = __instance.ng_customParasitesBig;
                }
                if (i > 0)
                {
                    flag = true;
                }
                while (i > 0)
                {
                    __instance.SpawnParasiteBig();
                    i--;
                }
                if (!flag || __instance.ng_customParasites > 0)
                {
                    int j = UnityEngine.Random.Range(-3, 3);
                    if (__instance.ng_customParasites > -1)
                    {
                        j = __instance.ng_customParasites;
                    }
                    while (j > 0)
                    {
                        __instance.SpawnParasite();
                        j--;
                    }
                }
            }
            float num = 0.1f;
            if (!__instance.dc.facingright)
            {
                num *= -1f;
            }
            Vector2 v = new Vector2(__instance.transform.position.x + num, __instance.transform.position.y + 1.6f);
            UnityEngine.Object.Instantiate<GameObject>(global.afx.purplegibsparts, v, Quaternion.identity);
            __instance.SpawnDoll();
            __instance.dc.EnemyDeath();
            global.statstat.gsfx.PlaySplat(__instance.transform.position, 0.55f, 0.95f, 0.1f, 1f);
            global.statstat.gsfx.PlayAnySound(__instance.mutate, __instance.transform.position, 1f, 1f, 0f, 0.5f);
            if (!global.simpleDeaths)
            {
                UnityEngine.Object.Instantiate<GameObject>(__instance.fx.newsplosion, new Vector2(__instance.dc.bulletpos.x + 0f, __instance.dc.bulletpos.y), Quaternion.identity);
            }
            float num2 = 1f;
            if (!__instance.dc.facingright)
            {
                num2 *= -1f;
            }
            Vector2 v2 = new Vector2(__instance.transform.position.x + num2, __instance.transform.position.y + 4f);
            UnityEngine.Object.Instantiate<GameObject>(global.afx.redbloodparts, v2, Quaternion.identity);
            global.statstat.bulletPools.SparkBurstAt(new Vector2(__instance.dc.bulletpos.x, __instance.dc.bulletpos.y), 0.1f, 0.7f, 4f, 30f, 35f, 0f);
            __instance.dc.SpawnBots(4, "small", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 2f), 0f, 0.8f);
            __instance.dc.SpawnBots(1, "medium", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 2f), 0f, 0.8f);
            if (__instance.dc.elbydistance.y < 5f && __instance.dc.elbydistance.x < 5f)
            {
                float num3 = 0.5f - __instance.dc.elbydistance.x / 10f;
                if (num3 < 0.05f)
                {
                    num3 = 0.05f;
                }
                global.elby.GetComponent<playermovement>().Blooded(num3, __instance.dc.bloodColor2);
                global.elby.GetComponent<lbflash>().RollFlash(__instance.dc.bloodColor2, 0.5f, 0.05f);
            }
            if (!global.simpleDeaths)
            {
                UnityEngine.Object.Instantiate<GameObject>(__instance.deathglow, new Vector2(__instance.transform.position.x + 0.2f, __instance.transform.position.y + 0.14f), Quaternion.identity).GetComponent<deathglow>().LetsGo(new Color(0.7f, 0.04f, 0.04f, 1f));
                __instance.BODYgibs.Go(__instance.dc.DeathForce);
                __instance.ABgibs.Go(__instance.dc.DeathForce);
                __instance.LEGgibs.Go(__instance.dc.DeathForce);
                if (__instance.isSpiked)
                {
                    __instance.SPIKEgibs.Go(__instance.dc.DeathForce);
                }
            }
            UnityEngine.Object.Destroy(__instance.gameObject, 0f);
            return false;
        }
    }
}