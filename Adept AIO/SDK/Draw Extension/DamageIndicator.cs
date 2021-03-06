﻿using System.Drawing;
using Aimtec;

namespace Adept_AIO.SDK.Draw_Extension
{
    internal class DamageIndicator
    {
        internal static int Height => 9;
        internal static int Width => 104;

        public Obj_AI_Base Unit { get; set; }

        private Vector2 Offset
        {
            get
            {
                if (Unit != null)
                {
                    return Unit.IsAlly ? new Vector2(34, 9) 
                                       : new Vector2(10, 20);
                }
                return new Vector2();
            }
        }

        public Vector2 StartPosition()
        {
            return new Vector2(Unit.FloatingHealthBarPosition.X + Offset.X, Unit.FloatingHealthBarPosition.Y + Offset.Y);
        }

        private Vector2 EndPosition(float dmg)
        {
            var w = GetHpProc(dmg) * Width;
            return new Vector2(StartPosition().X + w, StartPosition().Y);
        }

        private float GetHpProc(float dmg)
        {
            return (Unit.Health - dmg > 0 ? Unit.Health - dmg : 0) 
                 / Unit.MaxHealth;
        }

        public void DrawDmg(float dmg, Color color)
        {
            var from = EndPosition(0);
            var to   = EndPosition(dmg);

            if (from.IsZero || to.IsZero || Unit == null)
            {
                return;
            }

            Render.Line(new Vector2(from.X, from.Y - 5),
                        new Vector2(to.X, to.Y - 5), Height, false, color);
        }
    }
}
