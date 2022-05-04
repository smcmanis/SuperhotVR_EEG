using MelonLoader;
using UnityEngine;
using HarmonyLib;

namespace SuperhotVR_EEG
{
    public class Main : MelonMod
    {
        private NotionGodClass NotionGodClass = new NotionGodClass();

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                MelonLogger.Msg("You just pressed G");
                NotionGodClass.OnEnable();
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
