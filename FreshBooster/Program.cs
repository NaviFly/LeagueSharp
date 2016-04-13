using System;
using LeagueSharp.Common;

namespace FreshBooster
{
    class Program
    {
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }
        private static void Game_OnGameLoad(EventArgs args)
        {
            Loader.Load();
        }
    }
}