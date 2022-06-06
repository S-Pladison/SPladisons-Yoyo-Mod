﻿using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SPladisonsYoyoMod.Common
{
    public class PladConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        // ...

        [Header("$Mods.SPladisonsYoyoMod.Config.Misc")]

        [DefaultValue(true)]
        [Label("$Mods.SPladisonsYoyoMod.Config.YoyoCustomUseStyle")]
        public bool YoyoCustomUseStyle { get; set; }

        [DefaultValue(true)]
        [Label("$Mods.SPladisonsYoyoMod.Config.ShowYoyoGloveStandardDescription")]
        [Tooltip("$Mods.SPladisonsYoyoMod.Config.ShowYoyoGloveStandardDescription_Tooltip")]
        public bool ShowYoyoGloveStandardDescription { get; set; }
    }
}
