using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;
using HarmonyLib;

namespace QualityFramework
{
    class Mod_QFramework : Mod
    {
        private static Vector2 weapPos = Vector2.zero;
        private static Listing_Standard listing = new Listing_Standard();
        public static ModSettings_QFramework settings;
        private static int currentTab;

        public Mod_QFramework(ModContentPack content) : base(content)
        {
            //Mod_SettingsWindow.settings = 
            settings = GetSettings<ModSettings_QFramework>();
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
            Rect labelRect = new Rect (5f, 34f, inRect.width * .5f, 42f);
            listing.Begin(labelRect);
            Text.Font = GameFont.Medium;
            if (currentTab != 1) listing.Label("***Changes Require Restart***");
            Text.Font = GameFont.Small;
            listing.End();
            Rect rect = new Rect(605f, 34f, inRect.width * .3f, 42f);
            listing.Begin(rect);
            string buttonText = "Restore Category Defaults";
            if (currentTab >= 2) buttonText = "Reset All Individual Defaults";
            if (listing.ButtonText(buttonText, null))
            {
                if (currentTab < 2) Mod_SettingsUtility.RestoreDefaults();
                else
                {
                    ModSettings_QFramework.bldgDict.Clear();
                    ModSettings_QFramework.weapDict.Clear();
                    ModSettings_QFramework.appDict.Clear();
                    ModSettings_QFramework.otherDict.Clear();
                    ModSettings_QFramework.indivBuildings = false;
                    ModSettings_QFramework.indivWeapons = false;
                    ModSettings_QFramework.indivApparel = false;
                    ModSettings_QFramework.indivOther = false;
                }
            }
            listing.End();
            DoSettings(inRect);
        }

        public static void DoSettings(Rect canvas)
        {
            canvas = canvas.Rounded();
            canvas.height -= 60f;
            canvas.y += 60f;
            Widgets.DrawMenuSection(canvas);
            List<TabRecord> tabs = new List<TabRecord>
            {
                new TabRecord("Quality Limits by Category".Translate(), delegate()
                {
                    currentTab = 0;
                    settings.Write();
                },  currentTab == 0),
                new TabRecord("Skill and Inspiration".Translate(), delegate()
                {
                    currentTab = 1;
                    settings.Write();
                }, currentTab == 1),
                new TabRecord("Customize Buildings, Food and Other".Translate(), delegate()
                {
                    currentTab = 2;
                    settings.Write();
                }, currentTab == 2),
                new TabRecord("Customize Weapons and Apparel".Translate(), delegate()
                {
                    currentTab = 3;
                    settings.Write();
                }, currentTab == 3)
            };
            TabDrawer.DrawTabs(canvas, tabs, 200f);
            if (currentTab == 0)
            {
                DoQualityByCategory(canvas.ContractedBy(10f));
            }
            if (currentTab == 1)
            {
                DoSkillsAndInspiration(canvas.ContractedBy(10f));
            }
            if (currentTab == 2)
            {
                DoCustomizeBuildings(canvas.ContractedBy(10f));
            }
            if (currentTab == 3)
                DoCustomizeWeapons(canvas.ContractedBy(10f));
        }

        public static void DoQualityByCategory(Rect rect)
        {
            Rect firstCol = new Rect(5f, 110f, rect.width * .35f, rect.height);
            listing.Begin(firstCol);
            listing.CheckboxLabeled("    " + "QFramework.Work".Translate(), ref ModSettings_QFramework.workQuality);
            listing.CheckboxLabeled("    " + "QFramework.Security".Translate(), ref ModSettings_QFramework.securityQuality);
            listing.CheckboxLabeled("    " + "QFramework.Edifice".Translate(), ref ModSettings_QFramework.edificeQuality);
            listing.CheckboxLabeled("    " + "QFramework.Stuff".Translate(), ref ModSettings_QFramework.stuffQuality);
            listing.CheckboxLabeled("    " + "QFramework.Manuf".Translate(), ref ModSettings_QFramework.manufQuality);
            listing.CheckboxLabeled("    " + "QFramework.Meal".Translate(), ref ModSettings_QFramework.mealQuality);
            listing.CheckboxLabeled("    " + "QFramework.Ingredients".Translate(), ref ModSettings_QFramework.ingredientQuality);
            if (ModSettings_QFramework.ingredientQuality) listing.Label("        " + "QFramework.Tasty".Translate());
            listing.CheckboxLabeled("    " + "QFramework.Drugs".Translate(), ref ModSettings_QFramework.drugQuality);
            listing.CheckboxLabeled("    " + "QFramework.Med".Translate(), ref ModSettings_QFramework.medQuality);
            listing.CheckboxLabeled("    " + "QFramework.Apparel".Translate(), ref ModSettings_QFramework.apparelQuality);
            listing.CheckboxLabeled("    " + "QFramework.Weapons".Translate(), ref ModSettings_QFramework.weaponQuality);
            listing.CheckboxLabeled("    " + "QFramework.Shells".Translate(), ref ModSettings_QFramework.shellQuality);
            listing.End();

            //Column2
            Rect secondCol = new Rect(325f, 110f, rect.width * .3f, rect.height);
            listing.Begin(secondCol);
            if (ModSettings_QFramework.workQuality)
            {
                string labelWork = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minWorkQuality).ToString();
                string minWorkBuffer = ModSettings_QFramework.minWorkQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelWork, ref ModSettings_QFramework.minWorkQuality, ref minWorkBuffer, 1, 0, 2);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.securityQuality)
            {
                string labelSecurity = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minSecurityQuality).ToString();
                string minSecurityBuffer = ModSettings_QFramework.minSecurityQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelSecurity, ref ModSettings_QFramework.minSecurityQuality, ref minSecurityBuffer, 1, 0, 2);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.edificeQuality)
            {
                string labelConstruction = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minEdificeQuality).ToString();
                string minConstructionBuffer = ModSettings_QFramework.minEdificeQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelConstruction, ref ModSettings_QFramework.minEdificeQuality, ref minConstructionBuffer, 1, 0, 2);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.stuffQuality)
            {
                string labelStuff = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minStuffQuality).ToString();
                string minStuffBuffer = ModSettings_QFramework.minStuffQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelStuff, ref ModSettings_QFramework.minStuffQuality, ref minStuffBuffer, 1, 0, 2);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.manufQuality)
            {
                string labelManuf = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minManufQuality).ToString();
                string minManufBuffer = ModSettings_QFramework.minManufQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelManuf, ref ModSettings_QFramework.minManufQuality, ref minManufBuffer, 1, 0, 2);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.mealQuality)
            {
                string labelMeals = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minMealQuality).ToString();
                string minMealBuffer = ModSettings_QFramework.minMealQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelMeals, ref ModSettings_QFramework.minMealQuality, ref minMealBuffer, 1, 0, 2);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.ingredientQuality)
            {
                string labelIng = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minIngQuality).ToString();
                string minIngBuffer = ModSettings_QFramework.minIngQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelIng, ref ModSettings_QFramework.minIngQuality, ref minIngBuffer, 1, 0, 2);

                string labelTasty = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minTastyQuality).ToString();
                string minTastyBuffer = ModSettings_QFramework.minTastyQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelTasty, ref ModSettings_QFramework.minTastyQuality, ref minTastyBuffer, 1, 0, 2);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.drugQuality)
            {
                string labelDrug = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minDrugQuality).ToString();
                string minDrugBuffer = ModSettings_QFramework.minDrugQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelDrug, ref ModSettings_QFramework.minDrugQuality, ref minDrugBuffer, 1, 0, 2);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.medQuality)
            {
                string labelMed = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minMedQuality).ToString();
                string minMedBuffer = ModSettings_QFramework.minMedQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelMed, ref ModSettings_QFramework.minMedQuality, ref minMedBuffer, 1, 0, 2);
            }
            else listing.Gap(24);

            string labelApparel = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minApparelQuality).ToString();
            string minApparelBuffer = ModSettings_QFramework.minApparelQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelApparel, ref ModSettings_QFramework.minApparelQuality, ref minApparelBuffer, 1, 0, 2);

            string labelWeapons = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minWeaponQuality).ToString();
            string minWeaponBuffer = ModSettings_QFramework.minWeaponQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelWeapons, ref ModSettings_QFramework.minWeaponQuality, ref minWeaponBuffer, 1, 0, 2);

            if (ModSettings_QFramework.shellQuality)
            {
                string labelShell = "QFramework.Min".Translate() + ((QualityCategory)ModSettings_QFramework.minShellQuality).ToString();
                string minShellBuffer = ModSettings_QFramework.minShellQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelShell, ref ModSettings_QFramework.minShellQuality, ref minShellBuffer, 1, 0, 2);
            }
            else listing.Gap(24);
            listing.End();

            //Column 3
            Rect thirdCol = new Rect(600f, 110f, rect.width * .3f, rect.height);
            listing.Begin(thirdCol);
            if (ModSettings_QFramework.workQuality)
            {
                string labelWork2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxWorkQuality).ToString();
                string maxWorkBuffer = ModSettings_QFramework.maxWorkQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelWork2, ref ModSettings_QFramework.maxWorkQuality, ref maxWorkBuffer, 1, 2, 6);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.securityQuality)
            {
                string labelSecurity2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxSecurityQuality).ToString();
                string maxSecurityBuffer = ModSettings_QFramework.maxSecurityQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelSecurity2, ref ModSettings_QFramework.maxSecurityQuality, ref maxSecurityBuffer, 1, 2, 6);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.edificeQuality)
            {
                string labelConstruction2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxEdificeQuality).ToString();
                string maxConstructionBuffer = ModSettings_QFramework.maxEdificeQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelConstruction2, ref ModSettings_QFramework.maxEdificeQuality, ref maxConstructionBuffer, 1, 2, 6);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.stuffQuality)
            {
                string labelStuff2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxStuffQuality).ToString();
                string maxStuffBuffer = ModSettings_QFramework.maxStuffQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelStuff2, ref ModSettings_QFramework.maxStuffQuality, ref maxStuffBuffer, 1, 2, 6);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.manufQuality)
            {
                string labelManuf2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxManufQuality).ToString();
                string maxManufBuffer = ModSettings_QFramework.maxManufQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelManuf2, ref ModSettings_QFramework.maxManufQuality, ref maxManufBuffer, 1, 2, 6);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.mealQuality)
            {
                string labelMeals2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxMealQuality).ToString();
                string maxMealBuffer = ModSettings_QFramework.maxMealQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelMeals2, ref ModSettings_QFramework.maxMealQuality, ref maxMealBuffer, 1, 2, 6);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.ingredientQuality)
            {
                string labelIng2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxIngQuality).ToString();
                string maxIngBuffer = ModSettings_QFramework.maxIngQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelIng2, ref ModSettings_QFramework.maxIngQuality, ref maxIngBuffer, 1, 2, 6);

                string labelTasty2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxTastyQuality).ToString();
                string maxTastyBuffer = ModSettings_QFramework.maxTastyQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelTasty2, ref ModSettings_QFramework.maxTastyQuality, ref maxTastyBuffer, 1, 2, 6);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.drugQuality)
            {
                string labelDrug2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxDrugQuality).ToString();
                string maxDrugBuffer = ModSettings_QFramework.maxDrugQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelDrug2, ref ModSettings_QFramework.maxDrugQuality, ref maxDrugBuffer, 1, 2, 6);
            }
            else listing.Gap(24);
            if (ModSettings_QFramework.medQuality)
            {
                string labelMed2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxMedQuality).ToString();
                string maxMedBuffer = ModSettings_QFramework.maxMedQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelMed2, ref ModSettings_QFramework.maxMedQuality, ref maxMedBuffer, 1, 2, 6);
            }
            else listing.Gap(24);

            string labelApparel2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxApparelQuality).ToString();
            string maxApparelBuffer = ModSettings_QFramework.maxApparelQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelApparel2, ref ModSettings_QFramework.maxApparelQuality, ref maxApparelBuffer, 1, 2, 6);

            string labelWeapons2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxWeaponQuality).ToString();
            string maxWeaponBuffer = ModSettings_QFramework.maxWeaponQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelWeapons2, ref ModSettings_QFramework.maxWeaponQuality, ref maxWeaponBuffer, 1, 2, 6);

            if (ModSettings_QFramework.shellQuality)
            {
                string labelShell2 = "QFramework.Max".Translate() + ((QualityCategory)ModSettings_QFramework.maxShellQuality).ToString();
                string maxShellBuffer = ModSettings_QFramework.maxShellQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelShell2, ref ModSettings_QFramework.maxShellQuality, ref maxShellBuffer, 1, 2, 6);
            }
            listing.End();
        }

        public static void DoSkillsAndInspiration(Rect rect)
        {
            Rect firstCol = new Rect(5f, 110f, rect.width * .35f, rect.height);
            listing.Begin(firstCol);
            listing.CheckboxLabeled("QFramework.Materials".Translate(), ref ModSettings_QFramework.useMaterialQuality);
            listing.CheckboxLabeled("QFramework.Tables".Translate(), ref ModSettings_QFramework.useTableQuality);
            listing.GapLine();

            listing.CheckboxLabeled("QFramework.SkilledAnimals".Translate(), ref ModSettings_QFramework.skilledAnimals);
            listing.CheckboxLabeled("QFramework.SkilledButchering".Translate(), ref ModSettings_QFramework.skilledButchering);
            listing.CheckboxLabeled("QFramework.SkilledHarvesting".Translate(), ref ModSettings_QFramework.skilledHarvesting);
            listing.CheckboxLabeled("QFramework.SkilledMining".Translate(), ref ModSettings_QFramework.skilledMining);
            listing.CheckboxLabeled("QFramework.SkilledStoneCutting".Translate(), ref ModSettings_QFramework.skilledStoneCutting);
            listing.End();

            //Column Two
            Rect secondCol = new Rect(325f, 110f, rect.width * .3f, rect.height);
            listing.Begin(secondCol);
            if (ModSettings_QFramework.useMaterialQuality || ModSettings_QFramework.useTableQuality)
            {
                listing.Gap(12);
                string labelStd = "QFramework.Standard".Translate() + ((QualityCategory)ModSettings_QFramework.stdSupplyQuality).ToString();
                string stdBuffer = ModSettings_QFramework.stdSupplyQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelStd, ref ModSettings_QFramework.stdSupplyQuality, ref stdBuffer, 1, 0, 6);
                listing.Gap(12);
            }
            else listing.Gap(48);
            listing.GapLine();

            if (ModSettings_QFramework.stuffQuality)
            {
                if (ModSettings_QFramework.skilledAnimals)
                    listing.CheckboxLabeled("QFramework.InspiredGathering".Translate(), ref ModSettings_QFramework.inspiredGathering);
                else listing.Gap(24f);
                if (ModSettings_QFramework.skilledButchering)
                    listing.CheckboxLabeled("QFramework.InspiredButchering".Translate(), ref ModSettings_QFramework.inspiredButchering);
                else listing.Gap(24f);
                if (ModSettings_QFramework.skilledHarvesting)
                    listing.CheckboxLabeled("QFramework.InspiredHarvesting".Translate(), ref ModSettings_QFramework.inspiredHarvesting);
                else listing.Gap(24f);
                if (ModSettings_QFramework.skilledMining)
                    listing.CheckboxLabeled("QFramework.InspiredMining".Translate(), ref ModSettings_QFramework.inspiredMining);
                else listing.Gap(24f);
                if (ModSettings_QFramework.skilledStoneCutting)
                    listing.CheckboxLabeled("QFramework.InspiredStonecutting".Translate(), ref ModSettings_QFramework.inspiredStonecutting);
                else listing.Gap(24f);
            }
            listing.End();

            //Column Three
            Rect thirdCol = new Rect(600f, 110f, rect.width * .3f, rect.height);
            listing.Begin(thirdCol);
            if (ModSettings_QFramework.useMaterialQuality || ModSettings_QFramework.useTableQuality)
            {
                listing.Gap(15);
                string midLabel = (1 - ModSettings_QFramework.tableFactor).ToStringPercent() + " / " + ModSettings_QFramework.tableFactor.ToStringPercent();
                ModSettings_QFramework.tableFactor = Widgets.HorizontalSlider(listing.GetRect(23f), ModSettings_QFramework.tableFactor, 0, 1f, false, midLabel, "Materials", "Work Table");
                listing.Gap(10);
            }
            else listing.Gap(48);
            listing.GapLine();
            listing.CheckboxLabeled("QFramework.InspiredChemistry".Translate(), ref ModSettings_QFramework.inspiredChemistry);
            listing.CheckboxLabeled("QFramework.InspiredConstruction".Translate(), ref ModSettings_QFramework.inspiredConstruction);
            listing.CheckboxLabeled("QFramework.InspiredCooking".Translate(), ref ModSettings_QFramework.inspiredCooking);
            listing.End();
        }

        public static void DoCustomizeBuildings(Rect rect)
        {
            /*listing.Begin(rect);
            string button = "Populate Items";
            if (ModSettings_QFramework.bldgDict.Count > 0 || ModSettings_QFramework.weapDict.Count > 0 || ModSettings_QFramework.appDict.Count > 0 || ModSettings_QFramework.otherDict.Count > 0)
            {
                button = "Re-populate Items";
            }
            if (listing.ButtonTextLabeled("Click here first:", button)) Mod_SettingsUtility.PopulateDictionary();
            listing.End();*/
            Rect firstCol = new Rect(5f, 110f, rect.width * .24f, rect.height);
            listing.Begin(firstCol);
            if (!ModSettings_QFramework.indivBuildings)
            {
                if (listing.ButtonTextLabeled("Buildings:".Translate(), "Enable".Translate()))
                {
                    Mod_SettingsUtility.PopulateBuildings();
                    ModSettings_QFramework.indivBuildings = true;
                }
                listing.GapLine();
            }
            else 
            {
                if (listing.ButtonTextLabeled("Buildings:".Translate(), "Disable".Translate()))
                {
                    ModSettings_QFramework.indivBuildings = false;
                }
                listing.GapLine();
                List<string> buildings = new List<string>(ModSettings_QFramework.bldgDict.Keys);
                string bldg;
                if (listing.ButtonText("Select All", null))
                {
//                    foreach (KeyValuePair<string, bool> entry in ModSettings_QFramework.bldgDict) ModSettings_QFramework.bldgDict[entry.Key] = true;
                    for (int b1 = 0; b1 < buildings.Count; b1++)
                    {
                        bldg = buildings[b1];
                        ModSettings_QFramework.bldgDict[bldg] = true;
                    }
                }
                if (listing.ButtonText("Deselect All", null))
                {
                    //                  foreach (KeyValuePair<string, bool> entry in ModSettings_QFramework.bldgDict) ModSettings_QFramework.bldgDict[entry.Key] = false;
                    for (int b2 = 0; b2 < buildings.Count; b2++)
                    {
                        bldg = buildings[b2];
                        ModSettings_QFramework.bldgDict[bldg] = false;
                    }
                }
                listing.GapLine();
                Rect weapScroll = new Rect(0f, 120f, rect.width * .24f, rect.height);
                Rect weapRect = new Rect(0f, 0f, weapScroll.width - 35f, rect.height);
                Widgets.BeginScrollView(weapScroll, ref weapPos, weapRect, true);
                for (int b3 = 0; b3 < ModSettings_QFramework.bldgDict.Count; b3++)
                {
                    bldg = buildings[b3];
                    bool bldgQual = ModSettings_QFramework.bldgDict[bldg];
                    listing.CheckboxLabeled(bldg.CapitalizeFirst(), ref bldgQual);
                    ModSettings_QFramework.bldgDict[bldg] = bldgQual;
                }
                Widgets.EndScrollView();
            }
            listing.End();

            Rect secondCol = new Rect(rect.width * .25f + .10f, 110f, rect.width * .24f, rect.height);
            listing.Begin(secondCol);
            if (!ModSettings_QFramework.indivWeapons)
            {
                if (listing.ButtonTextLabeled("Weapons:".Translate(), "Enable".Translate()))
                {
                    Mod_SettingsUtility.PopulateWeapons();
                    ModSettings_QFramework.indivWeapons = true;
                    Log.Message("Weapon dictionary has " + ModSettings_QFramework.weapDict.Count + " items.");
                }
                listing.GapLine();
            }
            else
            {
                if (listing.ButtonTextLabeled("Weapons:".Translate(), "Disable".Translate()))
                {
                    ModSettings_QFramework.indivWeapons = false;
                    Log.Message("Weapon dictionary has " + ModSettings_QFramework.weapDict.Count + " items.");
                }
                listing.GapLine();
                List<string> weapons = new List<string>(ModSettings_QFramework.weapDict.Keys);
                string weap;
                if (listing.ButtonText("Select All", null))
                {
                    //                    foreach (KeyValuePair<string, bool> entry in ModSettings_QFramework.bldgDict) ModSettings_QFramework.bldgDict[entry.Key] = true;
                    for (int b1 = 0; b1 < weapons.Count; b1++)
                    {
                        weap = weapons[b1];
                        ModSettings_QFramework.weapDict[weap] = true;
                    }
                }
                if (listing.ButtonText("Deselect All", null))
                {
                    //                  foreach (KeyValuePair<string, bool> entry in ModSettings_QFramework.bldgDict) ModSettings_QFramework.bldgDict[entry.Key] = false;
                    for (int b2 = 0; b2 < weapons.Count; b2++)
                    {
                        weap = weapons[b2];
                        ModSettings_QFramework.weapDict[weap] = false;
                    }
                }
                listing.GapLine();
                Rect weapScroll = new Rect(0f, 120f, rect.width * .24f, rect.height);
                Rect weapRect = new Rect(0f, 0f, weapScroll.width - 35f, weapons.Count * 24f);
                Widgets.BeginScrollView(weapScroll, ref weapPos, weapRect, true);
                for (int b3 = 0; b3 < ModSettings_QFramework.weapDict.Count; b3++)
                {
                    weap = weapons[b3];
                    bool weapQual = ModSettings_QFramework.weapDict[weap];
                    listing.CheckboxLabeled(weap.CapitalizeFirst(), ref weapQual);
                    ModSettings_QFramework.weapDict[weap] = weapQual;
                }
                Widgets.EndScrollView();
            }
            listing.End();

            Rect thirdCol = new Rect(rect.width * .50f + .10f, 110f, rect.width * .24f, rect.height);
            listing.Begin(thirdCol);
            if (!ModSettings_QFramework.indivApparel)
            {
                if (listing.ButtonTextLabeled("Apparel:".Translate(), "Enable".Translate()))
                {
                    Mod_SettingsUtility.PopulateApparel();
                    ModSettings_QFramework.indivApparel = true;   
                }
            }
            else
            {
                if (listing.ButtonTextLabeled("Apparel:".Translate(), "Disable".Translate()))
                {
                    ModSettings_QFramework.indivApparel = false;
                }
            }
            listing.End();

            Rect fourthCol = new Rect(rect.width * .75f + .10f, 110f, rect.width * .24f, rect.height);
            listing.Begin(fourthCol);
            if (!ModSettings_QFramework.indivOther)
            {
                if (listing.ButtonTextLabeled("Food & Other:".Translate(), "Enable".Translate()))
                {
                    Mod_SettingsUtility.PopulateOther();
                    ModSettings_QFramework.indivOther = true;
                }
            }
            else
            {
                if (listing.ButtonTextLabeled("Food & Other:".Translate(), "Disable".Translate()))
                {
                    ModSettings_QFramework.indivOther = false;
                }
            }
            listing.End();
        }

        public static void DoCustomizeWeapons(Rect rect)
        {
            Rect firstCol = new Rect(5f, 110f, rect.width * .5f, rect.height);
            listing.Begin(firstCol);
            if (!ModSettings_QFramework.indivWeapons)
            {
                if (listing.ButtonTextLabeled("Weapons:".Translate(), "Enable".Translate()))
                {
                    Mod_SettingsUtility.PopulateWeapons();
                    ModSettings_QFramework.indivWeapons = true;
                    Log.Message("Weapon dictionary has " + ModSettings_QFramework.weapDict.Count + " items.");
                }
                listing.GapLine();
            }
            else
            {
                if (listing.ButtonTextLabeled("Weapons:".Translate(), "Disable".Translate()))
                {
                    ModSettings_QFramework.indivWeapons = false;
                    Log.Message("Weapon dictionary has " + ModSettings_QFramework.weapDict.Count + " items.");
                }
                listing.GapLine();
                List<string> weapons = new List<string>(ModSettings_QFramework.weapDict.Keys);
                string weap;
                if (listing.ButtonText("Select All", null))
                {
                    //                    foreach (KeyValuePair<string, bool> entry in ModSettings_QFramework.bldgDict) ModSettings_QFramework.bldgDict[entry.Key] = true;
                    for (int b1 = 0; b1 < weapons.Count; b1++)
                    {
                        weap = weapons[b1];
                        ModSettings_QFramework.weapDict[weap] = true;
                    }
                }
                if (listing.ButtonText("Deselect All", null))
                {
                    //                  foreach (KeyValuePair<string, bool> entry in ModSettings_QFramework.bldgDict) ModSettings_QFramework.bldgDict[entry.Key] = false;
                    for (int b2 = 0; b2 < weapons.Count; b2++)
                    {
                        weap = weapons[b2];
                        ModSettings_QFramework.weapDict[weap] = false;
                    }
                }
                listing.GapLine();
                Rect weapScroll = new Rect(0f, 120f, rect.width * .24f, rect.height);
                Rect weapRect = new Rect(0f, 0f, weapScroll.width - 35f, weapons.Count * 24f);
                Widgets.BeginScrollView(weapScroll, ref weapPos, weapRect, true);
                for (int b3 = 0; b3 < ModSettings_QFramework.weapDict.Count; b3++)
                {
                    weap = weapons[b3];
                    bool weapQual = ModSettings_QFramework.weapDict[weap];
                    listing.CheckboxLabeled(weap.CapitalizeFirst(), ref weapQual);
                    ModSettings_QFramework.weapDict[weap] = weapQual;
                }
                Widgets.EndScrollView();
            }
            listing.End();

            Rect thirdCol = new Rect(rect.width * .5f + .10f, 110f, rect.width * .49f, rect.height);
            listing.Begin(thirdCol);
            if (!ModSettings_QFramework.indivApparel)
            {
                if (listing.ButtonTextLabeled("Apparel:".Translate(), "Enable".Translate()))
                {
                    Mod_SettingsUtility.PopulateApparel();
                    ModSettings_QFramework.indivApparel = true;
                }
            }
            else
            {
                if (listing.ButtonTextLabeled("Apparel:".Translate(), "Disable".Translate()))
                {
                    ModSettings_QFramework.indivApparel = false;
                }
            }
            listing.End();
        }
    }
}

