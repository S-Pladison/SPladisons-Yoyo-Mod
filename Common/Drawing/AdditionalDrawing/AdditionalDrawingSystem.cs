﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SPladisonsYoyoMod.Common.Drawing.AdditionalDrawing
{
    [Autoload(Side = ModSide.Client)]
    public class AdditionalDrawingSystem : ModSystem
    {
        private static readonly Dictionary<DrawKey, List<DrawData>> dataCache = new();

        // ...

        public override void Load()
        {
            SPladisonsYoyoMod.Events.OnPostDraw += ClearDataCache;
        }

        public override void PostSetupContent()
        {
            foreach (var key in DrawingManager.GetAllPossibleKeys())
            {
                dataCache.Add(key, new List<DrawData>());
            }

            DrawingManager.AddToEachExistingVariant(Any, Draw);
        }

        public override void Unload()
        {
            ClearDataCache();
            dataCache.Clear();
        }

        public override void OnWorldUnload()
        {
            ClearDataCache();
        }

        // ...

        public static bool Any(DrawKey key) => dataCache[key].Any();

        public static void AddToDataCache(DrawLayers layer, DrawTypeFlags flags, DrawData data)
        {
            dataCache[new DrawKey(layer, flags)].Add(data);
        }

        public static void ClearDataCache(GameTime _ = null)
        {
            foreach (var list in dataCache.Values)
            {
                list.Clear();
            }
        }

        public static void Draw(SpriteBatch spriteBatch, DrawKey key)
        {
            foreach (var data in dataCache[key])
            {
                data.Draw(spriteBatch);
            }
        }
    }
}
