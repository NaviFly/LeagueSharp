using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Fresh_Thresh
{
    class Program
    {
        public const string ChampName = "Thresh";
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
                Game.PrintChat("FreshThresh is not working. plz send message by KorFresh (Code 2)");
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

                _Q = new Spell(SpellSlot.Q, 1100f);
                _Q.SetSkillshot(0.5f, 65f, 1900f, true, SkillshotType.SkillshotLine);
                _W = new Spell(SpellSlot.W, 950f);
                _E = new Spell(SpellSlot.E, 480f);
                _R = new Spell(SpellSlot.R, 450f);

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
                Game.PrintChat("FreshThresh is not working. plz send message by KorFresh (Code 3)");
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            try
            {
                if (Player.IsDead) return;
                // Set Target
                var QTarget = TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Magical);
                var WTarget = ObjectManager.Get<Obj_AI_Hero>().OrderByDescending(x => x.Health).FirstOrDefault(x => x.IsAlly && !x.IsDead && !x.IsMe && x.HealthPercent < MainMenu._MainMenu.Item("LenternHP").GetValue<Slider>().Value && x.Distance(Player) < 1600);
                var ETarget = TargetSelector.GetTarget(_E.Range, TargetSelector.DamageType.Magical);
                var RTarget = TargetSelector.GetTarget(_R.Range, TargetSelector.DamageType.Magical);

                if (WTarget != null && MainMenu._MainMenu.Item("LenternEnable").GetValue<bool>() && _W.IsReady())
                {
                    if (WTarget.Distance(Player) > 950)
                    {
                        _W.Cast(Player.ServerPosition.Extend(WTarget.ServerPosition, 950), true);
                    }
                    else
                    {
                        _W.Cast(WTarget.ServerPosition, true);
                    }
                }

                if (QTarget != null)
                {
                    foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
                    {
                        if (enemy.Team != Player.Team && QTarget != null
                            && MainMenu._MainMenu.Item("GrabSelect" + enemy.ChampionName).GetValue<StringList>().SelectedIndex == 2 && _Q.IsReady()
                            && QTarget.ChampionName == enemy.ChampionName && Player.Spellbook.GetSpell(SpellSlot.Q).Name == "ThreshQ")
                        {
                            _Q.CastIfHitchanceEquals(QTarget, Hitchance("CUseQ_Hit"), true);
                        }
                    }
                }

                if (MainMenu._MainMenu.Item("CKey").GetValue<KeyBind>().Active) // Combo
                {
                    if (MainMenu._MainMenu.Item("CUse_E").GetValue<bool>() && ETarget != null && _E.IsReady())
                    {
                        var EPosition = ETarget.ServerPosition.Extend(Player.ServerPosition, 500);
                        _E.Cast(EPosition, true);
                    }
                    if (MainMenu._MainMenu.Item("CUse_Q").GetValue<bool>() && QTarget != null && _Q.IsReady() && MainMenu._MainMenu.Item("GrabSelect" + QTarget.ChampionName).GetValue<StringList>().SelectedIndex != 1
                        && Player.Spellbook.GetSpell(SpellSlot.Q).Name == "ThreshQ")
                    {
                        _Q.CastIfHitchanceEquals(QTarget, Hitchance("CUseQ_Hit"), true);
                    }
                    if (MainMenu._MainMenu.Item("CUse_R").GetValue<bool>() && RTarget != null && _R.IsReady())
                    {
                        _R.Cast(true);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshThresh is not working. plz send message by KorFresh (Code 4)");
            }
        }

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            try
            {
                if (MainMenu._MainMenu.Item("GapE").GetValue<bool>() && _E.IsReady())
                {
                    if (gapcloser.Sender.IsEnemy && gapcloser.Sender.Distance(Player) < 480)
                    {
                        _E.Cast(gapcloser.Sender.ServerPosition, true);
                    }
                }
                if (MainMenu._MainMenu.Item("GapQ").GetValue<bool>() && _Q.IsReady())
                {
                    if (gapcloser.Sender.IsEnemy && gapcloser.Sender.Distance(Player) < 1100 && Player.Spellbook.GetSpell(SpellSlot.Q).Name == "ThreshQ")
                    {
                        _Q.CastIfHitchanceEquals(gapcloser.Sender, Hitchance("CUseQ_Hit"), true);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshThresh is not working. plz send message by KorFresh (Code 6)");
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
                Game.PrintChat("FreshThresh is not working. plz send message by KorFresh (Code 7)");
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
            if (MainMenu._MainMenu.Item("InterE").GetValue<bool>() && _E.IsReady())
            {
                if (sender.IsEnemy && sender.Distance(Player) < 480)
                {
                    _E.Cast(sender.ServerPosition, true);
                }
            }
            if (MainMenu._MainMenu.Item("InterQ").GetValue<bool>() && _Q.IsReady())
            {
                if (sender.IsEnemy && sender.Distance(Player) < 1100 && Player.Spellbook.GetSpell(SpellSlot.Q).Name == "ThreshQ")
                {
                    _Q.CastIfHitchanceEquals(sender, Hitchance("CUseQ_Hit"), true);
                }
            }
        }
    }
}