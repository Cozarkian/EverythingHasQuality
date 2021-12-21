using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace QualityEverything
{
    public class Quality_Generator
    {
        public static QualityCategory GenerateQualityCreatedByPawn(Pawn pawn, SkillDef relevantSkill, Thing thing, int supplyQuality = -1, List<SkillRequirement> recipeSkillReq = null)
        {
            //Log.Message("Applying custom quality with " + relevantSkill.label);
            ThingDef def = thing.def;
            if (ModSettings_QEverything.skilledStoneCutting && thing.HasThingCategory(ThingCategoryDefOf.StoneBlocks) && relevantSkill == null)
            {
                relevantSkill = SkillDefOf.Crafting;
            }
            if (relevantSkill == null || (!ModSettings_QEverything.skilledButchering && (def.IsMeat || def.IsLeather)))
            {
                return QualityUtility.GenerateQuality(QualityGenerator.BaseGen);
                //Log.Message("Generated random quality for " + thing.LabelShort);
            }
            QualityCategory qualityCategory;
            int minQuality = GetMinQuality(def);
            int maxQuality = GetMaxQuality(def);
            int level = pawn.skills.GetSkill(relevantSkill).Level; //Log.Message(relevantSkill.label + " without supplies is level " + level);
            if (ModSettings_QEverything.useSkillReq)
            {
                //Log.Message("Applying " + relevantSkill.label + " skill requirements.");
                if (recipeSkillReq != null)
                {
                    for (int sk = 0; sk < recipeSkillReq.Count; sk++)
                    {
                        level -= recipeSkillReq[sk].minLevel;
                    }
                }
                else if (relevantSkill == SkillDefOf.Plants)
                {
                    int plantSkill = 0;
                    foreach (var plantDef in DefDatabase<ThingDef>.AllDefsListForReading.Where(def => def.plant?.sowMinSkill != null))
                    {
                        if (plantDef.plant.harvestedThingDef == def) plantSkill = Mathf.Max(plantSkill, plantDef.plant.sowMinSkill);
                    }
                    level -= plantSkill;
                    //Log.Message("Deducted " + plantSkill + " from harvest quality");
                }
                else if (def is BuildableDef)
                {
                    level -= def.constructionSkillPrerequisite;
                    //Log.Message("Deducting " + thing.def.constructionSkillPrerequisite / 2 + " from construction skill level");
                }
            }
            InspirationDef inspirationDef = InspirationUtility.CheckInspired(def, relevantSkill);
            bool inspired = (inspirationDef != null && pawn.InspirationDef == inspirationDef);
            
            if (ModSettings_QEverything.useMaterialQuality || ModSettings_QEverything.useTableQuality)
            {
                if (supplyQuality >= 0)
                {
                    level += supplyQuality - Mathf.Min(maxQuality, ModSettings_QEverything.stdSupplyQuality);
                }
                level = Mathf.Clamp(level, 0, 20); //Log.Message("Level with supplies is " + level.ToString());
            }

            qualityCategory = QualityUtility.GenerateQualityCreatedByPawn(level, inspired);
            if (ModsConfig.IdeologyActive && pawn.Ideo != null)
            {
                Precept_Role role = pawn.Ideo.GetRole(pawn);
                if (role != null && role.def.roleEffects != null)
                {
                    RoleEffect roleEffect = role.def.roleEffects.FirstOrDefault((RoleEffect eff) => eff is RoleEffect_ProductionQualityOffset);
                    if (roleEffect != null)
                    {
                        qualityCategory = (QualityCategory)Mathf.Min((int)(qualityCategory + (byte)((RoleEffect_ProductionQualityOffset)roleEffect).offset), 6);
                    }
                }
            }
            if (inspired && maxQuality > 4)
            {
                pawn.mindState.inspirationHandler.EndInspiration(inspirationDef);
            }
            return (QualityCategory)Mathf.Clamp((int)qualityCategory, minQuality, maxQuality);
        }

        public static int GetMinQuality(ThingDef def)
        {
            //Log.Message("Finding minimum quality");
            int minQuality = 0;
            if (def.IsStuff)
            {
                minQuality = ModSettings_QEverything.minStuffQuality;
            }
            else if (def.building != null)
            {
                if (def.IsWorkTable) minQuality = ModSettings_QEverything.minWorkQuality;
                else if (def.IsWithinCategory(ThingCategoryDef.Named("BuildingsSecurity")) || def.building.IsTurret) minQuality = ModSettings_QEverything.minSecurityQuality;
                else minQuality = ModSettings_QEverything.minEdificeQuality;
            }
            else if (def.IsDrug) minQuality = ModSettings_QEverything.minDrugQuality;
            else if (def.IsMedicine) minQuality = ModSettings_QEverything.minMedQuality;
            else if (def.ingestible != null)
            {
                if (def.IsWithinCategory(ThingCategoryDefOf.FoodMeals)) minQuality = ModSettings_QEverything.minMealQuality;
                else if (def.IsNutritionGivingIngestible)
                {
                    if (def.ingestible.preferability == FoodPreferability.RawTasty) minQuality = ModSettings_QEverything.minTastyQuality;
                    else minQuality = ModSettings_QEverything.minIngQuality;
                }
            }
            else if (def.IsWeapon) minQuality = ModSettings_QEverything.minWeaponQuality;
            else if (def.IsApparel) minQuality = ModSettings_QEverything.minApparelQuality;
            else if (def.IsShell) minQuality = ModSettings_QEverything.minShellQuality;
            else if (def.IsWithinCategory(ThingCategoryDefOf.Manufactured)) minQuality = ModSettings_QEverything.minManufQuality;
            return minQuality;
        }

        public static int GetMaxQuality(ThingDef def)
        {
            //Log.Message("Finding maximum quality");
            int maxQuality = 6;
            //Art can always be legendary
            if (def.HasComp(typeof(CompArt)))
            {
                return maxQuality;
            }
            if (def.IsStuff) maxQuality = ModSettings_QEverything.maxStuffQuality;
            else if (def.building != null)
            {
                if (def.IsWorkTable) maxQuality = ModSettings_QEverything.maxWorkQuality;
                else if (def.IsWithinCategory(ThingCategoryDef.Named("BuildingsSecurity")) || def.building.IsTurret) maxQuality = ModSettings_QEverything.maxSecurityQuality;
                else maxQuality = ModSettings_QEverything.maxEdificeQuality;
            }
            else if (def.IsDrug) maxQuality = ModSettings_QEverything.maxDrugQuality;
            else if (def.IsMedicine) maxQuality = ModSettings_QEverything.maxMedQuality;
            else if (def.ingestible != null)
            {
                if (def.IsWithinCategory(ThingCategoryDefOf.FoodMeals)) maxQuality = ModSettings_QEverything.maxMealQuality;
                else if (def.IsNutritionGivingIngestible)
                {
                    if (def.ingestible.preferability == FoodPreferability.RawTasty) maxQuality = ModSettings_QEverything.maxTastyQuality;
                    else maxQuality = ModSettings_QEverything.maxIngQuality;
                }
            }
            else if (def.IsWeapon) maxQuality = ModSettings_QEverything.maxWeaponQuality;
            else if (def.IsApparel) maxQuality = ModSettings_QEverything.maxApparelQuality;
            else if (def.IsShell) maxQuality = ModSettings_QEverything.maxShellQuality;
            else if (def.IsWithinCategory(ThingCategoryDefOf.Manufactured)) maxQuality = ModSettings_QEverything.maxManufQuality;
            return maxQuality;
        }
    }
}
