using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Fresh_LeBlanc
{
    class Program
    {
        public const string ChampName = "Leblanc";
        public static Obj_AI_Hero Player;
        public static Spell _Q, _W, _E, _R;
        public static List<Spell> SpellList = new List<Spell>();
        public static List<int> SpellUseTree = new List<int>();
        public static HpBarIndicator Indicator = new HpBarIndicator();
        public static string[] SpellName = new string[]
        {
            "LeblancChaosOrb","LeblancChaosOrbM",   // 0,1
            "LeblancSlide", "LeblancSlideM", "leblancslidereturn","leblancslidereturnm",    // 2,3,4,5
            "LeblancSoulShackle","LeblancSoulShackleM"     // 6,7
        };
        public static int SpellUseCnt = 0, SpellUseCnt1 = 0, ERCC = 0;
        public static int SpellUseTime = 0, ERTIME = 0, WTime = 0, RTime = 0, ReturnTime = 0, PetTime = 0;

        static void Main(string[] args)
        {
            try
            {
                CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshLeBlanc is not working. plz send message by KorFresh (Code 2)");
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

                _Q = new Spell(SpellSlot.Q, 720);
                _Q.SetTargetted(0.5f, 1500f);
                _W = new Spell(SpellSlot.W, 700);
                _W.SetSkillshot(0.6f, 220f, 1450f, false, SkillshotType.SkillshotCircle);
                _E = new Spell(SpellSlot.E, 900);
                _E.SetSkillshot(0.3f, 55f, 1650f, true, SkillshotType.SkillshotLine);
                _R = new Spell(SpellSlot.R, 720);
                {
                    SpellList.Add(_Q);
                    SpellList.Add(_W);
                    SpellList.Add(_E);
                    SpellList.Add(_R);
                }

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
                Game.PrintChat("FreshLeBlanc is not working. plz send message by KorFresh (Code 3)");
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            try
            {
                if (Player.IsDead) return;
                if (MainMenu._MainMenu.Item("Flee").GetValue<KeyBind>().Active)
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);  // 커서방향 이동
                    if (_W.IsReady() && Player.Spellbook.GetSpell(SpellSlot.W).Name == "LeblancSlide")
                        _W.Cast(Game.CursorPos, true);
                    if (_R.IsReady() && Player.Spellbook.GetSpell(SpellSlot.R).Name == "LeblancSlideM")
                        _R.Cast(Game.CursorPos, true);
                }

                if (MainMenu._MainMenu.Item("ERCC").GetValue<KeyBind>().Active) // ERCC
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);  // 커서방향 이동
                    var ETarget = TargetSelector.GetTarget(_E.Range, TargetSelector.DamageType.Magical);
                    if (_E.IsReady() && ETarget != null && Environment.TickCount > ERTIME)
                    {
                        _E.CastIfHitchanceEquals(ETarget, HitChance.Low, true);
                    }
                    else if (ETarget != null && _R.IsReady() && Player.Spellbook.GetSpell(SpellSlot.R).Name == "LeblancSoulShackleM" && Environment.TickCount > ERTIME)
                    {
                        _R.CastIfHitchanceEquals(ETarget, HitChance.Low, true);
                    }
                }

                // Pet
                if (MainMenu._MainMenu.Item("Pet").GetValue<bool>() && Player.Pet != null && Player.Pet.IsValid && !Player.Pet.IsDead)
                {
                    var Enemy = ObjectManager.Get<Obj_AI_Hero>().OrderBy(x => x.Distance(Player)).FirstOrDefault(x => x.IsEnemy && !x.IsDead);
                    Random Ran = new Random();
                    var PetLocate = Player.ServerPosition.Extend(Enemy.ServerPosition, Ran.Next(150, 300));
                    PetLocate.X = PetLocate.X + Ran.Next(0, 20);
                    PetLocate.Y = PetLocate.Y + Ran.Next(0, 20);
                    PetLocate.Z = PetLocate.Z + Ran.Next(0, 20);
                    if (PetTime < Environment.TickCount)
                    {
                        Player.IssueOrder(GameObjectOrder.MovePet, PetLocate);
                        PetTime = TickCount(750);
                    }

                }

                if (MainMenu._MainMenu.Item("KUse_Q").GetValue<bool>() && _Q.IsReady())
                {
                    var QTarget = TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Magical);
                    if (QTarget == null) return;
                    if (QTarget.Health <= _Q.GetDamage(QTarget))
                    {
                        _Q.Cast(QTarget, true);
                        return;
                    }
                }
                if (MainMenu._MainMenu.Item("KUse_W").GetValue<bool>() && _W.IsReady() && Player.Spellbook.GetSpell(SpellSlot.W).Name == "LeblancSlide")
                {
                    var WTarget = TargetSelector.GetTarget(_W.Range, TargetSelector.DamageType.Magical);
                    if (WTarget == null) return;
                    if (WTarget.Health <= _W.GetDamage(WTarget))
                    {
                        _W.Cast(WTarget.ServerPosition, true);
                        return;
                    }
                }
                if (MainMenu._MainMenu.Item("KUse_E").GetValue<bool>() && _E.IsReady())
                {
                    var ETarget = TargetSelector.GetTarget(_E.Range, TargetSelector.DamageType.Magical);
                    if (ETarget == null) return;
                    if (ETarget.Health <= _E.GetDamage(ETarget))
                    {
                        _E.CastIfHitchanceEquals(ETarget, HitChance.Low, true);
                        return;
                    }
                }
                if (MainMenu._MainMenu.Item("KUse_Q2").GetValue<bool>() && _R.IsReady() && Player.Spellbook.GetSpell(SpellSlot.R).Name == "LeblancChaosOrbM")
                {
                    var QTarget = TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Magical);
                    if (QTarget == null) return;
                    if (QTarget.Health <= _Q.GetDamage(QTarget))
                    {
                        _R.Cast(QTarget, true);
                        return;
                    }
                }
                if (MainMenu._MainMenu.Item("KUse_W2").GetValue<bool>() && _R.IsReady() && Player.Spellbook.GetSpell(SpellSlot.R).Name == "LeblancSlideM")
                {
                    var QTarget = TargetSelector.GetTarget(_W.Range, TargetSelector.DamageType.Magical);
                    if (QTarget == null) return;
                    if (QTarget.Health <= _W.GetDamage(QTarget))
                    {
                        _R.Cast(QTarget.ServerPosition, true);
                        return;
                    }
                }
                if (MainMenu._MainMenu.Item("KUse_E2").GetValue<bool>() && _R.IsReady() && Player.Spellbook.GetSpell(SpellSlot.R).Name == "LeblancSoulShackleM")
                {
                    var QTarget = TargetSelector.GetTarget(_E.Range, TargetSelector.DamageType.Magical);
                    if (QTarget == null) return;
                    if (QTarget.Health <= _E.GetDamage(QTarget))
                    {
                        _R.CastIfHitchanceEquals(QTarget, HitChance.Low, true);
                        return;
                    }
                }

                if (MainMenu._MainMenu.Item("CKey").GetValue<KeyBind>().Active) // Combo
                {
                    var QTarget = TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Magical);
                    var WTarget = TargetSelector.GetTarget(_W.Range, TargetSelector.DamageType.Magical);
                    var ETarget = TargetSelector.GetTarget(_E.Range, TargetSelector.DamageType.Magical);
                    var TargetSel = TargetSelector.GetSelectedTarget();
                    if (QTarget == null && WTarget == null && ETarget == null) return;
                    
                    // Q
                    if (QTarget != null
                            && MainMenu._MainMenu.Item("CUse_Q").GetValue<bool>() && _Q.IsReady()
                            && Player.Spellbook.GetSpell(SpellSlot.Q).Name == "LeblancChaosOrb"
                            )
                    {
                        ReturnTime = TickCount(1000);
                        _Q.Cast(QTarget, true);
                        return;
                    }

                    if (Player.Level > 5 && TargetSel == null && MainMenu._MainMenu.Item("ComboMode").GetValue<KeyBind>().Active)   // teamcombo
                    {
                        // W
                        if (WTarget != null
                                && MainMenu._MainMenu.Item("CUse_W").GetValue<bool>() && _W.IsReady()
                                && Player.Spellbook.GetSpell(SpellSlot.W).Name == "LeblancSlide"
                                )
                        {
                            _W.Cast(WTarget.ServerPosition, true);
                            return;
                        }

                        // W2
                        if (WTarget != null
                                && MainMenu._MainMenu.Item("CUse_W2").GetValue<bool>() && _R.IsReady()
                                && Player.Spellbook.GetSpell(SpellSlot.R).Name == "LeblancSlideM")
                        {
                            _R.Cast(WTarget.ServerPosition, true);
                            return;
                        }
                    }

                    // Q2
                    if (QTarget != null
                            && MainMenu._MainMenu.Item("CUse_Q2").GetValue<bool>() && _R.IsReady()
                            && Player.Spellbook.GetSpell(SpellSlot.R).Name == "LeblancChaosOrbM"
                            )
                    {
                        _R.Cast(QTarget, true);
                        return;
                    }
                    // W
                    if (WTarget != null
                            && MainMenu._MainMenu.Item("CUse_W").GetValue<bool>() && _W.IsReady()
                            && Player.Spellbook.GetSpell(SpellSlot.W).Name == "LeblancSlide"
                            )
                    {
                        _W.Cast(WTarget.ServerPosition, true);
                        return;
                    }

                    // W2
                    if (WTarget != null
                            && MainMenu._MainMenu.Item("CUse_W2").GetValue<bool>() && _R.IsReady()
                            && Player.Spellbook.GetSpell(SpellSlot.R).Name == "LeblancSlideM")
                    {
                        _R.Cast(WTarget.ServerPosition, true);
                        return;
                    }

                    // E
                    if (ETarget != null
                            && MainMenu._MainMenu.Item("CUse_E").GetValue<bool>() && _E.IsReady()
                            && Player.Spellbook.GetSpell(SpellSlot.E).Name == "LeblancSoulShackle" && Player.Spellbook.GetSpell(SpellSlot.R).Name != "LeblancSlideM"
                            )
                    {
                        _E.CastIfHitchanceEquals(ETarget, Hitchance("CUseE_Hit"), true);
                        return;
                    }

                    // E2
                    if (ETarget != null
                            && MainMenu._MainMenu.Item("CUse_E2").GetValue<bool>() && _E.IsReady()
                            && Player.Spellbook.GetSpell(SpellSlot.E).Name == "LeblancSoulShackleM"
                            )
                    {
                        _R.CastIfHitchanceEquals(ETarget, Hitchance("CUseE_Hit"), true);
                        return;
                    }

                    // WReturn
                    if (ETarget.Buffs.Find(buff => (buff.Name == "LeblancSoulShackle" || buff.Name == "LeblancSoulShackleM") && buff.IsValidBuff()) == null
                        && MainMenu._MainMenu.Item("CUse_WReturn").GetValue<bool>()
                        && _W.IsReady() && Player.Spellbook.GetSpell(SpellSlot.W).Name == "leblancslidereturn" && !_Q.IsReady() && !_R.IsReady() && ReturnTime < Environment.TickCount)
                    {
                        _W.Cast(true);
                        return;
                    }

                    // WR Return
                    if (ETarget.Buffs.Find(buff => (buff.Name == "LeblancSoulShackle" || buff.Name == "LeblancSoulShackleM") && buff.IsValidBuff()) == null
                        && MainMenu._MainMenu.Item("CUse_W2Return").GetValue<bool>()
                        && _W.IsReady() && Player.Spellbook.GetSpell(SpellSlot.R).Name == "leblancslidereturnm" && !_Q.IsReady() && !_W.IsReady() && ReturnTime < Environment.TickCount)
                    {
                        _R.Cast(true);
                        return;
                    }

                }
                if ((MainMenu._MainMenu.Item("HKey").GetValue<KeyBind>().Active || MainMenu._MainMenu.Item("AHToggle").GetValue<bool>())
                    && MainMenu._MainMenu.Item("AManarate").GetValue<Slider>().Value < Player.ManaPercent) // Harass
                {

                    var QTarget = TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Magical);
                    var WTarget = TargetSelector.GetTarget(_W.Range, TargetSelector.DamageType.Magical);
                    var ETarget = TargetSelector.GetTarget(_E.Range, TargetSelector.DamageType.Magical);
                    // Q
                    if (QTarget != null
                            && MainMenu._MainMenu.Item("AUse_Q").GetValue<bool>() && _Q.IsReady()
                            && Player.Spellbook.GetSpell(SpellSlot.Q).Name == "LeblancChaosOrb"
                            )
                    {
                        _Q.Cast(QTarget, true);
                        return;
                    }

                    // W
                    if (WTarget != null
                            && MainMenu._MainMenu.Item("AUse_W").GetValue<bool>() && _W.IsReady()
                            && Player.Spellbook.GetSpell(SpellSlot.W).Name == "LeblancSlide"
                            )
                    {
                        _W.Cast(WTarget.ServerPosition, true);
                        return;
                    }

                    // E
                    if (ETarget != null
                            && MainMenu._MainMenu.Item("AUse_E").GetValue<bool>() && _E.IsReady()
                            && Player.Spellbook.GetSpell(SpellSlot.E).Name == "LeblancSoulShackle"
                            )
                    {
                        _E.CastIfHitchanceEquals(ETarget, Hitchance("CUseE_Hit"), true);
                        return;
                    }

                    // WW
                    if (WTarget != null
                            && MainMenu._MainMenu.Item("AUse_W").GetValue<bool>() && _W.IsReady()
                            && Player.Spellbook.GetSpell(SpellSlot.W).Name == "leblancslidereturn"
                            )
                    {
                        _W.Cast(true);
                        return;
                    }

                }
            }
            catch (Exception e)
            {
                //Console.Write(e);
                //Game.PrintChat("FreshLeBlanc is not working. plz send message by KorFresh (Code 4)");
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
                Game.PrintChat("FreshLeBlanc is not working. plz send message by KorFresh (Code 6)");
            }
        }

        private static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (sender.IsMe)
                {
                    //if (args.Target != null)
                    //Console.Write("Spell Name is " + args.Target.Name + "\n");                    
                    if (args.SData.Name == "LeblancSlide")
                        WTime = TickCount(4000);
                    if (args.SData.Name == "LeblancSlideM")
                        RTime = TickCount(4000);
                    if (MainMenu._MainMenu.Item("ERCC").GetValue<KeyBind>().Active)
                    {
                        if (ERCC == 0 && args.SData.Name == "LeblancSoulShackle")
                        {
                            ERTIME = TickCount(2000);
                        }
                        if (ERCC == 1 && args.SData.Name == "LeblancSoulShackleM")
                        {
                            ERTIME = TickCount(2000);
                        }
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

        private static int TickCount(int time)
        {
            return Environment.TickCount + time;
        }
    }
}