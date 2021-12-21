using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;

namespace QualityEverything
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

        public static void PopulateStuff()
        {
            ThingDef def;
            bool hasComp;
            for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
            {
                def = DefDatabase<ThingDef>.AllDefsListForReading[i];
                hasComp = def.HasComp(typeof(CompQuality));
                if (def.IsStuff)
                {
                    if (!ModSettings_QEverything.stuffDict.ContainsKey(def.defName)) ModSettings_QEverything.stuffDict.Add(def.defName, hasComp);
                }
            }
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
                    if (!def.IsBlueprint && !def.IsFrame && !ModSettings_QEverything.bldgDict.ContainsKey(def.defName)) ModSettings_QEverything.bldgDict.Add(def.defName, hasComp);
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
                    if (!ModSettings_QEverything.weapDict.ContainsKey(def.defName)) ModSettings_QEverything.weapDict.Add(def.defName, hasComp);
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
                    if (!ModSettings_QEverything.appDict.ContainsKey(def.defName)) ModSettings_QEverything.appDict.Add(def.defName, hasComp);
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
                if (def.IsWithinCategory(ThingCategoryDefOf.Manufactured) || def.IsDrug || def.IsMedicine || def.IsIngestible)
                {
                    if (def.IsStuff || def.IsCorpse || def.plant != null || def.IsShell) continue;
                    else if (!ModSettings_QEverything.otherDict.ContainsKey(def.defName)) ModSettings_QEverything.otherDict.Add(def.defName, hasComp);
                }
            }
            //Log.Message("Other dictionary has " + ModSettings_QFramework.otherDict.Count + " items");
        }

        public static void ApplySettingsChanges()
        {
            if (Startup.stuffPatchRan && !ModSettings_QEverything.stuffQuality && !ModSettings_QEverything.indivStuff)
            {
                Find.WindowStack.Add(new Window_RestartWarning("QEverything.RestartStuff".Translate()));
                return;
            }
            else if (!Startup.stuffPatchRan && (ModSettings_QEverything.stuffQuality || ModSettings_QEverything.indivStuff))
            {
                Find.WindowStack.Add(new Window_RestartWarning("QEverything.RestartStuff".Translate()));
                return;
            }
            Quality_CompPatch.DefPatch();
            Quality_CompPatch.ApplyNewQuality();
            Find.WindowStack.Add(new Window_RestartWarning("QEverything.Restart".Translate()));
        }

        public static void RestoreDefaults()
        {
            ModSettings_QEverything.useMaterialQuality = true;
            ModSettings_QEverything.useTableQuality = true;
            ModSettings_QEverything.useSkillReq = true;
            ModSettings_QEverything.stdSupplyQuality = 0;
            ModSettings_QEverything.tableFactor = .4f;

            ModSettings_QEverything.inspiredButchering = true;
            ModSettings_QEverything.inspiredChemistry = true;
            ModSettings_QEverything.inspiredCooking = true;
            ModSettings_QEverything.inspiredConstruction = true;
            ModSettings_QEverything.inspiredGathering = true;
            ModSettings_QEverything.inspiredHarvesting = true;
            ModSettings_QEverything.inspiredMining = true;
            ModSettings_QEverything.inspiredStonecutting = true;

            ModSettings_QEverything.skilledAnimals = false;
            ModSettings_QEverything.skilledButchering = false;
            ModSettings_QEverything.skilledHarvesting = false;
            ModSettings_QEverything.skilledMining = false;
            ModSettings_QEverything.skilledStoneCutting = false;

            ModSettings_QEverything.edificeQuality = true;
            ModSettings_QEverything.minEdificeQuality = 0;
            ModSettings_QEverything.maxEdificeQuality = 4;

            ModSettings_QEverything.workQuality = true;
            ModSettings_QEverything.minWorkQuality = 0;
            ModSettings_QEverything.maxWorkQuality = 4;

            ModSettings_QEverything.securityQuality = true;
            ModSettings_QEverything.minSecurityQuality = 0;
            ModSettings_QEverything.maxSecurityQuality = 4;

            ModSettings_QEverything.stuffQuality = true;
            ModSettings_QEverything.minStuffQuality = 0;
            ModSettings_QEverything.maxStuffQuality = 4;

            ModSettings_QEverything.ingredientQuality = true;
            ModSettings_QEverything.minIngQuality = 2;
            ModSettings_QEverything.maxIngQuality = 4;
            ModSettings_QEverything.minTastyQuality = 0;
            ModSettings_QEverything.maxTastyQuality = 4;

            ModSettings_QEverything.mealQuality = false;
            ModSettings_QEverything.minMealQuality = 0;
            ModSettings_QEverything.maxMealQuality = 4;

            ModSettings_QEverything.drugQuality = false;
            ModSettings_QEverything.minDrugQuality = 0;
            ModSettings_QEverything.maxDrugQuality = 4;

            ModSettings_QEverything.medQuality = false;
            ModSettings_QEverything.minMedQuality = 0;
            ModSettings_QEverything.maxMedQuality = 4;

            ModSettings_QEverything.manufQuality = true;
            ModSettings_QEverything.minManufQuality = 0;
            ModSettings_QEverything.maxManufQuality = 4;

            ModSettings_QEverything.apparelQuality = false;
            ModSettings_QEverything.minApparelQuality = 0;
            ModSettings_QEverything.maxApparelQuality = 6;

            ModSettings_QEverything.weaponQuality = false;
            ModSettings_QEverything.minWeaponQuality = 0;
            ModSettings_QEverything.maxWeaponQuality = 6;

            ModSettings_QEverything.shellQuality = false;
            ModSettings_QEverything.minShellQuality = 0;
            ModSettings_QEverything.maxShellQuality = 4;

            //FixSilver();
        }

        /*public static void FixSilver()
        {
            Log.Message("Fixing Silver");
            ThingDef silver = ThingDefOf.Silver;
            if (!silver.HasComp(typeof(CompQuality)))
            {
                return;
            }
            foreach (Map map in Find.Maps)
            {
                foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForDef(ThingDefOf.Silver)))
                {
                    thing.TryGetComp<CompQuality>().SetQuality(QualityCategory.Normal, ArtGenerationContext.Outsider);
                }
            }
        }*/
    }
}
