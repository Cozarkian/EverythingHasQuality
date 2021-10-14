using RimWorld;
using RimWorld.Planet;
using Verse;

namespace QualityFramework
{
    [StaticConstructorOnStartup]
    public class DefPatch
    {
        static DefPatch()
        {
            ThingDef def;
            CompProperties comp = new CompProperties();
            comp.compClass = typeof(CompQuality);
            for (int m = 0; m < DefDatabase<ThingDef>.AllDefsListForReading.Count; m++)
            {
                def = DefDatabase<ThingDef>.AllDefsListForReading[m];
                if (!def.HasComp(typeof(CompQuality)))
                {
                    if (def.IsBuildingArtificial)
                    {
                        if (def.IsWorkTable && ModSettings_QFramework.workQuality)
                        {
                            def.comps.Add(comp);
                        }
                        else if (ModSettings_QFramework.edificeQuality)
                        {
                            def.comps.Add(comp);
                        }
                    }
                    else if (def.IsStuff && ModSettings_QFramework.stuffQuality)
                    {
                        def.comps.Add(comp);
                    }
                    else if (def.IsDrug && !ModSettings_QFramework.drugQuality)
                    {
                        def.comps.Add(comp);
                    }
                    else if (def.IsMedicine && ModSettings_QFramework.medQuality)
                    {
                        def.comps.Add(comp);
                    }
                    else if (def.IsWithinCategory(ThingCategoryDefOf.Manufactured) && ModSettings_QFramework.manufQuality)
                    {
                        def.comps.Add(comp);
                    }
                    else if (def.IsIngestible && def.plant == null && !def.IsCorpse)
                    {
                        //Log.Message("Checking ingestible");
                        if (def.IsWithinCategory(ThingCategoryDefOf.FoodMeals) && !ModSettings_QFramework.mealQuality)
                            continue;
                        if ((def.IsMeat || def.IsAnimalProduct) && !ModSettings_QFramework.ingredientQuality)
                            continue;
                        if (def.IsNutritionGivingIngestible && !ModSettings_QFramework.ingredientQuality)
                            continue;
                        def.comps.Add(comp);
                    }
                    /*else if ((def.IsShell || def.thingCategories.Contains(ThingCategoryDefOf.gren && ModSettings_QualityFramework.shellQuality)
                    {
                        def.comps.Add(comp);
                        def.BaseMarketValue = def.BaseMarketValue * .8f;
                    }*/
                    //Log.Message(def.label + " now has quality");
                }
            }

        }
    }
}
