using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Fresh_Janna
{
    class Program
    {
        public const string ChampName = "Janna";
        public static Obj_AI_Hero Player;
        public static Spell _Q, _W, _E, _R;
        public static HpBarIndicator Indicator = new HpBarIndicator();
        public static bool QSpell = false;
        public static int SpellTime = 0, RTime;
        public static Obj_AI_Hero FleeQ;
        public static Vector3 QPosition;

        static void Main(string[] args)
        {
            try
            {
                CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshJanna is not working. plz send message by KorFresh (Code 2)");
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

                _Q = new Spell(SpellSlot.Q, 850f);
                _Q.SetSkillshot(0.25f, 120f, 900f, false, SkillshotType.SkillshotLine);
                _W = new Spell(SpellSlot.W, 600);
                _E = new Spell(SpellSlot.E, 800f);
                _R = new Spell(SpellSlot.R, 875f);

                MainMenu.Menu();
                Game.OnUpdate += OnGameUpdate;
                Drawing.OnDraw += OnDraw.Drawing_OnDraw;
                Drawing.OnEndScene += OnEndScene.Drawing_OnEndScene;
                AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
                Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
                Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
                Obj_AI_Base.OnIssueOrder += Obj_AI_Base_OnIssueOrder;
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshJanna is not working. plz send message by KorFresh (Code 3)");
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            try
            {
                if (Player.IsDead) return;
                // Set Target
                var QTarget = TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Magical);
                var WTarget = TargetSelector.GetTarget(_W.Range, TargetSelector.DamageType.Magical);

                if (QSpell && _Q.IsCharging)
                {
                    _Q.Cast(true);
                    QSpell = false;
                }
                if (MainMenu._MainMenu.Item("AutoREnable").GetValue<bool>())
                {
                    var AutoR = ObjectManager.Get<Obj_AI_Hero>().OrderByDescending(x => x.Health).FirstOrDefault(x => x.HealthPercent < MainMenu._MainMenu.Item("AutoRHP").GetValue<Slider>().Value && x.Distance(Player.ServerPosition) < _R.Range && !x.IsDead && x.IsAlly);
                    if (AutoR != null && _R.IsReady() && !Player.IsRecalling())
                    {
                        _R.Cast(true);
                        RTime = TickCount(1000);
                    }
                }
                if (MainMenu._MainMenu.Item("Flee").GetValue<KeyBind>().Active) // Flee
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                    if (QTarget != null && _Q.IsReady())
                    {
                        if (!_Q.IsCharging)
                        {
                            var prediction = _Q.GetPrediction(QTarget);
                            if (prediction.Hitchance <= HitChance.Medium)
                            {
                                _Q.Cast(QTarget, true);
                                FleeQ = QTarget;
                                QPosition = Player.ServerPosition;
                            }
                        }
                        if (_Q.IsCharging && QPosition.Distance(FleeQ.ServerPosition) < 100)
                        {
                            _Q.Cast(true);
                        }
                    }
                    if (WTarget != null && _W.IsReady() && WTarget.Distance(Player.ServerPosition) < 400)
                        _W.Cast(WTarget, true);
                }

                if (MainMenu._MainMenu.Item("CKey").GetValue<KeyBind>().Active) // Combo
                {
                    if (QTarget != null && MainMenu._MainMenu.Item("CUse_Q").GetValue<bool>() && _Q.IsReady())
                    {
                        var Prediction = _Q.GetPrediction(QTarget);
                        if (!_Q.IsCharging && Prediction.Hitchance >= HitChance.Medium)
                        {
                            _Q.Cast(Prediction.CastPosition, true);
                        }
                        if (_Q.IsCharging)
                        {
                            _Q.Cast(true);
                        }
                    }
                    if (WTarget != null && _W.IsReady())
                        _W.Cast(WTarget, true);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshJanna is not working. plz send message by KorFresh (Code 4)");
            }
        }

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            try
            {
                if (MainMenu._MainMenu.Item("GapQ").GetValue<bool>() && _Q.IsReady() && gapcloser.Sender.ServerPosition.Distance(Player.ServerPosition) < 850 && !_Q.IsCharging)
                {
                    _Q.Cast(gapcloser.Sender.ServerPosition, true);
                    QSpell = true;
                    SpellTime = TickCount(1000);
                }

                if (MainMenu._MainMenu.Item("GapR").GetValue<bool>() && _R.IsReady() && gapcloser.Sender.ServerPosition.Distance(Player.ServerPosition) < 875 && SpellTime < Environment.TickCount && !Player.IsRecalling())
                {
                    _R.Cast(true);
                    RTime = TickCount(1000);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshJanna is not working. plz send message by KorFresh (Code 6)");
            }
        }

        private static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (sender is Obj_AI_Hero && sender.IsEnemy && args.Target.IsAlly && !args.Target.IsDead && _E.IsReady() && args.Target.Position.Distance(Player.ServerPosition) <= _E.Range
                    && args.Target.Type != GameObjectType.obj_AI_Minion
                    && MainMenu._MainMenu.Item("AutoE").GetValue<bool>())
                {
                    //Console.Write("\n Sender: " + sender.BaseSkinName + " / Spell: " + args.SData.Name + " / Target: " + args.Target);
                    var target = ObjectManager.Get<Obj_AI_Hero>().OrderBy(x => x.ServerPosition.Distance(args.Target.Position)).FirstOrDefault(x => x.ServerPosition.Distance(args.Target.Position) < 5);
                    _E.Cast(target, true);
                }
                if (sender is Obj_AI_Hero && sender.IsAlly && args.Target.IsEnemy && !args.Target.IsDead && _E.IsReady()
                    && args.Target.Type == GameObjectType.obj_AI_Hero
                    && MainMenu._MainMenu.Item("AutoE").GetValue<bool>() && sender.ServerPosition.Distance(Player.ServerPosition) <= _E.Range)
                {
                    var Ally = ObjectManager.Get<Obj_AI_Hero>().OrderBy(x => x.ServerPosition.Distance(Player.ServerPosition)).FirstOrDefault(x => x.IsAlly && !x.IsMe && x.ServerPosition.Distance(Player.ServerPosition) < _E.Range);
                    if (sender.IsMe && Ally != null)
                    {

                    }
                    else
                    {
                        _E.Cast(sender, true);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                //Game.PrintChat("FreshJanna is not working. plz send message by KorFresh (Code 7)");
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
            if (MainMenu._MainMenu.Item("InterQ").GetValue<bool>() && _Q.IsReady() && sender.ServerPosition.Distance(Player.ServerPosition) < 850 && !_Q.IsCharging)
            {
                _Q.Cast(sender.ServerPosition, true);
                QSpell = true;
                SpellTime = TickCount(1000);
            }

            if (MainMenu._MainMenu.Item("InterR").GetValue<bool>() && _R.IsReady() && sender.ServerPosition.Distance(Player.ServerPosition) < 875 && SpellTime < Environment.TickCount && !Player.IsRecalling())
            {
                _R.Cast(true);
                RTime = TickCount(1000);
            }
        }

        private static int TickCount(int time)
        {
            return Environment.TickCount + time;
        }

        private static void Obj_AI_Base_OnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            if (sender.IsMe && RTime > Environment.TickCount)
                args.Process = false;
        }
    }
}