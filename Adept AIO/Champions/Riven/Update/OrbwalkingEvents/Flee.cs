﻿using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;
using GameObjects = Adept_AIO.SDK.Extensions.GameObjects;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents
{
    internal class Flee
    {
        public static void OnKeyPressed()
        {
            if (MenuConfig.Miscellaneous["Walljump"].Enabled &&
                ObjectManager.GetLocalPlayer().CountEnemyHeroesInRange(1800) == 0)
            {
                if (Extensions.CurrentQCount != 3)
                {
                    return;
                }

                const int dashRange = 350;
                var end = ObjectManager.GetLocalPlayer().Position.Extend(Game.CursorPos, dashRange);
                var wall = WallExtension.GeneratePoint(ObjectManager.GetLocalPlayer().Position, end).OrderBy(x => x.Distance(ObjectManager.GetLocalPlayer().Position)).FirstOrDefault();

                if (wall == Vector3.Zero)
                {
                    return;
                }

                var distance = wall.Distance(ObjectManager.GetLocalPlayer().Position);

                if (distance <= 5)
                {
                    return;
                }

                if (SpellConfig.E.Ready && distance <= SpellConfig.E.Range + 200 && distance > 100)
                {
                    SpellConfig.E.Cast(wall);
                    DelayAction.Queue(190, () => ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.Q, wall));
                }

                if (distance > 125)
                {
                    ObjectManager.GetLocalPlayer().IssueOrder(OrderType.MoveTo, wall);
                    return;
                }

                ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.Q, wall);
            }
            else
            {
                if (SpellConfig.W.Ready)
                {
                    foreach (var enemy in GameObjects.EnemyHeroes.Where(SpellManager.InsideKiBurst))
                    {
                        SpellManager.CastW(enemy);
                    }
                }

                if (SpellConfig.Q.Ready)
                {
                    SpellConfig.Q.Cast(Game.CursorPos);
                }
                else if (SpellConfig.E.Ready)
                {
                    if (SpellConfig.Q.Ready && Extensions.CurrentQCount != 3)
                    {
                        return;
                    }
                    SpellConfig.E.Cast(Game.CursorPos);
                }
            }
        }
    }
}