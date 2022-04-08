using MelonLoader;
using UnityEngine;
using HarmonyLib;

namespace SuperhotVR_EEG
{
    public class MyClass : MelonMod
    {
        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                MelonLogger.Msg("You just pressed G");
            }
        }
    }
    [HarmonyPatch(typeof(TimeControl), "GetNewInstantTimeShifter")]
    static class Patch10
    {
        static void Prefix(TimeControl __instance)
        {
            MelonLogger.Msg("TimeControl.GetNewInstantTimeShifter");
            foreach (var x in GameObject.FindObjectsOfType<PlayerTimeshiftSystem>())
            {
                x.SetPlayerTimeshift(false);
            }
            

        }
    }

}
