using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace Fresh_TahmKench
{
    class MainMenu
    {
        public static Menu _MainMenu;
        public static Orbwalking.Orbwalker _OrbWalker;
        public static void Menu()
        {
            try // try start
            {
                _MainMenu = new Menu("Fresh TahmKench", "FreshTahmKench", true).SetFontStyle(System.Drawing.FontStyle.Regular, Color.Aqua); ;
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
                    Combo.SubMenu("Use W").AddItem(new MenuItem("CUse_W1", "Devour Enemy").SetValue(true));
                    Combo.SubMenu("Use W").AddItem(new MenuItem("CUse_W2", "Regurgitate Minion").SetValue(true));
                    Combo.AddItem(new MenuItem("CKey", "Combo Key").SetValue(new KeyBind(32, KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Combo);

                var Harass = new Menu("Harass", "Harass");
                {
                    Harass.AddItem(new MenuItem("HUse_Q", "Use Q").SetValue(true));
                    Harass.AddItem(new MenuItem("HUse_W2", "Regurgitate Minion").SetValue(true));
                    Harass.AddItem(new MenuItem("HManarate", "Mana %").SetValue(new Slider(50)));
                    Harass.AddItem(new MenuItem("HToggle", "Auto Enable").SetValue(false));
                    Harass.AddItem(new MenuItem("HKey", "Harass Key").SetValue(new KeyBind('C', KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Harass);

                var KillSteal = new Menu("KillSteal", "KillSteal");
                {
                    KillSteal.AddItem(new MenuItem("KUse_Q", "Use Q").SetValue(true));
                    KillSteal.AddItem(new MenuItem("KUse_W", "Use W").SetValue(true));
                }
                _MainMenu.AddSubMenu(KillSteal);

                var Misc = new Menu("Misc", "Misc");
                {
                    Misc.SubMenu("Auto W").AddItem(new MenuItem("MyTeamCC", "When Ally has CC").SetValue(true));
                    Misc.SubMenu("Auto W").AddItem(new MenuItem("MyTeamMin", "Attck to Ally. Min HP %").SetValue(new Slider(15, 0, 100)));
                    Misc.SubMenu("Auto W").AddItem(new MenuItem("MyTeamMinEnable", "Attck to Ally Enable").SetValue(true));
                    Misc.SubMenu("Auto E").AddItem(new MenuItem("MinE", "Min. HP %").SetValue(new Slider(20, 0, 100)));
                    Misc.SubMenu("Auto E").AddItem(new MenuItem("AutoEUse", "Enable").SetValue(true));
                }
                _MainMenu.AddSubMenu(Misc);

                var Draw = new Menu("Draw", "Draw");
                {
                    Draw.AddItem(new MenuItem("QRange", "Q Range").SetValue(false));
                    Draw.AddItem(new MenuItem("WRange", "W Range").SetValue(false));
                    Draw.AddItem(new MenuItem("RRange", "R Range to Screen").SetValue(false));
                    Draw.AddItem(new MenuItem("RRange1", "R Range to MiniMap").SetValue(false));
                }
                _MainMenu.AddSubMenu(Draw);
            } // try end     
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshTahmKench is not working. plz send message by KorFresh (Code 1)");
            }

        }
    }
}
