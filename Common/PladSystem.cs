﻿using Microsoft.Xna.Framework;
using SPladisonsYoyoMod.Common.Misc;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace SPladisonsYoyoMod.Common
{
    public partial class PladSystem : ModSystem
    {
        public override void ResetNearbyTileEffects()
        {
            Main.LocalPlayer.GetPladPlayer().ZoneFlamingFlower = false;
        }

        public override void PostUpdateWorld()
        {
            UpdateFlamingFlower();
        }

        public override void PostUpdateEverything()
        {
            SPladisonsYoyoMod.Primitives?.UpdateTrails();
            SoulFilledFlameEffect.Instance?.UpdateParticles();
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int index = tasks.FindIndex(genpass => genpass.Name.Equals("Dungeon"));
            if (index >= 0)
            {
                tasks.Insert(index + 1, new PassLegacy(Language.GetTextValue("Mods.SPladisonsYoyoMod.WorldGen.SpaceChest"), GenerateSpaceChest));
            }
        }

        public override TagCompound SaveWorldData()
        {
            return new TagCompound
            {
                { "flamingFlowerPosition", FlamingFlowerPosition.ToVector2() }
            };
        }

        public override void LoadWorldData(TagCompound tag)
        {
            FlamingFlowerPosition = tag.Get<Vector2>("flamingFlowerPosition").ToPoint();
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WriteVector2(FlamingFlowerPosition.ToVector2());
        }

        public override void NetReceive(BinaryReader reader)
        {
            FlamingFlowerPosition = reader.ReadVector2().ToPoint();
        }
    }
}
