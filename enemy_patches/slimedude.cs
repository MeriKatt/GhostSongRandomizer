
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
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
    public class SlimeDudePatch : MonoBehaviour
    {

        [HarmonyPatch(typeof(slimedude), nameof(slimedude.Die))]
        [HarmonyPrefix]
        public static bool Die(slimedude __instance)
        {
            if (__instance.dropslivingblobs)
            {
                global.statstat.gsfx.PlayBossDeathChime(__instance.transform.position, 0f);
                CheckerComponent.newSpawnLoot("slimedude", __instance.gameObject);
            }
            global.statstat.gsfx.PlaySplat(__instance.transform.position, 0.5f, 0.95f, 0.1f, 1f);
            if (__instance.slimetype == 1)
            {
                __instance.dc.SpawnBots(UnityEngine.Random.Range(1, 2), "small", new Vector2(__instance.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), __instance.transform.position.y + UnityEngine.Random.Range(2f, 3f)), 0.4f, 0f);
            }
            else if (__instance.slimetype == 2)
            {
                __instance.dc.SpawnBots(UnityEngine.Random.Range(3, 5), "small", new Vector2(__instance.transform.position.x + UnityEngine.Random.Range(-0.8f, 0.8f), __instance.transform.position.y + UnityEngine.Random.Range(2f, 3f)), 0.4f, 0f);
            }
            else if (__instance.slimetype == 3)
            {
                __instance.dc.SpawnBots(UnityEngine.Random.Range(5, 7), "small", new Vector2(__instance.transform.position.x + UnityEngine.Random.Range(-1.4f, 1.4f), __instance.transform.position.y + UnityEngine.Random.Range(2f, 3f)), 0.4f, 0f);
            }
            float num = 0f;
            if (__instance.transform.localScale.x < 0f)
            {
                num *= -1f;
            }
            UnityEngine.Object.Instantiate<GameObject>(global.afx.newsplosion, new Vector2(__instance.transform.position.x, __instance.transform.position.y + 2f), Quaternion.identity);
            int numberOfSparks = 5;
            float sparkspeed = 200f;
            if (__instance.slimetype == 2)
            {
                numberOfSparks = 10;
                sparkspeed = 250f;
            }
            else if (__instance.slimetype == 3)
            {
                numberOfSparks = 20;
                sparkspeed = 300f;
            }
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(__instance.beanspray, new Vector2(__instance.transform.position.x, __instance.transform.position.y + 1f), Quaternion.identity);
            gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
            sparkspray component = gameObject.GetComponent<sparkspray>();
            component.numberOfSparks = numberOfSparks;
            component.sparkspeed = sparkspeed;
            if (__instance.slimetype == 3)
            {
                component.speedrandomizer = 100f;
                component.sparkspawnYoff = 3f;
            }
            if (global.ElbyDist(__instance.transform.position) < 5f)
            {
                float num2 = 0.4f - __instance.dc.elbydistance.x / 10f;
                if (num2 < 0.05f)
                {
                    num2 = 0.05f;
                }
                global.elby.GetComponent<playermovement>().Blooded(num2, __instance.dc.bloodColor2);
                global.elby.GetComponent<lbflash>().RollFlash(__instance.dc.bloodColor2, 0.5f, 0.05f);
            }
            for (int i = 5; i > 0; i--)
            {
                __instance.spawnblood();
            }
            __instance.gibsRB.transform.parent = null;
            __instance.gibsRB.simulated = true;
            float x;
            if (__instance.dc.bulletspeed.x > 0f)
            {
                x = (float)UnityEngine.Random.Range(150, 300);
            }
            else
            {
                x = (float)UnityEngine.Random.Range(-150, -300);
            }
            __instance.gibsRB.AddForce(new Vector2(x, (float)UnityEngine.Random.Range(400, 600)));
            while (__instance.smallslimestospawn > 0)
            {
                UnityEngine.Object.Instantiate<GameObject>(__instance.smallslime, new Vector2(__instance.transform.position.x + (float)UnityEngine.Random.Range(-2, 2), __instance.transform.position.y + (float)UnityEngine.Random.Range(1, 3)), Quaternion.identity);
                __instance.smallslimestospawn--;
            }
            if (__instance.slimetype == 3)
            {
                UnityEngine.Object.Instantiate<GameObject>(__instance.mediumslime, new Vector2(__instance.transform.position.x + 0f, __instance.transform.position.y + 1f), Quaternion.identity);
            }
            __instance.dc.EnemyDeath();
            UnityEngine.Object.Destroy(__instance.gameObject, 0f);
            return false;
        }
    }
}