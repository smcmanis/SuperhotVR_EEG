using HarmonyLib;
using MelonLoader;

namespace SuperhotVR_EEG
{ 
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
            __result = 0.2f; 
            return false; //don't let the original method run
        }
    }
}