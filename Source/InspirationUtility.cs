using RimWorld;
using Verse;
using HarmonyLib;

namespace QualityFramework
{
    [HarmonyPatch(typeof(InspirationWorker), "CommonalityFor")]
    class InspirationUtility
    {
        public static float Postfix(float __result, InspirationWorker __instance)
        {
            InspirationDef inspiration = __instance.def;
            if (inspiration == DefOf_QFramework.QF_Inspired_Butchering)
            {
                if (ModSettings_QFramework.stuffQuality && ModSettings_QFramework.inspiredButchering && ModSettings_QFramework.skilledButchering)
                {
                    return __result;
                }
                return 0f;
            }
            if (inspiration == DefOf_QFramework.QF_Inspired_Gathering)
            {
                if (ModSettings_QFramework.stuffQuality && ModSettings_QFramework.inspiredGathering && ModSettings_QFramework.skilledAnimals)
                {
                    return __result;
                }
                return 0f;
            }
            if (inspiration == DefOf_QFramework.QF_Inspired_Harvesting)
            {
                if (ModSettings_QFramework.stuffQuality && ModSettings_QFramework.inspiredHarvesting && ModSettings_QFramework.skilledHarvesting)
                {
                    return __result;
                }
                return 0f;
            }
            if (inspiration == DefOf_QFramework.QF_Inspired_Mining)
            {
                if (ModSettings_QFramework.stuffQuality && ModSettings_QFramework.inspiredMining && ModSettings_QFramework.skilledMining)
                {
                    return __result;
                }
                return 0f;
            }
            if (inspiration == DefOf_QFramework.QF_Inspired_Stonecutting)
            { 
                if(ModSettings_QFramework.stuffQuality && ModSettings_QFramework.inspiredStonecutting && ModSettings_QFramework.skilledStoneCutting)
                { 
                    return __result;
                }
                return 0f;
            }
            if (inspiration == DefOf_QFramework.QF_Inspired_Chemistry)
            {
                if (ModSettings_QFramework.inspiredChemistry && (ModSettings_QFramework.drugQuality || ModSettings_QFramework.medQuality))
                {
                    return __result;
                }
                return 0f;
            }
            if (inspiration == DefOf_QFramework.QF_Inspired_Construction)
            {
                if (ModSettings_QFramework.inspiredConstruction && (ModSettings_QFramework.edificeQuality || ModSettings_QFramework.workQuality || ModSettings_QFramework.securityQuality))
                {
                    return __result;
                }
                return 0f;
 
            }
            if (inspiration == DefOf_QFramework.QF_Inspired_Cooking)
            {
                if (ModSettings_QFramework.inspiredCooking && ModSettings_QFramework.mealQuality)
                {
                    return __result;
                }
                return 0f;
            }
            return __result;
        }

        public static InspirationDef CheckInspired(ThingDef thingDef, SkillDef relevantSkill)
        {
            //Log.Message("Setting inspiration type");
            if (relevantSkill == SkillDefOf.Construction)
            {
                if (thingDef.HasComp(typeof(CompArt))) return InspirationDefOf.Inspired_Creativity;
                else return DefOf_QFramework.QF_Inspired_Construction;
            }
            else if (relevantSkill == SkillDefOf.Artistic || thingDef.IsApparel || thingDef.IsWeapon)
            {
                return InspirationDefOf.Inspired_Creativity;
            }
            else if (relevantSkill == SkillDefOf.Crafting)
            {
                if (thingDef.IsDrug || thingDef.IsMedicine) return DefOf_QFramework.QF_Inspired_Chemistry;
                if (thingDef.thingCategories.Contains(ThingCategoryDefOf.StoneBlocks)) return DefOf_QFramework.QF_Inspired_Stonecutting;
            }
            else if (relevantSkill == SkillDefOf.Cooking)
            {
                if (thingDef.thingCategories.Contains(ThingCategoryDefOf.FoodMeals)) return DefOf_QFramework.QF_Inspired_Cooking;
                else if (thingDef.IsMeat || thingDef.IsStuff) return DefOf_QFramework.QF_Inspired_Butchering;
            }
            else if (relevantSkill == SkillDefOf.Animals) return DefOf_QFramework.QF_Inspired_Gathering;
            else if (relevantSkill == SkillDefOf.Plants) return DefOf_QFramework.QF_Inspired_Harvesting;
            else if (relevantSkill == SkillDefOf.Mining) return DefOf_QFramework.QF_Inspired_Mining;
            return null;
        }
    }
}
