﻿using System.Collections.Generic;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util.Cache;

namespace Adept_AIO.Champions.Yasuo.Core
{
    class MenuConfig
    {
        private static Menu MainMenu;

        public static Menu Combo,
                           Whitelist,
                           Harass,
                           LaneClear,
                           JungleClear,
                           Killsteal,
                           Drawings;

        public static void Attach()
        {
            MainMenu = new Menu(string.Empty, "Adept AIO", true);
            MainMenu.Attach();

            var orbwalker = new Orbwalker();
            orbwalker.Attach(MainMenu);

            Whitelist= new Menu("Whitelist", "Whitelist");
            foreach (var hero in GameObjects.EnemyHeroes)
            {
                Whitelist.Add(new MenuBool(hero.ChampionName, "Use R Against: " + hero.ChampionName));
            }

            Combo = new Menu("Combo", "Combo")
            {
                new MenuBool("Delay", "Delay R").SetToolTip("Tries to Knockup -> AA -> R"),
                new MenuBool("Flash", "Use Flash (Beyblade)").SetToolTip("Will try to E-Q -> Flash. Known as Beyblade"),
                new MenuBool("Turret", "Avoid Using E Under Turret"),
                new MenuBool("Stack", "Safely Stack Q"),
                new MenuList("Dash", "Dash Mode: ", new []{"Cursor", "From Player"}, 0),
                new MenuSlider("Range", "Mouse Dash Range: ", 650, 1, 1000)
            };
            
            // Todo: Add Check and go: EQ AA -> E Out 
            Harass = new Menu("Harass", "Harass")
            {
                new MenuBool("Q", "Use Q3"),
                new MenuBool("E", "Use E", false)
            };

            LaneClear = new Menu("LaneClear", "LaneClear")
            {
                new MenuBool("Check", "Don't Clear When Enemies Nearby"),
                new MenuBool("Turret", "Don't Clear Under Turret"),
                new MenuSliderBool("Q3", "Min. Hit For Q3", false, 2, 1, 7),
                new MenuList("Mode", "E Mode: ", new []{"Disabled", "Lasthit", "Fast Clear"}, 1)
            };

            JungleClear = new Menu("JungleClear", "JungleClear")
            {
                new MenuBool("Q3", "Allow Q3 Usage"),
                new MenuBool("Q",  "Allow Q1 Usage"),
                new MenuBool("E",  "Allow E  Usage")
            };

            Killsteal = new Menu("Killsteal", "Killsteal")
            {
                new MenuBool("Q", "Use Q"),
                new MenuBool("Q3", "Use Q3"),
                new MenuBool("E", "Use E")
            };

            Drawings = new Menu("Drawings", "Drawings")
            {
                new MenuSlider("Segments", "Segments", 200, 100, 300).SetToolTip("Smoothness of the circles. Less equals more FPS."),
                new MenuBool("R", "Draw R Range"),
            };

            var Credits = new Menu("Credits", "Credits")
            {
                new MenuSeperator("WhyAreYouReadingThis", "Written by: Nechrito | Haki | Adept"),
                new MenuSeperator("ThisStringIsUtterlyUseless", "Platform: LeageTec 2017"),
            };

            foreach (var menu in new List<Menu>
            {
                Whitelist,
                Combo,
                Harass,
                LaneClear,
                JungleClear,
                Killsteal,
                Drawings,
                Credits
            })
            MainMenu.Add(menu);
        }
    }
}