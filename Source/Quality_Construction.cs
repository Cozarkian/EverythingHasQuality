using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using RimWorld;
using Verse;
using HarmonyLib;

namespace QualityFramework
{
    [HarmonyPatch]
    public static class Quality_Construction
    {
        [HarmonyPatch(typeof(Frame), "CompleteConstruction")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> ConstructionQuality(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            MethodInfo OriginalQualityGenerator = AccessTools.Method(typeof(QualityUtility), "GenerateQualityCreatedByPawn", new Type[]{ typeof(Pawn), typeof(SkillDef) }, null);
            LocalBuilder materialQuality = generator.DeclareLocal(typeof(int)); //Log.Message("Declared variable");
            yield return new CodeInstruction(OpCodes.Ldarg_0);
            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Quality_Construction), "CalculateMaterialQuality"));
            yield return new CodeInstruction(OpCodes.Stloc, materialQuality);
            foreach (CodeInstruction codeInstruction in instructions)
            {
                if (codeInstruction.opcode == OpCodes.Call && (MethodInfo)codeInstruction.operand == OriginalQualityGenerator)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldloc, materialQuality);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Quality_Generator), "GenerateQualityCreatedByPawn"));
                }
                else
                {
                    yield return codeInstruction;
                }
            }
            //Log.Message("Construction patch successful");
            yield break;
        }

        public static int CalculateMaterialQuality(Frame frame)
        {
            float materialQuality = -1;
            ThingDef thingDef = frame.def.entityDefToBuild as ThingDef;
            if (ModSettings_QFramework.useMaterialQuality && thingDef != null && thingDef.HasComp(typeof(CompQuality)))
            {
                int numIng = 0;
                for (int i = 0; i < frame.resourceContainer.Count; i++)
                {
                    Thing resource = frame.resourceContainer[i];
                    CompQuality comp = resource.TryGetComp<CompQuality>();
                    if (comp != null)
                    {
                        //Log.Message("resource " + i + " is " + (int)comp.Quality);
                        //Log.Message("resource " + i + " has " + resource.stackCount);
                        materialQuality += (int)comp.Quality * resource.stackCount;
                        numIng += resource.stackCount;
                    }
                }
                if (numIng > 0)
                {
                    materialQuality = (materialQuality + 1) / numIng;
                    //Log.Message("Value is " + materialQuality.ToString() + " for " + numIng + " resources");
                }
            }
            return GenMath.RoundRandom(materialQuality);
        }
    }
}
