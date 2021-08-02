using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace Dametri.ActiveCircuits.Core_Patches
{
    [HarmonyPatch(typeof(PowerNetManager))]
    [HarmonyPatch("PowerNetsTick")]
    internal class PatchPowerNetsTickPostfix
    {
        public static void Postfix([NotNull] PowerNetManager __instance)
        {
            if (Find.CurrentMap == __instance.map &&
                (OverlayDrawHandler.ShouldDrawPowerGrid || ActiveCircuitsSettings.DisplayBadOnMain) &&
                GenTicks.TicksAbs % 149 == 0) ActiveCircuitsBase.UpdateAllNets(__instance.map);
        }
    }
}