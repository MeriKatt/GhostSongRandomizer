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

        public static void spawnloot(GameObject inst, bool Module, bool Item, int arrayNumber, int itemNumber, int moduleNumber, SparklyItem.modtype moduleType)
        {
            Vector2 v2 = new Vector2(inst.transform.position.x, inst.transform.position.y + 3f);
            SparklyItem sparklyItem2 = UnityEngine.Object.Instantiate<SparklyItem>(global.afx.itemdrop, v2, Quaternion.identity);
            sparklyItem2.wasdropped = true;
            sparklyItem2.Item = Item;
            sparklyItem2.Module = Module;
            sparklyItem2.arrayNumber = arrayNumber;
            sparklyItem2.moduleNumber = moduleNumber;
            sparklyItem2.moduleType = moduleType;
            sparklyItem2.itemNumber = itemNumber;
        }


        [HarmonyPatch(typeof(devilwink), nameof(devilwink.die))]
        [HarmonyPrefix]
        public static bool die(devilwink __instance)
        {
            __instance.dc.SpawnBots(UnityEngine.Random.Range(26, 26), "medium", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 4f), 2f, 1f);
            __instance.dc.SpawnBots(UnityEngine.Random.Range(9, 9), "small", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 4f), 2f, 1f);
            __instance.burnloop.Stop();
            CheckerComponent.spawnloot(__instance.gameObject, true, false, 94, 0, 12, SparklyItem.modtype.Modifier);
            __instance.dc.EnemySpecialDeath(true);
            if (__instance.dc.facingright)
            {
                global.statstat.thedata.OtherData[108] = 1;
            }
            else
            {
                global.statstat.thedata.OtherData[108] = 0;
            }
            __instance.rb.velocity = new Vector2(0f, 0f);
            global.statstat.devilwinkcorpsepos = new Vector2(__instance.transform.position.x, __instance.transform.position.y);
            global.statstat.thedata.OtherData[107] = 1;
            Collider2D[] array = __instance.thecols;
            for (int i = 0; i < array.Length; i++)
            {
                array[i].enabled = false;
            }
            __instance.fade = 1f;
            __instance.fadetarg = 1f;
            __instance.stealthed = false;
            __instance.StartCoroutine(__instance.diesounds());
            __instance.anim.Play("die", 0, 0f);
            __instance.StartCoroutine(__instance.dieanim());
            global.statstat.gsfx.PlayAnySound(__instance.dievoc, __instance.transform.position, 1f, 1f, 0f, 0.5f);
            return false;
        }



        [HarmonyPatch(typeof(surfacehuntergirl), nameof(surfacehuntergirl.Die))]
        [HarmonyPrefix]
        public static bool Die(surfacehuntergirl __instance)
        {
            global.statstat.gsfx.PlayBossDeathChime(__instance.transform.position, 0f);
            __instance.dead = true;
            CheckerComponent.spawnloot(__instance.gameObject, true, false, 0, 0, 12, SparklyItem.modtype.Modifier);
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
