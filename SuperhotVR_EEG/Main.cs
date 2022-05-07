using MelonLoader;
using UnityEngine;
using HarmonyLib;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SuperhotVR_EEG
{
    public class Main : MelonMod
    {
        private static WebSocketServer s;
        private static float focusTime = 0.2f;

        public override void OnApplicationStart()
        {
           
            s = new WebSocketServer(11111);
            s.AddWebSocketService<Focus>("/Focus");
            s.Start();

            MelonLogger.Msg("started socket");
           
        }
        public override void OnApplicationQuit()
        {
            s.Stop(); 
            base.OnApplicationQuit();
        }
        public override void OnUpdate()
        {
            
            if (Input.GetKeyDown(KeyCode.G))
            {
                MelonLogger.Msg("You just pressed G");
                MelonLogger.Msg(s.ToString());
                MelonLogger.Msg(focusTime);
            }
        }
        public class Focus : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                if (e.Data != null)
                {
                    MelonLogger.Msg(e.Data);
                    focusTime = float.Parse(e.Data);
                }
            }
        }

        [HarmonyPatch(typeof(TimeControl), "GetAggregateTargetTimescale")]
        static class TimeControl_GetAggregateTargetTimescale_Patch
        {
            static bool Prefix(ref float __result)
            {
                /**
                 *  GetAggregateTargetTimescale
                 *  This method is called onUpdate to set the current world timescale. 
                 *  The original method takes the sum of the CurrentTimeModifier values of every
                 *  in the TimeControl TimeShifterRegistry. There are TimeShifters for the HMD, 
                 *  leftHandControllor, and rightHandControllor, weapons, etc. 
                 *  
                 *  Here, we can inject are own timescale value, such as one calculated for a 
                 *  PlayerFocusLevel etc.
                **/
                MelonLogger.Msg("get aggregahate"+ focusTime); 
                   
                if (focusTime > 0.3)
                {
                    MelonLogger.Msg("Setting aggregate timescale to 0.1");
                    __result = focusTime;
                } else
                {
                    MelonLogger.Msg("Setting aggregate timescale to 0.3");
                    __result = focusTime;
                }
                return false; //don't let the original method run
            }
        }
    }

    

}
