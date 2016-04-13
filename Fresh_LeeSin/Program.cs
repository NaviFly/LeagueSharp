using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace FreshLeeSin
{
    class Program
    {
        public const string ChampName = "LeeSin";
        private static Obj_AI_Hero Player;
        private static Spell _Q, _W, _E, _R;
        private static Vector3 use_ward, ward_pos, use_minion, InsecST, InsecED, InsecPOS;
        private static float Ward_Time, WW_Time, QTime, WTime, ETime, InsecTime = 0;
        private static bool WW = true;
        private static string InsecText, InsecType = "Wait";
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
                Game.PrintChat("FreshLeesin is not working. plz send message by KorFresh (Code 2)");
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
                _W = new Spell(SpellSlot.W, 700f);
                _E = new Spell(SpellSlot.E, 330f);
                _R = new Spell(SpellSlot.R, 375f);



                _Q.SetSkillshot(0.25f, 65f, 1800f, true, SkillshotType.SkillshotLine);

                MainMenu.Menu();
                Game.OnUpdate += OnGameUpdate;
                Drawing.OnDraw += Drawing_OnDraw;
                Drawing.OnEndScene += Drawing_OnEndScene;
                AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
                Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshLeesin is not working. plz send message by KorFresh (Code 3)");
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            try
            {
                if (Player.IsDead) return;
                //킬스틸 타겟
                var KTarget = ObjectManager.Get<Obj_AI_Hero>().OrderByDescending(x => x.Health).FirstOrDefault(x => x.IsEnemy && x.Distance(Player) < 375);
                if (KTarget != null && MainMenu._MainMenu.Item("KUse_R").GetValue<bool>() && KTarget.Health < _R.GetDamage(KTarget) && _R.IsReady())
                    _R.Cast(KTarget, true);
                if (InsecTime < Environment.TickCount) InsecType = "Wait"; // 인섹킥 초기화
                if (Ward_Time < Environment.TickCount) WW = true;   // 와드방호 초기화
                if (MainMenu._MainMenu.Item("AutoKick").GetValue<Slider>().Value != 0 && _R.Level > 0 && _R.IsReady() && !MainMenu._MainMenu.Item("InsecKick").GetValue<KeyBind>().Active)
                {
                    AutoKick();  // 오토 킥
                }
                if (MainMenu._MainMenu.Item("CKey").GetValue<KeyBind>().Active) // Combo
                {
                    var QTarget = TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Physical);
                    var ETarget = TargetSelector.GetTarget(_E.Range, TargetSelector.DamageType.Physical);
                    if (QTarget != null && _Q.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "BlindMonkQOne" && MainMenu._MainMenu.Item("CUse_Q").GetValue<bool>() && QTime < Environment.TickCount)
                    {
                        var HC = HitChance.Medium;
                        switch (MainMenu._MainMenu.Item("CUseQ_Hit").GetValue<Slider>().Value)
                        {
                            case 1:
                                HC = HitChance.OutOfRange;
                                break;
                            case 2:
                                HC = HitChance.Impossible;
                                break;
                            case 3:
                                HC = HitChance.Low;
                                break;
                            case 4:
                                HC = HitChance.Medium;
                                break;
                            case 5:
                                HC = HitChance.High;
                                break;
                            case 6:
                                HC = HitChance.VeryHigh;
                                break;

                        }
                        _Q.CastIfHitchanceEquals(QTarget, HC, true);
                        QTime = TickCount(2000);
                    }
                    if (ETarget != null && _E.IsReady() && !Orbwalking.CanAttack() && Orbwalking.CanMove(10) && ETime < Environment.TickCount && MainMenu._MainMenu.Item("CUse_E").GetValue<bool>())
                    {
                        _E.Cast(true);
                        ETime = TickCount(1000);
                    }
                    if (!_Q.IsReady() && !_E.IsReady() && !Orbwalking.CanAttack() && Orbwalking.CanMove(10) && WTime < Environment.TickCount && MainMenu._MainMenu.Item("CUse_W").GetValue<bool>())
                    {
                        _W.Cast(Player, true);
                        WTime = TickCount(1000);
                    }
                }
                if (MainMenu._MainMenu.Item("HKey").GetValue<KeyBind>().Active) // Hafass
                {
                    var QTarget = TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Physical);
                    var ETarget = TargetSelector.GetTarget(_E.Range, TargetSelector.DamageType.Physical);
                    if (QTarget != null && _Q.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "BlindMonkQOne" && MainMenu._MainMenu.Item("HUse_Q").GetValue<bool>() && QTime < Environment.TickCount)
                    {
                        var HC = HitChance.Medium;
                        _Q.CastIfHitchanceEquals(QTarget, HC, true);
                        QTime = TickCount(2000);
                    }
                    if (ETarget != null && _E.IsReady() && !Orbwalking.CanAttack() && Orbwalking.CanMove(10) && ETime < Environment.TickCount && MainMenu._MainMenu.Item("HUse_E").GetValue<bool>())
                    {
                        _E.Cast(true);
                        ETime = TickCount(1000);
                    }
                    if (!_Q.IsReady() && !_E.IsReady() && !Orbwalking.CanAttack() && Orbwalking.CanMove(10) && WTime < Environment.TickCount && MainMenu._MainMenu.Item("HUse_W").GetValue<bool>())
                    {
                        _W.Cast(Player, true);
                        WTime = TickCount(1000);
                    }
                }
                if (MainMenu._MainMenu.Item("LKey").GetValue<KeyBind>().Active) // LaneClear
                {
                    var MinionTarget = MinionManager.GetMinions(1100, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.Health);
                    foreach (var minion in MinionTarget)
                    {
                        if (_Q.IsReady() && MainMenu._MainMenu.Item("LUse_Q").GetValue<bool>() && minion != null && Environment.TickCount > QTime)
                        {
                            _Q.CastIfHitchanceEquals(minion, HitChance.Medium, true);
                            QTime = TickCount(1000);
                        }
                        if (_E.IsReady() && MainMenu._MainMenu.Item("LUse_E").GetValue<bool>() && minion != null && Environment.TickCount > ETime
                            && !Orbwalking.CanAttack() && Orbwalking.CanMove(10))
                        {
                            _E.Cast(true);
                            ETime = TickCount(1000);
                        }
                        if (_W.IsReady() && MainMenu._MainMenu.Item("LUse_W").GetValue<bool>() && minion != null && Environment.TickCount > WTime
                            && !Orbwalking.CanAttack() && Orbwalking.CanMove(10))
                        {
                            _W.Cast(Player, true);
                            WTime = TickCount(1000);
                        }
                    }
                }
                if (MainMenu._MainMenu.Item("JKey").GetValue<KeyBind>().Active) // JungleClear
                {
                    var JungleTarget = MinionManager.GetMinions(1100, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
                    foreach (var minion in JungleTarget)
                    {
                        if (_Q.IsReady() && MainMenu._MainMenu.Item("JUse_Q").GetValue<bool>() && minion != null && Environment.TickCount > QTime)
                        {
                            _Q.CastIfHitchanceEquals(minion, HitChance.Medium, true);
                            QTime = TickCount(1500);
                        }
                        if (_E.IsReady() && MainMenu._MainMenu.Item("JUse_E").GetValue<bool>() && minion != null && Environment.TickCount > ETime
                            && !Orbwalking.CanAttack() && Orbwalking.CanMove(10))
                        {
                            _E.Cast(true);
                            ETime = TickCount(1500);
                        }
                        if (_W.IsReady() && MainMenu._MainMenu.Item("JUse_W").GetValue<bool>() && minion != null && Environment.TickCount > WTime
                            && !Orbwalking.CanAttack() && Orbwalking.CanMove(10))
                        {
                            _W.Cast(Player, true);
                            WTime = TickCount(1500);
                        }
                    }
                }
                if (MainMenu._MainMenu.Item("InsecKick").GetValue<KeyBind>().Active)    // 인섹킥
                {
                    var GetTarget = TargetSelector.GetSelectedTarget();
                    if (GetTarget == null || GetTarget.IsDead) return;

                    var Turrets = ObjectManager.Get<Obj_Turret>()
                    .OrderBy(obj => obj.Position.Distance(Player.Position))
                    .FirstOrDefault(obj => obj.IsAlly && obj.Health > 1);
                    var AllyChampion = ObjectManager.Get<Obj_AI_Hero>().FirstOrDefault(obj => obj.IsAlly && !obj.IsMe && !obj.IsDead && obj.Distance(Player.Position) < 2000);
                    if (Turrets == null && AllyChampion == null) return;
                    if (AllyChampion != null)
                    {
                        InsecST = AllyChampion.Position;
                    }
                    else
                    {
                        InsecST = Turrets.Position;
                    }
                    InsecED = GetTarget.Position;
                    InsecPOS = InsecST.Extend(InsecED, +InsecED.Distance(InsecST) + 230);
                    Player.IssueOrder(GameObjectOrder.MoveTo, InsecPOS);
                    //Drawing.DrawText(200, 200, Color.White, InsecType);

                    if (MainMenu._MainMenu.Item("KickAndFlash").GetValue<bool>() && InsecPOS.Distance(Player.Position) < 425
                        && GetTarget.Distance(Player.Position) < 375 && InsecType == "Wait" && _R.Level > 0 && _R.IsReady() &&
                        InsecType != "WF" && InsecType != "WF1" && Player.GetSpellSlot("SummonerFlash").IsReady())
                    {
                        InsecTime = TickCount(2000);
                        InsecText = "Flash";
                        InsecType = "RF";
                        _R.Cast(GetTarget, true);
                        return;
                    }
                    if (InsecPOS.Distance(Player.Position) < 575 && _R.Level > 0 && _R.IsReady() && InsecType != "RF")
                    {
                        InsecText = "Ward";
                        if (InsecType == "Wait" && InsecType != "WF" && InsecType != "WF1" && _W.IsReady())
                        {
                            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "blindmonkwtwo") return;
                            InsecTime = TickCount(2000);
                            InsecType = "WF";
                            var Ward = Items.GetWardSlot();
                            Player.Spellbook.CastSpell(Ward.SpellSlot, InsecPOS);
                        }
                        if (InsecType == "WF" && _W.IsReady())
                        {
                            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "blindmonkwtwo") return;
                            var WardObj = ObjectManager.Get<Obj_AI_Base>()  // 커서근처 와드 유무
                                    .OrderBy(obj => obj.Distance(InsecPOS))
                                    .FirstOrDefault(obj => obj.IsAlly && !obj.IsMe
                                    && obj.Distance(InsecPOS) <= 110 && obj.Name.ToLower().Contains("ward"));
                            if (WardObj != null)
                            {
                                InsecType = "WF1";
                                _W.Cast(WardObj, true);
                            }
                        }
                        if (InsecType == "WF1")
                        {
                            if (GetTarget.Distance(Player.Position) < 375)
                            {
                                _R.Cast(GetTarget, true);
                            }
                            else
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                            }
                        }
                        return;
                    }

                    // 플 425, 와드 575
                }
                if (MainMenu._MainMenu.Item("Ward_W").GetValue<KeyBind>().Active)   // 와드 방호
                {
                    //와드방호는 WW로 정의
                    var Cursor = Game.CursorPos;
                    var Ward = Items.GetWardSlot();
                    Player.IssueOrder(GameObjectOrder.MoveTo, Cursor);
                    if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "blindmonkwtwo") return;
                    if (Player.Distance(Cursor) > 700) Cursor = Game.CursorPos.Extend(Player.Position, +Player.Distance(Game.CursorPos) - 700);
                    //Render.Circle.DrawCircle(Cursor, 50, Color.Black, 2);
                    //Drawing.DrawText(200, 200, Color.White, "WW is: " + WW.ToString());                    
                    if (_W.IsReady())
                    {
                        var Object = ObjectManager.Get<Obj_AI_Hero>().FirstOrDefault(obj => obj.IsAlly && !obj.IsMe && obj.Distance(Cursor) < 110); // 커서근처 챔프유무
                        var Minion = MinionManager.GetMinions(Cursor, 110, MinionTypes.All, MinionTeam.Ally); // 아군 미니언 유무
                        var WardObj = ObjectManager.Get<Obj_AI_Base>()  // 커서근처 와드 유무
                                .OrderBy(obj => obj.Distance(Cursor))
                                .FirstOrDefault(obj => obj.IsAlly && !obj.IsMe
                                && obj.Distance(Cursor) <= 110 && obj.Name.ToLower().Contains("ward"));
                        if (WardObj != null)
                        {
                            _W.Cast(WardObj, true);
                            Ward_Time = TickCount(2000);
                            WW = true;
                            return;
                        }
                        if (Object != null)
                        {
                            _W.Cast(Object, true);
                            Ward_Time = TickCount(2000);
                            WW = true;
                            return;
                        }
                        if (Minion != null)
                        {
                            foreach (var minion in Minion)
                            {
                                if (minion != null)
                                {
                                    _W.Cast(minion, true);
                                    Ward_Time = TickCount(2000);
                                    WW = true;
                                    return;
                                }
                            }
                        }
                        if (Player.Distance(Cursor) > 575) Cursor = Game.CursorPos.Extend(Player.Position, +Player.Distance(Game.CursorPos) - 575);
                        //Render.Circle.DrawCircle(Cursor, 50, Color.Black, 2);                            
                        if (WW && Ward != null && Ward_Time < Environment.TickCount)
                        {
                            Player.Spellbook.CastSpell(Ward.SpellSlot, Cursor);
                            WW = false;
                            Ward_Time = TickCount(2000);
                        }
                        WardObj = ObjectManager.Get<Obj_AI_Base>()  // 커서근처 와드 유무
                                .OrderBy(obj => obj.Distance(Cursor))
                                .FirstOrDefault(obj => obj.IsAlly && !obj.IsMe
                                && obj.Distance(Cursor) <= 110 && obj.Name.ToLower().Contains("ward"));
                        if (WardObj != null)
                        {
                            _W.Cast(WardObj, true);
                            Ward_Time = TickCount(2000);
                            WW = true;
                            return;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshLeesin is not working. plz send message by KorFresh (Code 4)");
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            try
            {
                if (Player.IsDead) return;
                if (MainMenu._MainMenu.Item("Draw_Q").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _Q.Range, Color.White, 1);
                if (MainMenu._MainMenu.Item("Draw_W").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _W.Range, Color.White, 1);
                if (MainMenu._MainMenu.Item("Draw_E").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _E.Range, Color.White, 1);
                if (MainMenu._MainMenu.Item("Draw_Ward").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, 575, Color.White, 1);
                //if (TargetSelector.GetSelectedTarget() != null) Render.Circle.DrawCircle(TargetSelector.GetSelectedTarget().Position, 375, Color.Green, 2);                
                if (MainMenu._MainMenu.Item("InsecKick").GetValue<KeyBind>().Active && TargetSelector.GetSelectedTarget() != null && MainMenu._MainMenu.Item("PredictR").GetValue<bool>())
                {
                    var GetTarget = TargetSelector.GetSelectedTarget();
                    if (GetTarget == null || GetTarget.IsDead) return;
                    var Turrets = ObjectManager.Get<Obj_Turret>()
                    .OrderBy(obj => obj.Position.Distance(Player.Position))
                    .FirstOrDefault(obj => obj.IsAlly && !obj.IsDead);
                    var AllyChampion = ObjectManager.Get<Obj_AI_Hero>().FirstOrDefault(obj => obj.IsAlly && !obj.IsMe && !obj.IsDead && obj.Distance(Player.Position) < 2000);
                    if (Turrets == null && AllyChampion == null) return;
                    if (AllyChampion != null)
                    { var InsecPOS = InsecST.Extend(InsecED, +InsecED.Distance(InsecST) + 230); }
                    Render.Circle.DrawCircle(InsecPOS, 50, Color.Gold);
                    if (GetTarget.Distance(Player.Position) < 575)
                    {
                        Render.Circle.DrawCircle(Player.Position, 475, Color.LightGreen);
                    }
                    else
                    {
                        Render.Circle.DrawCircle(Player.Position, 475, Color.IndianRed);
                    }
                    Drawing.DrawLine(Drawing.WorldToScreen(InsecST)[0], Drawing.WorldToScreen(InsecST)[1], Drawing.WorldToScreen(InsecED)[0], Drawing.WorldToScreen(InsecED)[1], 2, Color.Green);
                }
                // test                             
                // test
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshLeesin is not working. plz send message by KorFresh (Code 5)");
            }
        }

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            try
            {
                if (MainMenu._MainMenu.Item("AntiGab").GetValue<Slider>().Value == 0 || !_R.IsReady()) return;
                var Anti = MainMenu._MainMenu.Item("AntiGab").GetValue<Slider>().Value;
                var Myhp = Player.HealthPercent;
                var Target = gapcloser.Sender;
                Console.Write(Target.ChampionName);
                if (Target != null && Player.Distance(Target) < 375 && Myhp < Anti && _R.Level > 0 && _R.IsReady()) _R.Cast(Target, true);
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshLeesin is not working. plz send message by KorFresh (Code 6)");
            }
        }

        private static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                foreach (var Me in HeroManager.Allies)
                {
                    if (Me.ChampionName == Player.ChampionName)
                    {
                        //if (MainMenu._MainMenu.Item("KickAndFlash").GetValue<KeyBind>().Active && args.SData.Name == "BlindMonkRKick")                        
                        //if (MainMenu._MainMenu.Item("KickAndFlash").GetValue<KeyBind>().Active && args.SData.Name == "summonerflash") RF = true;
                        if (MainMenu._MainMenu.Item("InsecKick").GetValue<KeyBind>().Active && args.SData.Name == "BlindMonkRKick" && InsecType == "RF" && InsecType != "WF" && InsecType != "WF1")
                        {
                            var Flash = Player.GetSpellSlot("SummonerFlash");
                            Player.Spellbook.CastSpell(Flash, InsecPOS, true);
                        }
                        //if (MainMenu._MainMenu.Item("InsecKick").GetValue<KeyBind>().Active && args.SData.Name == "blindmonkwtwo")
                    }
                    if (sender.IsMe)
                    {
                        Console.Write("Spell Name: " + args.SData.Name + "\n");
                    }
                }

            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshLeesin is not working. plz send message by KorFresh (Code 7)");
            }
        }

        private static int TickCount(int time)
        {
            return Environment.TickCount + time;
        }

        private static void AutoKick()
        {
            if (MainMenu._MainMenu.Item("AutoKick").GetValue<Slider>().Value == 0 || MainMenu._MainMenu.Item("InsecKick").GetValue<KeyBind>().Active) return;

            var target =
                HeroManager.Enemies.Where(x => x.Distance(Player) < 375 && !x.IsDead && x.IsValidTarget(375))
                    .OrderBy(x => x.Distance(Player)).FirstOrDefault();
            if (target == null) return;

            var ultPoly = new Geometry.Polygon.Rectangle(Player.ServerPosition,
                Player.ServerPosition.Extend(target.Position, 1100),
                target.BoundingRadius + 10);

            var count =
                HeroManager.Enemies.Where(x => x.Distance(Player) < 1100 && x.IsValidTarget(1100))
                    .Count(h => h.NetworkId != target.NetworkId && ultPoly.IsInside(h.ServerPosition));

            if (count >= MainMenu._MainMenu.Item("AutoKick").GetValue<Slider>().Value && _R.IsReady())
            {
                _R.Cast(target);
            }
        }

        static float getComboDamage(Obj_AI_Base enemy)
        {
            if (enemy != null)
            {
                float damage = 0;

                if (_Q.IsReady())
                    damage += _Q.GetDamage(enemy);
                if (_W.IsReady())
                    damage += _W.GetDamage(enemy);
                if (_E.IsReady())
                    damage += _E.GetDamage(enemy);
                if (_R.IsReady())
                    damage += _R.GetDamage(enemy);

                if (!Player.IsWindingUp)
                    damage += (float)Player.GetAutoAttackDamage(enemy, true);

                return damage;
            }
            return 0;
        }

        static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (MainMenu._MainMenu.Item("Indicator").GetValue<bool>())
                {
                    Indicator.unit = enemy;
                    Indicator.drawDmg(getComboDamage(enemy), new ColorBGRA(255, 204, 0, 160));
                }
            }
        }
    }
}