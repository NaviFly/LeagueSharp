using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Fresh_TahmKench
{
    class Program
    {
        public const string ChampName = "TahmKench";
        public static Obj_AI_Hero Player;
        public static Spell _Q, _W, _W2, _E, _R;
        public static HpBarIndicator Indicator = new HpBarIndicator();
        public static BuffInstance EnemyBuff;
        public static Obj_AI_Hero PassiveEnemy;        

        static void Main(string[] args)
        {
            try
            {
                CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshTahmKench is not working. plz send message by KorFresh (Code 2)");
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

                _Q = new Spell(SpellSlot.Q, 850);
                _Q.SetSkillshot(0.75f, 75f, 2000, true, SkillshotType.SkillshotLine);
                _W = new Spell(SpellSlot.W, 300);
                _W2 = new Spell(SpellSlot.W, 700);
                _W2.SetSkillshot(0.5f, 75f, 950, true, SkillshotType.SkillshotLine);
                _E = new Spell(SpellSlot.E);
                _R = new Spell(SpellSlot.R);

                MainMenu.Menu();
                Game.OnUpdate += OnGameUpdate;
                Drawing.OnDraw += OnDraw.Drawing_OnDraw;
                //Drawing.OnEndScene += OnEndScene.Drawing_OnEndScene;
                AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
                Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
                Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
                Orbwalking.BeforeAttack += Orbwalking_BeforeAttack;
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshTahmKench is not working. plz send message by KorFresh (Code 3)");
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            try
            {
                if (Player.IsDead) return;

                var QTarget = TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Magical);
                var WTarget = TargetSelector.GetTarget(_W.Range, TargetSelector.DamageType.Magical);
                var WTarget2 = TargetSelector.GetTarget(_W2.Range, TargetSelector.DamageType.Magical);

                if (MainMenu._MainMenu.Item("AutoEUse").GetValue<bool>() && MainMenu._MainMenu.Item("MinE").GetValue<Slider>().Value > Player.HealthPercent && _E.IsReady())
                    _E.Cast(true);
                if (MainMenu._MainMenu.Item("MyTeamCC").GetValue<bool>() && _W.IsReady() && Player.Spellbook.GetSpell(SpellSlot.W).Name != "tahmkenchwspitready")
                {
                    var Ally = ObjectManager.Get<Obj_AI_Hero>().FirstOrDefault(x => x.IsAlly && !x.IsMe && !x.IsDead && !x.IsZombie && x.ServerPosition.Distance(Player.ServerPosition) < _W.Range);
                    if (Ally != null && HasBuff.HasBuffsofCC(Ally))
                        _W.CastOnUnit(Ally, true);
                }

                if (MainMenu._MainMenu.Item("CKey").GetValue<KeyBind>().Active) // Combo
                {
                    if (QTarget != null && _Q.IsReady() && MainMenu._MainMenu.Item("CUse_Q").GetValue<bool>())
                        _Q.CastIfHitchanceEquals(QTarget, HitChance.Low, true);
                    if (WTarget != null && _W.IsReady() && WTarget.Buffs.Find(x => x.Name == "tahmkenchpdebuffcounter" && x.IsValidBuff()).Count == 3 && MainMenu._MainMenu.Item("CUse_W1").GetValue<bool>())
                        _W.CastOnUnit(WTarget, true);
                    if (WTarget2 != null && _W.IsReady() && MainMenu._MainMenu.Item("CUse_W2").GetValue<bool>())
                    {
                        if (Player.Spellbook.GetSpell(SpellSlot.W).Name == "tahmkenchw")
                        {
                            var getminion = MinionManager.GetMinions(_W.Range, MinionTypes.All, MinionTeam.NotAlly).OrderBy(x => x.Distance(Player.ServerPosition)).FirstOrDefault();
                            if (getminion != null)
                                _W.CastOnUnit(getminion, true);
                        }
                        if (Player.Spellbook.GetSpell(SpellSlot.W).Name == "tahmkenchwspitready")
                        {
                            _W2.CastIfHitchanceEquals(WTarget2, HitChance.Low, true);
                        }
                    }
                }

                if ((MainMenu._MainMenu.Item("HKey").GetValue<KeyBind>().Active || MainMenu._MainMenu.Item("HToggle").GetValue<bool>())
                    && MainMenu._MainMenu.Item("HManarate").GetValue<Slider>().Value < Player.ManaPercent) // Harass
                {
                    if (QTarget != null && _Q.IsReady() && MainMenu._MainMenu.Item("HUse_Q").GetValue<bool>())
                        _Q.CastIfHitchanceEquals(QTarget, HitChance.Low, true);
                    if (WTarget2 != null && _W.IsReady() && MainMenu._MainMenu.Item("HUse_W2").GetValue<bool>())
                    {
                        if (Player.Spellbook.GetSpell(SpellSlot.W).Name == "tahmkenchw")
                        {
                            var getminion = MinionManager.GetMinions(_W.Range, MinionTypes.All, MinionTeam.NotAlly).OrderBy(x => x.Distance(Player.ServerPosition)).FirstOrDefault();
                            if (getminion != null)
                                _W.CastOnUnit(getminion, true);
                        }
                        if (Player.Spellbook.GetSpell(SpellSlot.W).Name == "tahmkenchwspitready")
                        {
                            _W2.CastIfHitchanceEquals(WTarget2, HitChance.Low, true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                //Game.PrintChat("FreshTahmKench is not working. plz send message by KorFresh (Code 4)");
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
                Game.PrintChat("FreshTahmKench is not working. plz send message by KorFresh (Code 6)");
            }
        }

        private static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (!sender.IsMe && sender.IsEnemy)
                {
                    if (MainMenu._MainMenu.Item("MyTeamMinEnable").GetValue<bool>() && args.End.Distance(Player.ServerPosition) < _W.Range)
                    {
                        var EndPosition = ObjectManager.Get<Obj_AI_Hero>().OrderBy(x => x.Health).FirstOrDefault(x => x.IsAlly && !x.IsMe && !x.IsDead && !x.IsZombie
                        && x.HealthPercent < MainMenu._MainMenu.Item("MyTeamMin").GetValue<Slider>().Value);
                        if (EndPosition != null)
                            _W.CastOnUnit(EndPosition, true);
                    }                    
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshTahmKench is not working. plz send message by KorFresh (Code 7)");
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

        }
        public static void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (args.Unit.IsMe)
            {

            }
        }
        private static int TickCount(int time)
        {
            return Environment.TickCount + time;
        }
    }
}