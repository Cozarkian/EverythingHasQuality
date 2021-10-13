/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using HarmonyLib;

namespace QualityFramework
{
    [HarmonyPatch]
    class IngredientQuality_Patch
    {
        public static int IngredientQuality { get; set; } = 0;
        private static bool usesIngQuality = false;
        private static int tableQuality = 2;

        public static bool Prepare()
        {
            return ModLister.HasActiveModWithName("Crop and Meat Quality");
        }

        //MakeRecipeProducts Prefix - Store quality of ingredients before making recipe products
        [HarmonyPatch(typeof(GenRecipe), "MakeRecipeProducts")]
        public static void Prefix(RecipeDef recipeDef, List<Thing> ingredients, IBillGiver billGiver)
        {
            //Reset ingredients before making products
            IngredientQuality = 0;
            usesIngQuality = false;
            /*
            if (!recipeDef.HasModExtension<DefMod_ResProdQuality>())
            {
                return;
            }
            */
            //Check for quality ingredients and store value
 /*           if (ingredients != null)
            {
                int value = 0;
                int numIng = 0;
                for (int i = 0; i < ingredients.Count; i++)
                {
                    CompQuality comp = ingredients[i].TryGetComp<CompQuality>();
                    if (comp != null)
                    {
                        value += (int)comp.Quality * ingredients[i].stackCount;
                        numIng += ingredients[i].stackCount;
                    }
                }
                if (numIng > 0)
                {
                    value = value / numIng;
                    usesIngQuality = true;
                }
                IngredientQuality = Mathf.Clamp(value, 0, 6);
            }
        }

        //PostProcess - Has access to thing being made

        //GenerateQuality Postfix/New
        public static QualityCategory GenerateQualityCreatedByPawn(Pawn pawn, SkillDef relevantSkill)
        {
            int level = pawn.skills.GetSkill(relevantSkill).Level;
            bool inspired = pawn.InspirationDef == InspirationDefOf.Inspired_Creativity;
            QualityCategory qualityCategory = QualityUtility.GenerateQualityCreatedByPawn(level, inspired);
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
            if (inspired)
            {
                pawn.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Creativity);
            }
            return qualityCategory;
        }

        //Generate Quality #2 Prefix - adjust skill level based on ingredient quality
        [HarmonyPatch(typeof(QualityUtility), "GenerateQualityCreatedByPawn")]
        public static void Prefix(ref int relevantSkillLevel, bool inspired)
        {
            //Adjust skill level based on ingredient quality before calculating quality
            if (usesIngQuality)
            {
                int num;
                if (inspired)
                {
                    num = relevantSkillLevel + IngredientQuality;
                }
                else
                {
                    int rand = Rand.RangeInclusive(0, 1);
                    if (rand == 0) num = (int)(relevantSkillLevel * .9f) + IngredientQuality - 2;
                    else num = Mathf.CeilToInt(relevantSkillLevel * .9f) + IngredientQuality - 2;
                }
                relevantSkillLevel = Mathf.Clamp(num, 0, 20);
            }
            //Reset ingredients after finishing
            IngredientQuality = 0;
            usesIngQuality = false;
        }

        //PostProcess Postfix - Apply min/max limits to quality generated.
        [HarmonyPatch(typeof(GenRecipe), "PostProcessProduct")]
        [HarmonyPostfix]
        public static void AdjustQuality(ref Thing __result)
        {
            if (__result.TryGetComp<CompQuality>() == null)
            {
                return;
            }
            CompQuality quality = __result.TryGetComp<CompQuality>();
            QualityCategory minQuality = GetMinQuality(__result);
            QualityCategory maxQuality = GetMaxQuality(__result);
            ArtGenerationContext art = ArtGenerationContext.Colony;
            if (quality.Quality < minQuality) quality.SetQuality(minQuality, art);
            if (quality.Quality > maxQuality) quality.SetQuality(maxQuality, art);
        }

        public static QualityCategory GetMinQuality(Thing thing)
        {
            ThingDef def = thing.def;
            int minQuality = 0;
            if (def.IsWorkTable && !def.IsBlueprint && ModSettings_QualityFramework.workQuality) minQuality = ModSettings_QualityFramework.minWorkQuality;
            else if (def.IsStuff && ModSettings_QualityFramework.stuffQuality) minQuality = ModSettings_QualityFramework.minStuffQuality;
            else if (def.IsMedicine && ModSettings_QualityFramework.medQuality) minQuality = ModSettings_QualityFramework.minMedQuality;
            else if (def.ingestible != null)
            {
                if (def.ingestible.IsMeal && ModSettings_QualityFramework.mealQuality) minQuality = ModSettings_QualityFramework.minMealQuality;
                else if (def.IsMeat && ModSettings_QualityFramework.meatQuality) minQuality = ModSettings_QualityFramework.minMeatQuality;
                else if (def.IsAnimalProduct && ModSettings_QualityFramework.apQuality) minQuality = ModSettings_QualityFramework.minAPQuality;
                else if (def.IsDrug && ModSettings_QualityFramework.drugQuality) minQuality = ModSettings_QualityFramework.minDrugQuality;
                else if (ModSettings_QualityFramework.produceQuality) minQuality = ModSettings_QualityFramework.minProduceQuality;
            }
            else if (def.IsEdifice() && ModSettings_QualityFramework.edificeQuality) minQuality = ModSettings_QualityFramework.minEdificeQuality;
            else if (def.IsShell && ModSettings_QualityFramework.shellQuality) minQuality = ModSettings_QualityFramework.minShellQuality;


            return (QualityCategory)minQuality;
        }

        public static QualityCategory GetMaxQuality(Thing thing)
        {
            //Art can always be legendary
            if (thing.TryGetComp<CompArt>() != null)
            {
                return QualityCategory.Legendary;
            }

            ThingDef def = thing.def;
            int maxQuality = 4;

            if (def.IsWorkTable && !def.IsBlueprint && ModSettings_QualityFramework.workQuality) maxQuality = ModSettings_QualityFramework.maxWorkQuality;
            else if (def.IsStuff && ModSettings_QualityFramework.stuffQuality) maxQuality = ModSettings_QualityFramework.maxStuffQuality;
            else if (def.IsMedicine && ModSettings_QualityFramework.medQuality) maxQuality = ModSettings_QualityFramework.maxMedQuality;
            else if (def.ingestible != null)
            {
                if (def.ingestible.IsMeal && ModSettings_QualityFramework.mealQuality) maxQuality = ModSettings_QualityFramework.maxMealQuality;
                else if (def.IsMeat && ModSettings_QualityFramework.meatQuality) maxQuality = ModSettings_QualityFramework.maxMeatQuality;
                else if (def.IsAnimalProduct && ModSettings_QualityFramework.apQuality) maxQuality = ModSettings_QualityFramework.maxAPQuality;
                else if (def.IsDrug && ModSettings_QualityFramework.drugQuality) maxQuality = ModSettings_QualityFramework.maxDrugQuality;
                else if (ModSettings_QualityFramework.produceQuality) maxQuality = ModSettings_QualityFramework.maxProduceQuality;
            }
            else if (def.IsEdifice() && ModSettings_QualityFramework.edificeQuality) maxQuality = ModSettings_QualityFramework.maxEdificeQuality;
            else if (def.IsShell && ModSettings_QualityFramework.shellQuality) maxQuality = ModSettings_QualityFramework.maxShellQuality;


            return (QualityCategory)(Math.Min(maxQuality, tableQuality + 2));
        }


        public static bool CheckCreative(Thing product, Pawn worker)
        {

        }
    }
}
*/
/*

GetMaxQuality(def);
if (maxQuality) < 5
    inspired = false;
    skip inspiration check
    inspiration = false;
    skip end 
*/