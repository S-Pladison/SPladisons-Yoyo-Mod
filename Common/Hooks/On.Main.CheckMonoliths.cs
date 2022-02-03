﻿using SPladisonsYoyoMod.Content.Items.Weapons;
using Terraria;

namespace SPladisonsYoyoMod.Common.Hooks
{
    public partial class HookLoader
    {
        private static void On_Main_CheckMonoliths(On.Terraria.Main.orig_CheckMonoliths orig)
        {
            // Main.DoDraw_UpdateCameraPosition();
            // Main.CheckMonoliths(); <- Perfect after camera updating
            // Main.sunCircle += 0.01f;

            if (!Main.gameMenu && !Main.dedServ)
            {
                var device = Main.graphics.GraphicsDevice;
                var spriteBatch = Main.spriteBatch;

                BlackholeSpaceSystem.Instance?.DrawToTarget(device, spriteBatch);
                PaperEffectSystem.Instance?.DrawToTarget(device, spriteBatch);
            }

            orig();
        }
    }
}