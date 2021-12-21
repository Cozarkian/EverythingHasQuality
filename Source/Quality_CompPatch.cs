using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using RimWorld;
using Verse;
using HarmonyLib;

namespace QualityEverything
{
    [HarmonyPatch]
    public class Quality_CompPatch
    {
        [HarmonyPatch(typeof(CompQuality), "SetQuality")]
        [HarmonyPrefix]
        public static void ApplyQualityLimits(CompQuality __instance, ref QualityCategory q)
        {
            if (__instance?.parent == null)
            {
                return;
            }
            int minQuality = Quality_Generator.GetMinQuality(__instance.parent.def);
            int maxQuality = Quality_Generator.GetMaxQuality(__instance.parent.def);
            if ((int)q < minQuality) q = (QualityCategory)minQuality;
            if ((int)q > maxQuality) q = (QualityCategory)maxQuality;
        }

        //Sets default quality to normal instead of awful when adding to existing game.
        static readonly FieldInfo qc = AccessTools.Field(typeof(CompQuality), "qualityInt");
        [HarmonyPatch(typeof(CompQuality), "PostExposeData")]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var list = instructions.ToList();
            var idx = list.FindIndex(code => code.LoadsField(qc, true));
            if (idx == -1)
            {
                Log.Error("Cannot find FIELD CompQuality.qualityInt in CompQuality.PostExposeData");
                return list.AsEnumerable();
            }
            idx += 1;
            if (list[idx].opcode != OpCodes.Ldstr)
            {
                Log.Error("Not saving string after calling PostExposeData in CompQuality");
                return list.AsEnumerable();
            }
            idx += 1;
            if (list[idx].opcode != OpCodes.Ldc_I4_0)
            {
                Log.Warning("Not using awful quality as default at CompQuality.PostExposeData, no need to change");
                return list.AsEnumerable();
            }
            list.Replace(list[idx], new CodeInstruction(OpCodes.Ldc_I4_2));
            return list.AsEnumerable();
        }

        //Updated Def Database for QualityComps
        public static void DefPatch()
        {
            //Log.Message("QEverything: Starting def patch");
            ThingDef def;
            bool hasComp;
            CompProperties comp = new CompProperties();
            comp.compClass = typeof(CompQuality);
            for (int m = 0; m < DefDatabase<ThingDef>.AllDefsListForReading.Count; m++)
            {
                //Log.Message("QEverything: Checking " + def.label);
                def = DefDatabase<ThingDef>.AllDefsListForReading[m];
                hasComp = def.HasComp(typeof(CompQuality));
                if (def.plant != null || def.IsCorpse || def.race != null)
                    continue;
                else if (def.IsStuff)
                {
                    if (ModSettings_QEverything.indivStuff)
                    {
                        if (!ModSettings_QEverything.stuffDict.ContainsKey(def.defName)) ModSettings_QEverything.stuffDict.Add(def.defName, hasComp);
                        if (!hasComp && ModSettings_QEverything.stuffDict[def.defName] == true) def.comps.Add(comp);
                        else if (hasComp && ModSettings_QEverything.stuffDict[def.defName] == false) def.comps.Remove(comp);
                    }
                    else if (!hasComp && ModSettings_QEverything.stuffQuality) def.comps.Add(comp);
                    //Log.Message("QEverything: " + def.label + " is stuff");
                }
                else if (def.building != null)
                {
                    if (!def.Claimable || def.IsBlueprint || def.IsFrame)
                    {
                        continue;
                    }
                    else if (ModSettings_QEverything.indivBuildings)
                    {
                        if (!ModSettings_QEverything.bldgDict.ContainsKey(def.defName)) ModSettings_QEverything.bldgDict.Add(def.defName, hasComp);
                        if (!hasComp && ModSettings_QEverything.bldgDict[def.defName] == true) def.comps.Add(comp);
                        else if (hasComp && ModSettings_QEverything.bldgDict[def.defName] == false) def.comps.Remove(comp);
                    }
                    else if (hasComp) continue;
                    else if (def.IsWorkTable)
                    {
                        if (ModSettings_QEverything.workQuality) def.comps.Add(comp);
                    }
                    else if (def.IsWithinCategory(ThingCategoryDef.Named("BuildingsSecurity")) || def.building.IsTurret)
                    {
                        if (ModSettings_QEverything.securityQuality) def.comps.Add(comp);
                    }
                    else if (ModSettings_QEverything.edificeQuality) def.comps.Add(comp);
                }
                else if (!def.IsIngestible && (def.IsWeapon || def.IsShell || def.IsWithinCategory(ThingCategoryDef.Named("Grenades"))))
                {
                    if (ModSettings_QEverything.indivWeapons)
                    {
                        if (!ModSettings_QEverything.weapDict.ContainsKey(def.defName)) ModSettings_QEverything.weapDict.Add(def.defName, hasComp);
                        if (!hasComp && ModSettings_QEverything.weapDict[def.defName] == true) def.comps.Add(comp);
                        else if (hasComp && ModSettings_QEverything.weapDict[def.defName] == false) def.comps.Remove(comp);
                    }
                    else if (hasComp) continue;
                    else if (def.IsShell && ModSettings_QEverything.shellQuality) def.comps.Add(comp);
                    else if (def.IsWeapon && ModSettings_QEverything.weaponQuality) def.comps.Add(comp);
                }
                else if (def.IsApparel)
                {
                    if (ModSettings_QEverything.indivApparel)
                    {
                        if (!ModSettings_QEverything.appDict.ContainsKey(def.defName)) ModSettings_QEverything.appDict.Add(def.defName, hasComp);
                        if (!hasComp && ModSettings_QEverything.appDict[def.defName] == true) def.comps.Add(comp);
                        else if (hasComp && ModSettings_QEverything.appDict[def.defName] == false) def.comps.Remove(comp);
                    }
                    else if (!hasComp && ModSettings_QEverything.apparelQuality) def.comps.Add(comp);
                }
                else if (ModSettings_QEverything.indivOther)
                {
                    if (!ModSettings_QEverything.otherDict.ContainsKey(def.defName)) ModSettings_QEverything.otherDict.Add(def.defName, hasComp);
                    if (!hasComp && ModSettings_QEverything.otherDict[def.defName] == true) def.comps.Add(comp);
                    else if (hasComp && ModSettings_QEverything.otherDict[def.defName] == false) def.comps.Remove(comp);
                }
                else if (hasComp) continue; //Avoids duplicate comps
                else if (def.IsDrug && ModSettings_QEverything.drugQuality)
                {
                    def.comps.Add(comp);
                }
                else if (def.IsMedicine)
                {
                    if (ModSettings_QEverything.medQuality) def.comps.Add(comp);
                }
                else if (def.IsIngestible)
                {
                    //Log.Message("Checking ingestible");
                    if (def.IsWithinCategory(ThingCategoryDefOf.FoodMeals) && !ModSettings_QEverything.mealQuality)
                        continue;
                    if ((def.IsNutritionGivingIngestible || def.IsAnimalProduct) && !ModSettings_QEverything.ingredientQuality)
                        continue;
                    def.comps.Add(comp);
                }
                else if (def.IsWithinCategory(ThingCategoryDefOf.Manufactured) && ModSettings_QEverything.manufQuality)
                {
                    def.comps.Add(comp);
                }
            }
        }

        //Adds comp to items in current game
        public static void ApplyNewQuality()
        {
            if (Current.Game == null)
            {
                return;
            }
            //Log.Message("QEverything: Applying Changes");
            foreach (Map map in Find.Maps)
            {
                foreach (Thing thing in map.listerThings.AllThings)
                {
                    ThingWithComps thingWithComps = thing as ThingWithComps;
                    if (thingWithComps == null)
                    {
                        //Log.Message(thing.Label + " is not a thing with comps");
                        continue;
                    }
                    if (thing.TryGetComp<CompQuality>() == null && thing.def.HasComp(typeof(CompQuality)))
                    {
                        //Log.Message("Adding quality to " + thing.Label);
                        CompQuality comp = new CompQuality();
                        int qc = Mathf.Clamp(2, Quality_Generator.GetMinQuality(thing.def), Quality_Generator.GetMaxQuality(thing.def));
                        comp.SetQuality((QualityCategory)qc, ArtGenerationContext.Outsider);
                        List<ThingComp> comps = thingWithComps.AllComps ?? new List<ThingComp>();
                        comps.Add(comp);
                        AccessTools.Field(typeof(ThingWithComps), "comps").SetValue(thingWithComps, comps);
                    }
                    /*else if (!thing.def.HasComp(typeof(CompQuality)))
                    {
                        //Log.Message("Removing quality from " + thing.Label);
                        List<ThingComp> curComps = thingWithComps.AllComps;
                        for (int i = 0; i < curComps.Count; i++)
                        {
                            if (curComps[i] == thingWithComps.GetComp<CompQuality>())
                            {
                                curComps.RemoveAt(i);
                                break;
                            }
                        }
                        AccessTools.Field(typeof(ThingWithComps), "comps").SetValue(thingWithComps, curComps);
                    }*/
                }
            }
        }
    }
}

