
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
    public class SpikeRunnerPatch : MonoBehaviour
    {

        [HarmonyPatch(typeof(spikerunner), nameof(spikerunner.Die))]
        [HarmonyPrefix]
        public static bool Die(spikerunner __instance)
        {
                if (__instance.dropsitem)
                {
                    CheckerComponent.newSpawnLoot("spikerunner", __instance.gameObject);
                }
                __instance.Gibs1.Go(__instance.dc.DeathForce);
                __instance.spawndoll();
                __instance.dc.EnemyDeath();
                global.statstat.gsfx.PlaySplat(__instance.transform.position, 0.55f, 0.95f, 0.1f, 1f);
                UnityEngine.Object.Instantiate<GameObject>(__instance.fx.newsplosion, new Vector2(__instance.dc.bulletpos.x + 0f, __instance.dc.bulletpos.y), Quaternion.identity);
                global.statstat.bulletPools.SparkBurstAt(new Vector2(__instance.dc.bulletpos.x, __instance.dc.bulletpos.y), 0.1f, 0.7f, 4f, 30f, 35f, 0f);
                __instance.dc.SpawnBots(6, "small", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 2f), 0f, 0.8f);
                for (int i = 8; i > 0; i--)
                {
                    __instance.spawnblood();
                }
                UnityEngine.Object.Instantiate<GameObject>(__instance.deathglow, new Vector2(__instance.transform.position.x + 0.2f, __instance.transform.position.y + 0.14f), Quaternion.identity).GetComponent<deathglow>().LetsGo(new Color(0.7f, 0.04f, 0.04f, 1f));
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
                UnityEngine.Object.Destroy(__instance.gameObject, 0f);
            return false;
        }
    }
}