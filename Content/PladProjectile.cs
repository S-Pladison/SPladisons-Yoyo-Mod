﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SPladisonsYoyoMod.Content
{
    public abstract class PladProjectile : ModProjectile
    {
        public Vector2 GetDrawPosition() => GetDrawPosition(Projectile.position + Projectile.Size * 0.5f + Vector2.UnitY * Projectile.gfxOffY);
        public Vector2 GetDrawPosition(Vector2 position) => position - Main.screenPosition;

        // ...

        public override string Texture => "SPladisonsYoyoMod/Assets/Textures/Projectiles/" + this.Name;

        // ...

        /// <summary>
        /// This code is called after calling <see cref="Projectile.NewProjectile(Terraria.DataStructures.IProjectileSource, float, float, float, float, int, int, float, int, float, float)"/>.
        /// </summary>
        public virtual void OnSpawn() { }

        // ...

        public static void SetSpriteBatch(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, Effect effect = null, bool end = true)
        {
            if (end) Main.spriteBatch.End();
            Main.spriteBatch.Begin(sortMode, blendState ?? BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, effect, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
