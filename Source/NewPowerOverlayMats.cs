using RimWorld;
using UnityEngine;
using Verse;

namespace Dametri.ActiveCircuits
{
    [StaticConstructorOnStartup]
    internal static class NewPowerOverlayMats
    {
        static NewPowerOverlayMats()
        {
            var graphic = GraphicDatabase.Get<Graphic_Single>(TransmitterAtlasPath, TransmitterShader);
            LinkedOverlayGraphic =
                (Graphic_LinkedTransmitterOverlay) GraphicUtility.WrapLinked(graphic,
                    LinkDrawerType.TransmitterOverlay);
            graphic.MatSingle.renderQueue = 3600;
        }

        private const string TransmitterAtlasPath = "Things/Special/Power/WhiteTransmitterAtlas";
        public static readonly Shader TransmitterShader = ShaderDatabase.MetaOverlay;
        public static readonly Graphic_LinkedTransmitterOverlay LinkedOverlayGraphic;
    }
}