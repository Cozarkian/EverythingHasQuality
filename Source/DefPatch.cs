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
                    if (def.building != null && def.Claimable)
                    {
                        if (def.IsWorkTable)
                        {
                            if (ModSettings_QFramework.workQuality) def.comps.Add(comp);
                        }
                        else if (def.IsWithinCategory(ThingCategoryDef.Named("BuildingsSecurity")) || def.building.IsTurret)
                        {
                            if (ModSettings_QFramework.securityQuality) def.comps.Add(comp);
                        }
                        else if (ModSettings_QFramework.edificeQuality) def.comps.Add(comp);
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
                    else if (def.IsShell || def.IsWithinCategory(ThingCategoryDef.Named("Grenades")))
                    {
                        if (ModSettings_QFramework.shellQuality) def.comps.Add(comp);
                    }
                    else if (def.IsWeapon && ModSettings_QFramework.weaponQuality)
                    {
                        def.comps.Add(comp);
                    }
                    else if (def.IsApparel && ModSettings_QFramework.apparelQuality)
                    {
                        def.comps.Add(comp);
                    }
                    //Log.Message(def.label + " now has quality");
                }
            }
        }
    }
}
