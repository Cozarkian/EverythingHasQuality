using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;

namespace QualityFramework
{
    [HarmonyPatch]
    class IngredientsMatter
    {
        private static int score;

        [HarmonyPatch(typeof(GenRecipe), "MakeRecipeProducts")]
        [HarmonyPrefix]
        public static void CheckIngredientQuality(List<Thing> ingredients)
        {
            int score = 0;
            if (ingredients == null || ingredients.Count < 1)
            {
                return;
            }
            for (int i = 0; i < ingredients.Count; i++)
            {
                CompQuality comp = ingredients[i].TryGetComp<CompQuality>();
                if (comp != null)
                {
                    score += (int)comp.Quality;
                }
            }
            score = score / ingredients.Count;
        }


        private static void PostProcessProduct (Thing product, RecipeDef recipeDef)
        {

        }
    }
}
