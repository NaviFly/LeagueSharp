using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace FreshLeeSin
{
    class MainMenu
    {
        public static Menu _MainMenu;
        public static Orbwalking.Orbwalker _OrbWalker;
        public static void Menu()
        {
            try // try start
            {
                _MainMenu = new Menu("FreshLeeSin", "FreshLeeSin", true).SetFontStyle(System.Drawing.FontStyle.Regular, Color.Aqua);
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
                    Combo.AddItem(new MenuItem("CUseQ_Hit", "Q HitChance").SetValue(new Slider(4, 1, 6)));
                    Combo.AddItem(new MenuItem("CKey", "Combo Key").SetValue(new KeyBind("32".ToArray()[0], KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Combo);

                var Harass = new Menu("Harass", "Harass");
                {
                    Harass.AddItem(new MenuItem("HUse_Q", "Use Q").SetValue(true));
                    Harass.AddItem(new MenuItem("HUse_W", "Use W").SetValue(true));
                    Harass.AddItem(new MenuItem("HUse_E", "Use E").SetValue(true));
                    Harass.AddItem(new MenuItem("HKey", "Harass Key").SetValue(new KeyBind("C".ToArray()[0], KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Harass);

                var LaneClear = new Menu("LaneClear", "LaneClear");
                {
                    LaneClear.AddItem(new MenuItem("LUse_Q", "Use Q").SetValue(true));
                    LaneClear.AddItem(new MenuItem("LUse_W", "Use W").SetValue(true));
                    LaneClear.AddItem(new MenuItem("LUse_E", "Use E").SetValue(true));
                    LaneClear.AddItem(new MenuItem("LKey", "LaneClear Key").SetValue(new KeyBind("V".ToArray()[0], KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(LaneClear);

                var JungleClear = new Menu("JungleClear", "JungleClear");
                {
                    JungleClear.AddItem(new MenuItem("JUse_Q", "Use Q").SetValue(true));
                    JungleClear.AddItem(new MenuItem("JUse_W", "Use W").SetValue(true));
                    JungleClear.AddItem(new MenuItem("JUse_E", "Use E").SetValue(true));
                    JungleClear.AddItem(new MenuItem("JKey", "JungleClear Key").SetValue(new KeyBind("V".ToArray()[0], KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(JungleClear);

                var Misc = new Menu("Misc", "Misc");
                {
                    Misc.AddItem(new MenuItem("Ward_W", "W to Ward").SetValue(new KeyBind("A".ToArray()[0], KeyBindType.Press)));
                    Misc.AddItem(new MenuItem("InsecKick", "Insec Kick").SetValue(new KeyBind("G".ToArray()[0], KeyBindType.Press)));
                    Misc.AddItem(new MenuItem("KickAndFlash", "When use InsecKick, Do you use Flash + R?").SetValue(true));
                    Misc.AddItem(new MenuItem("KUse_R", "KillSteal, Use R").SetValue(true));
                    Misc.AddItem(new MenuItem("AutoKick", "When possible hit something, Use _R").SetValue(new Slider(3, 0, 5)));
                    Misc.AddItem(new MenuItem("AntiGab", "Anti-Gapcloser, If My hp < someone, Use _R").SetValue(new Slider(15, 0, 100)));
                }
                _MainMenu.AddSubMenu(Misc);

                var Draw = new Menu("Draw", "Draw");
                {
                    Draw.AddItem(new MenuItem("Draw_Q", "Draw Q").SetValue(false));
                    Draw.AddItem(new MenuItem("Draw_W", "Draw W").SetValue(false));
                    Draw.AddItem(new MenuItem("Draw_E", "Draw E").SetValue(false));
                    Draw.AddItem(new MenuItem("Draw_Ward", "Draw Ward").SetValue(true));
                    Draw.AddItem(new MenuItem("PredictR", "_R Prediction Location").SetValue(true));
                    Draw.AddItem(new MenuItem("Indicator", "Draw Damage Indicator").SetValue(true));
                }
                _MainMenu.AddSubMenu(Draw);
            } // try end     
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshLeesin is not working. plz send message by KorFresh (Code 1)");
            }

        }
    }
}
