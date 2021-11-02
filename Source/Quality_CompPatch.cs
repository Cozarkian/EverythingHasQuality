using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using RimWorld;
using Verse;
using HarmonyLib;

namespace QualityFramework
{
    [HarmonyPatch]
    public class Quality_CompPatch
    {
        [HarmonyPatch(typeof(CompQuality), "SetQuality")]
        [HarmonyPrefix]
        public static void ApplyQualityLimits(CompQuality __instance, ref QualityCategory q)
        {
            /*if (__instance.parent.def == ThingDefOf.Silver)
            {
                q = QualityCategory.Normal;
                return;
            }*/
            int minQuality = Quality_Generator.GetMinQuality(__instance.parent);
            int maxQuality = Quality_Generator.GetMaxQuality(__instance.parent);
            if ((int)q < minQuality) q = (QualityCategory)minQuality;
            if ((int)q > maxQuality) q = (QualityCategory)maxQuality;
        }

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
    }
}

