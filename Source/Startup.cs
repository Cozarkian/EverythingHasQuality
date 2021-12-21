using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using Verse;
using Verse.AI;
using HarmonyLib;

namespace QualityEverything
{
    [StaticConstructorOnStartup]
    public class Startup
    {

        public static bool stuffPatchRan = false;

        static Startup()
        {
            RunHarmony(); 
            Quality_CompPatch.DefPatch(); 
        }

        public static void RunHarmony()
        {
            //Log.Message("QEverything: Starting harmony");
            Harmony harmony = new Harmony("rimworld.qualityframework");
            if (ModSettings_QEverything.stuffQuality || ModSettings_QEverything.indivStuff)
            {
                stuffPatchRan = true;
                MethodInfo mJump = AccessTools.FindIncludingInnerTypes(typeof(Toils_Haul), t => AccessTools.FirstMethod(t, m => m.Name.Contains("JumpIfAlsoCollectingNextTargetInQueue") && m.ReturnType == typeof(void)));
                harmony.Patch(mJump, null, null, new HarmonyMethod(typeof(Quality_Construction), "CollectNextTarget_Transpiler"));

                Type fixes = typeof(TraderFixes);
                harmony.Patch(AccessTools.Method(typeof(ThingWithComps), "PostGeneratedForTrader"), null, new HarmonyMethod(fixes, "TraderSilverFix"));
                harmony.Patch(AccessTools.PropertyGetter(typeof(Tradeable), "IsCurrency"), null, new HarmonyMethod(fixes, "IsCurrencyFix"));
                harmony.Patch(AccessTools.PropertyGetter(typeof(TradeDeal), "CurrencyTradeable"), new HarmonyMethod(fixes, "QualityCurrencyTradeable"));
                harmony.Patch(AccessTools.Method(typeof(Tradeable), "InitPriceDataIfNeeded"), new HarmonyMethod(fixes, "SilverValueFix"));
                //harmony.Patch(AccessTools.Method(typeof(Dialog_Trade), "CacheTradeables"), null, new HarmonyMethod(fixes, "SortSilver"));
                harmony.Patch(AccessTools.Method(typeof(TransferableUIUtility), "DefaultListOrderPriority", new Type[] { typeof(ThingDef) }), new HarmonyMethod(fixes, "SortSilver"));
            }
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
