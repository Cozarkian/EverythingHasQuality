using System.Collections.Generic;
using Verse;

namespace QualityEverything
{
    public class ModSettings_QEverything : ModSettings
    {

        public static bool useMaterialQuality = true;
        public static bool useTableQuality = true;
        public static bool useSkillReq = true;
        public static int stdSupplyQuality = 0;
        public static float tableFactor = .4f;

        public static bool inspiredButchering = true;
        public static bool inspiredChemistry = true;
        public static bool inspiredCooking = true;
        public static bool inspiredConstruction = true;
        public static bool inspiredGathering = true;
        public static bool inspiredHarvesting = true;
        public static bool inspiredMining = true;
        public static bool inspiredStonecutting = true;

        public static bool skilledAnimals = false;
        public static bool skilledButchering = false;
        public static bool skilledHarvesting = false;
        public static bool skilledMining = false;
        public static bool skilledStoneCutting = false;

        public static bool edificeQuality = true;
        public static int minEdificeQuality = 0;
        public static int maxEdificeQuality = 4;

        public static bool workQuality = true;
        public static int minWorkQuality = 0;
        public static int maxWorkQuality = 4;

        public static bool securityQuality = true;
        public static int minSecurityQuality = 0;
        public static int maxSecurityQuality = 4;

        public static bool stuffQuality = true;
        public static int minStuffQuality = 0;
        public static int maxStuffQuality = 4;

        public static bool ingredientQuality = true;
        public static int minIngQuality = 2;
        public static int maxIngQuality = 4;
        public static int minTastyQuality = 0;
        public static int maxTastyQuality = 4;
        
        public static bool mealQuality = false;
        public static int minMealQuality = 0;
        public static int maxMealQuality = 4;
        
        public static bool drugQuality = false;
        public static int minDrugQuality = 0;
        public static int maxDrugQuality = 4;

        public static bool medQuality = false;
        public static int minMedQuality = 0;
        public static int maxMedQuality = 4;

        public static bool manufQuality = true;
        public static int minManufQuality = 0;
        public static int maxManufQuality = 4;

        public static bool apparelQuality = false;
        public static int minApparelQuality = 0;
        public static int maxApparelQuality = 6;

        public static bool weaponQuality = false;
        public static int minWeaponQuality = 0;
        public static int maxWeaponQuality = 6;

        public static bool shellQuality = false;
        public static int minShellQuality = 0;
        public static int maxShellQuality = 4;

        public static bool indivStuff = false;
        public static bool indivBuildings = false;
        public static bool indivWeapons = false;
        public static bool indivApparel = false;
        public static bool indivOther = false;

        public static Dictionary<string, bool> stuffDict = new Dictionary<string, bool>();
        public static List<string> stuffKeys = new List<string>();
        public static List<bool> stuffValues = new List<bool>();

        public static Dictionary<string, bool> bldgDict = new Dictionary<string, bool>();
        public static List<string> bldgKeys = new List<string>();
        public static List<bool> bldgValues = new List<bool>();

        public static Dictionary<string, bool> weapDict = new Dictionary<string, bool>();
        public static List<string> weapKeys = new List<string>();
        public static List<bool> weapValues = new List<bool>();

        public static Dictionary<string, bool> appDict = new Dictionary<string, bool>();
        public static List<string> appKeys = new List<string>();
        public static List<bool> appValues = new List<bool>();

        public static Dictionary<string, bool> otherDict = new Dictionary<string, bool>();
        public static List<string> otherKeys = new List<string>();
        public static List<bool> otherValues = new List<bool>();

        public override void ExposeData()
        {
            Scribe_Values.Look(ref useMaterialQuality, "useMaterialQuality", true);
            Scribe_Values.Look(ref useTableQuality, "useTableQuality", true);
            Scribe_Values.Look(ref useSkillReq, "useSkillReq", true);
            Scribe_Values.Look(ref stdSupplyQuality, "stdSupplyQuality", 0);

            Scribe_Values.Look(ref inspiredButchering, "inspiredButchering", true);
            Scribe_Values.Look(ref inspiredChemistry, "inspiredChemistry", true);
            Scribe_Values.Look(ref inspiredCooking, "inspiredCooking", true);
            Scribe_Values.Look(ref inspiredConstruction, "inspiredConstruction", true);
            Scribe_Values.Look(ref inspiredGathering, "inspiredGathering", true);
            Scribe_Values.Look(ref inspiredHarvesting, "inspiredHarvesting", true);
            Scribe_Values.Look(ref inspiredMining, "inspiredMining", true);
            Scribe_Values.Look(ref inspiredStonecutting, "inspiredStonecutting", true);

            Scribe_Values.Look(ref skilledAnimals, "skilledAnimals", false);
            Scribe_Values.Look(ref skilledButchering, "skilledButchering", false);
            Scribe_Values.Look(ref skilledHarvesting, "skilledHarvesting", false);
            Scribe_Values.Look(ref skilledMining, "skilledMining", false);
            Scribe_Values.Look(ref skilledStoneCutting, "skilledStoneCutting", false);

            Scribe_Values.Look(ref edificeQuality, "edificeQuality", true);
            Scribe_Values.Look(ref minEdificeQuality, "minEdificeQuality", 0);
            Scribe_Values.Look(ref maxEdificeQuality, "maxEdificeQuality", 4);

            Scribe_Values.Look(ref workQuality, "workQuality", true);
            Scribe_Values.Look(ref minWorkQuality, "minWorkQuality", 0);
            Scribe_Values.Look(ref maxWorkQuality, "maxWorkQuality", 4);

            Scribe_Values.Look(ref securityQuality, "securityQuality", true);
            Scribe_Values.Look(ref minSecurityQuality, "minSecurityQuality", 0);
            Scribe_Values.Look(ref maxSecurityQuality, "maxSecurityQuality", 4);

            Scribe_Values.Look(ref stuffQuality, "stuffQuality", true);
            Scribe_Values.Look(ref minStuffQuality, "minStuffQuality", 0);
            Scribe_Values.Look(ref maxStuffQuality, "maxStuffQuality", 4);

            Scribe_Values.Look(ref ingredientQuality, "ingredientQuality", true);
            Scribe_Values.Look(ref minIngQuality, "minIngQuality", 2);
            Scribe_Values.Look(ref maxIngQuality, "maxIngQuality", 4);
            Scribe_Values.Look(ref minTastyQuality, "minTastyQuality", 0);
            Scribe_Values.Look(ref maxTastyQuality, "maxTastyQuality", 4);

            Scribe_Values.Look(ref mealQuality, "mealQuality", false);
            Scribe_Values.Look(ref minMealQuality, "minMealQuality", 0);
            Scribe_Values.Look(ref maxMealQuality, "maxMealQuality", 4); 
            
            Scribe_Values.Look(ref drugQuality, "drugQuality", false);
            Scribe_Values.Look(ref minDrugQuality, "minDrugQuality", 0);
            Scribe_Values.Look(ref maxDrugQuality, "maxDrugQuality", 4); 
            
            Scribe_Values.Look(ref medQuality, "medQuality", false);
            Scribe_Values.Look(ref minMedQuality, "minMedQuality", 0);
            Scribe_Values.Look(ref maxMedQuality, "maxMedQuality", 4);

            Scribe_Values.Look(ref manufQuality, "manufQuality", true);
            Scribe_Values.Look(ref minManufQuality, "minManufQuality", 0);
            Scribe_Values.Look(ref maxManufQuality, "maxManufQuality", 4);

            Scribe_Values.Look(ref apparelQuality, "apparelQuality", false);
            Scribe_Values.Look(ref minApparelQuality, "minApparelQuality", 0);
            Scribe_Values.Look(ref maxApparelQuality, "maxApparelQuality", 6);

            Scribe_Values.Look(ref weaponQuality, "weaponQuality", false);
            Scribe_Values.Look(ref minWeaponQuality, "minWeapoQuality", 0);
            Scribe_Values.Look(ref maxWeaponQuality, "maxWeapoQuality", 6);

            Scribe_Values.Look(ref shellQuality, "shellQuality", true);
            Scribe_Values.Look(ref minShellQuality, "minShellQuality", 0);
            Scribe_Values.Look(ref maxShellQuality, "maxShellQuality", 4);

            Scribe_Values.Look(ref indivStuff, "indivStuff", false);
            Scribe_Values.Look(ref indivBuildings, "indivBuildings", false);
            Scribe_Values.Look(ref indivWeapons, "indivWeapons", false);
            Scribe_Values.Look(ref indivApparel, "indivApparel", false);
            Scribe_Values.Look(ref indivOther, "indivOther", false);

            Scribe_Collections.Look<string, bool>(ref stuffDict, "stuffDict", LookMode.Value, LookMode.Value, ref stuffKeys, ref stuffValues);
            Scribe_Collections.Look<string, bool>(ref bldgDict, "bldgDict", LookMode.Value, LookMode.Value, ref bldgKeys, ref bldgValues);
            Scribe_Collections.Look<string, bool>(ref weapDict, "weapDict", LookMode.Value, LookMode.Value, ref weapKeys, ref weapValues);
            Scribe_Collections.Look<string, bool>(ref appDict, "appDict", LookMode.Value, LookMode.Value, ref appKeys, ref appValues);
            Scribe_Collections.Look<string, bool>(ref otherDict, "otherDict", LookMode.Value, LookMode.Value, ref otherKeys, ref otherValues);

            base.ExposeData();
        }
    }
}
