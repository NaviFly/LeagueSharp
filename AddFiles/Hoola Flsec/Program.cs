
using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;

using SharpDX;


namespace HoolaFlsec
{
    public class Program
    {
        private static Menu Menu;
        private static Obj_AI_Hero Player = ObjectManager.Player;
        static SpellSlot Flash = Player.GetSpellSlot("summonerFlash");
        private static Vector3 insecPos;
        public static Vector2 InsecLinePos;
        public static bool isNullInsecPos = true;
        private static bool ToAlly { get { return Menu.Item("ToAlly").GetValue<KeyBind>().Active; } }
        private static bool ToDraw { get { return Menu.Item("ToDraw").GetValue<bool>(); } }
        static bool ToWhere { get { return Menu.Item("ToWhere").GetValue<KeyBind>().Active; } }

        static void Main()
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        static void OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "LeeSin") return;
            Game.PrintChat("Hoola Flsec - Loaded Successfully, Good Luck! :)");

            OnMenuLoad();
            Drawing.OnDraw += OnDraw;
            Obj_AI_Base.OnProcessSpellCast += OnCast;

        }

        private static List<Obj_AI_Hero> GetAllyHeroes(Obj_AI_Hero position, int range)
        {
            var temp = new List<Obj_AI_Hero>();
            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (hero.IsAlly && !hero.IsMe && !hero.IsDead && hero.Distance(position) < range)
                {
                    temp.Add(hero);
                }
            }
            return temp;
        }

        private static Vector3 InterceptionPoint(List<Obj_AI_Hero> heroes)
        {
            var result = new Vector3();
            foreach (var hero in heroes)
            {
                result += hero.Position;
            }
            result.X /= heroes.Count;
            result.Y /= heroes.Count;
            return result;
        }

        private static void OnCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || !args.SData.Name.Contains("BlindMonkRKick")) return;
            var target = (Obj_AI_Hero)args.Target;
            if (ToAlly && target.IsValid)
            {
                var turrets = (from tower in ObjectManager.Get<Obj_Turret>()
                               where
                                   tower.IsAlly && !tower.IsDead
                                   && target.Distance(tower.Position)
                                   < 1500 && tower.Health > 0
                               select tower).ToList();

                if (GetAllyHeroes(target, 2000).Count > 0 && (!turrets.Any() || !ToWhere))
                {
                    var insecPosition =
                        InterceptionPoint(
                            GetAllyInsec(
                                GetAllyHeroes(target, 2000)));
                    InsecLinePos = Drawing.WorldToScreen(insecPosition);
                    insecPos = insecPosition;
                    if (Flash != SpellSlot.Unknown && Flash.IsReady())
                    {
                        Player.Spellbook.CastSpell(Flash, target.Position.Extend(insecPos, -200));
                    }
                }
                if (turrets.Any())
                {
                    var insecPosition = turrets[0].Position;
                    insecPos = insecPosition;
                    if (Flash != SpellSlot.Unknown && Flash.IsReady())
                    {
                        Player.Spellbook.CastSpell(Flash, target.Position.Extend(insecPos, -200));
                    }
                }
                if (GetAllyHeroes(target, 2000).Count == 0 && !turrets.Any())
                {
                    var insecPosition = Player.Position;
                    insecPos = insecPosition;
                    if (Flash != SpellSlot.Unknown && Flash.IsReady())
                    {
                        Player.Spellbook.CastSpell(Flash, target.Position.Extend(insecPos, -200));
                    }
                }
            }
        }

        private static List<Obj_AI_Hero> GetAllyInsec(List<Obj_AI_Hero> heroes)
        {
            byte alliesAround = 0;
            var tempObject = new Obj_AI_Hero();
            foreach (var hero in heroes)
            {
                var localTemp =
                    GetAllyHeroes(hero, 500).Count;
                if (localTemp > alliesAround)
                {
                    tempObject = hero;
                    alliesAround = (byte)localTemp;
                }
            }
            return GetAllyHeroes(tempObject, 500);
        }
        private static void OnDraw(EventArgs args)
        {
            var target = TargetSelector.GetTarget(1400, TargetSelector.DamageType.Physical);
            if (ToAlly && target.IsValid)
            {
                var turrets = (from tower in ObjectManager.Get<Obj_Turret>()
                               where
                                   tower.IsAlly && !tower.IsDead
                                   && target.Distance(tower.Position)
                                   < 1500 && tower.Health > 0
                               select tower).ToList();

                if (GetAllyHeroes(target, 2000).Count > 0 && (!turrets.Any() || !ToWhere))
                {
                    var insecPosition =
                        InterceptionPoint(
                            GetAllyInsec(
                                GetAllyHeroes(target, 2000)));
                    InsecLinePos = Drawing.WorldToScreen(insecPosition);
                    insecPos = insecPosition;
                    if (ToDraw) Render.Circle.DrawCircle(target.Position.Extend(insecPos, -200), 70, Color.Red);
                }
                if (turrets.Any())
                {
                    var insecPosition = turrets[0].Position;
                    insecPos = insecPosition;
                    if (ToDraw) Render.Circle.DrawCircle(target.Position.Extend(insecPos, -200), 70, Color.Red);
                }
                if (GetAllyHeroes(target, 2000).Count == 0 && !turrets.Any())
                {
                    var insecPosition = Player.Position;
                    insecPos = insecPosition;
                    if (ToDraw) Render.Circle.DrawCircle(target.Position.Extend(Player.Position, -200), 70, Color.Red);
                }
            }
        }
        private static void OnMenuLoad()
        {
            Menu = new Menu("Hoola Flsec", "hoolaflsec", true);

            var FlashInSec = new Menu("FlashInSec", "FlashInSec");
            FlashInSec.AddItem(new MenuItem("ToAlly", "Flash Insec On (Toggle)").SetValue(new KeyBind('G', KeyBindType.Toggle)));
            FlashInSec.AddItem(new MenuItem("ToDraw", "Draw Flash Position").SetValue(true));
            FlashInSec.AddItem(new MenuItem("ToWhere", "Priorize to (On = Tower, Off = Ally)").SetValue(new KeyBind('T', KeyBindType.Toggle)));
            Menu.AddSubMenu(FlashInSec);

            Menu.AddToMainMenu();
        }


    }
}