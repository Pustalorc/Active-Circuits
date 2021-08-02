using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace Dametri.ActiveCircuits
{
    [UsedImplicitly]
    internal class ActiveCircuitsMod : Mod
    {
        [UsedImplicitly] private ActiveCircuitsSettings m_Settings;

        public ActiveCircuitsMod(ModContentPack content) : base(content)
        {
            m_Settings = GetSettings<ActiveCircuitsSettings>();

            var harmony = new Harmony("io.github.dametri.activecircuits");
            var assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            ActiveCircuitsSettings.DrawSettings(inRect);
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Active Circuits";
        }


        public override void WriteSettings() // called when settings window closes
        {
            ActiveCircuitsSettings.WriteAll();
            base.WriteSettings();
        }
    }
}