using MelonLoader;
using UnityEngine;
using HarmonyLib;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

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

       
        [HarmonyPatch(typeof(TimeControl), "GetAggregateTargetTimescale")]
        static class TimeControl_GetAggregateTargetTimescale_Patch
        {
            public async static void getFocus(float currentFocus)
            {

                queue.Enqueue(currentFocus + 0.1f);
           //    // if this works, i'll have to use a queue or something to store the async focus values
            //    // then pull from queue
            //    // if queue empty, use last value or something
            //    // Just need some way to handle the async stuff
            //    float focus = 0.1f;
            //    try
            //    {
            //        HttpResponseMessage response = await client.GetAsync("http://192.168.0.13:8000");
            //        response.EnsureSuccessStatusCode();
            //        string responseBody = await response.Content.ReadAsStringAsync();
            //        MelonLogger.Msg(responseBody);

            //    }
            //    catch (HttpRequestException ex)
            //    {
            //        MelonLogger.Msg("\nException Caught!");
            //        MelonLogger.Msg("Message :{0} ", ex.Message);
            //    }
            //    queue.Enqueue(focus);
            }

            //private static readonly HttpClient client = new HttpClient();

            
        
            private static Queue<float> queue = new Queue<float>();

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
                
                if (queue.Count > 0)
                {
                    __result = queue.Dequeue();
                } else
                {
                    __result = 0f;
                }
                getFocus(__result);
                MelonLogger.Msg(__result);
                return false; //don't let the original method run
            }
        }
    }
}
