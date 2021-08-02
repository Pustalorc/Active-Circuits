using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Dametri.ActiveCircuits.Core_Patches
{
    [HarmonyPatch(typeof(Graphic_Linked))]
    [HarmonyPatch("LinkedDrawMatFrom")]
    internal class PatchLinkedDrawMatFromPostfix
    {
        public static void Postfix(Thing parent, ref Material __result)
        {
            var comp = parent.TryGetComp<CompPower>();
            if (comp != null) __result = new PowerMaterial(__result, comp);
        }
    }
}