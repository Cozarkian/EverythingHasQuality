 using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using RimWorld;
using Verse;
using UnityEngine;
using HarmonyLib;

namespace QualityFramework
{
    [HarmonyPatch]
    class Quality_Recipes
    {
        public static int SupplyQuality { get; set; } = -1;

        [HarmonyPatch(typeof(GenRecipe), "PostProcessProduct")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> RecipeQuality(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo OriginalQualityGenerator = AccessTools.Method(typeof(QualityUtility), "GenerateQualityCreatedByPawn", new Type[] { typeof(Pawn), typeof(SkillDef) }, null);
            List<CodeInstruction> codeInstructions = instructions.ToList<CodeInstruction>();
            CodeInstruction code;
            bool errorFound = false; //Log.Message("Haven't found error");
            for (int i = 0; i < codeInstructions.Count; i++)
            {
                code = codeInstructions[i];
                if (!errorFound && code.opcode == OpCodes.Brtrue_S)
                {
                    yield return code;
                    //Log.Message("Looking for error");
                    int j = i + 1; //Log.Message("j set as " + j.ToString());
                    while (j < codeInstructions.Count)
                    {
                        code = codeInstructions[j];
                        if (code.opcode == OpCodes.Call && (MethodInfo)code.operand == AccessTools.Method(typeof(Verse.Log), "Error", new Type[] { typeof(string) }))
                        {
                            errorFound = true; //Log.Message("Error found is " + errorFound.ToString());
                            i = j; //Log.Message("i set as " + i.ToString());
                            yield return new CodeInstruction(OpCodes.Nop);
                            break;
                        }
                        j++;
                    }
                }
                else if (code.opcode == OpCodes.Call && (MethodInfo)code.operand == OriginalQualityGenerator)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(Quality_Recipes), "get_SupplyQuality"));
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Quality_Generator), "GenerateQualityCreatedByPawn"));
                }
                else
                {
                    yield return code;
                }
            }
            //Log.Message("Recipe patch successful");
            yield break;
        }

        [HarmonyPatch(typeof(GenRecipe), "MakeRecipeProducts")]
        public static void Prefix(List<Thing> ingredients, IBillGiver billGiver)
        {
            SupplyQuality = -1; //Precautionary reset before starting
            float value = -1;
            int numIng = 0;
            if (ingredients != null && ModSettings_QFramework.useMaterialQuality)
            {
                for (int i = 0; i < ingredients.Count; i++)
                {
                    CompQuality comp = ingredients[i].TryGetComp<CompQuality>();
                    if (comp != null)
                    {
                        value += (int)comp.Quality * ingredients[i].stackCount;
                        numIng += ingredients[i].stackCount;
                    }
                }
                if (numIng > 0)
                {
                    value = (value + 1) / numIng;
                }
                Log.Message("Ingredients are quality " + value);
            }
            //Check for work table quality
            Building_WorkTable workTable = billGiver as Building_WorkTable;
            if (workTable != null && ModSettings_QFramework.useTableQuality)
            {
                CompQuality tableQuality = workTable.TryGetComp<CompQuality>();
                if (tableQuality != null)
                {
                    Log.Message("Table is quality " + (int)tableQuality.Quality);
                    if (numIng == 0) value = (int)tableQuality.Quality;
                    else value = value * (1 - ModSettings_QFramework.tableFactor) + (int)tableQuality.Quality * ModSettings_QFramework.tableFactor;
                }
            }
            if (value >= 0)
            {
                SupplyQuality = Mathf.Clamp(GenMath.RoundRandom(value), 0, 6);
            }
            Log.Message("SupplyQuality is " + SupplyQuality + " and should be " + value);
        }
    }
}
