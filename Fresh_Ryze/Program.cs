using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Fresh_Ryze
{
    class Program
    {
        public const string ChampName = "Ryze";
        public static Obj_AI_Hero Player;
        public static Spell _Q, _W, _E, _R;
        public static HpBarIndicator Indicator = new HpBarIndicator();
        public static BuffInstance RyzePassive, Ryzepassivecharged;
        public static bool Buff_Recall;
        public static int RyzeStack = 0;
        public static float Recall_Time = 0, PassiveEndTime = 0, SpeedTime = 0;

        static void Main(string[] args)
        {
            try
            {
                CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshRyze is not working. plz send message by KorFresh (Code 2)");
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
                _Q.SetSkillshot(0.25f, 50f, 1700, true, SkillshotType.SkillshotLine);
                _W = new Spell(SpellSlot.W, 620);
                _E = new Spell(SpellSlot.E, 620);
                _R = new Spell(SpellSlot.R);

                MainMenu.Menu();
                Game.OnUpdate += OnGameUpdate;
                Drawing.OnDraw += OnDraw.Drawing_OnDraw;
                Drawing.OnEndScene += OnEndScene.Drawing_OnEndScene;
                AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
                Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
                Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
                Orbwalking.BeforeAttack += Orbwalking_BeforeAttack;
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshRyze is not working. plz send message by KorFresh (Code 3)");
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

                RyzePassive = ObjectManager.Player.Buffs.Find(DrawFX => DrawFX.Name == "ryzepassivestack" && DrawFX.IsValidBuff()); // 라이즈 스택
                Ryzepassivecharged = ObjectManager.Player.Buffs.Find(DrawFX => DrawFX.Name == "ryzepassivecharged" && DrawFX.IsValidBuff());
                Buff_Recall = ObjectManager.Player.IsRecalling();
                if (Buff_Recall)
                    Recall_Time = TickCount(1000);
                if (RyzePassive != null)
                {
                    RyzeStack = RyzePassive.Count;
                    PassiveEndTime = RyzePassive.EndTime;
                }
                else
                    RyzeStack = 0;

                if (MainMenu._MainMenu.Item("CKey").GetValue<KeyBind>().Active)
                    _Q.Collision = false;
                else
                    _Q.Collision = true;

                if (MainMenu._MainMenu.Item("KUse_Q").GetValue<bool>() && QTarget != null && _Q.IsReady() && _Q.GetDamage(QTarget) > QTarget.Health)
                    _Q.CastIfHitchanceEquals(QTarget, HitChance.High);
                if (MainMenu._MainMenu.Item("KUse_E").GetValue<bool>() && ETarget != null && _E.IsReady() && _E.GetDamage(ETarget) > ETarget.Health)
                    _E.CastOnUnit(ETarget, true);
                if (MainMenu._MainMenu.Item("KUse_W").GetValue<bool>() && WTarget != null && _W.IsReady() && _W.GetDamage(WTarget) > WTarget.Health)
                    _W.CastOnUnit(WTarget, true);

                if (MainMenu._MainMenu.Item("CKey").GetValue<KeyBind>().Active) // Combo
                {
                    if (SpeedTime > Environment.TickCount)
                        return;
                    if (MainMenu._MainMenu.Item("CUse_R").GetValue<bool>() && _R.IsReady() && WTarget != null && ((_Q.IsReady() || _W.IsReady() || _E.IsReady()) && RyzeStack > 2) || RyzeStack == 4 || Ryzepassivecharged != null)
                        _R.Cast(true);
                    if (MainMenu._MainMenu.Item("CUse_W").GetValue<bool>() && _W.IsReady() && WTarget != null)
                        _W.CastOnUnit(WTarget, true);
                    if (MainMenu._MainMenu.Item("CUse_Q").GetValue<bool>() && _Q.IsReady() && WTarget != null && _W.IsReady())
                        _Q.Cast(WTarget, true);
                    if (MainMenu._MainMenu.Item("CUse_Q").GetValue<bool>() && _Q.IsReady() && QTarget != null && !_W.IsReady())
                        _Q.Cast(QTarget, true);
                    if (MainMenu._MainMenu.Item("CUse_E").GetValue<bool>() && _E.IsReady() && ETarget != null)
                        _E.CastOnUnit(ETarget, true);
                }

                if ((MainMenu._MainMenu.Item("HKey").GetValue<KeyBind>().Active || MainMenu._MainMenu.Item("HToggle").GetValue<bool>())
                    && MainMenu._MainMenu.Item("HManarate").GetValue<Slider>().Value < Player.ManaPercent) // Harass
                {
                    if (MainMenu._MainMenu.Item("HToggle").GetValue<bool>() && Recall_Time > Environment.TickCount)
                        return;
                    if (MainMenu._MainMenu.Item("HUse_W").GetValue<bool>() && _W.IsReady() && WTarget != null)
                        _W.CastOnUnit(WTarget, true);
                    if (MainMenu._MainMenu.Item("HUse_Q").GetValue<bool>() && _Q.IsReady() && QTarget != null)
                        _Q.CastIfHitchanceEquals(QTarget, HitChance.VeryHigh, true);
                    if (MainMenu._MainMenu.Item("HUse_E").GetValue<bool>() && _E.IsReady() && ETarget != null)
                        _E.CastOnUnit(ETarget, true);
                }

                if (MainMenu._MainMenu.Item("LKey").GetValue<KeyBind>().Active && MainMenu._MainMenu.Item("LManaRate").GetValue<Slider>().Value < Player.ManaPercent) // LaneClear
                {
                    var MinionTarget = MinionManager.GetMinions(_W.Range, MinionTypes.All, MinionTeam.Enemy).OrderBy(x => x.ServerPosition.Distance(Player.ServerPosition));
                    foreach (var item in MinionTarget)
                    {
                        if (MainMenu._MainMenu.Item("LUseW").GetValue<bool>() && _W.IsReady())
                            _W.CastOnUnit(item, true);
                        if (MainMenu._MainMenu.Item("LUseQ").GetValue<bool>() && _Q.IsReady())
                            _Q.CastIfHitchanceEquals(item, HitChance.High, true);
                        if (MainMenu._MainMenu.Item("LUseE").GetValue<bool>() && _E.IsReady())
                            _E.CastOnUnit(item, true);
                    }
                }

                if (MainMenu._MainMenu.Item("JKey").GetValue<KeyBind>().Active && MainMenu._MainMenu.Item("JManaRate").GetValue<Slider>().Value < Player.ManaPercent) // JungleClear
                {
                    var MinionTarget = MinionManager.GetMinions(_W.Range, MinionTypes.All, MinionTeam.Neutral).OrderBy(x => x.ServerPosition.Distance(Player.ServerPosition));
                    foreach (var item in MinionTarget)
                    {
                        if (MainMenu._MainMenu.Item("JUseW").GetValue<bool>() && _W.IsReady())
                            _W.CastOnUnit(item, true);
                        if (MainMenu._MainMenu.Item("JUseQ").GetValue<bool>() && _Q.IsReady())
                            _Q.CastIfHitchanceEquals(item, HitChance.High, true);
                        if (MainMenu._MainMenu.Item("JUseE").GetValue<bool>() && _E.IsReady())
                            _E.CastOnUnit(item, true);
                    }
                }

                if (MainMenu._MainMenu.Item("AutoLasthit").GetValue<bool>() && !MainMenu._MainMenu.Item("CKey").GetValue<KeyBind>().Active && !MainMenu._MainMenu.Item("HKey").GetValue<KeyBind>().Active
                    && Recall_Time < Environment.TickCount && RyzeStack > 4)
                {
                    var MinionTarget = MinionManager.GetMinions(_W.Range, MinionTypes.All, MinionTeam.Enemy).OrderBy(x => x.Health).FirstOrDefault(x => !x.IsDead);
                    if (MainMenu._MainMenu.Item("AutoLasthitQ").GetValue<bool>() && _Q.IsReady() && MinionTarget != null && _Q.GetDamage(MinionTarget) > MinionTarget.Health)
                        _Q.CastIfHitchanceEquals(MinionTarget, HitChance.High, true);
                    if (MainMenu._MainMenu.Item("AutoLasthitE").GetValue<bool>() && _E.IsReady() && MinionTarget != null && _E.GetDamage(MinionTarget) > MinionTarget.Health)
                        _E.CastOnUnit(MinionTarget, true);
                    if (MainMenu._MainMenu.Item("AutoLasthitW").GetValue<bool>() && _W.IsReady() && MinionTarget != null && _W.GetDamage(MinionTarget) > MinionTarget.Health)
                        _W.CastOnUnit(MinionTarget, true);
                }

                if (MainMenu._MainMenu.Item("AutoKey").GetValue<KeyBind>().Active
                    && !MainMenu._MainMenu.Item("CKey").GetValue<KeyBind>().Active && !MainMenu._MainMenu.Item("HKey").GetValue<KeyBind>().Active
                    && Recall_Time < Environment.TickCount && MainMenu._MainMenu.Item("AutoStack").GetValue<Slider>().Value > RyzeStack
                    && MainMenu._MainMenu.Item("AutoStack").GetValue<Slider>().Value != RyzeStack
                    && MainMenu._MainMenu.Item("AutoStack").GetValue<Slider>().Value != 0
                    && _Q.IsReady() && PassiveEndTime <= Game.ClockTime + 0.5f)
                    _Q.Cast(Game.CursorPos, true);
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshRyze is not working. plz send message by KorFresh (Code 4)");
            }
        }

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            try
            {
                if (MainMenu._MainMenu.Item("WGap").GetValue<bool>() && _W.IsReady() && gapcloser.Sender.IsEnemy && gapcloser.Sender.ServerPosition.Distance(Player.ServerPosition) < _W.Range)
                    _W.CastOnUnit(gapcloser.Sender, true);
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshRyze is not working. plz send message by KorFresh (Code 6)");
            }
        }

        private static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (sender.IsMe)
                {
                    switch (MainMenu._MainMenu.Item("Speed").GetValue<StringList>().SelectedIndex)
                    {
                        case 0:
                            break;
                        case 1:
                            SpeedTime = TickCount(200);
                            break;
                        case 2:
                            SpeedTime = TickCount(400);
                            break;
                        case 3:
                            var ran = new Random().Next(0, 2);
                            if (ran == 1)
                                SpeedTime = TickCount(150);
                            else if(ran == 2)
                                SpeedTime = TickCount(400);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshLeBlanc is not working. plz send message by KorFresh (Code 7)");
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
                if (MainMenu._MainMenu.Item("CKey").GetValue<KeyBind>().Active)
                {
                    if (_Q.IsReady() || _W.IsReady() || _E.IsReady())
                        args.Process = false;
                    else
                        args.Process = true;
                }
            }
        }
        private static int TickCount(int time)
        {
            return Environment.TickCount + time;
        }
    }
}