using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace Fresh_LeBlanc
{
    class MainMenu
    {
        public static Menu _MainMenu;
        public static Orbwalking.Orbwalker _OrbWalker;
        public static void Menu()
        {
            try // try start
            {
                _MainMenu = new Menu("Fresh LeBlanc", "FreshLeBlanc", true).SetFontStyle(System.Drawing.FontStyle.Regular, Color.Aqua); ;
                _MainMenu.AddToMainMenu();

                Menu orbwalkerMenu = new Menu("OrbWalker", "OrbWalker");
                _OrbWalker = new Orbwalking.Orbwalker(orbwalkerMenu);
                _MainMenu.AddSubMenu(orbwalkerMenu);

                var targetSelectorMenu = new Menu("Target Selector", "TargetSelector");
                TargetSelector.AddToMenu(targetSelectorMenu);
                _MainMenu.AddSubMenu(targetSelectorMenu);

                var Combo = new Menu("Combo", "Combo");
                {
                    Combo.AddItem(new MenuItem("CUse_Q", "Use Q").SetValue(true));
                    Combo.AddItem(new MenuItem("CUse_W", "Use W").SetValue(true));
                    Combo.AddItem(new MenuItem("CUse_WReturn", "Use W Return").SetValue(true));
                    Combo.AddItem(new MenuItem("CUse_E", "Use E").SetValue(true));
                    Combo.SubMenu("Use R").AddItem(new MenuItem("CUse_Q2", "Use Q").SetValue(true));
                    Combo.SubMenu("Use R").AddItem(new MenuItem("CUse_W2", "Use W").SetValue(true));
                    Combo.SubMenu("Use R").AddItem(new MenuItem("CUse_W2Return", "Use W Return").SetValue(true));
                    Combo.SubMenu("Use R").AddItem(new MenuItem("CUse_E2", "Use E").SetValue(true));
                    Combo.AddItem(new MenuItem("CUseE_Hit", "E HitChance").SetValue(new Slider(3, 1, 6)));
                    Combo.AddItem(new MenuItem("ComboMode", "Combo Mode is Teamfight").SetValue(new KeyBind('N',KeyBindType.Toggle)));
                    Combo.AddItem(new MenuItem("CKey", "Combo Key").SetValue(new KeyBind(32, KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Combo);

                var Harass = new Menu("Harass", "Harass");
                {
                    Harass.AddItem(new MenuItem("HKey", "Harass Key").SetValue(new KeyBind('C', KeyBindType.Press)));
                    Harass.SubMenu("Auto Harass").AddItem(new MenuItem("AUse_Q", "Use Q").SetValue(true));
                    Harass.SubMenu("Auto Harass").AddItem(new MenuItem("AUse_W", "Use W").SetValue(true));
                    Harass.SubMenu("Auto Harass").AddItem(new MenuItem("AUse_E", "Use E").SetValue(true));
                    Harass.AddItem(new MenuItem("AManarate", "Mana %").SetValue(new Slider(20)));
                    Harass.SubMenu("Auto Harass").AddItem(new MenuItem("AHToggle", "Auto Enable").SetValue(false));
                }
                _MainMenu.AddSubMenu(Harass);

                var KillSteal = new Menu("KillSteal", "KillSteal");
                {
                    KillSteal.AddItem(new MenuItem("KUse_Q", "Use Q").SetValue(true));
                    KillSteal.AddItem(new MenuItem("KUse_W", "Use W").SetValue(true));
                    KillSteal.AddItem(new MenuItem("KUse_E", "Use E").SetValue(true));
                    KillSteal.SubMenu("Use R").AddItem(new MenuItem("KUse_Q2", "Use Q").SetValue(true));
                    KillSteal.SubMenu("Use R").AddItem(new MenuItem("KUse_W2", "Use W").SetValue(true));
                    KillSteal.SubMenu("Use R").AddItem(new MenuItem("KUse_E2", "Use E").SetValue(true));
                }
                _MainMenu.AddSubMenu(KillSteal);

                var Misc = new Menu("Misc", "Misc");
                {
                    Misc.AddItem(new MenuItem("ERCC", "Use _E + _R CC").SetValue(new KeyBind('G', KeyBindType.Press)));
                    Misc.AddItem(new MenuItem("Flee", "Flee & W + R").SetValue(new KeyBind('T', KeyBindType.Press)));
                    Misc.AddItem(new MenuItem("Pet", "Passive will be locating between Me & Enemy").SetValue(true));
                }
                _MainMenu.AddSubMenu(Misc);

                var Draw = new Menu("Draw", "Draw");
                {
                    Draw.AddItem(new MenuItem("Draw_Q", "Draw Q").SetValue(false));
                    Draw.AddItem(new MenuItem("Draw_W", "Draw W").SetValue(false));
                    Draw.AddItem(new MenuItem("Draw_E", "Draw E").SetValue(false));
                    Draw.AddItem(new MenuItem("Draw_WR", "Draw W + R").SetValue(false));
                    Draw.AddItem(new MenuItem("Indicator", "Draw Damage Indicator").SetValue(true));
                    Draw.AddItem(new MenuItem("WTimer", "Indicate _W Timer").SetValue(true));
                    Draw.AddItem(new MenuItem("Draw_ComboMode", "Draw Combo Mode").SetValue(true));
                    Draw.AddItem(new MenuItem("REnable", "Show _R Status").SetValue(true));
                }
                _MainMenu.AddSubMenu(Draw);
            } // try end     
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshLeBlanc is not working. plz send message by KorFresh (Code 1)");
            }

        }
    }
}
