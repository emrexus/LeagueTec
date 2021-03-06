﻿using System.Linq;
using System.Threading;
using Adept_AIO.Champions.Ezreal.Core;
using Adept_AIO.SDK.Generic;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Ezreal.Miscellaneous
{
    internal class Misc
    {
        public static void OnUpdate()
        {
            if (NavMesh.WorldToCell(Global.Player.ServerPosition).Flags.HasFlag(NavCellFlags.Grass))
            {
                return;
            }

            if (SpellConfig.Q.Ready
             && Global.Orbwalker.Mode == OrbwalkingMode.None
             && Global.Player.IsMoving
             && Global.Player.CountEnemyHeroesInRange(2500) == 0
             && MenuConfig.Miscellaneous["Stack"].Enabled 
             && Global.Player.ManaPercent() >= MenuConfig.Miscellaneous["Stack"].Value  
             && TargetState.HasTear())
            {
                var objects = GameObjects.Enemy.FirstOrDefault(x => x.IsValidTarget(SpellConfig.Q.Range) && x.MaxHealth >= 10);

                if (MenuConfig.Miscellaneous["TH"].Enabled)
                {
                    DelayAction.Queue(GetRandom.Next(400, 1200), ()=>
                    {
                        SpellConfig.Q.Cast(objects != null ? objects.ServerPosition : Game.CursorPos);
                    }, new CancellationToken(false));
                }
                else
                {
                    SpellConfig.Q.Cast(objects != null ? objects.ServerPosition : Game.CursorPos);
                }
            }

            if (SpellConfig.W.Ready
             && Global.Player.CountEnemyHeroesInRange(2500) == 0
             && MenuConfig.Miscellaneous["WT"].Enabled
             && Global.Player.ServerPosition.PointUnderEnemyTurret()
             && Global.Player.ManaPercent() >= 60)
            {
                var ally = GameObjects.AllyHeroes.FirstOrDefault(x => x.IsValidTarget(SpellConfig.W.Range - 100));
                if (ally != null)
                {
                    SpellConfig.W.Cast(ally);
                }
            }
        }
    }
}
