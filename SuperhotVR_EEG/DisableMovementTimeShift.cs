using HarmonyLib;
using UnityEngine;

namespace SuperhotVR_EEG
{
    [HarmonyPatch(typeof(TimeControl), "GetNewInstantTimeShifter")]
    static class DisableMovementTimeShift
    {
        static void Prefix(TimeControl __instance)
        {
            if (false)
            {
                foreach (var x in GameObject.FindObjectsOfType<PlayerTimeshiftSystem>())
                {
                    x.SetPlayerTimeshift(false);
                }
            }
        }
    }
}
