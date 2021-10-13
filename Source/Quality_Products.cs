using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using RimWorld;
using Verse;
using Verse.AI;
using HarmonyLib;

namespace QualityFramework
{
    [HarmonyPatch]
    public static class Quality_Products
    {
        public static int SupplyQuality { get; set; } = -1;

        [HarmonyPatch(typeof(Frame), "CompleteConstruction")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> ConstructionQuality(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            MethodInfo OriginalQualityGenerator = AccessTools.Method(typeof(QualityUtility), "GenerateQualityCreatedByPawn", new Type[]{ typeof(Pawn), typeof(SkillDef) }, null);
            MethodInfo CheckResourceQuality = AccessTools.Method(typeof(Quality_Utility), "CheckResourceQuality");

            LocalBuilder resourceQuality = generator.DeclareLocal(typeof(int)); //Log.Message("Declared variable");
            yield return new CodeInstruction(OpCodes.Ldarg_0);
            yield return new CodeInstruction(OpCodes.Call, CheckResourceQuality);
            yield return new CodeInstruction(OpCodes.Stloc, resourceQuality);
            foreach (CodeInstruction codeInstruction in instructions)
            {
                if (codeInstruction.opcode == OpCodes.Call && (MethodInfo)codeInstruction.operand == OriginalQualityGenerator)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Quality_Generator), "GenerateQualityCreatedByPawn"));
                }
                else
                {
                    yield return codeInstruction;
                }
            }
            Log.Message("Construction patch successful");
            yield break;
        }

        [HarmonyPatch(typeof(GenRecipe), "PostProcessProduct")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> RecipeQuality(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo OriginalQualityGenerator = AccessTools.Method(typeof(QualityUtility), "GenerateQualityCreatedByPawn", new Type[] { typeof(Pawn), typeof(SkillDef) }, null);
            MethodInfo CheckResourceQuality = AccessTools.Method(typeof(Quality_Utility), "CheckResourceQuality");

            //yield return new CodeInstruction(OpCodes.Ldarg_S, 1);
            //yield return new CodeInstruction(OpCodes.Call, CheckResourceQuality);
            List<CodeInstruction> codeInstructions = instructions.ToList<CodeInstruction>();
            CodeInstruction code;
            bool errorFound = false; Log.Message("Haven't found error");
            for (int i = 0; i < codeInstructions.Count; i++)
            {
                code = codeInstructions[i];
                if (!errorFound && code.opcode == OpCodes.Brtrue_S)
                {
                    yield return code;
                    Log.Message("Looking for error");
                    int j = i + 1; Log.Message("j set as " + j.ToString());
                    while (j < codeInstructions.Count)
                    {
                        code = codeInstructions[j];
                        if (code.opcode == OpCodes.Call && (MethodInfo)code.operand == AccessTools.Method(typeof(Verse.Log), "Error", new Type[] { typeof(string) }))
                        {
                            errorFound = true; Log.Message("Error found is no " + errorFound.ToString());
                            i = j; Log.Message("i set as " + i.ToString());
                            yield return new CodeInstruction(OpCodes.Nop);
                            break;
                        }
                        j++;
                    }
                }
                else if (code.opcode == OpCodes.Call && (MethodInfo)code.operand == OriginalQualityGenerator)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Quality_Generator), "GenerateQualityCreatedByPawn"));
                }
                else
                {
                    yield return code;
                }
            }
            Log.Message("Recipe patch successful");
            yield break;
        }

        [HarmonyPatch(typeof(GenRecipe), "MakeRecipeProducts")]
        public static void Prefix(RecipeDef recipeDef, List<Thing> ingredients, IBillGiver billGiver)
        {
            //Reset ingredients before making products
            SupplyQuality = -1;
            //Check for quality ingredients and store value
            if (ingredients != null)
            {
                int value = 0;
                int numIng = 0;
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
                    value = value / numIng;
                    usesIngQuality = true;
                }
                SupplyQuality = Mathf.Clamp(value, 0, 6);
            }
        }
    }
}
