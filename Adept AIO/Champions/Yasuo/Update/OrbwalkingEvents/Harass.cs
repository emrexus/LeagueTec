﻿using Adept_AIO.Champions.Yasuo.Core;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.TargetSelector;

namespace Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents
{
    class Harass
    {
        public static void OnPostAttack()
        {

        }

        public static void OnUpdate()
        {
            if (Orbwalker.Implementation.IsWindingUp)
            {
                return;
            }

            var target = TargetSelector.GetTarget(1100);
            if (target == null || Orbwalker.Implementation.IsWindingUp)
            {
                return;
            }

            if (SpellConfig.Q.Ready && target.IsValidTarget(SpellConfig.Q.Range))
            {
                if (Extension.CurrentMode == Mode.Tornado && !MenuConfig.Harass["Q"].Enabled)
                {
                    return;
                }

                SpellConfig.Q.Cast(target);
            }

            if (SpellConfig.E.Ready && MenuConfig.Harass["E"].Enabled && !target.IsUnderEnemyTurret())
            {
                var distance = target.Distance(ObjectManager.GetLocalPlayer());
                var minion = Extension.GetDashableMinion(target);

                if (!target.HasBuff("YasuoDashWrapper") && target.IsValidTarget(SpellConfig.E.Range))
                {
                    SpellConfig.E.CastOnUnit(target);
                }
                else if (distance > SpellConfig.E.Range)
                {
                    if (minion != null || distance < ObjectManager.GetLocalPlayer().AttackRange)
                    {
                        SpellConfig.E.CastOnUnit(minion);
                    }
                }
            }
        }
    }
}