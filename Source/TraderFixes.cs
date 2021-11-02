using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using HarmonyLib;

namespace QualityFramework
{ 
    public class TraderFixes
    {
        public static void TraderSilverFix(ThingWithComps __instance)
        {
            if (__instance.def == ThingDefOf.Silver)
            {
                CompQuality comp = __instance.GetComp<CompQuality>();
                if (comp != null) comp.SetQuality(QualityCategory.Normal, ArtGenerationContext.Outsider);
            }
        }

        public static bool CurrencyFix(bool __result, Tradeable __instance)
        {
            if (__result && __instance.ThingDef == ThingDefOf.Silver && __instance.ThingDef.HasComp(typeof(CompQuality)))
            {
                QualityCategory qc;
                __instance.AnyThing.TryGetQuality(out qc);
                return (qc == QualityCategory.Normal);
            }
            return __result;
        }

        public static bool QualityCurrencyTradeable(ref Tradeable __result, TradeDeal __instance, List<Tradeable> ___tradeables)
        {
            for (int i = 0; i < ___tradeables.Count; i++)
            {
                Tradeable tradeable = ___tradeables[i];
                if (tradeable.IsFavor && TradeSession.TradeCurrency == TradeCurrency.Favor)
                {
                    __result = tradeable;
                    return false;
                }
                if (tradeable.ThingDef == ThingDefOf.Silver)
                {
                    QualityCategory qc;
                    if (tradeable.AnyThing.TryGetQuality(out qc) && qc == QualityCategory.Normal)
                    {
                        __result = tradeable;
                        return false;
                    }
                }
            }
            __result = null;
            return false;
        }

        public static bool SilverValueFix(Tradeable __instance)
        {
            if (__instance.ThingDef == ThingDefOf.Silver)
            {
                AccessTools.Field(typeof(Tradeable), "pricePlayerBuy").SetValue(__instance, __instance.BaseMarketValue);
                AccessTools.Field(typeof(Tradeable), "pricePlayerSell").SetValue(__instance, __instance.BaseMarketValue);
                return false;
            }
            return true;
        }

        public static void SortSilver(ref float __result, ThingDef def)
        { 
            if (def == ThingDefOf.Silver)
            {
                __result = 0f;
            }        
        }
    }
}
