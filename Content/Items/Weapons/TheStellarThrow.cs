﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SPladisonsYoyoMod.Content.Trails;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SPladisonsYoyoMod.Content.Items.Weapons
{
    public class TheStellarThrow : YoyoItem
    {
        public TheStellarThrow() : base(gamepadExtraRange: 15) { }

        public override void YoyoSetDefaults()
        {
            Item.damage = 99;
            Item.knockBack = 2.5f;

            Item.shoot = ModContent.ProjectileType<TheStellarThrowProjectile>();

            Item.rare = ItemRarityID.LightRed;
            Item.value = Terraria.Item.sellPrice(platinum: 0, gold: 1, silver: 50, copper: 0);
        }
    }

    public class TheStellarThrowProjectile : YoyoProjectile
    {
        public static readonly Color[] TrailColors = new Color[] { new Color(255, 206, 90), new Color(255, 55, 125), new Color(137, 59, 114) };
        public static readonly Color[] DustColors = new Color[] { new Color(11, 25, 25), new Color(16, 11, 25), new Color(25, 11, 18) };

        public TheStellarThrowProjectile() : base(lifeTime: -1f, maxRange: 300f, topSpeed: 13f) { }

        public override void YoyoSetStaticDefaults()
        {
            this.SetDisplayName(eng: "The Stellar Throw", rus: "Звездный бросок");
        }

        public override void OnSpawn()
        {
            TriangularTrail trail = new
            (
                target: Projectile,
                length: 16 * 10,
                width: (progress) => 21 * (1 - progress * 0.44f),
                color: (progress) => ModHelper.GradientValue<Color>(method: Color.Lerp, percent: progress, values: TrailColors) * (1 - progress)
            );
            trail.SetEffectTexture(ModAssets.ExtraTextures[7].Value);
            trail.SetDissolveSpeed(0.15f);
            SPladisonsYoyoMod.Primitives?.NewTrail(trail);
        }

        public override void AI()
        {
            if (Projectile.velocity.Length() >= 1f && Main.rand.Next((int)Projectile.velocity.Length()) > 1)
            {
                Dust dust;

                if (Main.rand.Next(3) == 0)
                {
                    dust = Main.dust[Dust.NewDust(Projectile.Center - Vector2.One * 11, 21, 21, ModContent.DustType<Dusts.CircleDust>())];
                    dust.velocity = -Vector2.Normalize(Projectile.velocity);
                    dust.alpha = 10;
                    dust.color = DustColors[Main.rand.Next(DustColors.Length)];
                    dust.scale = Main.rand.NextFloat(0.5f, 1.7f);
                }

                dust = Main.dust[Dust.NewDust(Projectile.Center - Vector2.One * 11, 21, 21, ModContent.DustType<Dusts.StarDust>())];
                dust.velocity = -Vector2.Normalize(Projectile.velocity);
            }
        }

        public override bool PreDrawExtras()
        {
            Vector2 drawPosition = Projectile.position + new Vector2((float)Projectile.width, (float)Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            Vector2 origin = ModAssets.ExtraTextures[8].Size() * 0.5f + new Vector2(0, 6);
            float starRotation = Projectile.rotation * 0.05f;
            float starScalePulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f) * 0.15f;

            void DrawStar(Color color, float rotation, float scale)
            {
                Main.spriteBatch.Draw(ModAssets.ExtraTextures[8].Value, drawPosition, null, color, rotation, origin, scale + starScalePulse, SpriteEffects.None, 0);
            }

            SetSpriteBatch(SpriteSortMode.Deferred, BlendState.Additive);
            DrawStar(new Color(16, 11, 25, 90), -starRotation, 0.5f);
            DrawStar(new Color(16, 11, 25, 210), starRotation, 0.3f);

            SetSpriteBatch();
            Main.spriteBatch.Draw(ModAssets.ExtraTextures[5].Value, drawPosition, null, Color.White, Projectile.rotation, ModAssets.ExtraTextures[5].Size() * 0.5f, 1.3f, SpriteEffects.None, 0);

            return true;
        }
    }
}
