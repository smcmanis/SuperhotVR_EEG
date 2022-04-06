using MelonLoader;
using UnityEngine;

namespace SuperhotVR_EEG
{
    public class MyClass : MelonMod
    {
        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                MelonLogger.Msg("You just pressed T");
            }
        }
    }
}
