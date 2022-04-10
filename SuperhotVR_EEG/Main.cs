using MelonLoader;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;

namespace SuperhotVR_EEG
{
    public class SuperhotMods : MelonMod
    {
        public static bool isEnabled { get; private set; }
        public static List<ModRunner> ModRunners = new List<ModRunner>();

        public override void OnApplicationStart()
        {
            var runner = new ModRunner(StopTimeMod);
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F3))
            {
                DebugSettings.Instance.devMenu = true;
                VrDevMenu.Init();
            }
        }

        [HarmonyPatch(typeof(VrDevMenu), "Awake")]
        public static class VrDevMenuPatch
        {

            private static void Prefix(VrDevMenu __instance)
            {
                DebugSettings.Instance.devMenu = true;
            }

            private static void Postfix(VrDevMenu __instance)
            {
                foreach (var runner in ModRunners)
                {
                    VrDevMenu.AddAction(runner.Label + " : " + runner.isEnabled, delegate
                    {
                        Toggle(runner, !runner.isEnabled);
                        VrDevMenu.RelabelAction(runner.Label + " : " + runner.isEnabled);
                    }, oneLevelOnly: false, runner.Label);
                }
            }
            private static void Toggle(ModRunner runner, bool value)
            {
                runner.isEnabled = value;
                if (value)
                {
                    runner.Start();
                } else
                {
                    runner.Stop();
                }
            }
        }
    }
}
