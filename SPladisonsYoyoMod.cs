using SPladisonsYoyoMod.Common;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SPladisonsYoyoMod
{
	public class SPladisonsYoyoMod : Mod
	{
        public static Mod Instance { get; private set; }

        public static PrimitiveSystem Primitives { get; private set; }

        public SPladisonsYoyoMod()
        {
            Instance = this;
        }

        public override void Load()
        {
            Primitives = new PrimitiveSystem(Main.graphics.GraphicsDevice);
        }

        public override void Unload()
        {
            Primitives = null;

            Instance = null;
        }
    }
}