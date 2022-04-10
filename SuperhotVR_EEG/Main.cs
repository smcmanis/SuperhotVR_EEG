using MelonLoader;
using UnityEngine;
using HarmonyLib;

namespace SuperhotVR_EEG
{
    public class Main : MelonMod
    {
        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                MelonLogger.Msg("You just pressed G");
            }
        }
    }
}
