﻿using System.Threading;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Util;

namespace Adept_AIO.SDK.Generic
{
    internal class Maths
    {
        public static int Percent(double value1, double value2, int multiplier = 100)
        {
            return (int)(value2 / value1 * multiplier);
        }

        public static void DisableAutoAttack(int duration = 500)
        {
            duration += Game.Ping / 2 + 50;
            Global.Orbwalker.AttackingEnabled = false;

            DelayAction.Queue(duration, () => Global.Orbwalker.AttackingEnabled = true, new CancellationToken(false));
        }

        public static float GetEnergyCostPercent()
        {
            var total = 0f;

            if (Global.Player.SpellBook.GetSpell(SpellSlot.Q).State == SpellState.Ready)
            {
                total += Global.Player.SpellBook.GetSpell(SpellSlot.Q).Cost;
            }

            if (Global.Player.SpellBook.GetSpell(SpellSlot.W).State == SpellState.Ready)
            {
                total += Global.Player.SpellBook.GetSpell(SpellSlot.W).Cost;
            }

            if (Global.Player.SpellBook.GetSpell(SpellSlot.E).State == SpellState.Ready)
            {
                total += Global.Player.SpellBook.GetSpell(SpellSlot.E).Cost;
            }

            return (Global.Player.Mana - total) / Global.Player.MaxMana * 100;
        }

        public static float GetEnergyPercent()
        {
            return Global.Player.Mana / Global.Player.MaxMana * 100;
        }
    }
}
