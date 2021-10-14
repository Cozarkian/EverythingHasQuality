using System;
using System.Reflection;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;
using HarmonyLib;

namespace QualityFramework
{
    class Mod_QualityFramework : Mod
    {
        Listing_Standard listingStandard = new Listing_Standard();

        public Mod_QualityFramework(ModContentPack content) : base(content)
        {
            GetSettings<ModSettings_QualityFramework>();
            Harmony harmony = new Harmony("rimworld.qualityframework");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public override string SettingsCategory()
        {
            return "Quality Framework";
        }

        /*
        public override void WriteSettings()
        {
            DefDatabase<InspirationDef>.GetNamedSilentFail("QF_Inspired_Harvesting").baseCommonality = Convert.ToInt32(ModSettings_QualityFramework.inspiredHarvesting);
            //Log.Message("Inspired Harvesting commonality is " + DefDatabase<InspirationDef>.GetNamed("QF_Inspired_Harvesting").baseCommonality.ToString());
            base.WriteSettings();
        }
        */

        public override void DoSettingsWindowContents(Rect inRect)
        {
            //Log.Message("Width is " + inRect.width.ToString());
            Rect rect = new Rect(5f, 50f, inRect.width * .35f, inRect.height);
            listingStandard.Begin(rect);
            listingStandard.Label("QFramework.Enable".Translate());
            listingStandard.GapLine();
            listingStandard.CheckboxLabeled("     " + "QFramework.Work".Translate(), ref ModSettings_QualityFramework.workQuality);
            listingStandard.CheckboxLabeled("     " + "QFramework.Edifice".Translate(), ref ModSettings_QualityFramework.edificeQuality);
            listingStandard.CheckboxLabeled("     " + "QFramework.Stuff".Translate(), ref ModSettings_QualityFramework.stuffQuality);
            listingStandard.CheckboxLabeled("     " + "QFramework.Meal".Translate(), ref ModSettings_QualityFramework.mealQuality);
            listingStandard.CheckboxLabeled("     " + "QFramework.Ingredients".Translate(), ref ModSettings_QualityFramework.ingredientQuality);
            listingStandard.CheckboxLabeled("     " + "QFramework.Drugs".Translate(), ref ModSettings_QualityFramework.drugQuality);
            listingStandard.CheckboxLabeled("     " + "QFramework.Med".Translate(), ref ModSettings_QualityFramework.medQuality);
            listingStandard.CheckboxLabeled("     " + "QFramework.Manuf".Translate(), ref ModSettings_QualityFramework.manufQuality);
            //listingStandard.CheckboxLabeled("     " + "QFramework.Shells".Translate(), ref ModSettings_QualityFramework.shellQuality);
            //listingStandard.CheckboxLabeled("QFramework.".Translate(), ref ModSettings_QualityFramework);
            //listingStandard.CheckboxLabeled("QFramework.".Translate(), ref ModSettings_QualityFramework);

            listingStandard.GapLine();
            listingStandard.CheckboxLabeled("QFramework.Materials".Translate(), ref ModSettings_QualityFramework.useMaterialQuality);
            listingStandard.CheckboxLabeled("QFramework.Tables".Translate(), ref ModSettings_QualityFramework.useTableQuality);
            listingStandard.GapLine();

            listingStandard.CheckboxLabeled("QFramework.SkilledAnimals".Translate(), ref ModSettings_QualityFramework.skilledAnimals);
            listingStandard.CheckboxLabeled("QFramework.SkilledButchering".Translate(), ref ModSettings_QualityFramework.skilledButchering);
            listingStandard.CheckboxLabeled("QFramework.SkilledHarvesting".Translate(), ref ModSettings_QualityFramework.skilledHarvesting);
            listingStandard.CheckboxLabeled("QFramework.SkilledMining".Translate(), ref ModSettings_QualityFramework.skilledMining);
            listingStandard.CheckboxLabeled("QFramework.SkilledStoneCutting".Translate(), ref ModSettings_QualityFramework.skilledStoneCutting);
            listingStandard.End();

            //Column 2
            Rect rectMin = new Rect(325f, 50f, inRect.width * .3f, inRect.height);
            listingStandard.Begin(rectMin);
            listingStandard.Gap(24);
            listingStandard.GapLine();
            if (ModSettings_QualityFramework.workQuality)
            {
                string labelWork = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QualityFramework.minWorkQuality).ToString();
                string minWorkBuffer = ModSettings_QualityFramework.minWorkQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelWork, ref ModSettings_QualityFramework.minWorkQuality, ref minWorkBuffer, 1, 0, 3);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QualityFramework.edificeQuality)
            {
                string labelConstruction = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QualityFramework.minEdificeQuality).ToString();
                string minConstructionBuffer = ModSettings_QualityFramework.minEdificeQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelConstruction, ref ModSettings_QualityFramework.minEdificeQuality, ref minConstructionBuffer, 1, 0, 3);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QualityFramework.stuffQuality)
            {
                string labelStuff = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QualityFramework.minStuffQuality).ToString();
                string minStuffBuffer = ModSettings_QualityFramework.minStuffQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelStuff, ref ModSettings_QualityFramework.minStuffQuality, ref minStuffBuffer, 1, 0, 3);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QualityFramework.mealQuality)
            {
                string labelMeals = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QualityFramework.minMealQuality).ToString();
                string minMealBuffer = ModSettings_QualityFramework.minMealQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelMeals, ref ModSettings_QualityFramework.minMealQuality, ref minMealBuffer, 1, 0, 3);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QualityFramework.ingredientQuality)
            {
                string labelIng = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QualityFramework.minIngQuality).ToString();
                string minIngBuffer = ModSettings_QualityFramework.minIngQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelIng, ref ModSettings_QualityFramework.minIngQuality, ref minIngBuffer, 1, 0, 3);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QualityFramework.drugQuality)
            {
                string labelDrug = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QualityFramework.minDrugQuality).ToString();
                string minDrugBuffer = ModSettings_QualityFramework.minDrugQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelDrug, ref ModSettings_QualityFramework.minDrugQuality, ref minDrugBuffer, 1, 0, 3);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QualityFramework.medQuality)
            {
                string labelMed = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QualityFramework.minMedQuality).ToString();
                string minMedBuffer = ModSettings_QualityFramework.minMedQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelMed, ref ModSettings_QualityFramework.minMedQuality, ref minMedBuffer, 1, 0, 3);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QualityFramework.manufQuality)
            {
                string labelManuf = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QualityFramework.minManufQuality).ToString();
                string minManufBuffer = ModSettings_QualityFramework.minManufQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelManuf, ref ModSettings_QualityFramework.minManufQuality, ref minManufBuffer, 1, 0, 3);
            }
            else listingStandard.Gap(24);
            /*if (ModSettings_QualityFramework.shellQuality)
            {
                string labelShell = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QualityFramework.minShellQuality).ToString();
                string minShellBuffer = ModSettings_QualityFramework.minShellQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelShell, ref ModSettings_QualityFramework.minShellQuality, ref minShellBuffer, 1, 0, 3);
            }
            else listingStandard.Gap(24);*/

            listingStandard.GapLine();
            if (ModSettings_QualityFramework.useMaterialQuality || ModSettings_QualityFramework.useTableQuality)
            {
                listingStandard.Gap(12);
                string labelStd = "QFramework.Standard".Translate() + ((QualityCategory)ModSettings_QualityFramework.stdSupplyQuality).ToString();
                string stdBuffer = ModSettings_QualityFramework.stdSupplyQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelStd, ref ModSettings_QualityFramework.stdSupplyQuality, ref stdBuffer, 1, 0, 6);
                listingStandard.Gap(12);
            }
            else listingStandard.Gap(48);
            listingStandard.GapLine();
            listingStandard.End();

            //Column 3
            Rect maxRect = new Rect(600f, 50f, inRect.width * .3f, inRect.height);
            listingStandard.Begin(maxRect);
            listingStandard.Gap(24);
            listingStandard.GapLine();
            if (ModSettings_QualityFramework.workQuality)
            {
                string labelWork2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QualityFramework.maxWorkQuality).ToString();
                string maxWorkBuffer = ModSettings_QualityFramework.maxWorkQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelWork2, ref ModSettings_QualityFramework.maxWorkQuality, ref maxWorkBuffer, 1, 3, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QualityFramework.edificeQuality)
            {
                string labelConstruction2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QualityFramework.maxEdificeQuality).ToString();
                string maxConstructionBuffer = ModSettings_QualityFramework.maxEdificeQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelConstruction2, ref ModSettings_QualityFramework.maxEdificeQuality, ref maxConstructionBuffer, 1, 3, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QualityFramework.stuffQuality)
            {
                string labelStuff2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QualityFramework.maxStuffQuality).ToString();
                string maxStuffBuffer = ModSettings_QualityFramework.maxStuffQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelStuff2, ref ModSettings_QualityFramework.maxStuffQuality, ref maxStuffBuffer, 1, 3, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QualityFramework.mealQuality)
            {
                string labelMeals2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QualityFramework.maxMealQuality).ToString();
                string maxMealBuffer = ModSettings_QualityFramework.maxMealQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelMeals2, ref ModSettings_QualityFramework.maxMealQuality, ref maxMealBuffer, 1, 3, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QualityFramework.ingredientQuality)
            {
                string labelIng2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QualityFramework.maxIngQuality).ToString();
                string maxIngBuffer = ModSettings_QualityFramework.maxIngQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelIng2, ref ModSettings_QualityFramework.maxIngQuality, ref maxIngBuffer, 1, 3, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QualityFramework.drugQuality)
            {
                string labelDrug2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QualityFramework.maxDrugQuality).ToString();
                string maxDrugBuffer = ModSettings_QualityFramework.maxDrugQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelDrug2, ref ModSettings_QualityFramework.maxDrugQuality, ref maxDrugBuffer, 1, 3, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QualityFramework.medQuality)
            {
                string labelMed2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QualityFramework.maxMedQuality).ToString();
                string maxMedBuffer = ModSettings_QualityFramework.maxMedQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelMed2, ref ModSettings_QualityFramework.maxMedQuality, ref maxMedBuffer, 1, 3, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QualityFramework.manufQuality)
            {
                string labelManuf2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QualityFramework.maxManufQuality).ToString();
                string maxManufBuffer = ModSettings_QualityFramework.maxManufQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelManuf2, ref ModSettings_QualityFramework.maxManufQuality, ref maxManufBuffer, 1, 3, 6);
            }
            else listingStandard.Gap(24);
            /*if (ModSettings_QualityFramework.shellQuality)
            {
                string labelShell2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QualityFramework.maxShellQuality).ToString();
                string maxShellBuffer = ModSettings_QualityFramework.maxShellQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelShell2, ref ModSettings_QualityFramework.maxShellQuality, ref maxShellBuffer, 1, 3, 6);
            }
            else listingStandard.Gap(24);*/

            listingStandard.GapLine();
            if (ModSettings_QualityFramework.useMaterialQuality || ModSettings_QualityFramework.useTableQuality)
            {
                listingStandard.Gap(15);
                string midLabel = (1 - ModSettings_QualityFramework.tableFactor).ToStringPercent() + " / " + ModSettings_QualityFramework.tableFactor.ToStringPercent();
                ModSettings_QualityFramework.tableFactor = Widgets.HorizontalSlider(listingStandard.GetRect(23f), ModSettings_QualityFramework.tableFactor, 0, 1f, false, midLabel, "Materials", "Work Table");
                listingStandard.Gap(10);
            }
            else listingStandard.Gap(48);
            listingStandard.GapLine();
            listingStandard.End();
        }

        public void LabeledIntEntry(Rect rect, string label, ref int value, ref string editBuffer, int multiplier, int min, int max)
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
    }
}

