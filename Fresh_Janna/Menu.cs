using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace Fresh_Janna
{
    class MainMenu
    {
        public static Menu _MainMenu;
        public static Orbwalking.Orbwalker _OrbWalker;
        public static void Menu()
        {
            try // try start
            {
                _MainMenu = new Menu("FreshJanna", "FreshJanna", true).SetFontStyle(System.Drawing.FontStyle.Regular, Color.Aqua); ; ;
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
                    Combo.AddItem(new MenuItem("CKey", "Combo Key").SetValue(new KeyBind(32, KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Combo);

                var Misc = new Menu("Misc", "Misc");
                {
                    Misc.AddItem(new MenuItem("Flee", "Flee Key").SetValue(new KeyBind('G', KeyBindType.Press)));
                    Misc.AddItem(new MenuItem("AutoE", "Auto E").SetValue(true));
                    Misc.SubMenu("Auto R").AddItem(new MenuItem("AutoRHP", "Min HP %").SetValue(new Slider(10,0,100)));
                    Misc.SubMenu("Auto R").AddItem(new MenuItem("AutoREnable", "Enable").SetValue(true));
                    Misc.SubMenu("Interrupt").AddItem(new MenuItem("InterQ", "Use Q").SetValue(true));
                    Misc.SubMenu("Interrupt").AddItem(new MenuItem("InterR", "Use R").SetValue(true));
                    Misc.SubMenu("Anti-GapCloser").AddItem(new MenuItem("GapQ", "Use Q").SetValue(true));
                    Misc.SubMenu("Anti-GapCloser").AddItem(new MenuItem("GapR", "Use R").SetValue(true));
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
                Game.PrintChat("FreshJanna is not working. plz send message by KorFresh (Code 1)");
            }

        }
    }
}
