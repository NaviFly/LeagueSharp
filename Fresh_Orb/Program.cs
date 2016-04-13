using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Fresh_Orb
{
    class Program
    {
        public const string ChampName = "Leblanc";
        public static Obj_AI_Hero Player;
        public static Spell _Q, _W, _E, _R;
        public static List<Spell> SpellList = new List<Spell>();        

        static void Main(string[] args)
        {
            try
            {
                CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshBritzcrank is not working. plz send message by KorFresh (Code 2)");
            }
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            try
            {
                Player = ObjectManager.Player;
                MainMenu.Menu();
                Game.OnUpdate += OnGameUpdate;                
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshBritzcrank is not working. plz send message by KorFresh (Code 3)");
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            try
            {
                if (Player.IsDead) return;                      
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshBritzcrank is not working. plz send message by KorFresh (Code 4)");
            }
        }       

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            try
            {
                
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshBritzcrank is not working. plz send message by KorFresh (Code 6)");
            }
        }

        private static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshBritzcrank is not working. plz send message by KorFresh (Code 7)");
            }
        }       

        private static HitChance Hitchance(string Type)
        {
            HitChance result = HitChance.Low;
            switch (MainMenu._MainMenu.Item(Type).GetValue<Slider>().Value)
            {
                case 1:
                    result = HitChance.OutOfRange;
                    break;
                case 2:
                    result = HitChance.Impossible;
                    break;
                case 3:
                    result = HitChance.Low;
                    break;
                case 4:
                    result = HitChance.Medium;
                    break;
                case 5:
                    result = HitChance.High;
                    break;
                case 6:
                    result = HitChance.VeryHigh;
                    break;
            }
            return result;
        }

        private static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!sender.IsEnemy || !sender.IsValid<Obj_AI_Hero>())
                return;

            if (MainMenu._MainMenu.Item("InterQ").GetValue<bool>() && _Q.IsReady())
            {
                if (sender.Distance(Player.ServerPosition, true) <= _Q.RangeSqr)
                    _Q.Cast(sender);
            }
            if (MainMenu._MainMenu.Item("InterR").GetValue<bool>() && _R.IsReady())
            {
                if (sender.Distance(Player.ServerPosition, true) <= _R.RangeSqr)
                    _R.Cast();
            }
            if (MainMenu._MainMenu.Item("InterR").GetValue<bool>() && _E.IsReady())
            {
                if (sender.Distance(Player.ServerPosition, true) <= _E.RangeSqr)
                    _E.CastOnUnit(Player);
            }
        }
    }
}