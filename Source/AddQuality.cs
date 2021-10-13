using System;
using UnityEngine;
using RimWorld;
using Verse;
using HarmonyLib;

namespace QualityFramework
{
    [HarmonyPatch]
    public class AddQuality
    {
        [HarmonyPatch(typeof(QuestManager), "Notify_PlantHarvested")]
        [HarmonyPrefix]
        public static void HarvestQuality(Pawn worker, Thing harvested)
        {
            CompQuality compQuality = harvested.TryGetComp<CompQuality>();
            if (compQuality != null && worker.skills.GetSkill(SkillDefOf.Plants) != null)
            {
                int harvestSkill = worker.skills.GetSkill(SkillDefOf.Plants).Level;
                bool inspired = worker.InspirationDef == DefOf_QualityFramework.QF_Inspired_Harvesting;
                QualityCategory quality = RimWorld.QualityUtility.GenerateQualityCreatedByPawn(harvestSkill, inspired);
                if (harvested.def.IsStuff)
                {
                    if (quality < (QualityCategory)ModSettings_QualityFramework.minStuffQuality) 
                        quality = (QualityCategory)ModSettings_QualityFramework.minStuffQuality;
                    if (quality > (QualityCategory)ModSettings_QualityFramework.maxStuffQuality) 
                        quality = (QualityCategory)ModSettings_QualityFramework.maxStuffQuality;
                }
                else if (harvested.def.IsIngestible)
                {
                    if (quality < (QualityCategory)ModSettings_QualityFramework.minProduceQuality) 
                        quality = (QualityCategory)ModSettings_QualityFramework.minProduceQuality;
                    if (quality > (QualityCategory)ModSettings_QualityFramework.maxProduceQuality) 
                        quality = (QualityCategory)ModSettings_QualityFramework.maxProduceQuality;
                }
                compQuality.SetQuality(quality, ArtGenerationContext.Colony);
            }
        }

        [HarmonyPatch(typeof(GenRecipe), "PostProcessProduct")]
        [HarmonyPrefix]
        public static void MeatQuality(Thing product)
        {
            if (product.def.IsMeat)
            {
                CompQuality comp = product.TryGetComp<CompQuality>();
                if (comp != null)
                {
                    QualityCategory quality = comp.Quality;
                    if (quality < QualityCategory.Normal)
                        quality = QualityCategory.Normal;
                    if (quality > QualityCategory.Excellent)
                        quality = QualityCategory.Excellent;
                    comp.SetQuality(quality, ArtGenerationContext.Colony);
                }
            }
        }

    }
}