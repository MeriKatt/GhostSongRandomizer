
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
    public class PhzPatch : MonoBehaviour
    {

        [HarmonyPatch(typeof(phz), nameof(phz.Die))]
        [HarmonyPrefix]
        public static bool Die(phz __instance)
        {
            global.statstat.gsfx.PlayBossDeathChime(__instance.transform.position, 0f);
            __instance.gh1.Go(__instance.dc.DeathForce);
            __instance.gh2.Go(__instance.dc.DeathForce);
            __instance.gh3.Go(__instance.dc.DeathForce);
            __instance.gh4.Go(__instance.dc.DeathForce);
            global.statstat.gsfx.PlayAnySound(__instance.shattersplode, __instance.transform.position, 0.9f, 1.1f, 0.15f, 1f);
            global.statstat.gsfx.PlayAnySoundDelayed(__instance.shattersplode, __instance.transform.position, 0.8f, 1.15f, 0f, 1f, 0.2f);
            global.statstat.gsfx.PlayAnySoundDelayed(__instance.shattersplode, __instance.transform.position, 0.7f, 1.2f, 0f, 1f, 0.4f);
            global.statstat.gsfx.PlayAnySoundDelayed(__instance.shattersplode, __instance.transform.position, 0.6f, 1.3f, 0f, 1f, 0.6f);
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(__instance.phzscorpse, __instance.transform.position, Quaternion.identity);
            if (!__instance.dc.facingright)
            {
                gameObject.transform.localScale = new Vector2(-1f, 1f);
            }
            global.statstat.thedata.MiscWorldState[4].Value = __instance.transform.position.x;
            if (!__instance.dc.facingright)
            {
                global.statstat.thedata.MiscWorldState[5].Value = 1f;
            }
            UnityEngine.Random.Range(-0.5f, 0.5f);
            UnityEngine.Random.Range(4, 6);
            UnityEngine.Object.Instantiate<GameObject>(global.afx.newsplosion, new Vector2(__instance.transform.position.x + 0f, __instance.transform.position.y + 4.6f), Quaternion.identity);
            float zangle = (float)UnityEngine.Random.Range(50, 90);
            global.statstat.bulletPools.SparkBurstAt(new Vector2(__instance.dc.bulletpos.x, __instance.dc.bulletpos.y), 0.05f, 0.4f, 5f, 15f, 15f, zangle);
            CheckerComponent.newSpawnLoot("phz", __instance.gameObject);
            __instance.dc.EnemyDeath();
            UnityEngine.Object.Destroy(__instance.gameObject, 0f);
            return false;
        }
    }
}