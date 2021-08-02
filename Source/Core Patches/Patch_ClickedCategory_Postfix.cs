using HarmonyLib;
using RimWorld;
using Verse;

namespace Dametri.ActiveCircuits.Core_Patches
{
    [HarmonyPatch(typeof(MainTabWindow_Architect))]
    [HarmonyPatch("ClickedCategory")]
    internal class PatchClickedCategoryPostfix
    {
        public static void Postfix()
        {
            ActiveCircuitsBase.UpdateAllNets(Find.CurrentMap);
        }
    }
}