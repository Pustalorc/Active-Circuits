using JetBrains.Annotations;
using Verse;

namespace Dametri.ActiveCircuits
{
    [UsedImplicitly]
    public class CircuitMapComponent : MapComponent
    {
        private readonly Map m_ThisMap;

        public CircuitMapComponent(Map map) : base(map)
        {
            m_ThisMap = map;
        }

        public override void MapGenerated()
        {
            base.MapGenerated();
            ActiveCircuitsBase.UpdateAllNets(m_ThisMap);
        }
    }
}