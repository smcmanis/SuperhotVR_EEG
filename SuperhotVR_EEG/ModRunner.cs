using System;
using System.Collections.Generic;
using MelonLoader;

namespace SuperhotVR_EEG
{
    internal class ModRunner
    {
        internal MelonMod Mod;

        internal String Label;

        public ModRunner(MelonMod mod)
        {
            Mod = mod;
            Label = mod.Label
        }

    }
}
