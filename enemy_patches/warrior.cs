
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
    public class WarriorPatch : MonoBehaviour
    {
        public static void newLoot(RoslockWarrior inst) {
            int num = UnityEngine.Random.Range(1, 3);
            if (num == 1)
            {
                float value = UnityEngine.Random.value;
                if (!global.statstat.specialmodules[7].Acquired && value > 0.93f)
                {
                    global.statstat.gsfx.PlayBossDeathChime(inst.transform.position, 0f);
                    CheckerComponent.newSpawnLoot("warrior1", inst.gameObject);
                    return;
                }
            }
            else if (num == 2)
            {
                float value2 = UnityEngine.Random.value;
                if (!global.statstat.specialmodules[8].Acquired && value2 > 0.93f)
                {
                    global.statstat.gsfx.PlayBossDeathChime(inst.transform.position, 0f);
                    CheckerComponent.newSpawnLoot("warrior2", inst.gameObject);
                }
            }
        }

        [HarmonyPatch(typeof(RoslockWarrior), nameof(RoslockWarrior.Die))]
        [HarmonyPrefix]
        public static bool Die(RoslockWarrior __instance)
        {
            
            __instance.dc.EnemyDeath();
            WarriorPatch.newLoot(__instance);
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
            if (!global.simpleDeaths)
            {
                UnityEngine.Object.Instantiate<GameObject>(__instance.deathglow, new Vector2(__instance.transform.position.x + 0.2f, __instance.transform.position.y + 0.14f), Quaternion.identity).GetComponent<deathglow>().LetsGo(new Color(0.7f, 0.04f, 0.04f, 1f));
            }
            global.statstat.gsfx.PlayAnySound(__instance.mutate, __instance.transform.position, 1f, 1f, 0f, 0.5f);
            if (status.bugscraper > 0)
            {
                __instance.dc.SpawnBots(4, "bugblood", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 2f), 0f, 0.8f);
            }
            if (__instance.altmoneytimes.x > 0f || __instance.altmoneytimes.y > 0f)
            {
                __instance.dc.SpawnBots(Mathf.RoundToInt(__instance.altmoneytimes.x), "medium", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 2f), 0f, 0.8f);
                __instance.dc.SpawnBots(Mathf.RoundToInt(__instance.altmoneytimes.y), "small", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 2f), 0f, 0.8f);
            }
            else
            {
                __instance.dc.SpawnBots(5, "medium", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 2f), 0f, 0.8f);
            }
            float num2 = 0f;
            if (!__instance.dc.facingright)
            {
                num2 *= -1f;
            }
            Vector2 v = new Vector2(__instance.transform.position.x + num2, __instance.transform.position.y + 0.5f);
            UnityEngine.Object.Instantiate<GameObject>(global.afx.bluebloodparts, v, Quaternion.identity);
            float num3 = 0f;
            if (!__instance.dc.facingright)
            {
                num3 *= -1f;
            }
            Vector2 v2 = new Vector2(__instance.transform.position.x + num3, __instance.transform.position.y + 0.5f);
            UnityEngine.Object.Instantiate<GameObject>(global.afx.yellowgibsparts, v2, Quaternion.identity);
            UnityEngine.Object.Instantiate<GameObject>(__instance.fx.newsplosion, new Vector2(__instance.dc.bulletpos.x + 0f, __instance.dc.bulletpos.y), Quaternion.identity);
            global.statstat.bulletPools.SparkBurstAt(new Vector2(__instance.dc.bulletpos.x, __instance.dc.bulletpos.y), 0.1f, 0.7f, 4f, 30f, 35f, 0f);
            global.statstat.gsfx.PlaySplat(__instance.transform.position, 0.55f, 0.95f, 0.1f, 1f);
            __instance.theragdoll.SetActive(true);
            __instance.theragdoll.transform.parent = null;
            __instance.dollstreamer1.Getcolor(__instance.dc.bloodColor, __instance.dc.bloodColor2);
            __instance.dollstreamer2.Getcolor(__instance.dc.bloodColor, __instance.dc.bloodColor2);
            __instance.dollstreamer3.Getcolor(new Color(0.1f, 0.7f, 0.2f, 0.5f), new Color(0.1f, 0.6f, 0.1f, 0.25f));
            float num4 = (float)UnityEngine.Random.Range(-1300, -1600) * __instance.dc.DeathForce;
            float y = (float)UnityEngine.Random.Range(800, 900) * __instance.dc.DeathForce;
            float num5 = (float)UnityEngine.Random.Range(50, 300) * __instance.dc.DeathForce;
            if (__instance.dc.bulletspeed.y > 1f && __instance.dc.IsItMelee > 0f)
            {
                y = (float)UnityEngine.Random.Range(4000, 6000) * __instance.dc.DeathForce;
            }
            if (__instance.dc.bulletspeed.x > 0f)
            {
                num4 *= -1.2f;
                num5 *= -1f;
            }
            if (__instance.transform.localScale.x <= 0f)
            {
                num5 *= -1f;
            }
            foreach (Rigidbody2D rigidbody2D in __instance.dollbodyrbs)
            {
                rigidbody2D.AddForce(new Vector2(num4, y));
                rigidbody2D.AddTorque(num5);
            }
            UnityEngine.Object.Destroy(__instance.gameObject, 0f);
            return false;
        }
    }
}