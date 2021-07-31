﻿using Terraria;
using Terraria.ID;

namespace SPladisonsYoyoMod.Content.Items.Accessories
{
    public class CandyCane : PladItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 18;
            Item.height = 36;

            Item.rare = ItemRarityID.LightRed;
            Item.value = Terraria.Item.sellPrice(platinum: 0, gold: 1, silver: 50, copper: 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetPladPlayer().candyCaneEquipped = true;
        }
    }
}
