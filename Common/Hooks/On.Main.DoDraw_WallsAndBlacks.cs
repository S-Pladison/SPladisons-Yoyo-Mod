﻿using Microsoft.Xna.Framework.Graphics;
using SPladisonsYoyoMod.Content.Items.Weapons;
using Terraria;

namespace SPladisonsYoyoMod.Common.Hooks
{
    public partial class HookLoader
    {
        private static void On_Main_DoDraw_WallsAndBlacks(On.Terraria.Main.orig_DoDraw_WallsAndBlacks orig, Main main)
        {
            orig(main);

            var bms = BlackholeSpaceSystem.Instance;
            var sb = Main.spriteBatch;

            if (bms?.ElementsCount > 0)
            {
                sb.End();
                bms.DrawToScreen(sb);
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            }
        }
    }
}