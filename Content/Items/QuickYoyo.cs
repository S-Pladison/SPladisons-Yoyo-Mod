﻿using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SPladisonsYoyoMod.Content.Items
{

    /*
     
     | Pre-Hardmode
     -----------------------------------------------------------------------------------------------
     • Wooden Yoyo        | gamepadExtraRange: 4  | lifeTime: 3f  | maxRange: 130f | topSpeed: 9f
     • Rally              | gamepadExtraRange: 6  | lifeTime: 5f  | maxRange: 170f | topSpeed: 11f
     • Malaise            | gamepadExtraRange: 8  | lifeTime: 7f  | maxRange: 195f | topSpeed: 12.5f
     • Artery             | gamepadExtraRange: 8  | lifeTime: 6f  | maxRange: 207f | topSpeed: 12f
     • Amazon             | gamepadExtraRange: 9  | lifeTime: 8f  | maxRange: 215f | topSpeed: 13f
     • Code 1             | gamepadExtraRange: 10 | lifeTime: 9f  | maxRange: 220f | topSpeed: 13f
     • Valor              | gamepadExtraRange: 10 | lifeTime: 11f | maxRange: 225f | topSpeed: 14f
     • Cascade            | gamepadExtraRange: 10 | lifeTime: 13f | maxRange: 235f | topSpeed: 14f

     | Hardmode
     -----------------------------------------------------------------------------------------------
     • Chik               | gamepadExtraRange: 12 | lifeTime: 16f | maxRange: 275f | topSpeed: 17f
     • Format:C           | gamepadExtraRange: 10 | lifeTime: 8f  | maxRange: 235f | topSpeed: 15f
     • Hel-Fire           | gamepadExtraRange: 13 | lifeTime: 12f | maxRange: 275f | topSpeed: 15f
     • Amarok             | gamepadExtraRange: 11 | lifeTime: 15f | maxRange: 270f | topSpeed: 14f
     • Gradient           | gamepadExtraRange: 11 | lifeTime: 10f | maxRange: 250f | topSpeed: 12f
     • Code 2             | gamepadExtraRange: 13 | lifeTime: -   | maxRange: 280f | topSpeed: 17f
     • Yelets             | gamepadExtraRange: 13 | lifeTime: 14f | maxRange: 290f | topSpeed: 16f
     • Red's Throw        | gamepadExtraRange: 18 | lifeTime: -   | maxRange: 370f | topSpeed: 16f
     • Valkyrie Yoyo      | gamepadExtraRange: 18 | lifeTime: -   | maxRange: 370f | topSpeed: 16f
     • Kraken             | gamepadExtraRange: 17 | lifeTime: -   | maxRange: 340f | topSpeed: 16f
     • The Eye of Cthulhu | gamepadExtraRange: 18 | lifeTime: -   | maxRange: 360f | topSpeed: 16.5f
     • Terrarian          | gamepadExtraRange: 21 | lifeTime: -   | maxRange: 400f | topSpeed: 17.5f

    */

    public abstract class YoyoItem : PladItem
    {
        private readonly int _gamepadExtraRange;

        public YoyoItem(int gamepadExtraRange)
        {
            _gamepadExtraRange = gamepadExtraRange;
        }

        public sealed override void SetStaticDefaults(ref uint sacrificeCount)
        {
            ItemID.Sets.Yoyo[Type] = true;
            ItemID.Sets.GamepadExtraRange[Type] = _gamepadExtraRange;
            ItemID.Sets.GamepadSmartQuickReach[Type] = true;

            this.YoyoSetStaticDefaults();
        }

        public sealed override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
            Item.height = 26;
            Item.shootSpeed = 16f;

            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 25;
            Item.useTime = 25;

            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            this.YoyoSetDefaults();
        }

        public virtual void YoyoSetStaticDefaults() { }
        public virtual void YoyoSetDefaults() { }
    }

    public abstract class YoyoProjectile : PladProjectile
    {
        public bool YoyoGloveActivated { get; private set; }
        public bool IsReturning { get => Projectile.ai[0] == -1; }

        private readonly float _lifeTime;
        private readonly float _maxRange;
        private readonly float _topSpeed;

        public YoyoProjectile(float lifeTime, float maxRange, float topSpeed)
        {
            _lifeTime = lifeTime;
            _maxRange = maxRange;
            _topSpeed = topSpeed;
        }

        public override string Texture => ModContent.RequestIfExists<Texture2D>(base.Texture, out _) ? base.Texture : "SPladisonsYoyoMod/Assets/Textures/Projectiles/UnknownYoyoProjectile";

        // ...

        public sealed override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Type] = _lifeTime;
            ProjectileID.Sets.YoyosMaximumRange[Type] = _maxRange;
            ProjectileID.Sets.YoyosTopSpeed[Type] = _topSpeed;

            this.YoyoSetStaticDefaults();
        }

        public sealed override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;

            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = Common.Globals.YoyoGlobalProjectile.YoyoAIStyle;
            Projectile.friendly = true;
            Projectile.penetrate = -1;

            this.YoyoSetDefaults();
        }

        public sealed override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            var owner = Main.player[Projectile.owner];
            if (owner.yoyoGlove && !this.YoyoGloveActivated)
            {
                this.YoyoGloveActivated = true;
                this.OnActivateYoyoGlove();
            }

            this.YoyoOnHitNPC(owner, target, damage, knockback, crit);
        }

        public sealed override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(this.YoyoGloveActivated);
            this.YoyoSendExtraAI(writer);
        }

        public sealed override void ReceiveExtraAI(BinaryReader reader)
        {
            this.YoyoGloveActivated = reader.ReadBoolean();
            this.YoyoReceiveExtraAI(reader);
        }

        // ...

        public virtual void YoyoSetStaticDefaults() { }
        public virtual void YoyoSetDefaults() { }
        public virtual void YoyoOnHitNPC(Player owner, NPC target, int damage, float knockback, bool crit) { }
        public virtual void YoyoSendExtraAI(BinaryWriter writer) { }
        public virtual void YoyoReceiveExtraAI(BinaryReader reader) { }

        public virtual bool IsSoloYoyo() => false;
        public virtual void OnActivateYoyoGlove() { }
        public virtual void ModifyYoyoLifeTime(ref float lifeTime) { }
        public virtual void ModifyYoyoMaximumRange(ref float maxRange) { }
    }
}
