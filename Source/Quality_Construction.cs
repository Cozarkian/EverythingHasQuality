using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using RimWorld;
using Verse;
using Verse.AI;
using HarmonyLib;

namespace QualityEverything
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
                if (codeInstruction.opcode == OpCodes.Call && (MethodInfo)codeInstruction.operand == OriginalQualityGenerator) //Replace vanilla code for creating quality with custom code
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldloc, materialQuality);
                    yield return new CodeInstruction(OpCodes.Ldnull);
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
            if (ModSettings_QEverything.useMaterialQuality && thingDef != null && thingDef.HasComp(typeof(CompQuality)))
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

        //Prevents pawns from picking up stacks that can't stack when gathering construction materials
        public static IEnumerable<CodeInstruction> CollectNextTarget_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo mEnd = AccessTools.Method(typeof(Pawn_JobTracker), "EndCurrentJob");
            MethodInfo canStack = AccessTools.Method(typeof(ThingWithComps), "CanStackWith");
            FieldInfo fDef = AccessTools.Field(typeof(Thing), "def");
            List<CodeInstruction> list = instructions.ToList();
            //Log.Message("Starting jump patch");
            int start = -1;
            int end = -1;
            bool foundStart = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Calls(mEnd) && list[i + 1].opcode == OpCodes.Ret && list[i + 2].opcode == OpCodes.Ldloc_2)
                {
                    //Log.Message("Found the start of the def matching conditional");
                    start = i + 2;
                    foundStart = true;
                }
                if (foundStart && list[i].opcode == OpCodes.Bne_Un_S)
                {
                    //Log.Message("Found the end of def matching conditional");
                    end = i;
                    break;
                }
            }
            if (!foundStart || end == -1)
            {
                Log.Error("Can't find code range for def matching conditional in Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue");
                return list.AsEnumerable();
            }
            for (int j = start; j < end; j++)
            {
                if (list[j].LoadsField(fDef))
                {
                    list[j].opcode = OpCodes.Nop;
                }
            }
            list[end].opcode = OpCodes.Brfalse_S;
            list.Insert(end, new CodeInstruction(OpCodes.Call, canStack));
            return list.AsEnumerable();
        }
    }
}
