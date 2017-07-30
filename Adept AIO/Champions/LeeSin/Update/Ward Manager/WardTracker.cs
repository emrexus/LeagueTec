﻿using System;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.Ward_Manager
{
    public class WardTracker : IWardTracker
    {
        private readonly ISpellConfig SpellConfig;

        public WardTracker(ISpellConfig spellConfig)
        {
            SpellConfig = spellConfig;
        }

        public bool IsWardReady => WardNames.Any(Items.CanUseItem) && Environment.TickCount - LastWardCreated > 1500 || LastWardCreated <= 0;

        public string[] WardNames { get; } =
        {
            "TrinketTotemLvl1",
            "ItemGhostWard",
            "JammerDevice",
        };

        public void OnCreate(GameObject sender)
        {
            var ward = sender as Obj_AI_Minion;

            if (ward == null || WardPosition.Distance(ward.Position) > 600 ||
                Environment.TickCount - LastWardCreated > 1000 ||
                !SpellConfig.IsFirst(SpellConfig.W))
            {
                return;
            }

            Console.WriteLine("CREATED ");
            LastWardCreated = Environment.TickCount;
            WardName = ward.Name;
            WardPosition = ward.Position;

            GlobalExtension.Player.SpellBook.CastSpell(SpellSlot.W, ward.Position);
        }

        public float LastWardCreated { get; set; }

        public string WardName { get; private set; }

        public Vector3 WardPosition { get; set; }
    }
}