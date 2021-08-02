using HarmonyLib;
using RimWorld;
using Verse;

namespace Dametri.ActiveCircuits.Core_Patches
{
    [HarmonyPatch(typeof(MainTabWindow_Architect))]
#if V13
    [HarmonyPatch("PreOpen")]
#else
    [HarmonyPatch("PostOpen")]
#endif
    internal class PatchArchitectPostOpenPostfix
    {
        public static void Postfix()
        {
            ActiveCircuitsBase.UpdateAllNets(Find.CurrentMap);
        }
    }
}