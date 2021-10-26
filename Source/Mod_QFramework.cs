using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RimWorld;
using Verse;
using HarmonyLib;

namespace QualityFramework
{
    class Mod_QFramework : Mod
    {
        private static Vector2 bldgScroll = Vector2.zero;
        private static Vector2 bldgScroll2 = Vector2.zero;
        private static Vector2 weapScroll = Vector2.zero;
        private static Vector2 appScroll = Vector2.zero;
        private static Vector2 foodScroll = Vector2.zero;
        private static Vector2 otherScroll = Vector2.zero;
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

        public override void DoSettingsWindowContents(Rect inRect)
        {
            if (ModSettings_QFramework.bldgDict == null) ModSettings_QFramework.bldgDict = new Dictionary<string, bool>();
            if (ModSettings_QFramework.weapDict == null) ModSettings_QFramework.weapDict = new Dictionary<string, bool>();
            if (ModSettings_QFramework.appDict == null) ModSettings_QFramework.appDict = new Dictionary<string, bool>();
            if (ModSettings_QFramework.otherDict == null) ModSettings_QFramework.otherDict = new Dictionary<string, bool>();
            Rect labelRect = new Rect(5f, 34f, inRect.width * .5f, 42f);
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
                new TabRecord("QFramework.Tab0".Translate(), delegate()
                {
                    currentTab = 0;
                    settings.Write();
                },  currentTab == 0),
                new TabRecord("QFramework.Tab1".Translate(), delegate()
                {
                    currentTab = 1;
                    settings.Write();
                }, currentTab == 1),
                new TabRecord("QFramework.Tab2".Translate(), delegate()
                {
                    currentTab = 2;
                settings.Write();
                }, currentTab == 2),
                new TabRecord("QFramework.Tab3".Translate(), delegate()
                {
                    currentTab = 3;
                    settings.Write();
                }, currentTab == 3),
                new TabRecord("QFramework.Tab4".Translate(), delegate()
                {
                    currentTab = 4;
                    settings.Write();
                }, currentTab == 4)
            };
            TabDrawer.DrawTabs(canvas, tabs, 200f);
            if (currentTab == 0) DoQualityByCategory(canvas.ContractedBy(10f));
            if (currentTab == 1) DoSkillsAndInspiration(canvas.ContractedBy(10f));
            if (currentTab == 2) DoCustomizeBuildings(canvas);
            if (currentTab == 3) DoCustomizeWeapons(canvas.ContractedBy(10f));
            if (currentTab == 4) DoCustomizeOther(canvas);
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
            listing.Begin(new Rect(5f, 110f, rect.width * .33f - 5f, 30f));
            if (!ModSettings_QFramework.indivBuildings)
            {
                if (listing.ButtonTextLabeled("Buildings:".Translate(), "QFramework.Enable".Translate()))
                {
                    Mod_SettingsUtility.PopulateBuildings();
                    ModSettings_QFramework.indivBuildings = true;
                    //Log.Message("Building dictionary has " + ModSettings_QFramework.bldgDict.Count + " buildings.");
                }
            }
            else if (listing.ButtonTextLabeled("Buildings:".Translate(), "QFramework.Disable".Translate()))
            {
                ModSettings_QFramework.indivBuildings = false;
            }
            listing.End();
            List<string> keyList = new List<string>(ModSettings_QFramework.bldgDict.Keys);
            string key;
            listing.Begin(new Rect(rect.width * .33f + 5f, 110f, rect.width * .33f - 5f, 30f));
            if (listing.ButtonText("QFramework.Select".Translate(), null))
            {
                for (int i = 0; i < keyList.Count; i++)
                {
                    key = keyList[i];
                    ModSettings_QFramework.bldgDict[key] = true;
                }
            }
            listing.End();
            listing.Begin(new Rect(rect.width * .66f + 5f, 110f, rect.width * .33f - 5f, 30f));
            if (listing.ButtonText("QFramework.Deselect".Translate(), null))
            {
                for (int j = 0; j < keyList.Count; j++)
                {
                    key = keyList[j];
                    ModSettings_QFramework.bldgDict[key] = false;
                }
            }
            listing.End();
            listing.Begin(new Rect(0f, 140f, rect.width, 10f));
            listing.GapLine();
            listing.End();
            listing.Begin(new Rect(5f, 150f, rect.width * .5f - 10f, 38f));
            listing.Label("Furniture, Production & Art:".Translate());
            listing.GapLine();
            listing.End();
            listing.Begin(new Rect(rect.width * .5f + 5f, 150f, rect.width * .5f - 10f, 38f));
            listing.Label("Power, Security & Other:".Translate());
            listing.GapLine();
            listing.End();
            if (ModSettings_QFramework.indivBuildings)
            {
                List<string> list = new List<string>();
                List<string> other = new List<string>();
                bool value;
                for (int k = 0; k < ModSettings_QFramework.bldgDict.Count; k++)
                {
                    key = keyList[k];
                    ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(key);
                    if (def.IsWithinCategory(ThingCategoryDef.Named("BuildingsFurniture"))
                        || def.IsWithinCategory(ThingCategoryDef.Named("BuildingsProduction"))
                        || def.IsWithinCategory(ThingCategoryDefOf.BuildingsArt))
                    {
                        list.Add(key);
                    }
                    else other.Add(key);
                }

                Rect scrollRect = new Rect(5f, 190f, rect.width * .5f - 10f, rect.height - 110f);
                Rect viewRect = new Rect(0f, 0f, scrollRect.width - 30f, list.Count * 24f);
                Widgets.BeginScrollView(scrollRect, ref bldgScroll, viewRect, true);
                listing.Begin(viewRect);
                for (int m = 0; m < list.Count; m++)
                {
                    key = list[m];
                    value = ModSettings_QFramework.bldgDict[key];
                    listing.CheckboxLabeled(key.CapitalizeFirst(), ref value);
                    ModSettings_QFramework.bldgDict[key] = value;
                }
                listing.End();
                Widgets.EndScrollView();
                Rect scrollRect2 = new Rect(rect.width * .5f + 5f, 190f, rect.width * .5f - 10f, rect.height - 110f);
                Rect viewRect2 = new Rect(0f, 0f, scrollRect2.width - 30f, other.Count * 24f);
                Widgets.BeginScrollView(scrollRect2, ref bldgScroll2, viewRect2, true);
                listing.Begin(viewRect2);
                for (int n = 0; n < other.Count; n++)
                {
                    key = other[n];
                    value = ModSettings_QFramework.bldgDict[key];
                    listing.CheckboxLabeled(key.CapitalizeFirst(), ref value);
                    ModSettings_QFramework.bldgDict[key] = value;
                }
                listing.End();
                Widgets.EndScrollView();
            }
        }

        public static void DoCustomizeWeapons(Rect rect)
        {
            Rect firstCol = new Rect(5f, 110f, rect.width * .48f, rect.height);
            listing.Begin(firstCol);
            if (!ModSettings_QFramework.indivWeapons)
            {
                if (listing.ButtonTextLabeled("Weapons & Shells:".Translate(), "QFramework.Enable".Translate()))
                {
                    Mod_SettingsUtility.PopulateWeapons();
                    ModSettings_QFramework.indivWeapons = true;
                    //Log.Message("Weapon dictionary has " + ModSettings_QFramework.weapDict.Count + " items.");
                }
                //listing.GapLine();
            }
            else
            {
                if (listing.ButtonTextLabeled("Weapons & Shells:".Translate(), "QFramework.Disable".Translate()))
                {
                    ModSettings_QFramework.indivWeapons = false;
                    //Log.Message("Weapon dictionary has " + ModSettings_QFramework.weapDict.Count + " items.");
                }
            }
            List<string> weapons = new List<string>(ModSettings_QFramework.weapDict.Keys);
            string weap;
            if (listing.ButtonText("QFramework.Select".Translate(), null))
            {
                for (int b1 = 0; b1 < weapons.Count; b1++)
                {
                    weap = weapons[b1];
                    ModSettings_QFramework.weapDict[weap] = true;
                }
            }
            if (listing.ButtonText("QFramework.Deselect".Translate(), null))
            {
                for (int b2 = 0; b2 < weapons.Count; b2++)
                {
                    weap = weapons[b2];
                    ModSettings_QFramework.weapDict[weap] = false;
                }
            }
            listing.GapLine();
            listing.End();
            if (ModSettings_QFramework.indivWeapons)
            {
                Rect scrollRect = new Rect(5f, 220f, rect.width * .48f, rect.height - 110f);
                Rect viewRect = new Rect(0f, 0f, scrollRect.width - 30f, weapons.Count * 24f);
                Widgets.BeginScrollView(scrollRect, ref weapScroll, viewRect, true);
                listing.Begin(viewRect);
                for (int b3 = 0; b3 < ModSettings_QFramework.weapDict.Count; b3++)
                {
                    weap = weapons[b3];
                    bool weapQual = ModSettings_QFramework.weapDict[weap];
                    listing.CheckboxLabeled(weap.CapitalizeFirst(), ref weapQual);
                    ModSettings_QFramework.weapDict[weap] = weapQual;
                }
                listing.End();
                Widgets.EndScrollView();
            }

            Rect secondCol = new Rect(rect.width * .5f, 110f, rect.width * .48f, rect.height);
            listing.Begin(secondCol);
            if (!ModSettings_QFramework.indivApparel)
            {
                if (listing.ButtonTextLabeled("Apparel:".Translate(), "QFramework.Enable".Translate()))
                {
                    Mod_SettingsUtility.PopulateApparel();
                    ModSettings_QFramework.indivApparel = true;
                }
            }
            else
            {
                if (listing.ButtonTextLabeled("Apparel:".Translate(), "QFramework.Disable".Translate()))
                {
                    ModSettings_QFramework.indivApparel = false;
                }
            }
            List<string> apparel = new List<string>(ModSettings_QFramework.appDict.Keys);
            string app;
            if (listing.ButtonText("QFramework.Select".Translate(), null))
            {
                for (int b1 = 0; b1 < apparel.Count; b1++)
                {
                    app = apparel[b1];
                    ModSettings_QFramework.appDict[app] = true;
                }
            }
            if (listing.ButtonText("QFramework.Deselect".Translate(), null))
            {
                for (int b2 = 0; b2 < apparel.Count; b2++)
                {
                    app = apparel[b2];
                    ModSettings_QFramework.appDict[app] = false;
                }
            }
            listing.GapLine();
            listing.End();
            if (ModSettings_QFramework.indivApparel)
            {
                Rect scrollRect = new Rect(rect.width * .5f, 220f, rect.width * .48f, rect.height - 110f);
                Rect viewRect = new Rect(0f, 0f, scrollRect.width - 30f, apparel.Count * 24f);
                Widgets.BeginScrollView(scrollRect, ref appScroll, viewRect, true);
                listing.Begin(viewRect);
                for (int b3 = 0; b3 < ModSettings_QFramework.appDict.Count; b3++)
                {
                    app = apparel[b3];
                    bool appQual = ModSettings_QFramework.appDict[app];
                    listing.CheckboxLabeled(app.CapitalizeFirst(), ref appQual);
                    ModSettings_QFramework.appDict[app] = appQual;
                }
                listing.End();
                Widgets.EndScrollView();
            }
        }

        public static void DoCustomizeOther(Rect rect)
        {
            {
                listing.Begin(new Rect(5f, 110f, rect.width * .33f - 5f, 30f));
                if (!ModSettings_QFramework.indivOther)
                {
                    if (listing.ButtonText("QFramework.Enable".Translate(), null))
                    {
                        Mod_SettingsUtility.PopulateOther();
                        ModSettings_QFramework.indivOther = true;
                        //Log.Message("Other dictionary has " + ModSettings_QFramework.otherDict.Count + " buildings.");
                    }
                }
                else if (listing.ButtonText("QFramework.Disable".Translate(), null))
                {
                    ModSettings_QFramework.indivOther = false;
                }
                listing.End();
                List<string> keyList = new List<string>(ModSettings_QFramework.otherDict.Keys);
                string key;
                listing.Begin(new Rect(rect.width * .33f + 5f, 110f, rect.width * .33f - 5f, 30f));
                if (listing.ButtonText("QFramework.Select".Translate(), null))
                {
                    for (int i = 0; i < keyList.Count; i++)
                    {
                        key = keyList[i];
                        ModSettings_QFramework.otherDict[key] = true;
                    }
                }
                listing.End();
                listing.Begin(new Rect(rect.width * .66f + 5f, 110f, rect.width * .33f - 5f, 30f));
                if (listing.ButtonText("QFramework.Deselect".Translate(), null))
                {
                    for (int j = 0; j < keyList.Count; j++)
                    {
                        key = keyList[j];
                        ModSettings_QFramework.otherDict[key] = false;
                    }
                }
                listing.End();
                listing.Begin(new Rect(0f, 140f, rect.width, 10f));
                listing.GapLine();
                listing.End();
                listing.Begin(new Rect(5f, 150f, rect.width * .5f - 10f, 38f));
                listing.Label("Food:".Translate());
                listing.GapLine();
                listing.End();
                listing.Begin(new Rect(rect.width * .5f + 5f, 150f, rect.width * .5f - 10f, 38f));
                listing.Label("Drugs, Medicine, & Manufactured:".Translate());
                listing.GapLine();
                listing.End();
                if (ModSettings_QFramework.indivOther)
                {
                    List<string> list = new List<string>();
                    List<string> other = new List<string>();
                    bool value;
                    Log.Message("lists created");
                    for (int k = 0; k < ModSettings_QFramework.otherDict.Count; k++)
                    {
                        Log.Message("Filling lists");
                        key = keyList[k];
                        ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(key);
                        if (def.IsNutritionGivingIngestible)
                        {
                            list.Add(key);
                        }
                        else other.Add(key);
                    }
                    Log.Message("Displaying first scroll");
                    Rect scrollRect = new Rect(5f, 190f, rect.width * .5f - 10f, rect.height - 110f);
                    Rect viewRect = new Rect(0f, 0f, scrollRect.width - 30f, keyList.Count * 12f);
                    Widgets.BeginScrollView(scrollRect, ref foodScroll, viewRect, true);
                    listing.Begin(viewRect);
                    for (int m = 0; m < list.Count; m++)
                    {
                        key = list[m];
                        value = ModSettings_QFramework.otherDict[key];
                        listing.CheckboxLabeled(key.CapitalizeFirst(), ref value);
                        ModSettings_QFramework.otherDict[key] = value;
                    }
                    listing.End();
                    Widgets.EndScrollView();
                    Log.Message("Displaying second scroll");
                    Rect scrollRect2 = new Rect(rect.width * .5f + 5f, 190f, rect.width * .5f - 10f, rect.height - 110f);
                    Rect viewRect2 = new Rect(0f, 0f, scrollRect2.width - 30f, keyList.Count * 12f);
                    Widgets.BeginScrollView(scrollRect2, ref otherScroll, viewRect2, true);
                    listing.Begin(viewRect2);
                    for (int n = 0; n < other.Count; n++)
                    {
                        key = other[n];
                        value = ModSettings_QFramework.otherDict[key];
                        listing.CheckboxLabeled(key.CapitalizeFirst(), ref value);
                        ModSettings_QFramework.otherDict[key] = value;
                    }
                    listing.End();
                    Widgets.EndScrollView();
                }
            }
        }
    }
}

