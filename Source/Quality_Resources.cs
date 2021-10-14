using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using RimWorld;
using Verse;
using HarmonyLib;

namespace QualityFramework
{
    [HarmonyPatch]
    class Quality_Resources
    {   
        [HarmonyPatch(typeof(QuestManager), "Notify_PlantHarvested")]
        [HarmonyPrefix]
        public static void HarvestQuality(Pawn worker, Thing harvested)
        {
            GenerateResourceQuality(harvested, worker, SkillDefOf.Plants);
        }

        [HarmonyPatch(typeof(Mineable), "TrySpawnYield")]
        [HarmonyTranspiler]       
        static IEnumerable<CodeInstruction> MiningQuality(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codeInstructions = instructions.ToList<CodeInstruction>();
            CodeInstruction code;
            for (int i = 0; i < codeInstructions.Count; i++)
            {
                code = codeInstructions[i];
                yield return code;
                if (code.opcode == OpCodes.Call && (MethodInfo)code.operand == AccessTools.Method(typeof(ThingMaker), "MakeThing", new Type[] { typeof(ThingDef), typeof(ThingDef) }))
                {
                    CodeInstruction loadInstruction = CodeInstruction.LoadField(typeof(SkillDefOf), "Mining");
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Ldarg_S, 4);
                    yield return loadInstruction;
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Quality_Resources), "GenerateResourceQuality"));
                }
            }
            yield break;
            //Log.Message("Mining patch successful");
            //return InsertQualityCall(instructions);
        }

        [HarmonyPatch(typeof(CompHasGatherableBodyResource), "Gathered")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> AnimalQuality(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codeInstructions = instructions.ToList<CodeInstruction>();
            CodeInstruction code;
            for (int i = 0; i < codeInstructions.Count; i++)
            {
                code = codeInstructions[i];
                yield return code;
                if (code.opcode == OpCodes.Call && (MethodInfo)code.operand == AccessTools.Method(typeof(ThingMaker), "MakeThing", new Type[] { typeof(ThingDef), typeof(ThingDef) }))
                {
                    CodeInstruction loadInstruction = CodeInstruction.LoadField(typeof(SkillDefOf), "Animals");
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return loadInstruction;
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Quality_Resources), "GenerateResourceQuality"));
                }
            }
            yield break;
            //return InsertQualityCall(instructions);
        }

        [HarmonyPatch(typeof(CompEggLayer), "ProduceEgg")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> EggQuality(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codeInstructions = instructions.ToList<CodeInstruction>();
            CodeInstruction code;
            for (int i = 0; i < codeInstructions.Count; i++)
            {
                code = codeInstructions[i];
                yield return code;
                if (code.opcode == OpCodes.Call && (MethodInfo)code.operand == AccessTools.Method(typeof(ThingMaker), "MakeThing", new Type[] { typeof(ThingDef), typeof(ThingDef) }))
                {
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Quality_Resources), "CalculateResourceQuality"));
                }
            }
            yield break;
        }

        public static void GenerateResourceQuality(Thing thing, Pawn worker, SkillDef relevantSkill)
        {
            //Log.Message("Choosing quality generator");
            if ((relevantSkill == SkillDefOf.Mining && ModSettings_QFramework.skilledMining) ||
                (relevantSkill == SkillDefOf.Plants && ModSettings_QFramework.skilledHarvesting) ||
                (relevantSkill == SkillDefOf.Animals && ModSettings_QFramework.skilledAnimals))
            {
                Quality_Generator.GenerateQualityCreatedByPawn(worker, relevantSkill, thing);
            }
            else
            {
                CalculateResourceQuality(thing);
            }
        }

        public static void CalculateResourceQuality(Thing thing)
        {
            CompQuality compQuality = thing.TryGetComp<CompQuality>();
            if (compQuality == null)
            {
                return;
            }
            //Log.Message("Generating resource quality");
            QualityCategory quality = QualityUtility.GenerateQuality(QualityGenerator.BaseGen);
            compQuality.SetQuality(quality, ArtGenerationContext.Colony);
        }
    }
}
