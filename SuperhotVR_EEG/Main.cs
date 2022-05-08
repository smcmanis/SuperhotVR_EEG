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
        public static Queue<float> queue = new Queue<float>();
        public static WebSocketServer wss = new WebSocketServer(11111);

        public class Focus : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                if (e.Data != null)
                {
                    float focus = float.Parse(e.Data);
                    if (focus > 0)
                    { 
                        queue.Enqueue(focus);
                        if (queue.Count > 5)
                        {
                            queue.Dequeue();
                        }
                    }
                }
            }
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
                MelonLogger.Msg("You just pressed G");
 
            }
        }

       
        [HarmonyPatch(typeof(TimeControl), "GetAggregateTargetTimescale")]
        static class TimeControl_GetAggregateTargetTimescale_Patch
        {
            //public async static void getFocus()
            //{
                
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
            //}

            //private static readonly HttpClient client = new HttpClient();

            
            

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
                
                // IDEA = queue should have length of 10 (3 seconds?) and then i can take the average

                if (queue.Count > 0)
                {
                    // reveresed for testing it works
              
                    if (queue.Count > 5)
                    {

                        queue.Dequeue();
                    }
                    float focus = Queryable.Average(queue.AsQueryable());
                    MelonLogger.Msg("focus avg: " + focus + "(N = "+queue.Count+ ")");
                    if (focus < 0.1f) {
                        __result = 0.4f;
                    } else if (focus < 0.2) 
                    {
                        __result = 0.3f;
                    } else if (focus < 0.5)
                    {
                        __result = 0.1f;
                    } else 
                    {
                        __result = 0.05f;
                    }
                } else
                {
                    __result = 0f; 
                }
                return false; //don't let the original method run
            }
        }
    }
}
