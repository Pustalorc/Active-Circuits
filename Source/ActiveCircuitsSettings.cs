using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace Dametri.ActiveCircuits
{
    internal sealed class ActiveCircuitsSettings : ModSettings
    {
        private const int RectWidth = 7;

        public static Color Salmon = new Color(250f / 255f, 127f / 255f, 114f / 255f);

        public static Color JustConduits = Color.white;
        public static Color NothingConnected = Color.magenta;
        public static Color NoPower = Color.red;
        public static Color DrainingPower = Salmon;
        public static Color HasPower = Color.yellow;
        public static Color SurplusPower = Color.green;

        public static bool DisplayBadOnMain;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref JustConduits, "justConduits", Color.white);
            Scribe_Values.Look(ref NothingConnected, "nothingConnected", Color.magenta);
            Scribe_Values.Look(ref NoPower, "noPower", Color.red);
            Scribe_Values.Look(ref DrainingPower, "drainingPower", Salmon);
            Scribe_Values.Look(ref HasPower, "hasPower", Color.yellow);
            Scribe_Values.Look(ref SurplusPower, "surplusPower", Color.green);
            Scribe_Values.Look(ref DisplayBadOnMain, "displayBadOnMain");
            base.ExposeData();
        }

        public static void WriteAll() // called when settings window closes
        {
            if (Find.CurrentMap != null)
                ActiveCircuitsBase.UpdateAllNets(Find.CurrentMap);
        }

        public static void DrawSettings(Rect rect)
        {
            var ls = new Listing_Standard(GameFont.Small) {ColumnWidth = rect.width - 30f};
            ls.Begin(rect);

            ls.Gap();
            ls.CheckboxLabeled("Always display bad state circuits (may cause additional lag)", ref DisplayBadOnMain);

            ls.Gap();
            DrawColorChoice(ls, ref JustConduits, "Unconnected conduit (default 255, 255, 255)");
            DrawColorChoice(ls, ref NothingConnected, "Nothing drawing power (default 255, 0, 255)");
            DrawColorChoice(ls, ref NoPower, "No power (default 255, 0, 0)");
            DrawColorChoice(ls, ref DrainingPower, "Batteries draining (default 250, 127, 114)");
            DrawColorChoice(ls, ref HasPower, "Sufficient power (default 255, 235, 4)");
            DrawColorChoice(ls, ref SurplusPower, "Surplus power (default 0, 255, 0)");

            ls.Gap();
            ls.End();
        }

        public static void DrawColorChoice([NotNull] Listing_Standard ls, ref Color color, string description)
        {
            var colorRect = ls.GetRect(Text.LineHeight);
            var colorLabelRect = colorRect.LeftPartPixels(colorRect.width - RectWidth);
            var colorBoxRect = colorRect.RightPartPixels(RectWidth);
            colorBoxRect.height += 10f;
            colorBoxRect.y -= 3f;
            Widgets.Label(colorLabelRect, description);
            Widgets.DrawBoxSolid(colorBoxRect, color);
            ls.Gap();
            var colorSliders = ls.GetRect(Text.LineHeight);
            var sliderWidth = colorSliders.width / 3f;
            var colorSliderR = colorSliders.LeftPartPixels(sliderWidth - 5f);
            var colorSliderG = new Rect(colorSliders.xMin + sliderWidth + 5f, colorSliders.y, sliderWidth - 10f,
                colorSliders.height);
            var colorSliderB = colorSliders.RightPartPixels(sliderWidth - 5f);
            color.r = Widgets.HorizontalSlider(colorSliderR, color.r, 0, 1, false,
                "r: " + (color.r * 255).ToString("f0"), null, null, 1f / 255f);
            color.g = Widgets.HorizontalSlider(colorSliderG, color.g, 0, 1, false,
                "g: " + (color.g * 255).ToString("f0"), null, null, 1f / 255f);
            color.b = Widgets.HorizontalSlider(colorSliderB, color.b, 0, 1, false,
                "b: " + (color.b * 255).ToString("f0"), null, null, 1f / 255f);
            ls.Gap();
        }
    }
}