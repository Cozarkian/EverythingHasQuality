using RimWorld;
using Verse;

namespace QualityFramework
{
    [DefOf]
    public static class DefOf_QualityFramework
    {
        public static InspirationDef QF_Inspired_Harvesting;
        //public static InspirationDef QF_Inspired_Butchering;
        //public static InspirationDef QF_Inspired_Mining;

        static DefOf_QualityFramework()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DefOf_QualityFramework));
        }
    }
}
