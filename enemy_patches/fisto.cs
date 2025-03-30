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
    public class FistoPatch : MonoBehaviour
    {

        public static void NewSpawnRandomLoot(Fisto inst)
        {
            int num = UnityEngine.Random.Range(1, 3);
            float value = UnityEngine.Random.value;
            if (num == 1)
            {
                if (!global.statstat.modmodules[15].Acquired && value > 0.96f)
                {
                    RandomizerLocation loc = CheckerComponent.locations.Find((RandomizerLocation _loc) => {
                        return _loc.name == "fistorandom1";
                    });
                    CheckerComponent.spawnloot(inst.gameObject, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, loc.modType);
        
                    return;
                }
            }
            else if (num == 2 && !global.statstat.specialmodules[1].Acquired && value > 0.96f)
            {
                    RandomizerLocation loc = CheckerComponent.locations.Find((RandomizerLocation _loc) => {
                        return _loc.name == "fistorandom2";
                    });
                    CheckerComponent.spawnloot(inst.gameObject, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, loc.modType);
        
            }
        }

        [HarmonyPatch(typeof(Fisto), nameof(Fisto.Die))]
        [HarmonyPrefix]
        public static bool Die(Fisto __instance)
        {
            float num = 1f;
            if (!__instance.dc.facingright)
            {
                num *= -1f;
            }
            Vector2 v = new Vector2(__instance.transform.position.x + num, __instance.transform.position.y + 4f);
            UnityEngine.Object.Instantiate<GameObject>(global.afx.redbloodparts, v, Quaternion.identity);
            float num2 = 0.1f;
            if (!__instance.dc.facingright)
            {
                num2 *= -1f;
            }
            Vector2 v2 = new Vector2(__instance.transform.position.x + num2, __instance.transform.position.y + 1.6f);
            UnityEngine.Object.Instantiate<GameObject>(global.afx.bluegibsparts, v2, Quaternion.identity);
            if (__instance.Gumbelly || __instance.Jupiter)
            {
                global.statstat.gsfx.PlayBossDeathChime(__instance.transform.position, 0f);
            }
            if (__instance.isminiboss)
            {
                RandomizerLocation loc = CheckerComponent.locations.Find((RandomizerLocation _loc) => {
                    return _loc.name == "fistomini";
                });
                CheckerComponent.spawnloot(__instance.gameObject, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, loc.modType); 
            }
            else
            {
                FistoPatch.NewSpawnRandomLoot(__instance);
            }
            if (__instance.bloodcloud)
            {
                Vector2 v3 = new Vector2(__instance.transform.position.x + 0f, __instance.transform.position.y + 1f);
                UnityEngine.Object.Instantiate<GameObject>(__instance.bloodcloud, v3, Quaternion.identity);
            }
            if (__instance.yellowparts)
            {
                Vector2 v4 = new Vector2(__instance.transform.position.x + 0f, __instance.transform.position.y + 1.2f);
                UnityEngine.Object.Instantiate<GameObject>(__instance.yellowparts, v4, Quaternion.identity);
            }
            __instance.dc.EnemyDeath();
            if (__instance.diediet)
            {
                Vector2 v5 = new Vector2(__instance.transform.position.x, __instance.transform.position.y + 2f);
                UnityEngine.Object.Instantiate<GameObject>(__instance.diediet, v5, Quaternion.identity);
                global.statstat.gsfx.PlayAnySound(__instance.detsound, __instance.transform.position, 0.5f, 1f, 0.15f, 1f);
            }
            if (__instance.dc.elbydistance.y < 5f && __instance.dc.elbydistance.x < 5f)
            {
                global.elby.GetComponent<playermovement>().Blooded(0.9f, __instance.dc.bloodColor2);
                global.elby.GetComponent<lbflash>().RollFlash(__instance.dc.bloodColor2, 0.5f, 0.05f);
            }
            __instance.dc.SpawnBots(UnityEngine.Random.Range(1, 1), "medium", new Vector2(__instance.middlespawn.transform.position.x, __instance.middlespawn.transform.position.y), 0.7f, 0.6f);
            __instance.dc.SpawnBots(UnityEngine.Random.Range(5, 5), "small", new Vector2(__instance.middlespawn.transform.position.x, __instance.middlespawn.transform.position.y), 0.7f, 0.6f);
            if (__instance.SuperMoney > 0)
            {
                __instance.dc.SpawnBots(__instance.SuperMoney, "medium", new Vector2(__instance.middlespawn.transform.position.x, __instance.middlespawn.transform.position.y), 0.7f, 0.5f);
                __instance.dc.SpawnBots(UnityEngine.Random.Range(5, 5), "small", new Vector2(__instance.middlespawn.transform.position.x, __instance.middlespawn.transform.position.y), 0.7f, 0.6f);
            }
            bool flag2 = false;
            if (Mathf.Abs(__instance.lasthp - __instance.dc.Health) > __instance.dc.maxHealth / 2f || __instance.dc.lastDamType == DamageType.Impact)
            {
                flag2 = true;
            }
            global.statstat.gsfx.PlaySplat(__instance.transform.position, 0.75f, 0.8f, 0.1f, 1f);
            global.statstat.gsfx.PlayAnySound(__instance.mutate[2], __instance.transform.position, 1f, 1f, 0f, 0.5f);
            float num3 = 0.2f;
            if (__instance.transform.localScale.x < 0f)
            {
                num3 *= -1f;
            }
            UnityEngine.Object.Instantiate<GameObject>(global.afx.newsplosion, new Vector2(__instance.dc.bulletpos.x + -num3, __instance.dc.bulletpos.y), Quaternion.identity);
            global.statstat.bulletPools.SparkBurstAt(new Vector2(__instance.dc.bulletpos.x, __instance.dc.bulletpos.y), 0.1f, 0.7f, 4f, 30f, 35f, 0f);
            if (!global.simpleDeaths)
            {
                UnityEngine.Object.Instantiate<GameObject>(__instance.deathglow, new Vector2(__instance.transform.position.x + 0.2f, __instance.transform.position.y + 0.14f), Quaternion.identity).GetComponent<deathglow>().LetsGo(new Color(0.7f, 0.04f, 0.04f, 1f));
            }
            if (__instance.canflinch)
            {
                __instance.SpawnArmDoll();
            }
            if (flag2)
            {
                __instance.gibHolderTop.Go(__instance.dc.DeathForce);
                __instance.gibHolderBot.Go(__instance.dc.DeathForce);
            }
            else if (__instance.dc.lasthit == CollisionType.Upper || __instance.dc.lasthit == CollisionType.Head)
            {
                __instance.SpawnBotDoll();
                __instance.gibHolderTop.Go(__instance.dc.DeathForce);
            }
            else
            {
                __instance.SpawnTopDoll();
                __instance.gibHolderBot.Go(__instance.dc.DeathForce);
                __instance.gibHolderTop.Go(__instance.dc.DeathForce);
            }
            UnityEngine.Object.Destroy(__instance.gameObject, 0f);
            return false;
        }
    }
}