using Verse;

namespace QualityFramework
{
    public class ModSettings_QFramework : ModSettings
    {
        public static bool useMaterialQuality = true;
        public static bool useTableQuality = true;
        public static int stdSupplyQuality = 4;
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

        //public static bool lessRandomQuality = true;
        //public static int minSkillEx = 10;
        //public static int maxSkillAw = 17;

        public static bool frameQuality = true;
        public static int minFrameQuality = 0;
        public static int maxFrameQuality = 4;

        public static bool workQuality = true;
        public static int minWorkQuality = 0;
        public static int maxWorkQuality = 4;

        public static bool powerQuality = true;
        public static int minPowerQuality = 0;
        public static int maxPowerQuality = 4;

        public static bool securityQuality = true;
        public static int minSecurityQuality = 0;
        public static int maxSecurityQuality = 4;

        public static bool edificeQuality = true;
        public static int minEdificeQuality = 0;
        public static int maxEdificeQuality = 4;

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

        public override void ExposeData()
        {
            Scribe_Values.Look(ref useMaterialQuality, "useMaterialQuality", true);
            Scribe_Values.Look(ref useTableQuality, "useTableQuality", true);
            Scribe_Values.Look(ref stdSupplyQuality, "stdSupplyQuality", 4);

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

            Scribe_Values.Look(ref frameQuality, "frameQuality", true);
            Scribe_Values.Look(ref minFrameQuality, "minFrameQuality", 0);
            Scribe_Values.Look(ref maxFrameQuality, "maxFrameQuality", 4);

            Scribe_Values.Look(ref workQuality, "workQuality", true);
            Scribe_Values.Look(ref minWorkQuality, "minWorkQuality", 0);
            Scribe_Values.Look(ref maxWorkQuality, "maxWorkQuality", 4);

            Scribe_Values.Look(ref powerQuality, "powerQuality", true);
            Scribe_Values.Look(ref minPowerQuality, "minPowerQuality", 0);
            Scribe_Values.Look(ref maxPowerQuality, "maxPowerQuality", 4);

            Scribe_Values.Look(ref securityQuality, "securityQuality", true);
            Scribe_Values.Look(ref minSecurityQuality, "minSecurityQuality", 0);
            Scribe_Values.Look(ref maxSecurityQuality, "maxSecurityQuality", 4);

            Scribe_Values.Look(ref edificeQuality, "edificeQuality", true);
            Scribe_Values.Look(ref minEdificeQuality, "minEdificeQuality", 0);
            Scribe_Values.Look(ref maxEdificeQuality, "maxEdificeQuality", 4);

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
            base.ExposeData();
        }
    }
}
