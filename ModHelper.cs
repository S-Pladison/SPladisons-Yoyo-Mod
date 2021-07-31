﻿using SPladisonsYoyoMod.Common;
using SPladisonsYoyoMod.Common.Globals;
using System;
using System.Linq;
using Terraria;

namespace SPladisonsYoyoMod
{
    public static class ModHelper
    {
        public static bool active(this Tile tile) // Временно
        {
            return (tile.sTileHeader & 32) == 32;
        }

        public static PladPlayer GetPladPlayer(this Player player) => player.GetModPlayer<PladPlayer>();

        public static YoyoGlobalProjectile GetYoyoGlobalProjectile(this Projectile projectile) => projectile.GetGlobalProjectile<YoyoGlobalProjectile>();
        public static YoyoGlobalItem GetYoyoGlobalItem(this Item item) => item.GetGlobalItem<YoyoGlobalItem>();

        public static bool IsYoyo(this Projectile projectile) => projectile.aiStyle == YoyoGlobalProjectile.YoyoAIStyle;
        public static bool IsYoyo(this Item item) => SPladisonsYoyoMod.GetYoyos.Contains(item.type);
    }
}
