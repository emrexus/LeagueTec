﻿using System;
using System.Linq;
using System.Threading;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.Champions.Azir.OrbwalkingEvents;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Azir.Miscellaneous
{
    internal class Manager
    {
        private static int _lastAa;

        public static void OnUpdate()
        {
            try
            {
                if (Global.Player.IsDead || Global.Orbwalker.IsWindingUp || Global.Player.IsRecalling())
                {
                    return;
                }

                foreach (var soldier in SoldierManager.Soldiers)
                {
                    if (soldier.Distance(Global.Player) > 660)
                    {
                        continue;
                    }

                    var enemy = GameObjects.Enemy.FirstOrDefault(x => x.Distance(soldier) <= 250 + x.BoundingRadius && !x.IsDead && x.MaxHealth > 10 && soldier.Distance(Global.Player) <= SpellConfig.Q.Range + 65 && soldier.Distance(Global.Player) > Global.Player.AttackRange);
                    if (enemy == null || Game.TickCount - _lastAa <= 1000)
                    {
                        continue;
                    }

                    _lastAa = Game.TickCount;
                    Global.Player.IssueOrder(OrderType.AttackUnit, enemy);
                    DelayAction.Queue(250, ()=> Global.Player.IssueOrder(OrderType.MoveTo, Game.CursorPos), new CancellationToken(false));
                }
            
                SpellConfig.R.Width = 133 * (3 + Global.Player.GetSpell(SpellSlot.R).Level);

                Insec.OnKeyPressed();

                switch (Global.Orbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                        Combo.OnUpdate();
                        break;
                    case OrbwalkingMode.Mixed:
                        Harass.OnUpdate();
                        break;
                    case OrbwalkingMode.Laneclear:
                        JungleClear.OnUpdate();
                        LaneClear.OnUpdate();
                        break;
                    case OrbwalkingMode.None:
                        AzirHelper.Rect = null;
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
