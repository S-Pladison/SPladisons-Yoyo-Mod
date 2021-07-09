﻿using static Mono.Cecil.Cil.OpCodes;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using SPladisonsYoyoMod.Content.Items;

namespace SPladisonsYoyoMod.Common.Hooks
{
    public partial class Hooks
    {
        public static void LoadIL()
        {
            IL.Terraria.Player.Counterweight += SoloYoyoMethod;
        }

        public static void UnloadIL()
        {
            IL.Terraria.Player.Counterweight -= SoloYoyoMethod;
        }

        private static void SoloYoyoMethod(ILContext il)
        {
            //  bool flag3 = Main.projectile[i].aiStyle == 99;
            //  if (flag3)
            //  {
            //      num2++;
            //      num = i;
            //  }

            ILCursor c = new ILCursor(il);

            int projectileIndex = -1;
            int flag3Index = -1;
            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdsfld<Main>("projectile"),
                i => i.MatchLdloc(out projectileIndex),
                i => i.MatchLdelemRef(),
                i => i.MatchLdfld<Projectile>("aiStyle"),
                i => i.MatchLdcI4(99),
                i => i.MatchCeq(),
                i => i.MatchStloc(out flag3Index))) return;

            int num2Index = -1;
            if (!c.TryGotoNext(MoveType.Before,
                i => i.MatchLdloc(out num2Index),
                i => i.MatchLdcI4(1),
                i => i.MatchAdd(),
                i => i.MatchStloc(out num2Index),
                i => i.MatchLdloc(projectileIndex),
                i => i.MatchStloc(out _))) return;

            c.Emit(Ldloc, projectileIndex);
            c.EmitDelegate<Func<int, int>>((i) => (Main.projectile[i].ModProjectile is YoyoProjectile yoyo && yoyo.IsSoloYoyo()).ToInt());
            c.Emit(Ldloc, num2Index);
            c.Emit(Add);
            c.Emit(Stloc, num2Index);
        }
    }
}
