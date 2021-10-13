using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (ModSettings_QFramework.stuffQuality)
            {
                if ((inspiration == DefOf_QFramework.QF_Inspired_Butchering && ModSettings_QFramework.inspiredButchering && ModSettings_QFramework.skilledButchering) ||
                    (inspiration == DefOf_QFramework.QF_Inspired_Gathering && ModSettings_QFramework.inspiredGathering && ModSettings_QFramework.skilledAnimals) ||
                    (inspiration == DefOf_QFramework.QF_Inspired_Harvesting && ModSettings_QFramework.inspiredHarvesting && ModSettings_QFramework.skilledHarvesting) ||
                    (inspiration == DefOf_QFramework.QF_Inspired_Mining && ModSettings_QFramework.inspiredMining && ModSettings_QFramework.skilledMining) ||
                    (inspiration == DefOf_QFramework.QF_Inspired_Stonecutting && ModSettings_QFramework.inspiredStonecutting && ModSettings_QFramework.skilledStoneCutting))
                {
                    __result = 0f;
                }
            }
            else if (inspiration == DefOf_QFramework.QF_Inspired_Chemistry && 
                    ModSettings_QFramework.inspiredChemistry && 
                    (ModSettings_QFramework.drugQuality || ModSettings_QFramework.medQuality))
            {
                 __result = 0f;
            }
            else if ((inspiration == DefOf_QFramework.QF_Inspired_Construction && ModSettings_QFramework.inspiredConstruction && ModSettings_QFramework.edificeQuality) ||
                     (inspiration == DefOf_QFramework.QF_Inspired_Cooking && ModSettings_QFramework.inspiredCooking && ModSettings_QFramework.mealQuality))
            {
                __result = 0f;
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
                if (thingDef.IsWithinCategory(ThingCategoryDefOf.StoneBlocks)) return DefOf_QFramework.QF_Inspired_Stonecutting;
            }
            else if (relevantSkill == SkillDefOf.Cooking)
            {
                if (thingDef.IsWithinCategory(ThingCategoryDefOf.FoodMeals)) return DefOf_QFramework.QF_Inspired_Cooking;
                else if (thingDef.IsMeat || thingDef.IsStuff) return DefOf_QFramework.QF_Inspired_Butchering;
            }
            else if (relevantSkill == SkillDefOf.Animals) return DefOf_QFramework.QF_Inspired_Gathering;
            else if (relevantSkill == SkillDefOf.Plants) return DefOf_QFramework.QF_Inspired_Harvesting;
            else if (relevantSkill == SkillDefOf.Mining) return DefOf_QFramework.QF_Inspired_Mining;
            return null;
        }
    }
}
