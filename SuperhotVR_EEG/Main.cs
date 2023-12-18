using MelonLoader;
using UnityEngine;
using HarmonyLib;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Linq;

namespace SuperhotVR_EEG
{
    public class Main : MelonMod
    {
        public static Queue<float> focusHistory = new Queue<float>();
        public static WebSocketServer wss = new WebSocketServer(11111);
        public static float DEFAULT_TIMESCALE = 0.3f;
        public static float currentFocus = 0f;
        public static float currentTimescale = DEFAULT_TIMESCALE;
        public static bool useFocusTimescale = true;
        public static string difficulty = "NORMAL";

        public class Focus : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                if (e.Data != null)
                {
                    float focus = float.Parse(e.Data);
                    if (focus > 0)
                    { 
                        focusHistory.Enqueue(focus);
                        if (focusHistory.Count > 5)
                        {
                            focusHistory.Dequeue();
                        }
                        currentFocus = averageFocus();
                    }
                }
            }
        }

        public static float averageFocus()
        {
            float focus = Queryable.Average(focusHistory.AsQueryable());
            MelonLogger.Msg("focus avg: " + focus);
            return focus;
        }

        public static float timescaleFromFocus(float focus)
        {
            if (difficulty == "NORMAL")
            {
                return normalTimescaleFromFocus(focus);
            } 
            else if (difficulty == "EASY")
            {
                return easyTimescaleFromFocus(focus);
            } else
            {
                return -1f;
            }
        }
        public static float easyTimescaleFromFocus(float focus)
        {
            float timescale = 0f;
            if (focus < 0.1f)
            {
                timescale = 1f;
            } 
            else if (focus < 0.3f)
            {
                timescale = 0.3f;
            } else
            {
                timescale = 0.05f;
            }
            return timescale;
        }
            public static float normalTimescaleFromFocus(float focus)
        {
            float timescale = 0f;
            if (focus < 0.1f)
            {
                timescale = 1f;
            } 
            else if (focus < 0.2f)
            {
                timescale = 0.5f;
            }
            else if (focus < 0.25f)
            {
                timescale = 0.4f;
            }
            else if (focus < 0.3f)
            {
                timescale = 0.3f;
            }
            else if (focus < 0.5f)
            {
                timescale = 0.2f;
            }
            else if (focus < 0.6f)
            {
                timescale = 0.1f;
            }
            else if (focus < 0.7f)
            {
                timescale = 0.05f;
            }
            else
            {
                timescale = 0f;
            }

            return timescale;
        }

        public override void OnApplicationStart()
        {
            wss.AddWebSocketService<Focus>("/Focus");
            wss.Start();
        }

        public override void OnApplicationQuit()
        {
            wss.Stop();
        }

        public override void OnUpdate()
        {
            
            if (Input.GetKeyDown(KeyCode.G))
            {
                useFocusTimescale = !useFocusTimescale;
                MelonLogger.Msg("Focus Timescale is: " + useFocusTimescale);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                difficulty = "EASY";
                MelonLogger.Msg("Difficulty is: " + difficulty );
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                difficulty = "NORMAL";
                MelonLogger.Msg("Difficulty is: " + difficulty);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                difficulty = "HARD";
                MelonLogger.Msg("Difficulty is: " + difficulty);
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

                if (useFocusTimescale)
                {
                    __result = timescaleFromFocus(currentFocus);
                    return false;
                } else
                {
                    return true;
                }
            }
        }
    }
}
