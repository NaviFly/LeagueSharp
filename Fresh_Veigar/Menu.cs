using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace Fresh_Veigar
{
    class MainMenu
    {
        public static Menu _MainMenu;
        public static Orbwalking.Orbwalker _OrbWalker;
        public static void Menu()
        {
            try // try start
            {
                _MainMenu = new Menu("Fresh Veigar", "FreshVeigar", true).SetFontStyle(System.Drawing.FontStyle.Regular, Color.Aqua); ;
                _MainMenu.AddToMainMenu();

                Menu orbwalkerMenu = new Menu("OrbWalker", "OrbWalker");
                _OrbWalker = new Orbwalking.Orbwalker(orbwalkerMenu);
                _MainMenu.AddSubMenu(orbwalkerMenu);

                var targetSelectorMenu = new Menu("Target Selector", "TargetSelector");
                TargetSelector.AddToMenu(targetSelectorMenu);
                _MainMenu.AddSubMenu(targetSelectorMenu);

                var Combo = new Menu("Combo", "Combo");
                {
                    Combo.AddItem(new MenuItem("CUseQ", "Use Q").SetValue(true));
                    Combo.AddItem(new MenuItem("CUseW", "Use W").SetValue(true));
                    Combo.AddItem(new MenuItem("CUseE", "Use E").SetValue(true));
                    Combo.AddItem(new MenuItem("CUseR", "Use R").SetValue(true));
                    Combo.AddItem(new MenuItem("CUseR_Select", "When can be Kill, Only use R").SetValue(true));
                    Combo.AddItem(new MenuItem("CUseQ_Hit", "Q HitChance").SetValue(new Slider(3, 1, 6)));
                    Combo.AddItem(new MenuItem("CKey", "Combo Key").SetValue(new KeyBind(32, KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Combo);

                var Harass = new Menu("Harass", "Harass");
                {
                    Harass.AddItem(new MenuItem("HUseQ", "Use Q").SetValue(true));
                    Harass.AddItem(new MenuItem("HUseW", "Use W - When target can't only move").SetValue(true));
                    Harass.AddItem(new MenuItem("HUseE", "Use E - When target can't only move").SetValue(true));
                    Harass.AddItem(new MenuItem("HUseR", "Use R").SetValue(true));
                    Harass.AddItem(new MenuItem("HManarate", "Mana %").SetValue(new Slider(20)));
                    Harass.AddItem(new MenuItem("AutoHUseQ", "Auto Harass").SetValue(new KeyBind('T', KeyBindType.Toggle)));
                    Harass.AddItem(new MenuItem("HKey", "Harass Key").SetValue(new KeyBind('C', KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Harass);

                var LaneClear = new Menu("LaneClear", "LaneClear");
                {
                    LaneClear.AddItem(new MenuItem("LUseQ", "Use Q").SetValue(true));
                    LaneClear.AddItem(new MenuItem("LUseQSet", "Use Q Only use lasthit to minion").SetValue(true));
                    LaneClear.AddItem(new MenuItem("LManarate", "Mana %").SetValue(new Slider(20)));
                    LaneClear.AddItem(new MenuItem("LKey", "LaneClear Key").SetValue(new KeyBind('V', KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(LaneClear);

                var JungleClear = new Menu("JungleClear", "JungleClear");
                {
                    JungleClear.AddItem(new MenuItem("JUseQ", "Use Q").SetValue(true));
                    JungleClear.AddItem(new MenuItem("JUseQSet", "Use Q Only use lasthit to minion").SetValue(true));
                    JungleClear.AddItem(new MenuItem("JManarate", "Mana %").SetValue(new Slider(20)));
                    JungleClear.AddItem(new MenuItem("JKey", "JungleClear Key").SetValue(new KeyBind('V', KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(JungleClear);

                var KillSteal = new Menu("KillSteal", "KillSteal");
                {
                    KillSteal.AddItem(new MenuItem("KseQ", "Use Q").SetValue(true));
                    KillSteal.AddItem(new MenuItem("KseW", "Use W").SetValue(true));
                    KillSteal.AddItem(new MenuItem("KseR", "Use R").SetValue(true));
                }
                _MainMenu.AddSubMenu(KillSteal);

                var Misc = new Menu("Misc", "Misc");
                {
                    Misc.AddItem(new MenuItem("Anti-GapCloser", "Anti GapCloser").SetValue(true));
                    Misc.AddItem(new MenuItem("Interrupt", "E with Interrupt").SetValue(true));
                }
                _MainMenu.AddSubMenu(Misc);

                var Draw = new Menu("Draw", "Draw");
                {
                    Draw.AddItem(new MenuItem("Draw_Q", "Draw Q").SetValue(false));
                    Draw.AddItem(new MenuItem("Draw_W", "Draw W").SetValue(false));
                    Draw.AddItem(new MenuItem("Draw_E", "Draw E").SetValue(false));
                    Draw.AddItem(new MenuItem("Draw_R", "Draw R").SetValue(false));
                    Draw.AddItem(new MenuItem("Indicator", "Draw Damage Indicator").SetValue(true));
                }
                _MainMenu.AddSubMenu(Draw);
            } // try end     
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshVeigar is not working. plz send message by KorFresh (Code 1)");
            }

        }
    }
}
