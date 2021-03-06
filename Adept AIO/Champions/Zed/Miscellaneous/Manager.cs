﻿using System.Linq;
using Adept_AIO.Champions.Zed.Core;
using Adept_AIO.Champions.Zed.OrbwalkingEvents;
using Adept_AIO.SDK.Generic;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Zed.Miscellaneous
{
    internal class Manager
    {
        public static void OnUpdate()
        {
            if (Global.Player.IsDead || Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo.OnUpdate();
                    break;
                case OrbwalkingMode.Mixed:
                    Harass.OnUpdate();
                    break;
                case OrbwalkingMode.Laneclear:
                    LaneClear.OnUpdate();
                    JungleClear.OnUpdate();
                    break;
                case OrbwalkingMode.Lasthit:
                    Lasthit.OnUpdate();
                    break;
            }

            if (Global.Orbwalker.Mode != OrbwalkingMode.Mixed && !Global.Player.IsRecalling())
            {
                PermaSpells();
            }
        }

        private static void PermaSpells()
        {
            foreach (var enemy in GameObjects.EnemyHeroes.Where(x => x.IsValidTarget() && x.IsHero))
            {
                if (MenuConfig.Misc["Q"].Enabled && SpellManager.Q.Ready)
                {
                    SpellManager.CastQ(enemy);
                }

                if (MenuConfig.Misc["E"].Enabled && SpellManager.E.Ready)
                {
                    SpellManager.CastE(enemy);
                }
            }
        }
    }
}
