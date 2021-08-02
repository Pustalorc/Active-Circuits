using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace Dametri.ActiveCircuits.Core_Patches
{
    [HarmonyPatch(typeof(CompPower))]
    [HarmonyPatch("CompPrintForPowerGrid")]
    public static class PatchCompPrintForPowerGridPrefix
    {
        public static bool Prefix(SectionLayer layer, [NotNull] CompPower __instance)
        {
            ActiveCircuitsBase.DrawPowerGrid(layer, __instance);
            return false;
        }
    }
}