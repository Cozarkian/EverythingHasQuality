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
    [HarmonyPatch]
    class MedicalQuality
    {
        [HarmonyPatch(typeof(Recipe_Surgery), "GetAverageMedicalPotency"]
        [HarmonyPostfix]
        public static void Postfix (List<Thing> ingredients)
        {

        }


        private static float QualityFactor(QualityCategory quality)
        {
            switch (quality)
            {
                case QualityCategory.Awful:
                    return -.4f;
                case QualityCategory.Poor:
                    return -.2f;
                case QualityCategory.Good:
                    return .2f;
                case QualityCategory.Excellent:
                    return .4f;
                case QualityCategory.Masterwork:
                    return .7f;
                case QualityCategory.Legendary:
                    return 1.0f;
                default:
                    return 0f;
            }
        }
    }
}
