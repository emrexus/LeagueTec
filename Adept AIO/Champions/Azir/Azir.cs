﻿using System.Collections.Generic;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.Champions.Azir.Drawings;
using Adept_AIO.Champions.Azir.Miscellaneous;
using Adept_AIO.SDK.Delegates;
using Aimtec;

namespace Adept_AIO.Champions.Azir
{
    internal class Azir
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();
            SoldierManager.Soldiers = new List<Obj_AI_Minion>();

            Game.OnUpdate += Manager.OnUpdate;
            Game.OnUpdate += Killsteal.OnUpdate;

            Render.OnPresent += DrawManager.OnPresent;
            Render.OnRender += DrawManager.OnRender;

            GameObject.OnCreate += SoldierManager.OnCreate;
            GameObject.OnDestroy += SoldierManager.OnDelete;
            Obj_AI_Base.OnProcessSpellCast += AzirHelper.OnProcessSpellCast;
            Gapcloser.OnGapcloser += AntiGapcloser.OnGapcloser;
        }
    }
}
