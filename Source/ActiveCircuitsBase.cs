using System.Collections.Generic;
using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using UnityEngine;
using Verse;

namespace Dametri.ActiveCircuits
{
    public static class ActiveCircuitsBase
    {
        public static Dictionary<PowerNet, Color> NetColors = new Dictionary<PowerNet, Color>();

        public static void DrawPowerGrid(SectionLayer layer, [NotNull] CompPower comp)
        {
            if (comp.TransmitsPowerNow)
            {
                //Graphic_LinkedTransmitterOverlay graphic = (Graphic_LinkedTransmitterOverlay)PowerOverlayMats.LinkedOverlayGraphic;
                //Color color = GetNetColor(comp);
                //SectionLayer l = layer;

                //Graphic_LinkedTransmitterOverlay colored = GetColoredVersion(graphic, graphic.Shader, color, color);
                //colored.Print(l, comp.parent);
                var graphic = NewPowerOverlayMats.LinkedOverlayGraphic;
#if V13
                graphic.Print(layer, comp.parent, 0);
#else
                graphic.Print(layer, comp.parent);
#endif
            }

            if (comp.parent.def.ConnectToPower) PowerNetGraphics.PrintOverlayConnectorBaseFor(layer, comp.parent);
            if (comp.connectParent != null)
                PowerNetGraphics.PrintWirePieceConnecting(layer, comp.parent, comp.connectParent.parent, true);
        }

        public static void UpdateAllNets([NotNull] Map map)
        {
            NetColors = new Dictionary<PowerNet, Color>();
            foreach (var net in map.powerNetManager.AllNetsListForReading) SetNetColor(net);

            var sf = AccessTools.Field(typeof(MapDrawer), "sections");
            if (!(sf.GetValue(map.mapDrawer) is Section[,] sections)) return;

            foreach (var s in sections)
            {
                var pLayer = s.GetLayer(typeof(SectionLayer_ThingsPowerGrid));
                RegenerateColors(pLayer);
                if (!ActiveCircuitsSettings.DisplayBadOnMain) continue;

                var l = (SectionLayerTopPowerGrid) s.GetLayer(typeof(SectionLayerTopPowerGrid));
                l.subMeshes.Clear();
                l.Regenerate();
                var tpLayer = s.GetLayer(typeof(SectionLayerTopPowerGrid));
                RegenerateColors(tpLayer);
            }
        }

        public static void RegenerateColors([NotNull] SectionLayer layer)
        {
            foreach (var lsm in layer.subMeshes)
            {
                var mat = lsm.material as PowerMaterial;
                if (mat == null || mat.Comp.PowerNet == null) continue;

                var color = GetNetColor(mat.Comp);
                mat.color = color;
            }
        }

        public static void PowerMetrics([NotNull] PowerNet net, out float production, out float consumption)
        {
            consumption = 0;
            production = 0;
            foreach (var comp in net.powerComps)
                if (comp.EnergyOutputPerTick > 0f)
                    production += comp.EnergyOutputPerTick;
                else
                    consumption += comp.EnergyOutputPerTick;
        }


        public static void SetNetColor([NotNull] PowerNet net)
        {
            if (net.powerComps.Count == 0 && net.batteryComps.Count == 0) // just conduits
            {
                NetColors.SetOrAdd(net, ActiveCircuitsSettings.JustConduits);
                return;
            }

            PowerMetrics(net, out var production, out var consumption);
            if (consumption == 0) // nothing using power
            {
                NetColors.SetOrAdd(net, ActiveCircuitsSettings.NothingConnected);
                return;
            }

            var rate = consumption + production;

            if (rate < 0)
            {
                var stored = net.CurrentStoredEnergy();
                NetColors.SetOrAdd(net,
                    stored < 5f ? ActiveCircuitsSettings.NoPower : ActiveCircuitsSettings.DrainingPower);
            }
            else // rate >= 0
            {
                if (rate > 1.5 * consumption || net.CurrentStoredEnergy() >= consumption) // 1 day of power
                    NetColors.SetOrAdd(net, ActiveCircuitsSettings.SurplusPower);
                else
                    NetColors.SetOrAdd(net, ActiveCircuitsSettings.HasPower);
            }
        }

        public static Color GetNetColor([NotNull] CompPower comp)
        {
            return NetColors.TryGetValue(comp.PowerNet, out var result) ? result : Color.white;
        }
    }
}