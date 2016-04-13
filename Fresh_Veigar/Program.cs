using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Fresh_Veigar
{
    class Program
    {
        public const string ChampName = "Veigar";
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
                Game.PrintChat("FreshVeigar is not working. plz send message by KorFresh (Code 2)");
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

                _Q = new Spell(SpellSlot.Q, 900);
                _Q.SetSkillshot(0.25f, 70f, 2000f, false, SkillshotType.SkillshotLine);
                _W = new Spell(SpellSlot.W, 900);
                _W.SetSkillshot(0.5f, 200f, float.MaxValue, false, SkillshotType.SkillshotCircle);
                _E = new Spell(SpellSlot.E, 1150); // true range 750, circle 400
                _E.SetSkillshot(1.2f, 80f, float.MaxValue, false, SkillshotType.SkillshotCircle);
                _R = new Spell(SpellSlot.R, 615);

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
                Game.PrintChat("FreshVeigar is not working. plz send message by KorFresh (Code 3)");
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            try
            {
                if (Player.IsDead) return;
                var QTarget = TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Magical);
                var WTarget = TargetSelector.GetTarget(_W.Range, TargetSelector.DamageType.Magical);
                var ETarget = TargetSelector.GetTarget(_E.Range, TargetSelector.DamageType.Magical);
                var RTarget = TargetSelector.GetTarget(_E.Range, TargetSelector.DamageType.Magical);

                var KTarget = ObjectManager.Get<Obj_AI_Hero>().OrderBy(x => x.Health).FirstOrDefault(x => x.IsEnemy && Player.Distance(x) < 900);
                //KillSteal
                if (KTarget != null && !KTarget.IsDead && KTarget.IsEnemy && KTarget.Health > 0)
                {
                    if (MainMenu._MainMenu.Item("KseQ").GetValue<bool>() && _Q.IsReady() && KTarget.Health < _Q.GetDamage(KTarget) && KTarget.Distance(Player) <= _Q.Range)
                    {
                        _Q.CastIfHitchanceEquals(KTarget, Hitchance("CUseQ_Hit"), true);
                        return;
                    }
                    if (MainMenu._MainMenu.Item("KseR").GetValue<bool>() && _R.IsReady() && KTarget.Health < _R.GetDamage(KTarget) && KTarget.Distance(Player) <= _R.Range)
                    {
                        _R.Cast(KTarget, true);
                        return;
                    }

                    if (MainMenu._MainMenu.Item("KseW").GetValue<bool>() && _W.IsReady() && !KTarget.CanMove && KTarget.Health < _W.GetDamage(KTarget) && KTarget.Distance(Player) <= _W.Range)
                    {
                        _W.CastIfHitchanceEquals(KTarget, HitChance.VeryHigh, true);
                        return;
                    }
                }

                //Combo
                if (MainMenu._MainMenu.Item("CKey").GetValue<KeyBind>().Active)
                {
                    if (MainMenu._MainMenu.Item("CUseE").GetValue<bool>() && ETarget != null && _E.IsReady())
                    {
                        SpellUseE(ETarget);
                    }
                    if (MainMenu._MainMenu.Item("CUseW").GetValue<bool>() && WTarget != null && _W.IsReady())
                    {
                        _W.CastIfHitchanceEquals(WTarget, HitChance.VeryHigh, true);
                        return;
                    }
                    if (MainMenu._MainMenu.Item("CUseQ").GetValue<bool>() && QTarget != null && _Q.IsReady())
                    {
                        _Q.CastIfHitchanceEquals(QTarget, Hitchance("CUseQ_Hit"), true);
                        return;
                    }
                    if (MainMenu._MainMenu.Item("CUseR").GetValue<bool>() && RTarget != null && _R.IsReady() && !MainMenu._MainMenu.Item("CUseR_Select").GetValue<bool>())
                    {
                        _R.Cast(RTarget, true);
                        return;
                    }
                }

                //Harass
                if ((MainMenu._MainMenu.Item("HKey").GetValue<KeyBind>().Active || MainMenu._MainMenu.Item("AutoHUseQ").GetValue<KeyBind>().Active)
                    && MainMenu._MainMenu.Item("HManarate").GetValue<Slider>().Value < Player.ManaPercent)
                {
                    if (MainMenu._MainMenu.Item("HUseE").GetValue<bool>() && ETarget != null && _E.IsReady() && !ETarget.CanMove)
                    {
                        SpellUseE(ETarget);
                    }
                    if (MainMenu._MainMenu.Item("HUseW").GetValue<bool>() && WTarget != null && _W.IsReady() && !WTarget.CanMove)
                    {
                        _W.CastIfHitchanceEquals(WTarget, HitChance.VeryHigh, true);
                        return;
                    }
                    if (MainMenu._MainMenu.Item("HUseQ").GetValue<bool>() && QTarget != null && _Q.IsReady())
                    {
                        _Q.CastIfHitchanceEquals(QTarget, Hitchance("CUseQ_Hit"), true);
                        return;
                    }
                    if (MainMenu._MainMenu.Item("HUseR").GetValue<bool>() && RTarget != null && _R.IsReady())
                    {
                        _R.Cast(RTarget, true);
                        return;
                    }
                }

                //LaneClear
                if (MainMenu._MainMenu.Item("LKey").GetValue<KeyBind>().Active && MainMenu._MainMenu.Item("LManarate").GetValue<Slider>().Value < Player.ManaPercent)
                {
                    var MinionTarget = MinionManager.GetMinions(900, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.Health);
                    foreach (var item in MinionTarget)
                    {
                        if (MainMenu._MainMenu.Item("LUseQ").GetValue<bool>())
                        {
                            if (MainMenu._MainMenu.Item("LUseQSet").GetValue<bool>() && item.Health < _Q.GetDamage(item))
                            {
                                _Q.CastIfHitchanceEquals(item, HitChance.Low, true);
                                return;
                            }
                            if (!MainMenu._MainMenu.Item("LUseQSet").GetValue<bool>())
                            {
                                _Q.CastIfHitchanceEquals(item, HitChance.Low, true);
                                return;
                            }
                        }
                    }
                }

                //JungleClear
                if (MainMenu._MainMenu.Item("JKey").GetValue<KeyBind>().Active && MainMenu._MainMenu.Item("JManarate").GetValue<Slider>().Value < Player.ManaPercent)
                {
                    var MinionTarget = MinionManager.GetMinions(900, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.Health);
                    foreach (var item in MinionTarget)
                    {
                        if (MainMenu._MainMenu.Item("JUseQ").GetValue<bool>())
                        {
                            if (MainMenu._MainMenu.Item("JUseQSet").GetValue<bool>() && item.Health < _Q.GetDamage(item))
                            {
                                _Q.CastIfHitchanceEquals(item, HitChance.Low, true);
                                return;
                            }
                            if (!MainMenu._MainMenu.Item("JUseQSet").GetValue<bool>())
                            {
                                _Q.CastIfHitchanceEquals(item, HitChance.Low, true);
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Console.Write(e);
                //Game.PrintChat("FreshVeigar is not working. plz send message by KorFresh (Code 4)");
            }
        }

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            try
            {
                if (MainMenu._MainMenu.Item("Anti-GapCloser").GetValue<bool>() && _E.IsReady())
                {
                    var EPosition = gapcloser.End.Extend(Player.ServerPosition, 400);
                    _E.Cast(EPosition, true);
                    return;
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshVeigar is not working. plz send message by KorFresh (Code 6)");
            }
        }

        private static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (sender.IsMe)
                {

                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshVeigar is not working. plz send message by KorFresh (Code 7)");
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
            if (MainMenu._MainMenu.Item("Interrupt").GetValue<bool>() && sender.IsEnemy && _E.IsReady() && sender.Distance(Player) < 1150)
            {
                var EPosition = sender.ServerPosition.Extend(Player.Position, 400);
                _E.Cast(EPosition, true);
                return;
            }
        }

        private static int TickCount(int time)
        {
            return Environment.TickCount + time;
        }

        public static void SpellUseE(Obj_AI_Hero target)
        {
            if (!target.CanMove && !target.IsMoving)
            {
                var EPosition = target.ServerPosition.Extend(Player.Position, 400);
                _E.Cast(EPosition, true);
                return;
            }
            if (target.Distance(Player) > 750)
            {
                var EPosition = target.ServerPosition.Extend(Player.Position, 200);
                _E.Cast(EPosition, true);
                return;
            }
            else
            {
                var EPosition = target.ServerPosition;
                _E.Cast(EPosition, true);
                return;
            }
        }
    }
}