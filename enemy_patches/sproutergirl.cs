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
    public class SprouterPatch : MonoBehaviour
    {
        [HarmonyPatch(typeof(sproutergirl), nameof(sproutergirl.Update))]
        [HarmonyPrefix]
        public static bool Update(sproutergirl __instance)
        {
            bool flag = true;
            if (global.statstat)
            {
                Vector2 vector = global.statstat.elby.transform.position;
                flag = (vector.x > 97.3f && vector.y < -55f);
            }
            if (flag)
            {
                __instance.dc.Health = __instance.dc.maxHealth;
            }
            if (!__instance.started && __instance.dc.target && __instance.dc.targetdistance.x < 20f && __instance.dc.targetdistance.y < 5f && __instance.uptime > 10)
            {
                __instance.StartFightMusic();
            }
            __instance.blend = Mathf.Lerp(__instance.blend, __instance.blendtarg, 0.05f);
            __instance.anim.SetFloat("Blend", __instance.blend);
            if (__instance.dc.Health / __instance.dc.maxHealth < 0.6f && __instance.grounded && !__instance.hasmutated)
            {
                __instance.hasmutated = true;
                __instance.State = 3;
                __instance.MutateStart();
            }
            if (__instance.dc.Health <= 0f)
            {
                if (!__instance.loothasspawned)
                {
                    __instance.loothasspawned = true;
                    RandomizerItemBase loc = Plugin.layout.GetDialogueOrEnemy("sproutergirl", "enemy");
                    CheckerComponent.spawnloot(__instance.gameObject, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, Plugin.layout.GetModtypeFromString(loc.modType));
                }
                if (!__instance.dead)
                {
                    __instance.dead = true;
                    __instance.Die();
                }
            }
            if ((__instance.dc.justhit > 0 || __instance.dc.justtouched > 0) && __instance.timesincefirsthit == -1)
            {
                __instance.timesincefirsthit = 0;
            }
            __instance.grounded = Physics2D.OverlapArea(new Vector2(__instance.collider.bounds.min.x, __instance.collider.bounds.min.y - 0.2f), new Vector2(__instance.collider.bounds.max.x, __instance.collider.bounds.min.y + 0.2f), __instance.ground);
            switch (__instance.State)
            {
            case 0:
                __instance.WaitUpdate();
                return false;
            case 1:
                __instance.WanderUpdate();
                return false;
            case 2:
                __instance.JumpUpdate();
                return false;
            case 3:
                __instance.MutateUpdate();
                return false;
            case 4:
                __instance.MutateWaitUpdate();
                return false;
            case 5:
                __instance.MutateChaseUpdate();
                return false;
            default:
                return false;
            }
            return false;
        }
    }
}