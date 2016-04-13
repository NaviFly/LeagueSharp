using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace Fresh_Ryze
{
    class MainMenu
    {
        public static Menu _MainMenu;
        public static Orbwalking.Orbwalker _OrbWalker;
        public static void Menu()
        {
            try // try start
            {
                _MainMenu = new Menu("Fresh Ryze", "FreshRyze", true).SetFontStyle(System.Drawing.FontStyle.Regular, Color.Aqua); ;
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
                    Combo.AddItem(new MenuItem("CUse_E", "Use E").SetValue(true));
                    Combo.AddItem(new MenuItem("CUse_R", "Use R").SetValue(true));
                    Combo.AddItem(new MenuItem("CKey", "Combo Key").SetValue(new KeyBind(32, KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Combo);

                var Harass = new Menu("Harass", "Harass");
                {
                    Harass.AddItem(new MenuItem("HUse_Q", "Use Q").SetValue(true));
                    Harass.AddItem(new MenuItem("HUse_W", "Use W").SetValue(true));
                    Harass.AddItem(new MenuItem("HUse_E", "Use E").SetValue(true));
                    Harass.AddItem(new MenuItem("HManarate", "Mana %").SetValue(new Slider(50)));
                    Harass.AddItem(new MenuItem("HToggle", "Auto Enable").SetValue(false));
                    Harass.AddItem(new MenuItem("HKey", "Harass Key").SetValue(new KeyBind('C', KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Harass);

                var LaneClear = new Menu("LaneClear", "LaneClear");
                {
                    LaneClear.AddItem(new MenuItem("LUseQ", "Use Q").SetValue(true));
                    LaneClear.AddItem(new MenuItem("LUseW", "Use W").SetValue(true));
                    LaneClear.AddItem(new MenuItem("LUseE", "Use E").SetValue(true));
                    LaneClear.AddItem(new MenuItem("LManaRate", "Mana %").SetValue(new Slider(20)));
                    LaneClear.AddItem(new MenuItem("LKey", "LaneClear Key").SetValue(new KeyBind('V', KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(LaneClear);

                var JungleClear = new Menu("JungleClear", "JungleClear");
                {
                    JungleClear.AddItem(new MenuItem("JUseQ", "Use Q").SetValue(true));
                    JungleClear.AddItem(new MenuItem("JUseW", "Use W").SetValue(true));
                    JungleClear.AddItem(new MenuItem("JUseE", "Use E").SetValue(true));
                    JungleClear.AddItem(new MenuItem("JManaRate", "Mana %").SetValue(new Slider(20)));
                    JungleClear.AddItem(new MenuItem("JKey", "JungleClear Key").SetValue(new KeyBind('V', KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(JungleClear);

                var KillSteal = new Menu("KillSteal", "KillSteal");
                {
                    KillSteal.AddItem(new MenuItem("KUse_Q", "Use Q").SetValue(true));
                    KillSteal.AddItem(new MenuItem("KUse_W", "Use W").SetValue(true));
                    KillSteal.AddItem(new MenuItem("KUse_E", "Use E").SetValue(true));
                }
                _MainMenu.AddSubMenu(KillSteal);

                var Misc = new Menu("Misc", "Misc");
                {
                    Misc.SubMenu("Auto Stack").AddItem(new MenuItem("AutoStack", "Auto Stack").SetValue(new Slider(0, 0, 4)));
                    Misc.SubMenu("Auto Stack").AddItem(new MenuItem("AutoKey", "Toggle Key").SetValue(new KeyBind('H', KeyBindType.Toggle)));
                    Misc.SubMenu("Auto Lasthit").AddItem(new MenuItem("AutoLasthitQ", "Use Q").SetValue(false));
                    Misc.SubMenu("Auto Lasthit").AddItem(new MenuItem("AutoLasthitW", "Use W").SetValue(false));
                    Misc.SubMenu("Auto Lasthit").AddItem(new MenuItem("AutoLasthitE", "Use E").SetValue(false));
                    Misc.SubMenu("Auto Lasthit").AddItem(new MenuItem("AutoLasthit", "Enable").SetValue(false));
                    Misc.AddItem(new MenuItem("WGap", "Auto W, When Anti-GapCloser").SetValue(true));
                    Misc.AddItem(new MenuItem("Speed", "Skill Speed mode").SetValue(new StringList(new[] { "Crazy", "Fast", "Nomal", "Random" })));
                }
                _MainMenu.AddSubMenu(Misc);

                var Draw = new Menu("Draw", "Draw");
                {
                    Draw.AddItem(new MenuItem("QRange", "Q Range").SetValue(false));
                    Draw.AddItem(new MenuItem("WRange", "W Range").SetValue(false));
                    Draw.AddItem(new MenuItem("ERange", "E Range").SetValue(false));
                    Draw.AddItem(new MenuItem("DisplayStack", "Display Stack").SetValue(true));
                    Draw.AddItem(new MenuItem("DisplayTime", "Display Time").SetValue(true));
                    Draw.AddItem(new MenuItem("AutoStackText", "Auto Stack Enable").SetValue(true));
                    Draw.AddItem(new MenuItem("Indicator", "Draw Damage Indicator").SetValue(true));
                }
                _MainMenu.AddSubMenu(Draw);
            } // try end     
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshRyze is not working. plz send message by KorFresh (Code 1)");
            }

        }
    }
}
