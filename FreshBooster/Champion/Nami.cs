﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
using static FreshBooster.FreshCommon;

namespace FreshBooster.Champion
{
    class Nami
    {
        // Default Setting
        public static int ErrorTime;
        public const string ChampName = "Nami";   // Edit
        public static Obj_AI_Hero Player;
        public static Spell _Q, _W, _E, _R;
        public static HpBarIndicator Indicator = new HpBarIndicator();
        // Default Setting

        private void SkillSet()
        {
            try
            {
                _Q = new Spell(SpellSlot.Q, 875);
                _Q.SetSkillshot(1.25f, 150, float.MaxValue, false, SkillshotType.SkillshotCircle);
                _W = new Spell(SpellSlot.W, 725);
                _E = new Spell(SpellSlot.E, 800);
                _R = new Spell(SpellSlot.R, 2200);
            }
            catch (Exception)
            {
                if (NowTime() > ErrorTime)
                {
                    Game.PrintChat(ChampName + " in FreshBooster isn't Load. Error Code 01");
                    ErrorTime = TickCount(10000);
                }
            }
        }
        private void Menu()
        {
            try
            {
                var Combo = new Menu("Combo", "Combo");
                {
                    Combo.AddItem(new MenuItem("Nami_CUse_Q", "Use Q").SetValue(true));
                    Combo.AddItem(new MenuItem("Nami_CUse_W", "Use W").SetValue(true));
                    Combo.AddItem(new MenuItem("Nami_CUse_E", "Use E").SetValue(true));
                    Combo.AddItem(new MenuItem("Nami_CUse_R", "Use R").SetValue(true));
                    Combo.SubMenu("Auto Use R").AddItem(new MenuItem("Nami_CUse_MinR", "Auto R Min Enemy is").SetValue(new Slider(3, 0, 5)));
                    Combo.SubMenu("Auto Use R").AddItem(new MenuItem("Nami_CUse_MinR1", "Enable").SetValue(true));
                    Combo.AddItem(new MenuItem("CKey", "Combo Key").SetValue(new KeyBind(32, KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Combo);

                var Harass = new Menu("Harass", "Harass");
                {
                    Harass.AddItem(new MenuItem("Nami_HUse_Q", "Use Q").SetValue(true));
                    Harass.AddItem(new MenuItem("Nami_HUse_W", "Use W").SetValue(true));
                    Harass.AddItem(new MenuItem("Nami_HUse_E", "Use E").SetValue(true));
                    Harass.AddItem(new MenuItem("Nami_Auto_HEnable", "Auto Harass").SetValue(true));
                    Harass.AddItem(new MenuItem("Nami_HMana", "Min. Mana %").SetValue(new Slider(50, 0, 100)));
                    Harass.AddItem(new MenuItem("HKey", "Harass Key").SetValue(new KeyBind('C', KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Harass);

                var KillSteal = new Menu("KillSteal", "KillSteal");
                {
                    KillSteal.AddItem(new MenuItem("Nami_KUse_Q", "Use Q").SetValue(true));
                    KillSteal.AddItem(new MenuItem("Nami_KUse_W", "Use W").SetValue(true));
                }
                _MainMenu.AddSubMenu(KillSteal);

                var Misc = new Menu("Misc", "Misc");
                {
                    Misc.SubMenu("Heal W").AddItem(new MenuItem("Nami_HealWMin", "Min HP %").SetValue(new Slider(20, 0, 100)));
                    Misc.SubMenu("Heal W").AddItem(new MenuItem("Nami_HealWMinEnable", "Enable").SetValue(true));
                    Misc.SubMenu("Anti-Gab").AddItem(new MenuItem("Nami_AntiQ", "Use Q").SetValue(true));
                    Misc.SubMenu("Anti-Gab").AddItem(new MenuItem("Nami_AntiR", "Use R").SetValue(true));
                    Misc.AddItem(new MenuItem("Nami_Flee", "Flee").SetValue(new KeyBind('G', KeyBindType.Press)));
                }
                _MainMenu.AddSubMenu(Misc);

                var Draw = new Menu("Draw", "Draw");
                {
                    Draw.AddItem(new MenuItem("Nami_QRange", "Q Range").SetValue(false));
                    Draw.AddItem(new MenuItem("Nami_WRange", "W Range").SetValue(false));
                    Draw.AddItem(new MenuItem("Nami_ERange", "E Range").SetValue(false));
                    Draw.AddItem(new MenuItem("Nami_RRange", "R Range").SetValue(false));
                    Draw.AddItem(new MenuItem("Nami_Indicator", "Draw Damage Indicator").SetValue(false));
                }
                _MainMenu.AddSubMenu(Draw);
            }
            catch (Exception)
            {
                if (NowTime() > ErrorTime)
                {
                    Game.PrintChat(ChampName + " in FreshBooster isn't Load. Error Code 02");
                    ErrorTime = TickCount(10000);
                }
            }
        }
        public static void Drawing_OnDraw(EventArgs args)
        {
            try
            {
                if (Player.IsDead) return;
                var Rint = (_R.Level * 1000) + 3000;
                if (_MainMenu.Item("Nami_QRange").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _Q.Range, Color.White, 1);
                if (_MainMenu.Item("Nami_WRange").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _W.Range, Color.White, 1);
                if (_MainMenu.Item("Nami_ERange").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _E.Range, Color.White, 1);
                if (_MainMenu.Item("Nami_RRange").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _R.Range, Color.White, 1);
                var QTarget = TargetSelector.GetTarget(1000, TargetSelector.DamageType.Magical);
                if (QTarget != null)
                    Drawing.DrawCircle(QTarget.Position, 100, Color.Green);
            }
            catch (Exception)
            {
                if (NowTime() > ErrorTime)
                {
                    Game.PrintChat(ChampName + " in FreshBooster isn't Load. Error Code 03");
                    ErrorTime = TickCount(10000);
                }
            }
        }

        static float getComboDamage(Obj_AI_Base enemy)
        {
            try
            {
                if (enemy != null)
                {
                    float damage = 0;

                    if (_Q.IsReady())
                        damage += _Q.GetDamage(enemy);
                    if (_W.IsReady())
                        damage += _W.GetDamage(enemy);
                    if (_R.IsReady())
                        damage += _R.GetDamage(enemy);
                    if (!Player.IsWindingUp)
                        damage += (float)Player.GetAutoAttackDamage(enemy, true);
                    return damage;
                }
                return 0;
            }
            catch (Exception)
            {
                if (NowTime() > ErrorTime)
                {
                    Game.PrintChat(ChampName + " in FreshBooster isn't Load. Error Code 04");
                    ErrorTime = TickCount(10000);
                }
                return 0;
            }
        }
        public static void Drawing_OnEndScene(EventArgs args)
        {
            try
            {
                if (Player.IsDead) return;
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget() && !ene.IsZombie))
                {
                    if (_MainMenu.Item("Nami_Indicator").GetValue<bool>())
                    {
                        Indicator.unit = enemy;
                        Indicator.drawDmg(getComboDamage(enemy), new ColorBGRA(255, 204, 0, 160));
                    }
                }
            }
            catch (Exception)
            {
                if (NowTime() > ErrorTime)
                {
                    Game.PrintChat(ChampName + " in FreshBooster isn't Load. Error Code 05");
                    ErrorTime = TickCount(10000);
                }
            }
        }

        // OnLoad
        public Nami()
        {
            Player = ObjectManager.Player;
            SkillSet();
            Menu();
            Game.OnUpdate += OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            Orbwalking.BeforeAttack += Orbwalking_BeforeAttack;
            Obj_AI_Base.OnIssueOrder += Obj_AI_Base_OnIssueOrder;
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
        }

        private static void OnGameUpdate(EventArgs args)
        {
            try
            {
                if (Player.IsDead) return;
                var QTarget = TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Magical);
                var WTarget = TargetSelector.GetTarget(_W.Range, TargetSelector.DamageType.Magical);
                var RTarget = TargetSelector.GetTarget(_R.Range, TargetSelector.DamageType.Magical);
                // Flee
                if (_MainMenu.Item("Nami_Flee").GetValue<KeyBind>().Active)
                {
                    MovingPlayer(Game.CursorPos);
                    if (_E.IsReady())
                        _E.CastOnUnit(Player, true);
                    if (_Q.IsReady() && QTarget != null && _Q.GetPrediction(QTarget).Hitchance >= HitChance.Low)
                        _Q.CastIfHitchanceEquals(QTarget, _Q.GetPrediction(QTarget).Hitchance, true);
                }

                // Auto W Heal
                if (_MainMenu.Item("Nami_HealWMinEnable").GetValue<bool>())
                {
                    var Ally = HeroManager.Allies.FirstOrDefault(f => f.Position.Distance(Player.Position) <= _W.Range && !f.IsDead
                    && f.HealthPercent <= _MainMenu.Item("Nami_HealWMin").GetValue<Slider>().Value
                    && !f.InFountain(Utility.FountainType.OwnFountain));
                    if (Ally != null && !Ally.IsRecalling())
                    {
                        _W.CastOnUnit(Ally, true);
                        //Game.PrintChat("local 1");
                    }
                        
                }

                //KillSteal
                if (_MainMenu.Item("Nami_KUse_W").GetValue<bool>())
                    if (_W.IsReady() && WTarget != null)
                        if (WTarget.Health < _W.GetDamage(WTarget))
                            _W.CastOnUnit(WTarget, true);
                if (_MainMenu.Item("Nami_KUse_Q").GetValue<bool>())
                    if (_Q.IsReady() && QTarget != null)
                        if (QTarget.Health < _Q.GetDamage(QTarget))
                            if (_Q.GetPrediction(QTarget).Hitchance >= HitChance.Medium)
                                _Q.CastIfHitchanceEquals(QTarget, HitChance.Medium, true);
                //Auto R 
                if (_MainMenu.Item("Nami_CUse_MinR1").GetValue<bool>() && _R.IsReady())
                {
                    var Enemy = HeroManager.Enemies.OrderBy(f => f.Distance(Player.Position));
                    if (Enemy != null)
                    {
                        foreach (var item in Enemy)
                        {
                            if (item.Position.Distance(Player.Position) < _R.Range && !item.IsDead)
                            {
                                var RRange = new Geometry.Polygon.Rectangle(Player.Position, Player.Position.Extend(item.Position, +1200), 250);
                                var Count = HeroManager.Enemies.Where(f => f.Distance(Player.Position) <= 1500).Count(f => RRange.IsInside(f.Position));
                                if (Count >= _MainMenu.Item("Nami_CUse_MinR").GetValue<Slider>().Value)
                                {
                                    _R.Cast(item.Position, true);
                                    return;
                                }
                            }
                        }
                    }
                }

                // Combo
                if (_MainMenu.Item("CKey").GetValue<KeyBind>().Active)
                {
                    if (_MainMenu.Item("Nami_CUse_Q").GetValue<bool>())
                        if (QTarget != null)
                            if (_Q.IsReady())
                                if (_Q.GetPrediction(QTarget).Hitchance >= HitChance.Medium)
                                {
                                    _Q.CastIfHitchanceEquals(QTarget, _Q.GetPrediction(QTarget).Hitchance, true);
                                    return;
                                }
                    if (_MainMenu.Item("Nami_CUse_W").GetValue<bool>() && _W.IsReady())
                    {
                        // W to Ally 
                        foreach (var item in HeroManager.Allies.OrderByDescending(f => f.Health))
                        {
                            if (item.Position.Distance(Player.Position) <= _W.Range)
                                if ((HeroManager.Enemies.FirstOrDefault(f => f.Position.Distance(item.Position) <= _W.Range - 50 && !f.IsDead) != null))
                                {
                                    _W.CastOnUnit(item, true);
                                    //Game.PrintChat("local 2");
                                    return;
                                }
                        }
                        if (WTarget != null && WTarget.Position.Distance(Player.Position) <= _W.Range)
                        {
                            _W.CastOnUnit(WTarget, true);
                            //Game.PrintChat("local 3");
                        }                            
                    }
                    if (_MainMenu.Item("Nami_CUse_R").GetValue<bool>() && _R.IsReady() && RTarget != null)
                        if (RTarget.Distance(Player.Position) <= 1300)
                            if (_R.GetPrediction(RTarget).Hitchance >= HitChance.Medium)
                                _R.CastIfHitchanceEquals(RTarget, _R.GetPrediction(RTarget).Hitchance);
                }

                // Harass
                if ((_MainMenu.Item("HKey").GetValue<KeyBind>().Active || _MainMenu.Item("Nami_Auto_HEnable").GetValue<bool>())
                    && _MainMenu.Item("Nami_HMana").GetValue<Slider>().Value < Player.ManaPercent)
                {
                    if (_MainMenu.Item("Nami_HUse_Q").GetValue<bool>())
                        if (QTarget != null)
                            if (_Q.IsReady())
                                if (_Q.GetPrediction(QTarget).Hitchance >= HitChance.Medium)
                                {
                                    _Q.CastIfHitchanceEquals(QTarget, _Q.GetPrediction(QTarget).Hitchance, true);
                                    return;
                                }
                    if (_MainMenu.Item("Nami_HUse_W").GetValue<bool>() && _W.IsReady())
                    {
                        // W to Ally 
                        foreach (var item in HeroManager.Allies.OrderByDescending(f => f.Health))
                        {
                            if (item.Position.Distance(Player.Position) <= _W.Range && !item.IsDead)
                                if ((HeroManager.Enemies.FirstOrDefault(f => f.Position.Distance(item.Position) <= _W.Range - 50 && !f.IsDead) != null) && !item.Name.ToLower().Contains("name"))
                                {
                                    _W.CastOnUnit(item, true);
                                    //Game.PrintChat("local 4");
                                    return;
                                }
                        }
                        if (WTarget != null && WTarget.Position.Distance(Player.Position) <= _W.Range && WTarget.IsChampion())
                        {
                            _W.CastOnUnit(WTarget, true);
                            //Game.PrintChat("local 5");
                        }                            
                    }
                }
            }
            catch (Exception)
            {
                if (NowTime() > ErrorTime)
                {
                    Game.PrintChat(ChampName + " in FreshBooster isn't Load. Error Code 06");
                    ErrorTime = TickCount(10000);
                }
            }
        }
        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            try
            {
                if (_MainMenu.Item("Nami_AntiQ").GetValue<bool>())
                    if (_Q.IsReady() && gapcloser.Sender.Position.Distance(Player.Position) < _Q.Range)
                        _Q.CastIfHitchanceEquals(gapcloser.Sender, _Q.GetPrediction(gapcloser.Sender).Hitchance, true);
                if (_MainMenu.Item("Nami_AntiR").GetValue<bool>())
                    if (_R.IsReady() && gapcloser.Sender.Position.Distance(Player.Position) < _R.Range)
                        _R.CastIfHitchanceEquals(gapcloser.Sender, HitChance.Medium, true);
            }
            catch (Exception)
            {
                if (NowTime() > ErrorTime)
                {
                    Game.PrintChat(ChampName + " in FreshBooster isn't Load. Error Code 07");
                    ErrorTime = TickCount(10000);
                }
            }
        }
        private static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (args.Target is Obj_AI_Hero)
                {
                    // Combo E
                    if (_MainMenu.Item("CKey").GetValue<KeyBind>().Active)
                        if (_MainMenu.Item("Nami_CUse_E").GetValue<bool>())
                            if (_E.IsReady())
                                if (sender.IsAlly)
                                    if (sender.Position.Distance(Player.Position) < _E.Range)
                                        if (args.SData.Name.ToLower().Contains("attack"))
                                            _E.CastOnUnit(sender);
                    // Harass E
                    if (_MainMenu.Item("HKey").GetValue<KeyBind>().Active || _MainMenu.Item("Nami_Auto_HEnable").GetValue<bool>())
                        if (_MainMenu.Item("Nami_HUse_E").GetValue<bool>())
                            if (_E.IsReady())
                                if (sender.IsAlly)
                                    if (sender.Position.Distance(Player.Position) < _E.Range)
                                        if (args.SData.Name.ToLower().Contains("attack"))
                                            _E.CastOnUnit(sender);
                }

            }
            catch (Exception)
            {
                if (NowTime() > ErrorTime)
                {
                    Game.PrintChat(ChampName + " in FreshBooster isn't Load. Error Code 08");
                    ErrorTime = TickCount(10000);
                }
            }

        }
        private static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            try
            {

            }
            catch (Exception)
            {
                if (NowTime() > ErrorTime)
                {
                    Game.PrintChat(ChampName + " in FreshBooster isn't Load. Error Code 09");
                    ErrorTime = TickCount(10000);
                }
            }

        }
        public static void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            try
            {

            }
            catch (Exception)
            {
                if (NowTime() > ErrorTime)
                {
                    Game.PrintChat(ChampName + " in FreshBooster isn't Load. Error Code 10");
                    ErrorTime = TickCount(10000);
                }
            }

        }
        private static void Obj_AI_Base_OnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            try
            {

            }
            catch (Exception)
            {
                if (NowTime() > ErrorTime)
                {
                    Game.PrintChat(ChampName + " in FreshBooster isn't Load. Error Code 11");
                    ErrorTime = TickCount(10000);
                }
            }

        }
        private static void GameObject_OnCreate(GameObject obj, EventArgs args)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshPoppy is not working. plz send message by KorFresh (Code 13)");
            }
        }

        private static void GameObject_OnDelete(GameObject obj, EventArgs args)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshPoppy is not working. plz send message by KorFresh (Code 14)");
            }
        }
    }
}
