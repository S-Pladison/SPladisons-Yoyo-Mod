﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SPladisonsYoyoMod.Common;
using SPladisonsYoyoMod.Common.Interfaces;
using SPladisonsYoyoMod.Content.Trails;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace SPladisonsYoyoMod.Content.Items.Weapons
{
    public class BellowingThunder : YoyoItem
    {
        public BellowingThunder() : base(gamepadExtraRange: 10) { }

        public override void YoyoSetDefaults()
        {
            Item.damage = 43;
            Item.knockBack = 2.5f;

            Item.shoot = ModContent.ProjectileType<BellowingThunderProjectile>();
            Item.autoReuse = true;

            Item.rare = ItemRarityID.Orange;
            Item.value = Terraria.Item.sellPrice(platinum: 0, gold: 1, silver: 50, copper: 0);
        }

        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.AddIngredient<OutburstingGust>();
            recipe.AddIngredient<WakeOfEarth>();
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }

    public class BellowingThunderProjectile : YoyoProjectile, IDrawAdditive
    {
        public BellowingThunderProjectile() : base(lifeTime: 10f, maxRange: 220f, topSpeed: 13f) { }

        public int Counter
        {
            get => (int)Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }

        public override string GlowTexture => this.Texture + "_Glowmask";

        public override void YoyoSetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 1;
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
        }

        public override void OnSpawn()
        {
            RoundedTrail trail = new
            (
                target: Projectile,
                length: 16 * 10,
                width: (p) => 6 * (1 - p),
                color: (p) => _effectColor * (1 - p) * 0.8f,
                additive: true,
                smoothness: 20
            );
            trail.SetMaxPoints(15);
            PrimitiveTrailSystem.NewTrail(trail);
        }

        public override void AI()
        {
            Projectile.rotation -= 0.2f;
            Lighting.AddLight(Projectile.Center, _effectColor.ToVector3() * 0.2f);

            Counter++;
            if (Counter >= 20)
            {
                Counter = 0;
                _effect.Reset();
                return;
            }

            _effect.Update(Counter);
        }

        public override bool PreDrawExtras()
        {
            var texture = SPladisonsYoyoMod.GetExtraTextures[5];

            for (int k = 1; k < Projectile.oldPos.Length; k++)
            {
                var position = GetDrawPosition(Projectile.oldPos[k] + Projectile.Size * 0.5f);
                var num = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                var color = _effectColor * num * 0.3f;

                Main.EntitySpriteDraw(texture.Value, position, null, color, Projectile.oldRot[k], texture.Size() * .5f, Projectile.scale * num * 0.65f, SpriteEffects.None, 0);
            }

            return true;
        }

        public override void YoyoOnHitNPC(Player owner, NPC target, int damage, float knockback, bool crit)
        {
            if (!Main.rand.NextBool(2 + (Main.raining ? 0 : 2))) return;

            Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BellowingThunderLightningProjectile>(), Projectile.damage * 2, Projectile.knockBack * 3f, Projectile.owner);
        }

        void IDrawAdditive.DrawAdditive()
        {
            var texture = SPladisonsYoyoMod.GetExtraTextures[21];
            var drawPosition = GetDrawPosition();

            Main.EntitySpriteDraw(texture.Value, GetDrawPosition(), null, _effectColor * 0.6f, 0f, texture.Size() * .5f, Projectile.scale * 0.25f, SpriteEffects.None, 0);
            _effect.Draw(drawPosition, 0.45f * Projectile.scale);

            texture = SPladisonsYoyoMod.GetExtraTextures[23];
            Main.EntitySpriteDraw(texture.Value, drawPosition, null, _effectColor * 0.4f, 0f, texture.Size() * 0.5f, Projectile.scale * 0.1f, SpriteEffects.None, 0);

            texture = SPladisonsYoyoMod.GetExtraTextures[21];
            Main.EntitySpriteDraw(texture.Value, Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition, null, _effectColor * 0.2f, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * 0.6f, SpriteEffects.None, 0);
        }

        private readonly LightningEffect _effect = new();
        private static readonly Color _effectColor = new Color(137, 90, 248);

        // ...

        private class LightningEffect
        {
            private bool _active;
            private int _time;
            private int _frame;
            private float _rotation;
            private SpriteEffects _spriteEffects;

            public LightningEffect() => this.Reset();

            public void Draw(Vector2 position, float scale)
            {
                if (!_active) return;

                var texture = SPladisonsYoyoMod.GetExtraTextures[22];
                var rectangle = new Rectangle(_time * 96, _frame * 96, 96, 96);

                Main.EntitySpriteDraw(texture.Value, position, rectangle, Color.White, _rotation, new Vector2(48, 48), scale, _spriteEffects, 0);
            }

            public void Update(int counter)
            {
                if (counter % 3 == 0) _time++;
                if (_time >= 4) _active = false;
            }

            public void Reset()
            {
                _active = true;
                _time = 0;
                _frame = Main.rand.Next(4);
                _rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                _spriteEffects = Main.rand.NextBool(2) ? SpriteEffects.None : SpriteEffects.FlipVertically;
            }
        }
    }

    public class BellowingThunderLightningProjectile : PladProjectile
    {
        // TODO: Доделать звук удара молнии...

        public static Asset<Effect> BellowingThunderEffect { get; private set; }

        public ref float BeamProgress => ref Projectile.ai[0];
        public ref float LightningProgress => ref Projectile.ai[1];
        public ref float Timer => ref Projectile.localAI[0];

        // ...

        public override void Unload() => BellowingThunderEffect = null;

        public override void SetStaticDefaults()
        {
            if (Main.dedServ) return;

            BellowingThunderEffect = ModContent.Request<Effect>("SPladisonsYoyoMod/Assets/Effects/BellowingThunder", AssetRequestMode.ImmediateLoad);
            BellowingThunderEffect.Value.Parameters["texture1"].SetValue(SPladisonsYoyoMod.GetExtraTextures[24].Value);
        }

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 25;

            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -2;
        }

        public override void OnSpawn()
        {
            Timer += Main.rand.Next(2000);
        }

        public override void AI()
        {
            var progress = 1 - Projectile.timeLeft / 25f;

            Timer += 0.01f;
            BeamProgress = ModUtils.GradientValue<float>(MathHelper.Lerp, progress, new float[] { 0f, 0.1f, 0.2f, 0.9f, 0.45f, 0f });
            LightningProgress = ModUtils.GradientValue<float>(MathHelper.Lerp, progress, new float[] { 0f, 0f, 0f, 0.7f, 0.3f, 0f });

            Lighting.AddLight(Projectile.Center, new Color(90, 40, 255).ToVector3() * BeamProgress * 1.3f);

            if (Projectile.timeLeft != 7) return;

            ScreenShakeSystem.NewScreenShake(position: Projectile.Center, power: 7f, range: 16 * 50, time: 50);
            // SoundEngine.PlaySound(SoundLoader.GetSoundSlot(Mod, "Content/Sounds/BellowingThunderLightningSound"), Projectile.Center);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var value = (int)(TextureAssets.Projectile[Type].Height() * Projectile.scale * 9);
            hitbox.Y -= value;
            hitbox.Height += value;
        }

        public override bool? CanHitNPC(NPC target) => LightningProgress >= 0.5f;

        public override bool PreDraw(ref Color lightColor)
        {
            var drawPosition = GetDrawPosition();
            var scale = Projectile.scale * 3;
            var effect = BellowingThunderEffect.Value;
            var texture = TextureAssets.Projectile[Type];
            var origin = new Vector2(texture.Width() * 0.5f, texture.Height());
            var offset = Vector2.UnitY * texture.Height() * scale;

            SetSpriteBatch(sortMode: SpriteSortMode.Immediate, blendState: BlendState.Additive, effect: effect);
            {
                void DrawLightning(Vector2 position, Color color) => Main.EntitySpriteDraw(texture.Value, position, null, color, 0f, origin, scale, SpriteEffects.None, 0);

                for (int i = 0; i < 3; i++)
                {
                    Vector2 position = drawPosition - offset * i;

                    effect.Parameters["time"].SetValue(Timer + 300);
                    DrawLightning(position + new Vector2(7, 0), _effectColor * LightningProgress);
                    DrawLightning(position - new Vector2(7, 0), _effectColor * LightningProgress);

                    effect.Parameters["time"].SetValue(Timer + 600);
                    DrawLightning(position, new Color(179, 166, 255) * LightningProgress);

                    if (BeamProgress > 0.7f)
                    {
                        effect.Parameters["time"].SetValue(Timer);
                        DrawLightning(position, Color.White);
                    }
                }
            }
            SetSpriteBatch(sortMode: SpriteSortMode.Deferred, blendState: BlendState.Additive);
            {
                Main.EntitySpriteDraw(SPladisonsYoyoMod.GetExtraTextures[25].Value, drawPosition - offset * 3, new Rectangle(0, 0, texture.Width(), (int)offset.Y), _effectColor * BeamProgress, 0f, Vector2.UnitX * texture.Width() * 0.5f, scale, SpriteEffects.None, 0);

                texture = SPladisonsYoyoMod.GetExtraTextures[26];
                Main.EntitySpriteDraw(texture.Value, drawPosition, null, Color.White * LightningProgress * 2f, 0f, texture.Size() * 0.5f, scale * BeamProgress, SpriteEffects.None, 0);

                texture = SPladisonsYoyoMod.GetExtraTextures[23];
                Main.EntitySpriteDraw(texture.Value, drawPosition, null, _effectColor * LightningProgress * 0.5f, 0f, texture.Size() * 0.5f, scale * LightningProgress * 0.4f, SpriteEffects.None, 0);
            }
            SetSpriteBatch();

            return false;
        }

        private static readonly Color _effectColor = new Color(90, 40, 255);
    }
}
