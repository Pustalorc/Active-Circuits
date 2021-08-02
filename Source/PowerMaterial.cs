using RimWorld;
using UnityEngine;

namespace Dametri.ActiveCircuits
{
    internal sealed class PowerMaterial : Material
    {
        public PowerMaterial(Material source, CompPower c) : base(source)
        {
            Comp = c;
        }

        public readonly CompPower Comp;
    }
}