using RimWorld;
using Verse;

namespace QualityFramework
{
    [StaticConstructorOnStartup]
    public class DefPatch
    {
        static DefPatch()
        {
            ThingDef def;
            bool hasComp;
            CompProperties comp = new CompProperties();
            comp.compClass = typeof(CompQuality);
            for (int m = 0; m < DefDatabase<ThingDef>.AllDefsListForReading.Count; m++)
            {
                def = DefDatabase<ThingDef>.AllDefsListForReading[m];
                hasComp = def.HasComp(typeof(CompQuality));
                if (def.plant != null || def.IsCorpse) 
                    continue;
                else if (def.IsStuff)
                {
                    if (!hasComp && ModSettings_QFramework.stuffQuality) def.comps.Add(comp);
                    //Log.Message(def.label + " is stuff");
                }
                else if (def.building != null)
                {
                    if (!def.Claimable || def.IsBlueprint || def.IsFrame)
                    {
                        continue;
                    }
                    else if (ModSettings_QFramework.indivBuildings)
                    {
                        if (!ModSettings_QFramework.bldgDict.ContainsKey(def.defName)) ModSettings_QFramework.bldgDict.Add(def.defName, hasComp);
                        if (!hasComp && ModSettings_QFramework.bldgDict[def.defName] == true) def.comps.Add(comp);
                        else if (hasComp && ModSettings_QFramework.bldgDict[def.defName] == false) def.comps.Remove(comp);
                    }
                    else if (hasComp) continue;
                    else if (def.IsWorkTable)
                    {
                        if (ModSettings_QFramework.workQuality) def.comps.Add(comp);
                    }
                    else if (def.IsWithinCategory(ThingCategoryDef.Named("BuildingsSecurity")) || def.building.IsTurret)
                    {
                        if (ModSettings_QFramework.securityQuality) def.comps.Add(comp);
                    }
                    else if (ModSettings_QFramework.edificeQuality) def.comps.Add(comp);
                }
                else if (!def.IsIngestible && (def.IsWeapon || def.IsShell || def.IsWithinCategory(ThingCategoryDef.Named("Grenades"))))
                {
                    if (ModSettings_QFramework.indivWeapons)
                    {
                        if (!ModSettings_QFramework.weapDict.ContainsKey(def.defName)) ModSettings_QFramework.weapDict.Add(def.defName, hasComp);
                        if (!hasComp && ModSettings_QFramework.weapDict[def.defName] == true) def.comps.Add(comp);
                        else if (hasComp && ModSettings_QFramework.weapDict[def.defName] == false) def.comps.Remove(comp);
                    }
                    else if (hasComp) continue;
                    else if (def.IsShell && ModSettings_QFramework.shellQuality) def.comps.Add(comp);
                    else if (def.IsWeapon && ModSettings_QFramework.weaponQuality) def.comps.Add(comp);
                }
                else if (def.IsApparel)
                {
                    if (ModSettings_QFramework.indivApparel)
                    {
                        if (!ModSettings_QFramework.appDict.ContainsKey(def.defName)) ModSettings_QFramework.appDict.Add(def.defName, hasComp);
                        if (!hasComp && ModSettings_QFramework.appDict[def.defName] == true) def.comps.Add(comp);
                        else if (hasComp && ModSettings_QFramework.appDict[def.defName] == false) def.comps.Remove(comp);
                    }
                    else if (!hasComp && ModSettings_QFramework.apparelQuality) def.comps.Add(comp);
                }
                else if (ModSettings_QFramework.indivOther)
                {
                    if (!ModSettings_QFramework.otherDict.ContainsKey(def.defName)) ModSettings_QFramework.otherDict.Add(def.defName, hasComp);
                    if (!hasComp && ModSettings_QFramework.otherDict[def.defName] == true) def.comps.Add(comp);
                    else if (hasComp && ModSettings_QFramework.otherDict[def.defName] == false) def.comps.Remove(comp);
                }
                else if (def.IsDrug && ModSettings_QFramework.drugQuality)
                {
                    def.comps.Add(comp);
                }
                else if (def.IsMedicine) 
                {
                    if (ModSettings_QFramework.medQuality) def.comps.Add(comp);
                }
                else if (def.IsIngestible)
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
                else if (def.IsWithinCategory(ThingCategoryDefOf.Manufactured) && ModSettings_QFramework.manufQuality)
                {
                    def.comps.Add(comp);
                }
            }
                //Log.Message(def.label + " now has quality");
/*

            ThingDef def;
            CompProperties comp = new CompProperties();
            comp.compClass = typeof(CompQuality);
            for (int m = 0; m < DefDatabase<ThingDef>.AllDefsListForReading.Count; m++)
            {
                def = DefDatabase<ThingDef>.AllDefsListForReading[m];
                if (!def.HasComp(typeof(CompQuality)))
                {
                    if (def.building != null)
                    {
                        if (!def.Claimable || def.IsBlueprint)
                        {
                            //Log.Message("Skipping " + def.defName);
                            continue;
                        }
                        else if (def.IsWorkTable)
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
            }*/
        }
    }
}
