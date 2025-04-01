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
    public class DevilWinkPatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(devilwink), nameof(devilwink.die))]
        [HarmonyPrefix]
        public static bool die(devilwink __instance)
        {
            __instance.dc.SpawnBots(UnityEngine.Random.Range(26, 26), "medium", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 4f), 2f, 1f);
            __instance.dc.SpawnBots(UnityEngine.Random.Range(9, 9), "small", new Vector2(__instance.transform.position.x, __instance.transform.position.y + 4f), 2f, 1f);
            __instance.burnloop.Stop();
            RandomizerItemBase loc = Plugin.layout.GetDialogueOrEnemy("devilwink", "enemy");
            CheckerComponent.spawnloot(__instance.gameObject, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, Plugin.layout.GetModtypeFromString(loc.modType));
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
    }
}