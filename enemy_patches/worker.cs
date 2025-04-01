
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
    public class WorkerPatch : MonoBehaviour
    {

        [HarmonyPatch(typeof(RoslockWorker), nameof(RoslockWorker.Die))]
        [HarmonyPrefix]
        public static bool Die(RoslockWorker __instance)
        {
            float num = 0f;
            if (!__instance.dc.facingright)
            {
                num *= -1f;
            }
            Vector2 v = new Vector2(__instance.transform.position.x + num, __instance.transform.position.y + 2.5f);
            UnityEngine.Object.Instantiate<GameObject>(global.afx.bluebloodparts, v, Quaternion.identity);
            float num2 = 0f;
            if (!__instance.dc.facingright)
            {
                num2 *= -1f;
            }
            Vector2 v2 = new Vector2(__instance.transform.position.x + num2, __instance.transform.position.y + 1.6f);
            UnityEngine.Object.Instantiate<GameObject>(global.afx.yellowgibsparts, v2, Quaternion.identity);
            allfx afx = global.afx;
            __instance.dc.EnemyDeath();
            global.statstat.gsfx.PlaySplat(__instance.transform.position, 0.55f, 0.95f, 0.1f, 1f);
            global.statstat.gsfx.PlayAnySound(__instance.mutatesound, __instance.transform.position, 1f, 1f, 0f, 0.5f);
            global.statstat.bulletPools.SparkBurstAt(new Vector2(__instance.dc.bulletpos.x, __instance.dc.bulletpos.y), 0.1f, 0.7f, 4f, 30f, 35f, 0f);
            int num3 = 0;
            __instance.dc.SpawnBots(4, "small", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 2f), 0f, 0.8f);
            __instance.dc.SpawnBots(1 + num3, "medium", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 2f), 0f, 0.8f);
            if (!global.simpleDeaths)
            {
                UnityEngine.Object.Instantiate<GameObject>(afx.newsplosion, new Vector2(__instance.dc.bulletpos.x + 0f, __instance.dc.bulletpos.y), Quaternion.identity);
            }
            if (__instance.dc.elbydistance.y < 5f && __instance.dc.elbydistance.x < 5f)
            {
                float num4 = 0.5f - __instance.dc.elbydistance.x / 10f;
                if (num4 < 0.05f)
                {
                    num4 = 0.05f;
                }
                global.elby.GetComponent<playermovement>().Blooded(num4, __instance.dc.bloodColor2);
                global.elby.GetComponent<lbflash>().RollFlash(__instance.dc.bloodColor2, 0.5f, 0.05f);
            }
            __instance.dolegdolls();
            if (!global.simpleDeaths)
            {
                UnityEngine.Object.Instantiate<GameObject>(__instance.deathglow, new Vector2(__instance.transform.position.x + 0.2f, __instance.transform.position.y + 0.14f), Quaternion.identity).GetComponent<deathglow>().LetsGo(new Color(0.7f, 0.04f, 0.04f, 1f));
            }
            UnityEngine.Object.Destroy(__instance.gameObject, 0f);
            return false;
        }
    }
}