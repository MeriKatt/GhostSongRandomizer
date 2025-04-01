
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
    public class NewSpikeMouthPatch : MonoBehaviour
    {


        [HarmonyPatch(typeof(NewSpikeMouth), nameof(NewSpikeMouth.Die))]
        [HarmonyPrefix]
        public static bool Die(NewSpikeMouth __instance)
        {
            if (charcon.currentRoom == "fa4" && __instance.mutatingVersion)
            {
                global.statstat.gsfx.PlayBossDeathChime(__instance.transform.position, 0f);
            }
            if (charcon.InNewGamePlus)
            {
                bool flag = false;
                int i = UnityEngine.Random.Range(-3, 2);
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
                    int j = UnityEngine.Random.Range(-2, 4);
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
            allfx afx = global.afx;
            __instance.SpawnDoll();
            __instance.dc.EnemyDeath();
            global.statstat.gsfx.PlaySplat(__instance.transform.position, 0.55f, 0.95f, 0.1f, 1f);
            global.statstat.gsfx.PlayAnySound(__instance.mutate, __instance.transform.position, 1f, 1f, 0f, 0.5f);
            UnityEngine.Object.Instantiate<GameObject>(afx.newsplosion, new Vector2(__instance.dc.bulletpos.x + 0f, __instance.dc.bulletpos.y), Quaternion.identity);
            global.statstat.bulletPools.SparkBurstAt(new Vector2(__instance.dc.bulletpos.x, __instance.dc.bulletpos.y), 0.1f, 0.7f, 4f, 30f, 35f, 0f);
            int num = 0;
            if (__instance.bigform)
            {
                num = 4 + __instance.additionalbigmoney;
            }
            __instance.dc.SpawnBots(4, "small", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 2f), 0f, 0.8f);
            __instance.dc.SpawnBots(1 + num, "medium", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 2f), 0f, 0.8f);
            for (int k = 10; k > 0; k--)
            {
                __instance.spawnblood();
            }
            if (__instance.dc.elbydistance.y < 5f && __instance.dc.elbydistance.x < 5f)
            {
                float num2 = 0.5f - __instance.dc.elbydistance.x / 10f;
                if (num2 < 0.05f)
                {
                    num2 = 0.05f;
                }
                global.elby.GetComponent<playermovement>().Blooded(num2, __instance.dc.bloodColor2);
                global.elby.GetComponent<lbflash>().RollFlash(__instance.dc.bloodColor2, 0.5f, 0.05f);
            }
            UnityEngine.Object.Instantiate<GameObject>(__instance.deathglow, new Vector2(__instance.transform.position.x + 0.2f, __instance.transform.position.y + 0.14f), Quaternion.identity).GetComponent<deathglow>().LetsGo(new Color(0.7f, 0.04f, 0.04f, 1f));
            __instance.gibs1.Go(__instance.dc.DeathForce);
            __instance.gibs2.Go(__instance.dc.DeathForce);
            __instance.gibs3.Go(__instance.dc.DeathForce);
            UnityEngine.Object.Destroy(__instance.gameObject, 0f);
            return false;
        }
    }
}