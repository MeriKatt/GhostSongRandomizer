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
    public class LeafPatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(leafsc), nameof(leafsc.Die))]
        [HarmonyPostfix]
        public static void Die(leafsc __instance)
        {
            __instance.guncol.enabled = false;
            global.statstat.thedata.OtherData[120] = 1;
            __instance.fgc.OpenGates();
            if (__instance.leafplayer)
            {
                __instance.leafplayer.Stop();
            }
            global.statstat.cameraholder.GetComponent<smoothcamerafollow>().specialTarget = null;
            __instance.resetall();
            __instance.lst.Die();
            GameObject elby = global.elby;
            RandomizerItemBase loc = Plugin.layout.GetDialogueOrEnemy("leaf", "enemy");
            CheckerComponent.spawnloot(elby, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, Plugin.layout.GetModtypeFromString(loc.modType));
            global.statstat.gsfx.PlayAnyVoice(__instance.lst.vo_morepains[2], __instance.transform.position, 1f, 1f, 0f, 0.5f, 2, __instance.gameObject);
            __instance.chestsmoke.Play();
            __instance.StartCoroutine(__instance.unplay());
            global.statstat.gsfx.PlaySplat(__instance.transform.position, 0.55f, 0.95f, 0.1f, 1f);
            global.statstat.gsfx.PlayAnySound(__instance.deathsplode, __instance.transform.position, 0.8f, 1f, 0f, 0.5f);
            UnityEngine.Object.Instantiate<GameObject>(global.afx.newsplosion, new Vector2(__instance.dc.bulletpos.x + 0f, __instance.dc.bulletpos.y), Quaternion.identity);
            global.statstat.bulletPools.SparkBurstAt(new Vector2(__instance.thecols[2].transform.position.x, __instance.thecols[2].transform.position.y), 0.1f, 0.7f, 4f, 30f, 35f, 0f);
            __instance.rb.velocity = new Vector2(0f, 0f);
            __instance.suspendactions = true;
            __instance.flipcols(false);
            __instance.dead = true;
            __instance.facetargnow();
            __instance.anim.Play("die", 0);
            __instance.StartCoroutine(__instance.delaydieanim());
        }
    }
}