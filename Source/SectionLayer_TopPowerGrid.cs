using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace Dametri.ActiveCircuits
{
    internal sealed class SectionLayerTopPowerGrid : SectionLayer_Things
    {
        public SectionLayerTopPowerGrid(Section section) : base(section)
        {
            requireAddToMapMesh = false;
            relevantChangeTypes = (MapMeshFlag) 32768
                ;
        }

        public override void DrawLayer()
        {
            if (ActiveCircuitsSettings.DisplayBadOnMain) base.DrawLayer();
        }

        protected override void TakePrintFrom(Thing t)
        {
            if (!ActiveCircuitsSettings.DisplayBadOnMain) return;
            if (t.Faction != null && t.Faction != Faction.OfPlayer) return;

            if (!(t is Building building)) return;

            var comp = t.TryGetComp<CompPower>();
            if (comp != null && ShouldPrint(comp)) building.PrintForPowerGrid(this);
        }

        public static bool ShouldPrint([NotNull] CompPower comp)
        {
            var color = ActiveCircuitsBase.GetNetColor(comp);
            return color == ActiveCircuitsSettings.NoPower || color == ActiveCircuitsSettings.NothingConnected ||
                   color == ActiveCircuitsSettings.JustConduits;
        }
    }
}