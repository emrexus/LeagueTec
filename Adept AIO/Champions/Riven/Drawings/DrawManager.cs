﻿using System.Drawing;
using Adept_AIO.Champions.Riven.Core;
using Aimtec;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Riven.Drawings
{
    internal class DrawManager
    {
        public static void RenderManager()
        {
            if (ObjectManager.GetLocalPlayer().IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["Harass"].Enabled && Orbwalker.Implementation.Mode == OrbwalkingMode.Mixed)
            {
                Vector2 screenPos;
                Render.WorldToScreen(ObjectManager.GetLocalPlayer().Position, out screenPos);
                Render.Text(new Vector2(screenPos.X - 65, screenPos.Y + 30), Color.Aqua, "PATTERN: " + Extensions.Current.ToString().ToUpper());
            }

            if (MenuConfig.Drawings["Engage"].Enabled)
            {
                Render.Circle(ObjectManager.GetLocalPlayer().Position, Extensions.GetRange(),
                    (uint) MenuConfig.Drawings["Segments"].Value, Extensions.AllIn ? Color.Yellow : Color.White);
            }

            if (MenuConfig.Drawings["R2"].Enabled && SpellConfig.R2.Ready && Extensions.UltimateMode == UltimateMode.Second)
            {
                Render.Circle(ObjectManager.GetLocalPlayer().Position, SpellConfig.R2.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.OrangeRed);
            }
        }
    }
}