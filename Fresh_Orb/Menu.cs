using System;
using LeagueSharp;
using LeagueSharp.Common;


namespace Fresh_Orb
{
    class MainMenu
    {
        public static Menu _MainMenu;
        public static Orbwalking.Orbwalker _OrbWalker;
        public static void Menu()
        {
            try // try start
            {
                _MainMenu = new Menu("FreshOrb", "FreshOrb", true);
                _MainMenu.AddToMainMenu();

                Menu orbwalkerMenu = new Menu("OrbWalker", "OrbWalker");
                _OrbWalker = new Orbwalking.Orbwalker(orbwalkerMenu);
                _MainMenu.AddSubMenu(orbwalkerMenu);

                var targetSelectorMenu = new Menu("Target Selector", "TargetSelector");
                TargetSelector.AddToMenu(targetSelectorMenu);
                _MainMenu.AddSubMenu(targetSelectorMenu);
                
            } // try end     
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshLeesin is not working. plz send message by KorFresh (Code 1)");
            }           
            
        }
    }
}
