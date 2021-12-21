using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
using Verse;

namespace QualityEverything
{
    class Mod_QEverything : Mod
    {
        private static Vector2 texScroll = Vector2.zero;
        private static Vector2 resScroll = Vector2.zero;
        private static Vector2 bldgScroll = Vector2.zero;
        private static Vector2 bldgScroll2 = Vector2.zero;
        private static Vector2 weapScroll = Vector2.zero;
        private static Vector2 appScroll = Vector2.zero;
        private static Vector2 foodScroll = Vector2.zero;
        private static Vector2 otherScroll = Vector2.zero;
        private static Listing_Standard listing = new Listing_Standard();
        public static ModSettings_QEverything settings;
        private static int currentTab;

        public Mod_QEverything(ModContentPack content) : base(content)
        {
            settings = GetSettings<ModSettings_QEverything>();
        }

        public override string SettingsCategory()
        {
            return "Everything Has Quality";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            /*ModSettings_QEverything.stuffDict ??= new Dictionary<string, bool>();
            ModSettings_QEverything.bldgDict ??= new Dictionary<string, bool>();
            ModSettings_QEverything.weapDict ??= new Dictionary<string, bool>();
            ModSettings_QEverything.appDict ??= new Dictionary<string, bool>();
            ModSettings_QEverything.otherDict ??= new Dictionary<string, bool>();*/

            Rect labelRect = new Rect(5f, 34f, inRect.width * .5f, 42f);
            listing.Begin(labelRect);
            if (listing.ButtonText("QEverything.ApplyCat".Translate()))
            {
                Mod_SettingsUtility.ApplySettingsChanges();
            }
            listing.End();
            Rect rect = new Rect(605f, 34f, inRect.width * .3f, 42f);
            listing.Begin(rect);
            string buttonText = "QEverything.CatDef".Translate();
            if (currentTab >= 2) buttonText = "QEverything.IndDef".Translate();
            if (listing.ButtonText(buttonText, null))
            {
                if (currentTab < 2) Mod_SettingsUtility.RestoreDefaults();
                else
                {
                    ModSettings_QEverything.stuffDict.Clear();
                    ModSettings_QEverything.bldgDict.Clear();
                    ModSettings_QEverything.weapDict.Clear();
                    ModSettings_QEverything.appDict.Clear();
                    ModSettings_QEverything.otherDict.Clear();
                    ModSettings_QEverything.indivStuff = false;
                    ModSettings_QEverything.indivBuildings = false;
                    ModSettings_QEverything.indivWeapons = false;
                    ModSettings_QEverything.indivApparel = false;
                    ModSettings_QEverything.indivOther = false;
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
                new TabRecord("QEverything.Tab0".Translate(), delegate()
                {
                    currentTab = 0;
                    settings.Write();
                },  currentTab == 0),
                new TabRecord("QEverything.Tab1".Translate(), delegate()
                {
                    currentTab = 1;
                    settings.Write();
                }, currentTab == 1),
                new TabRecord("QEverything.Tab2".Translate(), delegate()
                {
                    currentTab = 2;
                settings.Write();
                }, currentTab == 2),
                new TabRecord("QEverything.Tab3".Translate(), delegate()
                {
                    currentTab = 3;
                    settings.Write();
                }, currentTab == 3),
                new TabRecord("QEverything.Tab4".Translate(), delegate()
                {
                    currentTab = 4;
                    settings.Write();
                }, currentTab == 4),
                new TabRecord("QEverything.Tab5".Translate(), delegate()
                {
                    currentTab = 5;
                    settings.Write();
                }, currentTab == 5)
            };
            TabDrawer.DrawTabs(canvas, tabs, 200f);
            if (currentTab == 0) DoQualityByCategory(canvas.ContractedBy(10f));
            if (currentTab == 1) DoSkillsAndInspiration(canvas.ContractedBy(10f));
            if (currentTab == 2) DoCustomizeStuff(canvas);
            if (currentTab == 3) DoCustomizeBuildings(canvas);
            if (currentTab == 4) DoCustomizeWeapons(canvas.ContractedBy(10f));
            if (currentTab == 5) DoCustomizeOther(canvas);
        }

        public static void DoQualityByCategory(Rect rect)
        {
            Rect firstCol = new Rect(5f, 110f, rect.width * .35f, rect.height);
            listing.Begin(firstCol);
            listing.CheckboxLabeled("    " + "QEverything.Work".Translate(), ref ModSettings_QEverything.workQuality);
            listing.CheckboxLabeled("    " + "QEverything.Security".Translate(), ref ModSettings_QEverything.securityQuality);
            listing.CheckboxLabeled("    " + "QEverything.Edifice".Translate(), ref ModSettings_QEverything.edificeQuality);
            listing.CheckboxLabeled("    " + "QEverything.Stuff".Translate(), ref ModSettings_QEverything.stuffQuality);
            listing.CheckboxLabeled("    " + "QEverything.Manuf".Translate(), ref ModSettings_QEverything.manufQuality);
            listing.CheckboxLabeled("    " + "QEverything.Meal".Translate(), ref ModSettings_QEverything.mealQuality);
            listing.CheckboxLabeled("    " + "QEverything.Ingredients".Translate(), ref ModSettings_QEverything.ingredientQuality);
            if (ModSettings_QEverything.ingredientQuality) listing.Label("        " + "QEverything.Tasty".Translate());
            listing.CheckboxLabeled("    " + "QEverything.Drugs".Translate(), ref ModSettings_QEverything.drugQuality);
            listing.CheckboxLabeled("    " + "QEverything.Med".Translate(), ref ModSettings_QEverything.medQuality);
            listing.CheckboxLabeled("    " + "QEverything.Apparel".Translate(), ref ModSettings_QEverything.apparelQuality);
            listing.CheckboxLabeled("    " + "QEverything.Weapons".Translate(), ref ModSettings_QEverything.weaponQuality);
            listing.CheckboxLabeled("    " + "QEverything.Shells".Translate(), ref ModSettings_QEverything.shellQuality);
            listing.End();

            //Column2 - Min Quality Values
            Rect secondCol = new Rect(325f, 110f, rect.width * .3f, rect.height);
            listing.Begin(secondCol);

            string labelWork = "QEverything.Min".Translate() + ((QualityCategory)ModSettings_QEverything.minWorkQuality).ToString();
            string minWorkBuffer = ModSettings_QEverything.minWorkQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelWork, ref ModSettings_QEverything.minWorkQuality, ref minWorkBuffer, 1, 0, 2);
            

            string labelSecurity = "QEverything.Min".Translate() + ((QualityCategory)ModSettings_QEverything.minSecurityQuality).ToString();
            string minSecurityBuffer = ModSettings_QEverything.minSecurityQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelSecurity, ref ModSettings_QEverything.minSecurityQuality, ref minSecurityBuffer, 1, 0, 2);            

            string labelConstruction = "QEverything.Min".Translate() + ((QualityCategory)ModSettings_QEverything.minEdificeQuality).ToString();
            string minConstructionBuffer = ModSettings_QEverything.minEdificeQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelConstruction, ref ModSettings_QEverything.minEdificeQuality, ref minConstructionBuffer, 1, 0, 2);
            
            string labelStuff = "QEverything.Min".Translate() + ((QualityCategory)ModSettings_QEverything.minStuffQuality).ToString();
            string minStuffBuffer = ModSettings_QEverything.minStuffQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelStuff, ref ModSettings_QEverything.minStuffQuality, ref minStuffBuffer, 1, 0, 2);

            string labelManuf = "QEverything.Min".Translate() + ((QualityCategory)ModSettings_QEverything.minManufQuality).ToString();
            string minManufBuffer = ModSettings_QEverything.minManufQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelManuf, ref ModSettings_QEverything.minManufQuality, ref minManufBuffer, 1, 0, 2);

            string labelMeals = "QEverything.Min".Translate() + ((QualityCategory)ModSettings_QEverything.minMealQuality).ToString();
            string minMealBuffer = ModSettings_QEverything.minMealQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelMeals, ref ModSettings_QEverything.minMealQuality, ref minMealBuffer, 1, 0, 2);

            string labelIng = "QEverything.Min".Translate() + ((QualityCategory)ModSettings_QEverything.minIngQuality).ToString();
            string minIngBuffer = ModSettings_QEverything.minIngQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelIng, ref ModSettings_QEverything.minIngQuality, ref minIngBuffer, 1, 0, 2);

            string labelTasty = "QEverything.Min".Translate() + ((QualityCategory)ModSettings_QEverything.minTastyQuality).ToString();
            string minTastyBuffer = ModSettings_QEverything.minTastyQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelTasty, ref ModSettings_QEverything.minTastyQuality, ref minTastyBuffer, 1, 0, 2);

            string labelDrug = "QEverything.Min".Translate() + ((QualityCategory)ModSettings_QEverything.minDrugQuality).ToString();
            string minDrugBuffer = ModSettings_QEverything.minDrugQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelDrug, ref ModSettings_QEverything.minDrugQuality, ref minDrugBuffer, 1, 0, 2);

            string labelMed = "QEverything.Min".Translate() + ((QualityCategory)ModSettings_QEverything.minMedQuality).ToString();
            string minMedBuffer = ModSettings_QEverything.minMedQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelMed, ref ModSettings_QEverything.minMedQuality, ref minMedBuffer, 1, 0, 2);

            string labelApparel = "QEverything.Min".Translate() + ((QualityCategory)ModSettings_QEverything.minApparelQuality).ToString();
            string minApparelBuffer = ModSettings_QEverything.minApparelQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelApparel, ref ModSettings_QEverything.minApparelQuality, ref minApparelBuffer, 1, 0, 2);

            string labelWeapons = "QEverything.Min".Translate() + ((QualityCategory)ModSettings_QEverything.minWeaponQuality).ToString();
            string minWeaponBuffer = ModSettings_QEverything.minWeaponQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelWeapons, ref ModSettings_QEverything.minWeaponQuality, ref minWeaponBuffer, 1, 0, 2);

            string labelShell = "QEverything.Min".Translate() + ((QualityCategory)ModSettings_QEverything.minShellQuality).ToString();
            string minShellBuffer = ModSettings_QEverything.minShellQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelShell, ref ModSettings_QEverything.minShellQuality, ref minShellBuffer, 1, 0, 2);

            listing.End();

            //Column 3 - Max Qualiyt Values
            Rect thirdCol = new Rect(600f, 110f, rect.width * .3f, rect.height);
            listing.Begin(thirdCol);

            string labelWork2 = "QEverything.Max".Translate() + ((QualityCategory)ModSettings_QEverything.maxWorkQuality).ToString();
            string maxWorkBuffer = ModSettings_QEverything.maxWorkQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelWork2, ref ModSettings_QEverything.maxWorkQuality, ref maxWorkBuffer, 1, 2, 6);

            string labelSecurity2 = "QEverything.Max".Translate() + ((QualityCategory)ModSettings_QEverything.maxSecurityQuality).ToString();
            string maxSecurityBuffer = ModSettings_QEverything.maxSecurityQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelSecurity2, ref ModSettings_QEverything.maxSecurityQuality, ref maxSecurityBuffer, 1, 2, 6);

            string labelConstruction2 = "QEverything.Max".Translate() + ((QualityCategory)ModSettings_QEverything.maxEdificeQuality).ToString();
            string maxConstructionBuffer = ModSettings_QEverything.maxEdificeQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelConstruction2, ref ModSettings_QEverything.maxEdificeQuality, ref maxConstructionBuffer, 1, 2, 6);

            string labelStuff2 = "QEverything.Max".Translate() + ((QualityCategory)ModSettings_QEverything.maxStuffQuality).ToString();
            string maxStuffBuffer = ModSettings_QEverything.maxStuffQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelStuff2, ref ModSettings_QEverything.maxStuffQuality, ref maxStuffBuffer, 1, 2, 6);

            string labelManuf2 = "QEverything.Max".Translate() + ((QualityCategory)ModSettings_QEverything.maxManufQuality).ToString();
            string maxManufBuffer = ModSettings_QEverything.maxManufQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelManuf2, ref ModSettings_QEverything.maxManufQuality, ref maxManufBuffer, 1, 2, 6);

            string labelMeals2 = "QEverything.Max".Translate() + ((QualityCategory)ModSettings_QEverything.maxMealQuality).ToString();
            string maxMealBuffer = ModSettings_QEverything.maxMealQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelMeals2, ref ModSettings_QEverything.maxMealQuality, ref maxMealBuffer, 1, 2, 6);

            string labelIng2 = "QEverything.Max".Translate() + ((QualityCategory)ModSettings_QEverything.maxIngQuality).ToString();
            string maxIngBuffer = ModSettings_QEverything.maxIngQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelIng2, ref ModSettings_QEverything.maxIngQuality, ref maxIngBuffer, 1, 2, 6);

            string labelTasty2 = "QEverything.Max".Translate() + ((QualityCategory)ModSettings_QEverything.maxTastyQuality).ToString();
            string maxTastyBuffer = ModSettings_QEverything.maxTastyQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelTasty2, ref ModSettings_QEverything.maxTastyQuality, ref maxTastyBuffer, 1, 2, 6);

            string labelDrug2 = "QEverything.Max".Translate() + ((QualityCategory)ModSettings_QEverything.maxDrugQuality).ToString();
            string maxDrugBuffer = ModSettings_QEverything.maxDrugQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelDrug2, ref ModSettings_QEverything.maxDrugQuality, ref maxDrugBuffer, 1, 2, 6);

            string labelMed2 = "QEverything.Max".Translate() + ((QualityCategory)ModSettings_QEverything.maxMedQuality).ToString();
            string maxMedBuffer = ModSettings_QEverything.maxMedQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelMed2, ref ModSettings_QEverything.maxMedQuality, ref maxMedBuffer, 1, 2, 6);

            string labelApparel2 = "QEverything.Max".Translate() + ((QualityCategory)ModSettings_QEverything.maxApparelQuality).ToString();
            string maxApparelBuffer = ModSettings_QEverything.maxApparelQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelApparel2, ref ModSettings_QEverything.maxApparelQuality, ref maxApparelBuffer, 1, 2, 6);

            string labelWeapons2 = "QEverything.Max".Translate() + ((QualityCategory)ModSettings_QEverything.maxWeaponQuality).ToString();
            string maxWeaponBuffer = ModSettings_QEverything.maxWeaponQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelWeapons2, ref ModSettings_QEverything.maxWeaponQuality, ref maxWeaponBuffer, 1, 2, 6);

            string labelShell2 = "QEverything.Max".Translate() + ((QualityCategory)ModSettings_QEverything.maxShellQuality).ToString();
            string maxShellBuffer = ModSettings_QEverything.maxShellQuality.ToString();
            Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelShell2, ref ModSettings_QEverything.maxShellQuality, ref maxShellBuffer, 1, 2, 6);
            listing.End();
        }

        public static void DoSkillsAndInspiration(Rect rect)
        {
            Rect firstCol = new Rect(5f, 110f, rect.width * .35f, rect.height);
            listing.Begin(firstCol);
            listing.CheckboxLabeled("QEverything.Materials".Translate(), ref ModSettings_QEverything.useMaterialQuality);
            listing.CheckboxLabeled("QEverything.Tables".Translate(), ref ModSettings_QEverything.useTableQuality);
            listing.CheckboxLabeled("QEverything.SkillReq".Translate(), ref ModSettings_QEverything.useSkillReq);
            listing.GapLine();

            listing.CheckboxLabeled("QEverything.SkilledAnimals".Translate(), ref ModSettings_QEverything.skilledAnimals);
            listing.CheckboxLabeled("QEverything.SkilledButchering".Translate(), ref ModSettings_QEverything.skilledButchering);
            listing.CheckboxLabeled("QEverything.SkilledHarvesting".Translate(), ref ModSettings_QEverything.skilledHarvesting);
            listing.CheckboxLabeled("QEverything.SkilledMining".Translate(), ref ModSettings_QEverything.skilledMining);
            listing.CheckboxLabeled("QEverything.SkilledStoneCutting".Translate(), ref ModSettings_QEverything.skilledStoneCutting);
            listing.End();

            //Column Two
            Rect secondCol = new Rect(325f, 110f, rect.width * .3f, rect.height);
            listing.Begin(secondCol);
            if (ModSettings_QEverything.useMaterialQuality || ModSettings_QEverything.useTableQuality)
            {
                listing.Gap(12);
                string labelStd = "QEverything.Standard".Translate() + ((QualityCategory)ModSettings_QEverything.stdSupplyQuality).ToString();
                string stdBuffer = ModSettings_QEverything.stdSupplyQuality.ToString();
                Mod_SettingsUtility.LabeledIntEntry(listing.GetRect(24f), labelStd, ref ModSettings_QEverything.stdSupplyQuality, ref stdBuffer, 1, 0, 6);
                listing.Gap(12);
            }
            else listing.Gap(48);
            listing.Gap(24f);
            listing.GapLine();

            if (ModSettings_QEverything.stuffQuality)
            {
                if (ModSettings_QEverything.skilledAnimals)
                    listing.CheckboxLabeled("QEverything.InspiredGathering".Translate(), ref ModSettings_QEverything.inspiredGathering);
                else listing.Gap(24f);
                if (ModSettings_QEverything.skilledButchering)
                    listing.CheckboxLabeled("QEverything.InspiredButchering".Translate(), ref ModSettings_QEverything.inspiredButchering);
                else listing.Gap(24f);
                if (ModSettings_QEverything.skilledHarvesting)
                    listing.CheckboxLabeled("QEverything.InspiredHarvesting".Translate(), ref ModSettings_QEverything.inspiredHarvesting);
                else listing.Gap(24f);
                if (ModSettings_QEverything.skilledMining)
                    listing.CheckboxLabeled("QEverything.InspiredMining".Translate(), ref ModSettings_QEverything.inspiredMining);
                else listing.Gap(24f);
                if (ModSettings_QEverything.skilledStoneCutting)
                    listing.CheckboxLabeled("QEverything.InspiredStonecutting".Translate(), ref ModSettings_QEverything.inspiredStonecutting);
                else listing.Gap(24f);
            }
            listing.End();

            //Column Three
            Rect thirdCol = new Rect(600f, 110f, rect.width * .3f, rect.height);
            listing.Begin(thirdCol);
            if (ModSettings_QEverything.useMaterialQuality || ModSettings_QEverything.useTableQuality)
            {
                listing.Gap(15);
                string midLabel = (1 - ModSettings_QEverything.tableFactor).ToStringPercent() + " / " + ModSettings_QEverything.tableFactor.ToStringPercent();
                ModSettings_QEverything.tableFactor = Widgets.HorizontalSlider(listing.GetRect(23f), ModSettings_QEverything.tableFactor, 0, 1f, false, midLabel, "Materials", "Work Table");
                listing.Gap(10);
            }
            else listing.Gap(48);
            listing.Gap(24f);
            listing.GapLine();
            listing.CheckboxLabeled("QEverything.InspiredChemistry".Translate(), ref ModSettings_QEverything.inspiredChemistry);
            listing.CheckboxLabeled("QEverything.InspiredConstruction".Translate(), ref ModSettings_QEverything.inspiredConstruction);
            listing.CheckboxLabeled("QEverything.InspiredCooking".Translate(), ref ModSettings_QEverything.inspiredCooking);
            listing.End();
        }

        public static void DoCustomizeStuff(Rect rect)
        {
            {
                listing.Begin(new Rect(5f, 110f, rect.width * .33f - 5f, 30f));
                if (!ModSettings_QEverything.indivStuff)
                {
                    if (listing.ButtonTextLabeled("QEverything.Resources".Translate(), "QEverything.Enable".Translate()))
                    {
                        Mod_SettingsUtility.PopulateStuff();
                        ModSettings_QEverything.indivStuff = true;
                    }
                }
                else if (listing.ButtonTextLabeled("QEverything.Resources".Translate(), "QEverything.Disable".Translate()))
                {
                    ModSettings_QEverything.indivStuff = false;
                }
                listing.End();
                List<string> keyList = ModSettings_QEverything.stuffDict.Keys.ToList<string>();
                keyList.Sort();

                listing.Begin(new Rect(rect.width * .33f + 5f, 110f, rect.width * .33f - 5f, 30f));
                if (listing.ButtonText("QEverything.Select".Translate(), null))
                {
                    for (int i = 0; i < keyList.Count; i++)
                    {
                        ModSettings_QEverything.stuffDict[keyList[i]] = true;
                    }
                }
                listing.End();
                listing.Begin(new Rect(rect.width * .66f + 5f, 110f, rect.width * .33f - 5f, 30f));
                if (listing.ButtonText("QEverything.Deselect".Translate(), null))
                {
                    for (int j = 0; j < keyList.Count; j++)
                    {
                        ModSettings_QEverything.stuffDict[keyList[j]] = false;
                    }
                }
                listing.End();

                listing.Begin(new Rect(0f, 140f, rect.width, 10f));
                listing.GapLine();
                listing.End();
                listing.Begin(new Rect(5f, 150f, rect.width * .5f - 10f, 38f));
                listing.Label("QEverything.Textiles".Translate());
                listing.GapLine();
                listing.End();
                listing.Begin(new Rect(rect.width * .5f + 5f, 150f, rect.width * .5f - 10f, 38f));
                listing.Label("QEverything.OtherRes".Translate());
                listing.GapLine();
                listing.End();
                if (ModSettings_QEverything.indivStuff)
                {
                    List<string> list = new List<string>();
                    List<string> other = new List<string>();
                    bool value;
                    //Log.Message("lists created");
                    for (int k = 0; k < ModSettings_QEverything.stuffDict.Count; k++)
                    {
                        //Log.Message("Filling lists");
                        string key2 = keyList[k];
                        ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(key2);
                        if (def != null)
                        {
                            if (def.IsLeather || def.IsWithinCategory(ThingCategoryDefOf.Textiles))
                            {
                                list.Add(key2);
                            }
                            else other.Add(key2);
                        }
                    }
                    //Log.Message("Displaying first scroll");
                    Rect scrollRect = new Rect(5f, 190f, rect.width * .5f - 10f, rect.height - 110f);
                    Rect viewRect = new Rect(0f, 0f, scrollRect.width - 30f, list.Count * 24f);
                    Widgets.BeginScrollView(scrollRect, ref texScroll, viewRect, true);
                    listing.Begin(viewRect);
                    string key;
                    for (int m = 0; m < list.Count; m++)
                    {
                        key = list[m];
                        value = ModSettings_QEverything.stuffDict[key];
                        listing.CheckboxLabeled(key.CapitalizeFirst(), ref value);
                        ModSettings_QEverything.stuffDict[key] = value;
                    }
                    listing.End();
                    Widgets.EndScrollView();
                    //Log.Message("Displaying second scroll");
                    Rect scrollRect2 = new Rect(rect.width * .5f + 5f, 190f, rect.width * .5f - 10f, rect.height - 110f);
                    Rect viewRect2 = new Rect(0f, 0f, scrollRect2.width - 30f, other.Count * 24f);
                    Widgets.BeginScrollView(scrollRect2, ref resScroll, viewRect2, true);
                    listing.Begin(viewRect2);
                    for (int n = 0; n < other.Count; n++)
                    {
                        key = other[n];
                        value = ModSettings_QEverything.stuffDict[key];
                        listing.CheckboxLabeled(key.CapitalizeFirst(), ref value);
                        ModSettings_QEverything.stuffDict[key] = value;
                    }
                    listing.End();
                    Widgets.EndScrollView();
                }
            }
        }

        public static void DoCustomizeBuildings(Rect rect)
        {
            listing.Begin(new Rect(5f, 110f, rect.width * .33f - 5f, 30f));
            if (!ModSettings_QEverything.indivBuildings)
            {
                if (listing.ButtonTextLabeled("QEverything.Buildings".Translate(), "QEverything.Enable".Translate()))
                {
                    Mod_SettingsUtility.PopulateBuildings();
                    ModSettings_QEverything.indivBuildings = true;
                    //Log.Message("Building dictionary has " + ModSettings_QFramework.bldgDict.Count + " buildings.");
                }
            }
            else if (listing.ButtonTextLabeled("QEverything.Buildings".Translate(), "QEverything.Disable".Translate()))
            {
                ModSettings_QEverything.indivBuildings = false;
            }
            listing.End();
            List<string> keyList = ModSettings_QEverything.bldgDict.Keys.ToList<string>();
            keyList.Sort();
            string key;
            listing.Begin(new Rect(rect.width * .33f + 5f, 110f, rect.width * .33f - 5f, 30f));
            if (listing.ButtonText("QEverything.Select".Translate(), null))
            {
                for (int i = 0; i < keyList.Count; i++)
                {
                    key = keyList[i];
                    ModSettings_QEverything.bldgDict[key] = true;
                }
            }
            listing.End();
            listing.Begin(new Rect(rect.width * .66f + 5f, 110f, rect.width * .33f - 5f, 30f));
            if (listing.ButtonText("QEverything.Deselect".Translate(), null))
            {
                for (int j = 0; j < keyList.Count; j++)
                {
                    key = keyList[j];
                    ModSettings_QEverything.bldgDict[key] = false;
                }
            }
            listing.End();
            listing.Begin(new Rect(0f, 140f, rect.width, 10f));
            listing.GapLine();
            listing.End();
            listing.Begin(new Rect(5f, 150f, rect.width * .5f - 10f, 38f));
            listing.Label("QEverything.Furniture".Translate());
            listing.GapLine();
            listing.End();
            listing.Begin(new Rect(rect.width * .5f + 5f, 150f, rect.width * .5f - 10f, 38f));
            listing.Label("QEverything.Power".Translate());
            listing.GapLine();
            listing.End();
            if (ModSettings_QEverything.indivBuildings)
            {
                List<string> list = new List<string>();
                List<string> other = new List<string>();
                bool value;
                for (int k = 0; k < ModSettings_QEverything.bldgDict.Count; k++)
                {
                    string key2 = keyList[k];
                    ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(key2);
                    if (def != null)
                    {
                        if (def.IsWithinCategory(ThingCategoryDef.Named("BuildingsFurniture"))
                            || def.IsWithinCategory(ThingCategoryDef.Named("BuildingsProduction"))
                            || def.IsWithinCategory(ThingCategoryDefOf.BuildingsArt))
                        {
                            list.Add(key2);
                        }
                        else other.Add(key2);
                    }
                }
                Rect scrollRect = new Rect(5f, 190f, rect.width * .5f - 10f, rect.height - 110f);
                Rect viewRect = new Rect(0f, 0f, scrollRect.width - 30f, list.Count * 24f);
                Widgets.BeginScrollView(scrollRect, ref bldgScroll, viewRect, true);
                listing.Begin(viewRect);
                for (int m = 0; m < list.Count; m++)
                {
                    key = list[m];
                    value = ModSettings_QEverything.bldgDict[key];
                    listing.CheckboxLabeled(key.CapitalizeFirst(), ref value);
                    ModSettings_QEverything.bldgDict[key] = value;
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
                    value = ModSettings_QEverything.bldgDict[key];
                    listing.CheckboxLabeled(key.CapitalizeFirst(), ref value);
                    ModSettings_QEverything.bldgDict[key] = value;
                }
                listing.End();
                Widgets.EndScrollView();
            }
        }

        public static void DoCustomizeWeapons(Rect rect)
        {
            Rect firstCol = new Rect(5f, 110f, rect.width * .48f, rect.height);
            listing.Begin(firstCol);
            if (!ModSettings_QEverything.indivWeapons)
            {
                if (listing.ButtonTextLabeled("QEverything.WeapShells".Translate(), "QEverything.Enable".Translate()))
                {
                    Mod_SettingsUtility.PopulateWeapons();
                    ModSettings_QEverything.indivWeapons = true;
                    //Log.Message("Weapon dictionary has " + ModSettings_QFramework.weapDict.Count + " items.");
                }
                //listing.GapLine();
            }
            else
            {
                if (listing.ButtonTextLabeled("QEverything.WeapShells".Translate(), "QEverything.Disable".Translate()))
                {
                    ModSettings_QEverything.indivWeapons = false;
                    //Log.Message("Weapon dictionary has " + ModSettings_QFramework.weapDict.Count + " items.");
                }
            }
            List<string> weapons = new List<string>(ModSettings_QEverything.weapDict.Keys);
            weapons.Sort();
            string weap;
            if (listing.ButtonText("QEverything.Select".Translate(), null))
            {
                for (int b1 = 0; b1 < weapons.Count; b1++)
                {
                    weap = weapons[b1];
                    ModSettings_QEverything.weapDict[weap] = true;
                }
            }
            if (listing.ButtonText("QEverything.Deselect".Translate(), null))
            {
                for (int b2 = 0; b2 < weapons.Count; b2++)
                {
                    weap = weapons[b2];
                    ModSettings_QEverything.weapDict[weap] = false;
                }
            }
            listing.GapLine();
            listing.End();
            if (ModSettings_QEverything.indivWeapons)
            {
                Rect scrollRect = new Rect(5f, 220f, rect.width * .48f, rect.height - 110f);
                Rect viewRect = new Rect(0f, 0f, scrollRect.width - 30f, weapons.Count * 24f);
                Widgets.BeginScrollView(scrollRect, ref weapScroll, viewRect, true);
                listing.Begin(viewRect);
                for (int b3 = 0; b3 < ModSettings_QEverything.weapDict.Count; b3++)
                {
                    weap = weapons[b3];
                    bool weapQual = ModSettings_QEverything.weapDict[weap];
                    listing.CheckboxLabeled(weap.CapitalizeFirst(), ref weapQual);
                    ModSettings_QEverything.weapDict[weap] = weapQual;
                }
                listing.End();
                Widgets.EndScrollView();
            }

            Rect secondCol = new Rect(rect.width * .5f, 110f, rect.width * .48f, rect.height);
            listing.Begin(secondCol);
            if (!ModSettings_QEverything.indivApparel)
            {
                if (listing.ButtonTextLabeled("QEverything.Clothing".Translate(), "QEverything.Enable".Translate()))
                {
                    Mod_SettingsUtility.PopulateApparel();
                    ModSettings_QEverything.indivApparel = true;
                }
            }
            else
            {
                if (listing.ButtonTextLabeled("QEverything.Clothing".Translate(), "QEverything.Disable".Translate()))
                {
                    ModSettings_QEverything.indivApparel = false;
                }
            }
            List<string> apparel = new List<string>(ModSettings_QEverything.appDict.Keys);
            apparel.Sort();
            string app;
            if (listing.ButtonText("QEverything.Select".Translate(), null))
            {
                for (int b1 = 0; b1 < apparel.Count; b1++)
                {
                    app = apparel[b1];
                    ModSettings_QEverything.appDict[app] = true;
                }
            }
            if (listing.ButtonText("QEverything.Deselect".Translate(), null))
            {
                for (int b2 = 0; b2 < apparel.Count; b2++)
                {
                    app = apparel[b2];
                    ModSettings_QEverything.appDict[app] = false;
                }
            }
            listing.GapLine();
            listing.End();
            if (ModSettings_QEverything.indivApparel)
            {
                Rect scrollRect = new Rect(rect.width * .5f, 220f, rect.width * .48f, rect.height - 110f);
                Rect viewRect = new Rect(0f, 0f, scrollRect.width - 30f, apparel.Count * 24f);
                Widgets.BeginScrollView(scrollRect, ref appScroll, viewRect, true);
                listing.Begin(viewRect);
                for (int b3 = 0; b3 < ModSettings_QEverything.appDict.Count; b3++)
                {
                    app = apparel[b3];
                    bool appQual = ModSettings_QEverything.appDict[app];
                    listing.CheckboxLabeled(app.CapitalizeFirst(), ref appQual);
                    ModSettings_QEverything.appDict[app] = appQual;
                }
                listing.End();
                Widgets.EndScrollView();
            }
        }

        public static void DoCustomizeOther(Rect rect)
        {
            {
                listing.Begin(new Rect(5f, 110f, rect.width * .33f - 5f, 30f));
                if (!ModSettings_QEverything.indivOther)
                {
                    if (listing.ButtonText("QEverything.Enable".Translate(), null))
                    {
                        Mod_SettingsUtility.PopulateOther();
                        ModSettings_QEverything.indivOther = true;
                        //Log.Message("Other dictionary has " + ModSettings_QFramework.otherDict.Count + " buildings.");
                    }
                }
                else if (listing.ButtonText("QEverything.Disable".Translate(), null))
                {
                    ModSettings_QEverything.indivOther = false;
                }
                listing.End();
                List<string> keyList = new List<string>(ModSettings_QEverything.otherDict.Keys);
                keyList.Sort();
                string key;
                listing.Begin(new Rect(0f, 140f, rect.width, 10f));
                listing.GapLine();
                listing.End();
                listing.Begin(new Rect(5f, 150f, rect.width * .5f - 10f, 38f));
                listing.Label("QEverything.Food".Translate());
                listing.GapLine();
                listing.End();
                listing.Begin(new Rect(rect.width * .5f + 5f, 150f, rect.width * .5f - 10f, 38f));
                listing.Label("QEverything.OtherFood".Translate());
                listing.GapLine();
                listing.End();
                if (ModSettings_QEverything.indivOther)
                {
                    List<string> list = new List<string>();
                    List<string> other = new List<string>();
                    bool value;
                    //Log.Message("lists created");
                    for (int k = 0; k < ModSettings_QEverything.otherDict.Count; k++)
                    {
                        //Log.Message("Filling lists");
                        key = keyList[k];
                        ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(key);
                        if (def != null)
                        {
                            if (def.IsDrug)
                            {
                                other.Add(key);
                            }
                            else if (def.IsNutritionGivingIngestible)
                            {
                                list.Add(key);
                            }
                            else other.Add(key);
                        }
                    }
                    //Log.Message("Displaying first scroll");
                    Rect scrollRect = new Rect(5f, 190f, rect.width * .5f - 10f, rect.height - 110f);
                    Rect viewRect = new Rect(0f, 0f, scrollRect.width - 30f, list.Count * 24f);
                    Widgets.BeginScrollView(scrollRect, ref foodScroll, viewRect, true);
                    listing.Begin(viewRect);
                    for (int m = 0; m < list.Count; m++)
                    {
                        key = list[m];
                        value = ModSettings_QEverything.otherDict[key];
                        listing.CheckboxLabeled(key.CapitalizeFirst(), ref value);
                        ModSettings_QEverything.otherDict[key] = value;
                    }
                    listing.End();
                    Widgets.EndScrollView();
                    //Log.Message("Displaying second scroll");
                    Rect scrollRect2 = new Rect(rect.width * .5f + 5f, 190f, rect.width * .5f - 10f, rect.height - 110f);
                    Rect viewRect2 = new Rect(0f, 0f, scrollRect2.width - 30f, other.Count * 24f);
                    Widgets.BeginScrollView(scrollRect2, ref otherScroll, viewRect2, true);
                    listing.Begin(viewRect2);
                    for (int n = 0; n < other.Count; n++)
                    {
                        key = other[n];
                        value = ModSettings_QEverything.otherDict[key];
                        listing.CheckboxLabeled(key.CapitalizeFirst(), ref value);
                        ModSettings_QEverything.otherDict[key] = value;
                    }
                    listing.End();
                    Widgets.EndScrollView();
                }
            }
        }
    }
}

