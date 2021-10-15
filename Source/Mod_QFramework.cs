using System;
using System.Reflection;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;
using HarmonyLib;

namespace QualityFramework
{
    class Mod_QFramework : Mod
    {
        Listing_Standard listingStandard = new Listing_Standard();

        public Mod_QFramework(ModContentPack content) : base(content)
        {
            GetSettings<ModSettings_QFramework>();
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
            Rect rect = new Rect(5f, 40f, inRect.width * .35f, inRect.height);
            listingStandard.Begin(rect);
            listingStandard.Gap(8f);
            listingStandard.Label("QFramework.Enable".Translate());
            listingStandard.GapLine();
            listingStandard.CheckboxLabeled("    " + "QFramework.Work".Translate(), ref ModSettings_QFramework.workQuality);
            listingStandard.CheckboxLabeled("    " + "QFramework.Security".Translate(), ref ModSettings_QFramework.securityQuality);
            listingStandard.CheckboxLabeled("    " + "QFramework.Edifice".Translate(), ref ModSettings_QFramework.edificeQuality);
            listingStandard.CheckboxLabeled("    " + "QFramework.Stuff".Translate(), ref ModSettings_QFramework.stuffQuality);
            listingStandard.CheckboxLabeled("    " + "QFramework.Manuf".Translate(), ref ModSettings_QFramework.manufQuality);
            listingStandard.CheckboxLabeled("    " + "QFramework.Meal".Translate(), ref ModSettings_QFramework.mealQuality);
            listingStandard.CheckboxLabeled("    " + "QFramework.Ingredients".Translate(), ref ModSettings_QFramework.ingredientQuality);
            if (ModSettings_QFramework.ingredientQuality) listingStandard.Label("      " + "QFramework.Tasty".Translate());
            listingStandard.CheckboxLabeled("    " + "QFramework.Drugs".Translate(), ref ModSettings_QFramework.drugQuality);
            listingStandard.CheckboxLabeled("    " + "QFramework.Med".Translate(), ref ModSettings_QFramework.medQuality);
            listingStandard.CheckboxLabeled("    " + "QFramework.Apparel".Translate(), ref ModSettings_QFramework.apparelQuality);
            listingStandard.CheckboxLabeled("    " + "QFramework.Weapons".Translate(), ref ModSettings_QFramework.weaponQuality);
            listingStandard.CheckboxLabeled("    " + "QFramework.Shells".Translate(), ref ModSettings_QFramework.shellQuality);
            //listingStandard.CheckboxLabeled("QFramework.".Translate(), ref ModSettings_QFramework);
            //listingStandard.CheckboxLabeled("QFramework.".Translate(), ref ModSettings_QFramework);

            listingStandard.GapLine();
            listingStandard.CheckboxLabeled("QFramework.Materials".Translate(), ref ModSettings_QFramework.useMaterialQuality);
            listingStandard.CheckboxLabeled("QFramework.Tables".Translate(), ref ModSettings_QFramework.useTableQuality);
            listingStandard.GapLine();

            listingStandard.CheckboxLabeled("QFramework.SkilledAnimals".Translate(), ref ModSettings_QFramework.skilledAnimals);
            listingStandard.CheckboxLabeled("QFramework.SkilledButchering".Translate(), ref ModSettings_QFramework.skilledButchering);
            listingStandard.CheckboxLabeled("QFramework.SkilledHarvesting".Translate(), ref ModSettings_QFramework.skilledHarvesting);
            listingStandard.CheckboxLabeled("QFramework.SkilledMining".Translate(), ref ModSettings_QFramework.skilledMining);
            listingStandard.CheckboxLabeled("QFramework.SkilledStoneCutting".Translate(), ref ModSettings_QFramework.skilledStoneCutting);

            listingStandard.End();

            //Column 2
            Rect rectMin = new Rect(325f, 40f, inRect.width * .3f, inRect.height);
            listingStandard.Begin(rectMin);
            if (listingStandard.ButtonText("Restore Defaults", null)) RestoreDefaults();
            listingStandard.GapLine();
            if (ModSettings_QFramework.workQuality)
            {
                string labelWork = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minWorkQuality).ToString();
                string minWorkBuffer = ModSettings_QFramework.minWorkQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelWork, ref ModSettings_QFramework.minWorkQuality, ref minWorkBuffer, 1, 0, 2);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.securityQuality)
            {
                string labelSecurity = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minSecurityQuality).ToString();
                string minSecurityBuffer = ModSettings_QFramework.minSecurityQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelSecurity, ref ModSettings_QFramework.minSecurityQuality, ref minSecurityBuffer, 1, 0, 2);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.edificeQuality)
            {
                string labelConstruction = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minEdificeQuality).ToString();
                string minConstructionBuffer = ModSettings_QFramework.minEdificeQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelConstruction, ref ModSettings_QFramework.minEdificeQuality, ref minConstructionBuffer, 1, 0, 2);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.stuffQuality)
            {
                string labelStuff = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minStuffQuality).ToString();
                string minStuffBuffer = ModSettings_QFramework.minStuffQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelStuff, ref ModSettings_QFramework.minStuffQuality, ref minStuffBuffer, 1, 0, 2);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.manufQuality)
            {
                string labelManuf = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minManufQuality).ToString();
                string minManufBuffer = ModSettings_QFramework.minManufQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelManuf, ref ModSettings_QFramework.minManufQuality, ref minManufBuffer, 1, 0, 2);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.mealQuality)
            {
                string labelMeals = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minMealQuality).ToString();
                string minMealBuffer = ModSettings_QFramework.minMealQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelMeals, ref ModSettings_QFramework.minMealQuality, ref minMealBuffer, 1, 0, 2);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.ingredientQuality)
            {
                string labelIng = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minIngQuality).ToString();
                string minIngBuffer = ModSettings_QFramework.minIngQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelIng, ref ModSettings_QFramework.minIngQuality, ref minIngBuffer, 1, 0, 2);

                string labelTasty = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minTastyQuality).ToString();
                string minTastyBuffer = ModSettings_QFramework.minTastyQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelTasty, ref ModSettings_QFramework.minTastyQuality, ref minTastyBuffer, 1, 0, 2);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.drugQuality)
            {
                string labelDrug = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minDrugQuality).ToString();
                string minDrugBuffer = ModSettings_QFramework.minDrugQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelDrug, ref ModSettings_QFramework.minDrugQuality, ref minDrugBuffer, 1, 0, 2);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.medQuality)
            {
                string labelMed = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minMedQuality).ToString();
                string minMedBuffer = ModSettings_QFramework.minMedQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelMed, ref ModSettings_QFramework.minMedQuality, ref minMedBuffer, 1, 0, 2);
            }
            else listingStandard.Gap(24);

            string labelApparel = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minApparelQuality).ToString();
            string minApparelBuffer = ModSettings_QFramework.minApparelQuality.ToString();
            LabeledIntEntry(listingStandard.GetRect(24f), labelApparel, ref ModSettings_QFramework.minApparelQuality, ref minApparelBuffer, 1, 0, 2);

            string labelWeapons = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minWeaponQuality).ToString();
            string minWeaponBuffer = ModSettings_QFramework.minWeaponQuality.ToString();
            LabeledIntEntry(listingStandard.GetRect(24f), labelWeapons, ref ModSettings_QFramework.minWeaponQuality, ref minWeaponBuffer, 1, 0, 2);

            if (ModSettings_QFramework.shellQuality)
            {
                string labelShell = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minShellQuality).ToString();
                string minShellBuffer = ModSettings_QFramework.minShellQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelShell, ref ModSettings_QFramework.minShellQuality, ref minShellBuffer, 1, 0, 2);
            }
            else listingStandard.Gap(24);

            listingStandard.GapLine();
            if (ModSettings_QFramework.useMaterialQuality || ModSettings_QFramework.useTableQuality)
            {
                listingStandard.Gap(12);
                string labelStd = "QFramework.Standard".Translate() + ((QualityCategory)ModSettings_QFramework.stdSupplyQuality).ToString();
                string stdBuffer = ModSettings_QFramework.stdSupplyQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelStd, ref ModSettings_QFramework.stdSupplyQuality, ref stdBuffer, 1, 0, 6);
                listingStandard.Gap(12);
            }
            else listingStandard.Gap(48);
            listingStandard.GapLine();

            if (ModSettings_QFramework.stuffQuality)
            {
                if (ModSettings_QFramework.skilledAnimals)
                    listingStandard.CheckboxLabeled("QFramework.InspiredGathering".Translate(), ref ModSettings_QFramework.inspiredGathering);
                else listingStandard.Gap(24f);
                if (ModSettings_QFramework.skilledButchering)
                    listingStandard.CheckboxLabeled("QFramework.InspiredButchering".Translate(), ref ModSettings_QFramework.inspiredButchering);
                else listingStandard.Gap(24f);
                if (ModSettings_QFramework.skilledHarvesting)
                    listingStandard.CheckboxLabeled("QFramework.InspiredHarvesting".Translate(), ref ModSettings_QFramework.inspiredHarvesting);
                else listingStandard.Gap(24f);
                if (ModSettings_QFramework.skilledMining)
                    listingStandard.CheckboxLabeled("QFramework.InspiredMining".Translate(), ref ModSettings_QFramework.inspiredMining);
                else listingStandard.Gap(24f);
                if (ModSettings_QFramework.skilledStoneCutting)
                    listingStandard.CheckboxLabeled("QFramework.InspiredStonecutting".Translate(), ref ModSettings_QFramework.inspiredStonecutting);
                else listingStandard.Gap(24f);
            }
            listingStandard.End();

            //Column 3
            Rect maxRect = new Rect(600f, 40f, inRect.width * .3f, inRect.height);
            listingStandard.Begin(maxRect);
            listingStandard.Gap(32);
            listingStandard.GapLine();
            if (ModSettings_QFramework.workQuality)
            {
                string labelWork2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxWorkQuality).ToString();
                string maxWorkBuffer = ModSettings_QFramework.maxWorkQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelWork2, ref ModSettings_QFramework.maxWorkQuality, ref maxWorkBuffer, 1, 2, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.securityQuality)
            {
                string labelSecurity2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxSecurityQuality).ToString();
                string maxSecurityBuffer = ModSettings_QFramework.maxSecurityQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelSecurity2, ref ModSettings_QFramework.maxSecurityQuality, ref maxSecurityBuffer, 1, 2, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.edificeQuality)
            {
                string labelConstruction2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxEdificeQuality).ToString();
                string maxConstructionBuffer = ModSettings_QFramework.maxEdificeQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelConstruction2, ref ModSettings_QFramework.maxEdificeQuality, ref maxConstructionBuffer, 1, 2, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.stuffQuality)
            {
                string labelStuff2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxStuffQuality).ToString();
                string maxStuffBuffer = ModSettings_QFramework.maxStuffQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelStuff2, ref ModSettings_QFramework.maxStuffQuality, ref maxStuffBuffer, 1, 2, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.manufQuality)
            {
                string labelManuf2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxManufQuality).ToString();
                string maxManufBuffer = ModSettings_QFramework.maxManufQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelManuf2, ref ModSettings_QFramework.maxManufQuality, ref maxManufBuffer, 1, 2, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.mealQuality)
            {
                string labelMeals2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxMealQuality).ToString();
                string maxMealBuffer = ModSettings_QFramework.maxMealQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelMeals2, ref ModSettings_QFramework.maxMealQuality, ref maxMealBuffer, 1, 2, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.ingredientQuality)
            {
                string labelIng2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxIngQuality).ToString();
                string maxIngBuffer = ModSettings_QFramework.maxIngQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelIng2, ref ModSettings_QFramework.maxIngQuality, ref maxIngBuffer, 1, 2, 6);

                string labelTasty2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxTastyQuality).ToString();
                string maxTastyBuffer = ModSettings_QFramework.maxTastyQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelTasty2, ref ModSettings_QFramework.maxTastyQuality, ref maxTastyBuffer, 1, 2, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.drugQuality)
            {
                string labelDrug2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxDrugQuality).ToString();
                string maxDrugBuffer = ModSettings_QFramework.maxDrugQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelDrug2, ref ModSettings_QFramework.maxDrugQuality, ref maxDrugBuffer, 1, 2, 6);
            }
            else listingStandard.Gap(24);
            if (ModSettings_QFramework.medQuality)
            {
                string labelMed2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxMedQuality).ToString();
                string maxMedBuffer = ModSettings_QFramework.maxMedQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelMed2, ref ModSettings_QFramework.maxMedQuality, ref maxMedBuffer, 1, 2, 6);
            }
            else listingStandard.Gap(24);

            string labelApparel2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxApparelQuality).ToString();
            string maxApparelBuffer = ModSettings_QFramework.maxApparelQuality.ToString();
            LabeledIntEntry(listingStandard.GetRect(24f), labelApparel2, ref ModSettings_QFramework.maxApparelQuality, ref maxApparelBuffer, 1, 2, 6);

            string labelWeapons2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxWeaponQuality).ToString();
            string maxWeaponBuffer = ModSettings_QFramework.maxWeaponQuality.ToString();
            LabeledIntEntry(listingStandard.GetRect(24f), labelWeapons2, ref ModSettings_QFramework.maxWeaponQuality, ref maxWeaponBuffer, 1, 2, 6);

            if (ModSettings_QFramework.shellQuality)
            {
                string labelShell2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxShellQuality).ToString();
                string maxShellBuffer = ModSettings_QFramework.maxShellQuality.ToString();
                LabeledIntEntry(listingStandard.GetRect(24f), labelShell2, ref ModSettings_QFramework.maxShellQuality, ref maxShellBuffer, 1, 2, 6);
            }
            else listingStandard.Gap(24);
            listingStandard.GapLine();
            if (ModSettings_QFramework.useMaterialQuality || ModSettings_QFramework.useTableQuality)
            {
                listingStandard.Gap(15);
                string midLabel = (1 - ModSettings_QFramework.tableFactor).ToStringPercent() + " / " + ModSettings_QFramework.tableFactor.ToStringPercent();
                ModSettings_QFramework.tableFactor = Widgets.HorizontalSlider(listingStandard.GetRect(23f), ModSettings_QFramework.tableFactor, 0, 1f, false, midLabel, "Materials", "Work Table");
                listingStandard.Gap(10);
            }
            else listingStandard.Gap(48);
            listingStandard.GapLine();
            if (ModSettings_QFramework.drugQuality || ModSettings_QFramework.medQuality)
                listingStandard.CheckboxLabeled("QFramework.InspiredChemistry".Translate(), ref ModSettings_QFramework.inspiredChemistry);
            if (ModSettings_QFramework.edificeQuality)
                listingStandard.CheckboxLabeled("QFramework.InspiredConstruction".Translate(), ref ModSettings_QFramework.inspiredConstruction);
            if (ModSettings_QFramework.mealQuality)
                listingStandard.CheckboxLabeled("QFramework.InspiredCooking".Translate(), ref ModSettings_QFramework.inspiredCooking);
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

        public void RestoreDefaults()
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

