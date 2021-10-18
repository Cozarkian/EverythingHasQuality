using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;

namespace QualityFramework
{
    class Mod_SettingsUtility
    {
        public static void LabeledIntEntry(Rect rect, string label, ref int value, ref string editBuffer, int multiplier, int min, int max)
        {
            int num = (int)rect.width / 15;
            Widgets.Label(rect, label);
            if (Widgets.ButtonText(new Rect(rect.xMax - 90f, rect.yMin, 25f, rect.height), (-1 * multiplier).ToString(), true, true, true))
            {
                value -= GenUI.CurrentAdjustmentMultiplier();
                editBuffer = value.ToString();
                SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
            }
            if (Widgets.ButtonText(new Rect(rect.xMax - 30f, rect.yMin, 25f, rect.height), "+" + multiplier.ToString(), true, true, true))
            {
                value += multiplier * GenUI.CurrentAdjustmentMultiplier();
                editBuffer = value.ToString();
                SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
            }
            Widgets.TextFieldNumeric<int>(new Rect(rect.xMax - 60f, rect.yMin, 25f, rect.height), ref value, ref editBuffer, min, max);
        }

        public static void PopulateDictionary()
        {
            ThingDef def;
            bool hasComp;
            for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
            {
                def = DefDatabase<ThingDef>.AllDefsListForReading[i];
                hasComp = def.HasComp(typeof(CompQuality));
                if (def.IsStuff || def.IsCorpse || def.plant == null) continue;
                if (def.building != null)
                {
                    if (ModSettings_QFramework.indivBuildings && !def.IsBlueprint && !def.IsFrame && !ModSettings_QFramework.bldgDict.ContainsKey(def.defName))
                        ModSettings_QFramework.bldgDict.Add(def.defName, hasComp);
                }
                else if ((def.IsWeapon || def.IsShell || def.IsWithinCategory(ThingCategoryDef.Named("Grenades"))) && !def.IsIngestible)
                {
                    if (ModSettings_QFramework.indivWeapons && !ModSettings_QFramework.weapDict.ContainsKey(def.defName))
                        ModSettings_QFramework.weapDict.Add(def.defName, hasComp);
                }
                else if (def.IsApparel)
                {
                    if (ModSettings_QFramework.indivApparel && !ModSettings_QFramework.appDict.ContainsKey(def.defName))
                        ModSettings_QFramework.appDict.Add(def.defName, hasComp);
                }
                else if (def.IsWithinCategory(ThingCategoryDefOf.Manufactured) || def.IsDrug || def.IsMedicine || def.IsIngestible)
                {
                    if (ModSettings_QFramework.indivOther && !ModSettings_QFramework.otherDict.ContainsKey(def.defName)) 
                        ModSettings_QFramework.otherDict.Add(def.defName, hasComp);
                }
            }
            Log.Message("Dictionaries have been built");
        }

        public static void PopulateBuildings()
        {
            ThingDef def;
            bool hasComp;
            for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
            {
                def = DefDatabase<ThingDef>.AllDefsListForReading[i];
                hasComp = def.HasComp(typeof(CompQuality));
                if (def.building != null)
                {
                    if (!def.IsBlueprint && !def.IsFrame && !ModSettings_QFramework.bldgDict.ContainsKey(def.defName)) ModSettings_QFramework.bldgDict.Add(def.defName, hasComp);
                }
            }
        }

        public static void PopulateWeapons()
        {
            ThingDef def;
            bool hasComp;
            for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
            {
                def = DefDatabase<ThingDef>.AllDefsListForReading[i];
                hasComp = def.HasComp(typeof(CompQuality));
                if ((def.IsWeapon || def.IsShell || def.IsWithinCategory(ThingCategoryDef.Named("Grenades"))) && !def.IsIngestible && !def.IsStuff)
                {
                    //Log.Message(def.defName + " is a weapon");
                    if (!ModSettings_QFramework.weapDict.ContainsKey(def.defName)) ModSettings_QFramework.weapDict.Add(def.defName, hasComp);
                }
            }
        }

        public static void PopulateApparel()
        {
            ThingDef def;
            bool hasComp;
            for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
            {
                def = DefDatabase<ThingDef>.AllDefsListForReading[i];
                hasComp = def.HasComp(typeof(CompQuality));
                if (def.IsApparel)
                {
                    if (!ModSettings_QFramework.appDict.ContainsKey(def.defName)) ModSettings_QFramework.appDict.Add(def.defName, hasComp);
                }
            }
        }

        public static void PopulateOther()
        {
            ThingDef def;
            bool hasComp;
            for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
            {
                def = DefDatabase<ThingDef>.AllDefsListForReading[i];
                hasComp = def.HasComp(typeof(CompQuality));
                if ((def.IsWithinCategory(ThingCategoryDefOf.Manufactured) || def.IsDrug || def.IsMedicine || def.IsIngestible))
                {
                    if (def.IsStuff || def.IsCorpse || def.plant != null || def.IsShell) continue;
                    else if (!ModSettings_QFramework.otherDict.ContainsKey(def.defName)) ModSettings_QFramework.otherDict.Add(def.defName, hasComp);
                }
            }
            Log.Message("Other dictionary has " + ModSettings_QFramework.otherDict.Count + " items");
        }

        public static void RestoreDefaults()
        {
            ModSettings_QFramework.useMaterialQuality = true;
            ModSettings_QFramework.useTableQuality = true;
            ModSettings_QFramework.stdSupplyQuality = 4;
            ModSettings_QFramework.tableFactor = .4f;

            ModSettings_QFramework.inspiredButchering = true;
            ModSettings_QFramework.inspiredChemistry = true;
            ModSettings_QFramework.inspiredCooking = true;
            ModSettings_QFramework.inspiredConstruction = true;
            ModSettings_QFramework.inspiredGathering = true;
            ModSettings_QFramework.inspiredHarvesting = true;
            ModSettings_QFramework.inspiredMining = true;
            ModSettings_QFramework.inspiredStonecutting = true;

            ModSettings_QFramework.skilledAnimals = false;
            ModSettings_QFramework.skilledButchering = false;
            ModSettings_QFramework.skilledHarvesting = false;
            ModSettings_QFramework.skilledMining = false;
            ModSettings_QFramework.skilledStoneCutting = false;

            //ModSettings_QFramework.lessRandomQuality = true;
            //ModSettings_QFramework.minSkillEx = 10;
            //ModSettings_QFramework.maxSkillAw = 17;
            ModSettings_QFramework.edificeQuality = true;
            ModSettings_QFramework.minEdificeQuality = 0;
            ModSettings_QFramework.maxEdificeQuality = 4;

            ModSettings_QFramework.workQuality = true;
            ModSettings_QFramework.minWorkQuality = 0;
            ModSettings_QFramework.maxWorkQuality = 4;

            ModSettings_QFramework.securityQuality = true;
            ModSettings_QFramework.minSecurityQuality = 0;
            ModSettings_QFramework.maxSecurityQuality = 4;

            ModSettings_QFramework.stuffQuality = true;
            ModSettings_QFramework.minStuffQuality = 0;
            ModSettings_QFramework.maxStuffQuality = 4;

            ModSettings_QFramework.ingredientQuality = true;
            ModSettings_QFramework.minIngQuality = 2;
            ModSettings_QFramework.maxIngQuality = 4;
            ModSettings_QFramework.minTastyQuality = 0;
            ModSettings_QFramework.maxTastyQuality = 4;

            ModSettings_QFramework.mealQuality = false;
            ModSettings_QFramework.minMealQuality = 0;
            ModSettings_QFramework.maxMealQuality = 4;

            ModSettings_QFramework.drugQuality = false;
            ModSettings_QFramework.minDrugQuality = 0;
            ModSettings_QFramework.maxDrugQuality = 4;

            ModSettings_QFramework.medQuality = false;
            ModSettings_QFramework.minMedQuality = 0;
            ModSettings_QFramework.maxMedQuality = 4;

            ModSettings_QFramework.manufQuality = true;
            ModSettings_QFramework.minManufQuality = 0;
            ModSettings_QFramework.maxManufQuality = 4;

            ModSettings_QFramework.apparelQuality = false;
            ModSettings_QFramework.minApparelQuality = 0;
            ModSettings_QFramework.maxApparelQuality = 6;

            ModSettings_QFramework.weaponQuality = false;
            ModSettings_QFramework.minWeaponQuality = 0;
            ModSettings_QFramework.maxWeaponQuality = 6;

            ModSettings_QFramework.shellQuality = false;
            ModSettings_QFramework.minShellQuality = 0;
            ModSettings_QFramework.maxShellQuality = 4;
        }
    }
}
