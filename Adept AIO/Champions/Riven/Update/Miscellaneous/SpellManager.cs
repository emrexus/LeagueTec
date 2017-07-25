﻿using System;
using System.Linq;
using System.Threading;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class SpellManager
    {
        private static bool CanUseQ;
        private static bool CanUseW;
        private static Obj_AI_Base Unit;
      
        /// <summary>
        /// Tracks spells recently used by Riven
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            switch (args.SpellData.Name)
            {
                case "RivenTriCleave":
                    CanUseQ = false;
                    Extensions.AttackedStructure = false;
                    break;
                case "RivenMartyr":
                    CanUseW = false;
                    break;
                case "RivenFengShuiEngine":
                    Extensions.UltimateMode = UltimateMode.Second;
                    break;
                case "RivenIzunaBlade":
                    Extensions.UltimateMode = UltimateMode.First;
                    break;
            }
        }

        public static void OnUpdate()
        {
            if (Unit == null)
            {
                return;
            }

            if (CanUseQ)
            {
                if (Extensions.CurrentQCount == 3)
                {
                    Items.CastTiamat();
                }
               // Console.WriteLine((int)Orbwalker.Implementation.WindUpTime);
                if (Orbwalker.Implementation.CanMove())
                {
                    ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.Q, Unit);
                }
            }

            if (CanUseW)
            {
                Items.CastTiamat();
                SpellConfig.W.Cast();
            }
        }

        public static void ER(Obj_AI_Base target)
        {
            
        }

        public static void WQ()
        {
            if (SpellConfig.Q.Ready && SpellConfig.W.Ready && Environment.TickCount - Extensions.LastETime < 400)
            {
                CanUseW = true;
                CanUseQ = true;
            }
        }

        public static void QWQ(Obj_AI_Base target)
        {
            
        }

        public static void CastQ(Obj_AI_Base target)
        {
            if (target.HasBuff("FioraW") || target.HasBuff("PoppyW"))
            {
                return;
            }

            Unit = target;
            CanUseQ = true;
            DelayAction.Queue(150, () => CanUseQ = false);
        }

        public static void CastW(Obj_AI_Base target)
        {
            if (target.HasBuff("FioraW"))
            {
                return;
            }

            CanUseW = SpellConfig.W.Ready && InsideKiBurst(target);
            Unit = target;  
            DelayAction.Queue(300, () => CanUseW = false);
        }

        public static void CastR2(Obj_AI_Base target)
        {
            if (target.HasBuff(Extensions.InvulnerableList.Any().ToString()) && target.ValidActiveBuffs()
                .Where(buff => buff.Name.Contains(Extensions.InvulnerableList.Any().ToString()))
                .Any(buff => buff.Remaining > Time(target)))
            {
                return;
            }
            Items.CastTiamat();
            SpellConfig.R2.Cast(target);
        }

        private static int Time(GameObject target)
        {
            return (int)(ObjectManager.GetLocalPlayer().Distance(target) / (SpellConfig.R2.Speed * 1000 + SpellConfig.R2.Delay));
        }

        private static readonly uint GetTiamat = ObjectManager.GetLocalPlayer().HasItem(ItemId.Tiamat)        ? ItemId.Tiamat :
                                                 ObjectManager.GetLocalPlayer().HasItem(ItemId.RavenousHydra) ? ItemId.RavenousHydra :
                                                 ObjectManager.GetLocalPlayer().HasItem(ItemId.TitanicHydra)  ? ItemId.TitanicHydra : 0;

        public static bool InsideKiBurst(GameObject target)
        {
            return ObjectManager.GetLocalPlayer().HasBuff("RivenFengShuiEngine")
                 ? ObjectManager.GetLocalPlayer().Distance(target) <= 265 + target.BoundingRadius
                 : ObjectManager.GetLocalPlayer().Distance(target) <= 195 + target.BoundingRadius;
        }
    }
}