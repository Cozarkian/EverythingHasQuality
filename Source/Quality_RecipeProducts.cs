using System;
using RimWorld;
using Verse;
using HarmonyLib;

namespace QualityFramework
{
	[HarmonyPatch]
    public class Quality_RecipeProducts
    {
		private static Thing thing = null;
		private static bool customQuality = false;

		[HarmonyPatch(typeof(GenRecipe), "PostProcessProduct")]
		[HarmonyPrefix]
		private static void PostProcessProduct(Thing product, RecipeDef recipeDef, Pawn worker)
        {
			thing = null;
			customQuality = false;
			if ((product.def.ingestible != null && product.def.ingestible.IsMeal && ModSettings_QualityFramework.mealQuality) ||
				(product.def.IsMeat && ModSettings_QualityFramework.meatQuality) ||
				(product.def.IsStuff && ModSettings_QualityFramework.stuffQuality) ||
				(product.def.IsDrug && ModSettings_QualityFramework.drugQuality) ||
				(product.def.IsMedicine && ModSettings_QualityFramework.medQuality))
			{
				thing = product;
				customQuality = true;
            }
		}

		[HarmonyPatch(typeof(QualityUtility), "GenerateQualityCreatedByPawn")]
		[HarmonyPrefix]
		public static bool CustomQuality(ref QualityCategory __result, Pawn pawn, SkillDef relevantSkill)
        {
			if (!customQuality || thing == null || relevantSkill == SkillDefOf.Crafting || relevantSkill == SkillDefOf.Artistic)
            {
				return true;
            }
			InspirationDef inspirationDef = null;
			if (thing.def.IsMeat || thing.def.IsLeather) inspirationDef = DefOf_QualityFramework.QF_Inspired_Butchering;

			
			
			int level = pawn.skills.GetSkill(relevantSkill).Level;
			bool inspired = pawn.InspirationDef == inspirationDef;
			QualityCategory qualityCategory = QualityUtility.GenerateQualityCreatedByPawn(level, inspired);
			if (inspired)
			{
				pawn.mindState.inspirationHandler.EndInspiration(inspirationDef);
			}
			__result = qualityCategory;
			return false;
        }



        public static QualityCategory ButcheredQuality(Pawn pawn, SkillDef relevantSkill)
        {
			int level = pawn.skills.GetSkill(relevantSkill).Level;
			bool inspired = false;
			InspirationDef def;
			if ()
			= pawn.InspirationDef == InspirationDefOf.Inspired_Creativity;
			QualityCategory qualityCategory = QualityUtility.GenerateQualityCreatedByPawn(level, flag);
			if (ModsConfig.IdeologyActive && pawn.Ideo != null)
			{
				Precept_Role role = pawn.Ideo.GetRole(pawn);
				if (role != null && role.def.roleEffects != null)
				{
					RoleEffect roleEffect = role.def.roleEffects.FirstOrDefault((RoleEffect eff) => eff is RoleEffect_ProductionQualityOffset);
					if (roleEffect != null)
					{
						qualityCategory = QualityUtility.AddLevels(qualityCategory, ((RoleEffect_ProductionQualityOffset)roleEffect).offset);
					}
				}
			}
			if (flag)
			{
				pawn.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Creativity);
			}
			return qualityCategory;
		}
    }
}
