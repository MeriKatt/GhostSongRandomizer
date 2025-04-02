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
    public class Walker5Patch : MonoBehaviour
    {
        [HarmonyPatch(typeof(walker5behavior), nameof(walker5behavior.Die))]
        [HarmonyPrefix]
        public static bool Die(walker5behavior __instance)
        {
            if (charcon.InNewGamePlus)
            {
                bool flag = false;
                int i = UnityEngine.Random.Range(-10, 2);
                if (__instance.ng_customParasitesBig > -1)
                {
                    i = __instance.ng_customParasitesBig;
                }
                if (i > 0)
                {
                    flag = true;
                }
                while (i > 0)
                {
                    __instance.SpawnParasiteBig();
                    i--;
                }
                if (!flag || __instance.ng_customParasites > 0)
                {
                    int j = UnityEngine.Random.Range(-3, 3);
                    if (__instance.ng_customParasites > -1)
                    {
                        j = __instance.ng_customParasites;
                    }
                    while (j > 0)
                    {
                        __instance.SpawnParasite();
                        j--;
                    }
                }
            }
            if (__instance.belle)
            {
                global.statstat.gsfx.PlayBossDeathChime(__instance.transform.position, 0f);
            }
            float num = 1f;
            if (!__instance.dc.facingright)
            {
                num *= -1f;
            }
            Vector2 v = new Vector2(__instance.transform.position.x + num, __instance.transform.position.y + 4f);
            UnityEngine.Object.Instantiate<GameObject>(global.afx.redbloodparts, v, Quaternion.identity);
            if (__instance.fistdet && __instance.Electro)
            {
                float num2 = 0f;
                bool facingright = __instance.dc.facingright;
                AudioClip clip = __instance.explode[UnityEngine.Random.Range(0, __instance.explode.Length)];
                global.statstat.gsfx.PlayAnySound(clip, __instance.transform.position, 0.6f, 1f, 0.1f, 0.8f);
                Vector2 v2 = new Vector2(__instance.transform.position.x + num2, __instance.transform.position.y + 1f);
                UnityEngine.Object.Instantiate<GameObject>(__instance.fistdet, v2, Quaternion.identity);
                Vector2 v3 = new Vector2(__instance.transform.position.x + num2, __instance.transform.position.y + 1f);
                UnityEngine.Object.Instantiate<GameObject>(__instance.yellowparts, v3, Quaternion.identity);
            }
            if (global.simpleDeaths)
            {
                float num3 = 0.1f;
                if (!__instance.dc.facingright)
                {
                    num3 *= -1f;
                }
                Vector2 v4 = new Vector2(__instance.transform.position.x + num3, __instance.transform.position.y + 1.6f);
                UnityEngine.Object.Instantiate<GameObject>(global.afx.bluegibsparts, v4, Quaternion.identity);
            }
            if (__instance.bloodcloud && !global.simpleDeaths)
            {
                Vector2 v5 = new Vector2(__instance.transform.position.x + 0f, __instance.transform.position.y + 1.2f);
                UnityEngine.Object.Instantiate<GameObject>(__instance.bloodcloud, v5, Quaternion.identity);
            }
            if (__instance.hasREGEN)
            {
                RandomizerItemBase loc = Plugin.layout.GetDialogueOrEnemy("walker5", "enemy");
                CheckerComponent.spawnloot(__instance.gameObject, loc.Module, loc.Item, loc.arrayNumber, loc.itemNumber, loc.moduleNumber, Plugin.layout.GetModtypeFromString(loc.modType));
            }
            __instance.dc.EnemyDeath();
            global.statstat.gsfx.PlaySplat(__instance.transform.position, 0.55f, 0.95f, 0.1f, 1f);
            allfx component = GameObject.FindGameObjectWithTag("generalfx").GetComponent<allfx>();
            GameObject.FindGameObjectWithTag("status").GetComponent<status>();
            if (!global.simpleDeaths && __instance.dc.lastDamType == DamageType.Chill)
            {
                foreach (GameObject gameObject in __instance.gibs)
                {
                    gameObject.GetComponent<SpriteRenderer>();
                    gibsies component2 = gameObject.GetComponent<gibsies>();
                    component2.icebaby = true;
                    component2.icecolor = __instance.dfx.chillgibcolor;
                }
            }
            int num4 = 0;
            if (__instance.dc.lastDamType == DamageType.Impact)
            {
                num4 = 2;
            }
            float num5 = 0.2f;
            if (__instance.transform.localScale.x < 0f)
            {
                num5 *= -1f;
            }
            UnityEngine.Object.Instantiate<GameObject>(component.newsplosion, new Vector2(__instance.dc.bulletpos.x + -num5, __instance.dc.bulletpos.y), Quaternion.identity);
            global.statstat.bulletPools.SparkBurstAt(new Vector2(__instance.dc.bulletpos.x + -num5, __instance.dc.bulletpos.y), 0.1f, 0.7f, 4f, 30f, 35f, 0f);
            if (__instance.Electro)
            {
                __instance.dc.SpawnBots(UnityEngine.Random.Range(2, 2), "medium", new Vector2(__instance.chest.transform.position.x, __instance.chest.transform.position.y), 0.7f, 0.6f);
                __instance.dc.SpawnBots(UnityEngine.Random.Range(3, 9), "small", new Vector2(__instance.chest.transform.position.x, __instance.chest.transform.position.y), 0.7f, 0.6f);
            }
            else if (__instance.speedy)
            {
                __instance.dc.SpawnBots(UnityEngine.Random.Range(5, 10), "small", new Vector2(__instance.chest.transform.position.x, __instance.chest.transform.position.y), 0.7f, 0.6f);
            }
            else if (__instance.hasREGEN)
            {
                __instance.dc.SpawnBots(UnityEngine.Random.Range(3, 3), "medium", new Vector2(__instance.chest.transform.position.x, __instance.chest.transform.position.y), 0.7f, 0.6f);
                __instance.dc.SpawnBots(UnityEngine.Random.Range(8, 8), "small", new Vector2(__instance.chest.transform.position.x, __instance.chest.transform.position.y), 0.7f, 0.6f);
            }
            else
            {
                __instance.dc.SpawnBots(UnityEngine.Random.Range(3, 4), "small", new Vector2(__instance.chest.transform.position.x, __instance.chest.transform.position.y), 0.7f, 0.6f);
            }
            if (!global.simpleDeaths)
            {
                foreach (GameObject gameObject2 in __instance.gibs)
                {
                    if (gameObject2.transform.parent.localScale.x > 0f)
                    {
                        if (__instance.dc.bulletspeed.x > 0f)
                        {
                            gameObject2.GetComponent<gibsies>().flippy = true;
                        }
                    }
                    else if (__instance.dc.bulletspeed.x < 0f)
                    {
                        gameObject2.GetComponent<gibsies>().flippy = true;
                    }
                }
            }
            if (__instance.dc.Temperature >= __instance.dc.fireThresh)
            {
                __instance.forcegibs = true;
            }
            if (__instance.dc.lastDamType == DamageType.Chill)
            {
                __instance.forcegibs = true;
            }
            if (Mathf.Abs(__instance.lasthp - __instance.dc.Health) > __instance.dc.maxHealth / 2f || __instance.anim.GetCurrentAnimatorStateInfo(0).IsName("sleep") || __instance.anim.GetCurrentAnimatorStateInfo(0).IsName("wakeup") || __instance.forcegibs || __instance.dc.lastDamType == DamageType.Explosive)
            {
                __instance.forcegibs = true;
                if (__instance.anim.GetCurrentAnimatorStateInfo(0).IsName("sleep") || __instance.anim.GetCurrentAnimatorStateInfo(0).IsName("wakeup") || __instance.Electro)
                {
                    __instance.forcegibs = true;
                }
                float num6 = 1f;
                if (__instance.Electro)
                {
                    num6 = 1.75f;
                    if (global.GetDistBetweenAny(__instance.transform.position, global.elby.transform.position) < 16f)
                    {
                        global.statstat.shake(0.7f, 25);
                        global.statstat.buzz(0.7f, 0.7f, 5);
                    }
                }
                if (__instance.dc.elbydistance.y < 5f && __instance.dc.elbydistance.x < 5f)
                {
                    global.elby.GetComponent<playermovement>().Blooded(0.65f - __instance.dc.elbydistance.x / 10f, __instance.dc.bloodColor2);
                    global.elby.GetComponent<lbflash>().RollFlash(__instance.dc.bloodColor2, 0.5f, 0.05f);
                }
                UnityEngine.Object.Instantiate<GameObject>(__instance.deathglow, new Vector2(__instance.transform.position.x + 0.2f, __instance.transform.position.y + 0.14f), Quaternion.identity).GetComponent<deathglow>().LetsGo(new Color(0.7f, 0.04f, 0.04f, 1f));
                if (__instance.dc.lasthit == CollisionType.Upper || __instance.forcegibs)
                {
                    if (!global.simpleDeaths)
                    {
                        GameObject[] array = __instance.gibs;
                        for (int k = 0; k < array.Length; k++)
                        {
                            gibsies component3 = array[k].GetComponent<gibsies>();
                            if (component3.Style == gibsies.gibtype.Upper)
                            {
                                component3.forcemod = __instance.dc.DeathForce * num6;
                                component3.Go(false);
                            }
                            if (component3.Style == gibsies.gibtype.Lower)
                            {
                                component3.forcemod = __instance.dc.DeathForce * num6;
                                component3.Go(false);
                            }
                            else if (component3.Style == gibsies.gibtype.Always)
                            {
                                component3.forcemod = __instance.dc.DeathForce * num6;
                                component3.Go(false);
                            }
                        }
                    }
                }
                else if ((__instance.dc.lasthit == CollisionType.Head && !__instance.forcegibs) || num4 == 2)
                {
                    __instance.Spawnbottom();
                    GameObject[] array = __instance.gibs;
                    for (int k = 0; k < array.Length; k++)
                    {
                        gibsies component4 = array[k].GetComponent<gibsies>();
                        if (component4.Style == gibsies.gibtype.Upper)
                        {
                            component4.forcemod = __instance.dc.DeathForce * num6;
                            if (!global.simpleDeaths)
                            {
                                component4.Go(false);
                            }
                        }
                        else if (component4.Style == gibsies.gibtype.Always)
                        {
                            component4.forcemod = __instance.dc.DeathForce * num6;
                            if (!global.simpleDeaths)
                            {
                                component4.Go(false);
                            }
                        }
                    }
                }
                else
                {
                    __instance.Spawntop();
                    if (!global.simpleDeaths)
                    {
                        GameObject[] array = __instance.gibs;
                        for (int k = 0; k < array.Length; k++)
                        {
                            gibsies component5 = array[k].GetComponent<gibsies>();
                            if (component5.Style == gibsies.gibtype.Lower)
                            {
                                component5.forcemod = __instance.dc.DeathForce;
                                component5.Go(false);
                            }
                            else if (component5.Style == gibsies.gibtype.Always)
                            {
                                component5.forcemod = __instance.dc.DeathForce;
                                component5.Go(false);
                            }
                        }
                    }
                }
            }
            else
            {
                if (__instance.dc.elbydistance.y < 5f && __instance.dc.elbydistance.x < 5f)
                {
                    float num7 = 0.4f - __instance.dc.elbydistance.x / 10f;
                    if (num7 < 0.05f)
                    {
                        num7 = 0.05f;
                    }
                    global.elby.GetComponent<playermovement>().Blooded(num7, __instance.dc.bloodColor2);
                    global.elby.GetComponent<lbflash>().RollFlash(__instance.dc.bloodColor2, 0.5f, 0.05f);
                }
                if (__instance.dc.lasthit == CollisionType.Lower && num4 != 2)
                {
                    __instance.Spawntop();
                    if (!global.simpleDeaths)
                    {
                        GameObject[] array = __instance.gibs;
                        for (int k = 0; k < array.Length; k++)
                        {
                            gibsies component6 = array[k].GetComponent<gibsies>();
                            if (component6.Style == gibsies.gibtype.Lower)
                            {
                                component6.forcemod = __instance.dc.DeathForce;
                                component6.Go(false);
                            }
                            else if (component6.Style == gibsies.gibtype.Always)
                            {
                                component6.forcemod = __instance.dc.DeathForce;
                                component6.Go(false);
                            }
                        }
                    }
                }
                else
                {
                    __instance.Spawnbottom();
                    if (!global.simpleDeaths)
                    {
                        GameObject[] array = __instance.gibs;
                        for (int k = 0; k < array.Length; k++)
                        {
                            gibsies component7 = array[k].GetComponent<gibsies>();
                            if (component7.Style == gibsies.gibtype.Upper)
                            {
                                component7.forcemod = __instance.dc.DeathForce;
                                component7.Go(false);
                            }
                            else if (component7.Style == gibsies.gibtype.Always)
                            {
                                component7.forcemod = __instance.dc.DeathForce;
                                component7.Go(false);
                            }
                        }
                    }
                }
            }
            bool simpleDeaths = global.simpleDeaths;
            UnityEngine.Object.Destroy(__instance.gameObject);
            return false;
        }
          
    }
}