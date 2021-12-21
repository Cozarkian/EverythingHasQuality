using RimWorld;

namespace QualityEverything
{
    [DefOf]
    public static class DefOf_QEverything
    {
        public static InspirationDef QF_Inspired_Butchering;
        public static InspirationDef QF_Inspired_Chemistry;
        public static InspirationDef QF_Inspired_Construction;
        public static InspirationDef QF_Inspired_Cooking;
        public static InspirationDef QF_Inspired_Gathering;
        public static InspirationDef QF_Inspired_Harvesting;
        public static InspirationDef QF_Inspired_Mining;
        public static InspirationDef QF_Inspired_Stonecutting;

        static DefOf_QEverything()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DefOf_QEverything));
        }
    }
}
