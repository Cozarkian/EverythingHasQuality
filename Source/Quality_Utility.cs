using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using HarmonyLib;

namespace QualityFramework
{
    [HarmonyPatch]
    public static class Quality_Utility
    {


        /*public static void CheckResourceQuality(Frame __instance)
        {
            ThingDef thingDef = __instance.def.entityDefToBuild as ThingDef;
            if (thingDef == null || !thingDef.HasComp(typeof(CompQuality)))
            {
                return;
            }
            ResourceQuality = 0;
            usesResQuality = false;
            float value = 0;
            int numIng = 0;
            for (int i = 0; i < __instance.resourceContainer.Count; i++)
            {
                Thing resource = __instance.resourceContainer[i];
                CompQuality comp = resource.TryGetComp<CompQuality>();
                if (comp != null)
                {
                    value += (int)comp.Quality * resource.stackCount;
                    numIng += resource.stackCount;
                }
            }
            if (numIng > 0)
            {
                value = value / numIng;
                Log.Message("value is " + value.ToString());
                ResourceQuality = (int)value + ((Rand.Value < value % (int)value) ? 1 : 0);
                usesResQuality = true;
            }
            Log.Message("resource quality is " + ResourceQuality.ToString());
        }

        [HarmonyPatch(typeof(CompQuality), "SetQuality")]
        [HarmonyPrefix]
        public static void ApplyQualityLimits(CompQuality __instance, ref QualityCategory q)
        {
            int minQuality = GetMinQuality(__instance.parent);
            int maxQuality = GetMaxQuality(__instance.parent);
            if ((int)q < minQuality) q = (QualityCategory)minQuality;
            if ((int)q > maxQuality) q = (QualityCategory)maxQuality;
        }

        public static InspirationDef CheckInspired(ThingDef thingDef, SkillDef relevantSkill)
        {
            Log.Message("Setting inspiration type");
            if (relevantSkill == SkillDefOf.Construction)
            {
                if (thingDef.HasComp(typeof(CompArt)))
                {
                    return InspirationDefOf.Inspired_Creativity;
                }
            }
            if (relevantSkill == SkillDefOf.Artistic || thingDef.IsApparel || thingDef.IsWeapon)
            {
                return InspirationDefOf.Inspired_Creativity;
            }
            return null;
        }*/





   }
}
