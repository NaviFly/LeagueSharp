using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Fresh_Britzcrank
{
    class Program
    {
        public const string ChampName = "Blitzcrank";
        public static Obj_AI_Hero Player;
        public static Spell _Q, _W, _E, _R;
        public static HpBarIndicator Indicator = new HpBarIndicator();

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
                if (Player.ChampionName != ChampName)
                {
                    Game.PrintChat("No Support Champion");
                    return;
                }

                _Q = new Spell(SpellSlot.Q, 950f);
                _Q.SetSkillshot(0.25f, 70f, 1800f, true, SkillshotType.SkillshotLine);
                _W = new Spell(SpellSlot.W, 700f);
                _E = new Spell(SpellSlot.E, 150f);
                _R = new Spell(SpellSlot.R, 540f);

                MainMenu.Menu();
                Game.OnUpdate += OnGameUpdate;
                Drawing.OnDraw += OnDraw.Drawing_OnDraw;
                Drawing.OnEndScene += OnEndScene.Drawing_OnEndScene;
                AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
                Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
                Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshBlitzcrank is not working. plz send message by KorFresh (Code 3)");
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            try
            {
                if (Player.IsDead) return;
                var QTarget = TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Magical);
                var WTarget = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Physical);
                var RTarget = TargetSelector.GetTarget(_R.Range, TargetSelector.DamageType.Magical);

                if (QTarget == null) return;    // auto grab
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
                {
                    if (enemy.Team != Player.Team && QTarget != null
                        && MainMenu._MainMenu.Item("GrabSelect" + enemy.ChampionName).GetValue<StringList>().SelectedIndex == 2 && _Q.IsReady()
                        && QTarget.ChampionName == enemy.ChampionName)
                    {
                        _Q.CastIfHitchanceEquals(QTarget, Hitchance("CUseQ_Hit"), true);
                        //Console.Write(MainMenu._MainMenu.Item("GrabSelect" + enemy.ChampionName).GetValue<StringList>().SelectedIndex+"\n");   0~2
                    }
                }

                if (MainMenu._MainMenu.Item("CKey").GetValue<KeyBind>().Active) // Combo
                {
                    if (MainMenu._MainMenu.Item("CUse_Q").GetValue<bool>() && _Q.IsReady() && QTarget != null
                        && MainMenu._MainMenu.Item("GrabSelect" + QTarget.ChampionName).GetValue<StringList>().SelectedIndex != 1)
                    {
                        _Q.CastIfHitchanceEquals(QTarget, Hitchance("CUseQ_Hit"), true);
                    }
                    if (MainMenu._MainMenu.Item("CUse_W").GetValue<bool>() && _W.IsReady() && WTarget != null)
                        _W.Cast(Player, true);
                    if (MainMenu._MainMenu.Item("CUse_E").GetValue<bool>() && _E.IsReady() && QTarget.Distance(Player.ServerPosition) < 230)
                        _E.Cast(Player);
                    if (MainMenu._MainMenu.Item("CUse_R").GetValue<bool>() && _R.IsReady() && RTarget != null)
                        _R.Cast();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshBlitzcrank is not working. plz send message by KorFresh (Code 4)");
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
                Game.PrintChat("FreshBlitzcrank is not working. plz send message by KorFresh (Code 6)");
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
                Game.PrintChat("FreshBlitzcrank is not working. plz send message by KorFresh (Code 7)");
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