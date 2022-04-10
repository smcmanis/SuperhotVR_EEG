using HarmonyLib;
using UnityEngine;

namespace SuperhotVR_EEG
{
    internal class StopTimeMod : IMod
    {
        public static string Label = "StopTime";

        [HarmonyPatch(typeof(TimeControl), "GetNewInstantTimeShifter")]
        static class DisableMovementTimeShift
        {
            static void Prefix(TimeControl __instance)
            {
                foreach (var x in GameObject.FindObjectsOfType<PlayerTimeshiftSystem>())
                {
                    x.SetPlayerTimeshift(false);
                }
            }
        }
    }
}
