using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace FreshPoppy
{
    class MainMenu
    {
        public static Menu _MainMenu;
        public static Orbwalking.Orbwalker _OrbWalker;
        public static void Menu()
        {
            try // try start
            {
                _MainMenu = new Menu("Fresh Poppy", "FreshPoppy", true).SetFontStyle(System.Drawing.FontStyle.Regular, Color.Aqua);
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
                    Combo.SubMenu("Use _R to Enemy").AddItem(new MenuItem("CUse_R_Enable", "Enable").SetValue(true));
                    foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
                    {
                        if (enemy.Team != Program.Player.Team)
                        {
                            Combo.SubMenu("Use _R to Enemy").AddItem(new MenuItem("CUse"+enemy.ChampionName, enemy.ChampionName).SetValue(true));
                        }
                    }
                    Combo.AddItem(new MenuItem("CKey", "Combo Key").SetValue(new KeyBind(32, KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Combo);
                

                var LaneClear = new Menu("LaneClear", "LaneClear");
                {
                    LaneClear.AddItem(new MenuItem("LUse_Q", "Use Q").SetValue(true));                    
                    LaneClear.AddItem(new MenuItem("LUse_E", "Use E").SetValue(true));
                    LaneClear.AddItem(new MenuItem("LKey", "LaneClear Key").SetValue(new KeyBind('V', KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(LaneClear);

                var JungleClear = new Menu("JungleClear", "JungleClear");
                {
                    JungleClear.AddItem(new MenuItem("JUse_Q", "Use Q").SetValue(true));                    
                    JungleClear.AddItem(new MenuItem("JUse_E", "Use E").SetValue(true));
                    JungleClear.AddItem(new MenuItem("JKey", "JungleClear Key").SetValue(new KeyBind('V', KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(JungleClear);

                var KillSteal = new Menu("KillSteal", "KillSteal");
                {
                    KillSteal.AddItem(new MenuItem("KUse_Q", "Use Q").SetValue(true));
                    KillSteal.AddItem(new MenuItem("KUse_E", "Use E").SetValue(true));
                    KillSteal.AddItem(new MenuItem("KUse_R", "Use R").SetValue(true));
                }
                _MainMenu.AddSubMenu(KillSteal);

                var Misc = new Menu("Misc", "Misc");
                {                    
                    Misc.SubMenu("Anti-GapCloser").AddItem(new MenuItem("Gap_W", "Use W").SetValue(true));
                    Misc.SubMenu("Anti-GapCloser").AddItem(new MenuItem("Gap_E", "Use E - WallBreak").SetValue(true));                    
                    foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
                    {
                        if (enemy.Team != Program.Player.Team)
                        {                            
                            Misc.SubMenu("Anti-GapCloser").SubMenu("Use R").AddItem(new MenuItem(enemy.ChampionName, enemy.ChampionName).SetValue(true));                            
                        }
                    }
                    Misc.AddItem(new MenuItem("inter_E", "Interrupt Use E").SetValue(true));
                    Misc.AddItem(new MenuItem("inter_R", "Interrupt Use R").SetValue(true));
                    Misc.AddItem(new MenuItem("CC", "CC use _R Super Airborne").SetValue(new KeyBind('G', KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Misc);

                var Draw = new Menu("Draw", "Draw");
                {
                    Draw.AddItem(new MenuItem("Draw_Q", "Draw Q").SetValue(false));
                    Draw.AddItem(new MenuItem("Draw_E", "Draw E").SetValue(false));
                    Draw.AddItem(new MenuItem("Draw_E2", "Draw After use E").SetValue(false));
                    Draw.AddItem(new MenuItem("Draw_R", "Draw R").SetValue(false));
                    Draw.AddItem(new MenuItem("Indicator", "Draw Damage Indicator").SetValue(true));
                    Draw.AddItem(new MenuItem("Shield", "Show location of My Shield").SetValue(true));
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
