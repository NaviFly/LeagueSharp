using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace FreshPoppy
{
    public class Shield
    {
        public GameObject ShieldMe;
    }

    public class Use_R_Target
    {
        public Obj_AI_Hero Sender;
        public bool Enable = false;
        public string Type;
    }

    class Program
    {
        public const string ChampName = "Poppy";
        public static Obj_AI_Hero Player;
        public static Spell _Q, _W, _E, _R;
        public static HpBarIndicator Indicator = new HpBarIndicator();
        public static Obj_AI_Hero Enemy_Obj;
        public static Shield shield = new Shield();
        public static Use_R_Target Rsender = new Use_R_Target();
        public static BuffInstance PoppyPassive;
        public static bool anti_R = false;

        static void Main(string[] args)
        {
            try
            {
                CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshPoppy is not working. plz send message by KorFresh (Code 2)");
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

                _Q = new Spell(SpellSlot.Q, 430f);
                _Q.SetSkillshot(0.6f, 100f, float.MaxValue, false, SkillshotType.SkillshotLine);
                _W = new Spell(SpellSlot.W);
                _E = new Spell(SpellSlot.E, 590f);
                _R = new Spell(SpellSlot.R, 1230f);
                _R.SetSkillshot(0.6f, 120f, float.MaxValue, false, SkillshotType.SkillshotLine);
                _R.SetCharged("PoppyR", "PoppyR", 470, 1230, 1.5f);
                // PoppyR, PoppyRSpell

                MainMenu.Menu();
                Game.OnUpdate += OnGameUpdate;
                Drawing.OnDraw += OnDraw.Drawing_OnDraw;
                Drawing.OnEndScene += OnEndScene.Drawing_OnEndScene;
                GameObject.OnCreate += GameObject_OnCreate;
                GameObject.OnDelete += GameObject_OnDelete;
                AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
                Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
                Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
                Orbwalking.BeforeAttack += Orbwalking_BeforeAttack;
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshPoppy is not working. plz send message by KorFresh (Code 3)");
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            try
            {
                if (Player.IsDead) return;
                var QTarget = TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.True);
                var ETarget = TargetSelector.GetTarget(_E.Range, TargetSelector.DamageType.Physical);
                //var RTarget = TargetSelector.GetTarget(_R.Range, TargetSelector.DamageType.Physical);
                //var RTarget = ObjectManager.Get<Obj_AI_Hero>().OrderByDescending(x => x.Health).FirstOrDefault(x => x.IsEnemy && x.Distance(Player) < 1200);                
                PoppyPassive = ObjectManager.Player.Buffs.Find(DrawFX => DrawFX.Name == "poppypassiveshield" && DrawFX.IsValidBuff());
                if (PoppyPassive != null)
                    shield.ShieldMe = null;

                if (Rsender.Enable == true && _R.IsReady() && _R.IsCharging && Rsender.Type == "gap")
                    _R.Cast(Rsender.Sender, true);
                if (Rsender.Enable == true && _R.IsReady() && _R.IsCharging && Rsender.Type == "KillSteal" && Player.Distance(Rsender.Sender) < 1200)
                    _R.Cast(Rsender.Sender, true);
                if (Rsender.Enable == true && _R.IsReady() && _R.IsCharging && Rsender.Type == "Combo" && Player.Distance(Rsender.Sender) < 1200)
                    _R.Cast(Rsender.Sender, true);

                // KillSteal
                if (MainMenu._MainMenu.Item("KUse_Q").GetValue<bool>() && QTarget != null && QTarget.Health < _Q.GetDamage(QTarget) && _Q.IsReady())
                    _Q.Cast(QTarget, true);
                if (MainMenu._MainMenu.Item("KUse_E").GetValue<bool>() && ETarget != null && _E.IsReady())
                {
                    var FinalPosition = ETarget.Position.Extend(ObjectManager.Player.Position, -360);
                    if (FinalPosition.IsWall() && ETarget.Health < (_E.GetDamage(ETarget) * 2))
                        _E.Cast(ETarget, true);
                    if (ETarget.Health < _E.GetDamage(ETarget))
                        _E.Cast(ETarget, true);
                }
                if (MainMenu._MainMenu.Item("KUse_R").GetValue<bool>() && _R.IsReady() && !Rsender.Enable)
                {
                    foreach (var item in ObjectManager.Get<Obj_AI_Hero>().OrderByDescending(x => x.Health))
                    {                        
                        if (item.IsEnemy && !item.IsDead && item.Distance(Player) < 1200 && !_R.IsCharging && item.Health < _R.GetDamage(item))
                        {
                            _R.StartCharging();
                            Rsender.Sender = item;
                            Rsender.Enable = true;
                            Rsender.Type = "KillSteal";
                        }
                    }
                }
                // KillSteal

                //CC
                if (MainMenu._MainMenu.Item("CC").GetValue<KeyBind>().Active)
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                    if (_R.IsReady())
                    {
                        foreach (var item in ObjectManager.Get<Obj_AI_Hero>().OrderByDescending(x => x.IsEnemy))
                        {
                            if (item.Distance(Player) < 600 && item.Distance(Game.CursorPos) < 200)
                            {
                                if (_R.IsCharging)
                                {
                                    _R.Cast(item, true);
                                }
                                else
                                {
                                    _R.StartCharging();
                                }
                            }
                        }
                    }
                }

                if (MainMenu._MainMenu.Item("CKey").GetValue<KeyBind>().Active) // Combo
                {
                    if (MainMenu._MainMenu.Item("CUse_R").GetValue<bool>() && _R.IsReady() && !Rsender.Enable)
                    {
                        if (MainMenu._MainMenu.Item("CUse_R_Enable").GetValue<bool>())
                        {
                            foreach (var item in ObjectManager.Get<Obj_AI_Hero>().OrderBy(x => x.MaxHealth))
                            {
                                if (item.IsEnemy && !item.IsDead && MainMenu._MainMenu.Item("CUse" + item.ChampionName).GetValue<bool>() && item.Distance(Player) < 1200)
                                {
                                    _R.StartCharging();
                                    Rsender.Sender = item;
                                    Rsender.Enable = true;
                                    Rsender.Type = "Combo";
                                }
                            }
                        }
                        else
                        {
                            var RTarget = ObjectManager.Get<Obj_AI_Hero>().OrderBy(x => x.MaxHealth).FirstOrDefault(x => x.IsEnemy && x.Distance(Player) < 1200);
                            _R.StartCharging();
                            Rsender.Sender = RTarget;
                            Rsender.Enable = true;
                            Rsender.Type = "Combo";
                        }
                    }
                    if (MainMenu._MainMenu.Item("CUse_Q").GetValue<bool>() && QTarget != null && _Q.IsReady())
                        _Q.Cast(QTarget, true);
                    //벽꿍
                    if (MainMenu._MainMenu.Item("CUse_E").GetValue<bool>() && ETarget != null && _E.IsReady())
                    {
                        var FinalPosition = ETarget.Position.Extend(ObjectManager.Player.Position, -360);
                        if (FinalPosition.IsWall())
                        {
                            _E.Cast(ETarget, true);
                        }
                    }
                }

                if (MainMenu._MainMenu.Item("LKey").GetValue<KeyBind>().Active) // LaneClear
                {
                    var MinionTarget = MinionManager.GetMinions(1100, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.Health);
                    foreach (var minion in MinionTarget)
                    {
                        if (_Q.IsReady() && MainMenu._MainMenu.Item("LUse_Q").GetValue<bool>() && minion != null && !Orbwalking.CanAttack() && Orbwalking.CanMove(5))
                        {
                            _Q.Cast(minion, true);

                        }
                        if (_E.IsReady() && MainMenu._MainMenu.Item("LUse_E").GetValue<bool>() && minion != null && !Orbwalking.CanAttack() && Orbwalking.CanMove(5))
                        {
                            _E.Cast(minion, true);
                        }
                    }
                }
                if (MainMenu._MainMenu.Item("JKey").GetValue<KeyBind>().Active) // JungleClear
                {
                    var JungleTarget = MinionManager.GetMinions(1100, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
                    foreach (var minion in JungleTarget)
                    {
                        if (_Q.IsReady() && MainMenu._MainMenu.Item("JUse_Q").GetValue<bool>() && minion != null && !Orbwalking.CanAttack() && Orbwalking.CanMove(5))
                        {
                            _Q.Cast(minion, true);
                        }
                        if (_E.IsReady() && MainMenu._MainMenu.Item("JUse_E").GetValue<bool>() && minion != null && !Orbwalking.CanAttack() && Orbwalking.CanMove(5))
                        {
                            _E.Cast(minion, true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                //Game.PrintChat("FreshPoppy is not working. plz send message by KorFresh (Code 4)");
            }
        }

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            try
            {
                if (MainMenu._MainMenu.Item("Gap_W").GetValue<bool>() && _W.IsReady())
                {
                    _W.Cast();
                    return;
                }

                if (MainMenu._MainMenu.Item("Gap_E").GetValue<bool>() && _E.IsReady())
                {
                    var FinalPosition = gapcloser.Sender.Position.Extend(ObjectManager.Player.Position, -360);
                    if (FinalPosition.IsWall())
                        _E.Cast(gapcloser.Sender, true);
                    return;
                }
                if (MainMenu._MainMenu.Item(gapcloser.Sender.ChampionName).GetValue<bool>() && _R.IsReady())
                {
                    if (!_R.IsCharging)
                    {
                        _R.StartCharging();
                        Rsender.Sender = gapcloser.Sender;
                        Rsender.Enable = true;
                        Rsender.Type = "gap";
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshPoppy is not working. plz send message by KorFresh (Code 6)");
            }
        }

        private static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (sender.IsMe)
                {
                    if (args.SData.Name == "PoppyRSpell")
                        Rsender.Enable = false;
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshPoppy is not working. plz send message by KorFresh (Code 7)");
            }
        }

        private static void GameObject_OnCreate(GameObject obj, EventArgs args)
        {
            try
            {
                if (obj.IsAlly && obj.Name == "Shield")
                {
                    shield.ShieldMe = obj;
                }
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
                if (obj.IsAlly && obj.Name == "Shield")
                {
                    shield.ShieldMe = null;
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshPoppy is not working. plz send message by KorFresh (Code 14)");
            }
        }

        private static void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (_R.IsChanneling)
                args.Process = false;
        }

        private static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (sender.IsEnemy && sender.IsValid<Obj_AI_Hero>())
            {
                if (MainMenu._MainMenu.Item("inter_E").GetValue<bool>() && _E.IsReady() && Player.Distance(sender) < 525)
                {
                    _E.Cast(sender, true);
                }
                if (MainMenu._MainMenu.Item("inter_R").GetValue<bool>() && _R.IsReady() && Player.Distance(sender) < 1200)
                {
                    if (_R.IsCharging)
                    {
                        _R.Cast(sender, true);
                    }
                    else
                    {
                        _R.StartCharging();
                    }
                }
            }
        }

        private static int TickCount(int time)
        {
            return Environment.TickCount + time;
        }
    }
}