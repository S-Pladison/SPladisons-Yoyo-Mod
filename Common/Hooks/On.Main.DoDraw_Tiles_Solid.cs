﻿using SPladisonsYoyoMod.Common.Drawing;
using Terraria;

namespace SPladisonsYoyoMod.Common.Hooks
{
    public partial class HookLoader
    {
        private static void On_Main_DoDraw_Tiles_Solid(On.Terraria.Main.orig_DoDraw_Tiles_Solid orig, Main main)
        {
            orig(main);

            DrawingManager.DrawLayer(DrawLayers.OverTiles);
        }
    }
}