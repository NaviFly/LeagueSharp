using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;

namespace _xcsoft__Information
{
    internal class Program
    {
        static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        static Obj_AI_Hero Target;

        static Menu Menu;

        static Render.Text Text;

        const string Font = "monospace";

        const string NewLine = "\n";

        static Spell Q, W, E, R;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            Q = new Spell(SpellSlot.Q);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E);
            R = new Spell(SpellSlot.R);

            Menu = new Menu("[xcsoft] Information", "xcsoft_information", true);
            TargetSelector.AddToMenu(Menu.AddSubMenu(new Menu("Target Selector", "Target Selector")));
            Menu.AddToMainMenu();

            Menu.AddItem(new MenuItem("switch", "Switch")).SetValue<Boolean>(true);
            Menu.AddItem(new MenuItem("x", "X")).SetValue<Slider>(new Slider(250, 0, Drawing.Width));
            Menu.AddItem(new MenuItem("y", "Y")).SetValue<Slider>(new Slider(0, 0, Drawing.Height));
            Menu.AddItem(new MenuItem("size", "Size")).SetValue<Slider>(new Slider(11, 10, 20));

            Menu.Item("x").ValueChanged +=
                delegate (object sender, OnValueChangeEventArgs eventArgs)
                {
                    Text.X = eventArgs.GetNewValue<Slider>().Value;
                };

            Menu.Item("y").ValueChanged +=
                delegate (object sender, OnValueChangeEventArgs eventArgs)
                {
                    Text.Y = eventArgs.GetNewValue<Slider>().Value;
                };

            Menu.Item("size").ValueChanged +=
                delegate (object sender, OnValueChangeEventArgs eventArgs)
                {
                    Text = new Render.Text(Menu.Item("x").GetValue<Slider>().Value, Menu.Item("y").GetValue<Slider>().Value, "", Menu.Item("size").GetValue<Slider>().Value, SharpDX.Color.White, Font);
                };

            Text = new Render.Text(Menu.Item("x").GetValue<Slider>().Value, Menu.Item("y").GetValue<Slider>().Value, "", Menu.Item("size").GetValue<Slider>().Value, SharpDX.Color.White, Font);

            Drawing.OnDraw += Drawing_OnDraw;
        }

        static void Drawing_OnDraw(EventArgs args)
        {
            if (!Menu.Item("switch").GetValue<Boolean>())
                return;

            Target = Hud.SelectedUnit != null ? Hud.SelectedUnit as Obj_AI_Hero : Player;

            var buffs = string.Empty;

            foreach (var buff in Target.Buffs)
            {
                buffs += "\n" + buff.Name + "[Count: " + buff.Count + "/Duration: " + (buff.EndTime - Game.ClockTime).ToString("0.00") + "],";
            }

            var Mobs = MinionManager.GetMinions(1500, MinionTypes.All, MinionTeam.NotAlly, MinionOrderTypes.MaxHealth);
            var MobsList = string.Empty;

            foreach (var Mob in Mobs)
            {
                MobsList += "\n" + Mob.Name + "[BaseSkin: " + Mob.CharData.BaseSkinName + "/HP: " + Mob.Health + " / " + Mob.MaxHealth + "(" + Mob.HealthPercent.ToString("0.0") + "%)],";
            }

            Render.Circle.DrawCircle(Player.Position, 1500, Color.Red, 10);

            Text.text =
                //"Name: " + Target.Name + NewLine +
                "ChampionName: " + Target.ChampionName + NewLine +
                "SkinName: " + Target.SkinName + NewLine +
                //"Gold: " + Target.Gold + NewLine +
                //"Level: " + Target.Level + NewLine +
                "TotalAttackDamage: " + Utility.TotalAttackDamage(Target) + NewLine +
                "TotalMagicalDamage: " + Utility.TotalMagicalDamage(Target) + NewLine +
                "Armor: " + Target.Armor + NewLine +
                "Health: " + Target.Health + " / " + Target.MaxHealth + " (" + Target.HealthPercent.ToString("0.0") + "%)" + NewLine +
                "Mana: " + Target.Mana + " / " + Target.MaxMana + " (" + Target.HealthPercent.ToString("0.0") + "%)" + NewLine +
                "HPRegenRate: " + Target.HPRegenRate + NewLine +
                "PARRegenRate: " + Target.PARRegenRate + NewLine +
                "Experience: " + Target.Experience + NewLine +
                "Position: " + Target.Position + NewLine +
                "ServerPosition: " + Target.ServerPosition + NewLine +
                "Team: " + Target.Team + NewLine +
                "NetworkId: " + Target.NetworkId + NewLine +
                "MoveSpeed: " + Target.MoveSpeed + NewLine +
                "AttackRange: " + Target.AttackRange + NewLine +
                "RealAutoAttackRange: " + Orbwalking.GetRealAutoAttackRange(Target) + NewLine +
                //"DeathDuration: " + Target.DeathDuration + NewLine +
                "BoundingRadius: " + Target.BoundingRadius + NewLine +
                NewLine +
                "Buffs: " + buffs + NewLine +
                NewLine +
                "IsDead: " + Target.IsDead + NewLine +
                "IsImmovable: " + Target.IsImmovable + NewLine +
                "IsInvulnerable: " + Target.IsInvulnerable + NewLine +
                "IsMoving: " + Target.IsMoving + NewLine +
                "IsPacified: " + Target.IsPacified + NewLine +
                "IsTargetable: " + Target.IsTargetable + NewLine +
                "IsWindingUp: " + Target.IsWindingUp + NewLine +
                "IsZombie: " + Target.IsZombie + NewLine +
                "IsRecalling: " + Target.IsRecalling() + NewLine +
                "IsStunned: " + Target.IsStunned + NewLine +
                "IsRooted: " + Target.IsRooted + NewLine +
                "IsMelee: " + Target.IsMelee() + NewLine +
                "IsDashing: " + Target.IsDashing() + NewLine +
                "IsAlly: " + Target.IsAlly + NewLine +
                //"IsCanMove: " + Orbwalking.CanMove(1) + NewLine +
                "UnderTurret: " + Target.UnderTurret() + NewLine +
                NewLine +
                "Mobs: " + MobsList + NewLine +
                NewLine +
                "Game_CursorPos: " + Game.CursorPos + NewLine +
                "Game_ClockTime: " + Game.ClockTime + NewLine +
                "Game_Time: " + Game.Time + NewLine +
                "Game_Type: " + Game.Type + NewLine +
                "Game_Version: " + Game.Version + NewLine +
                "Game_Region: " + Game.Region + NewLine +
                "Game_IP: " + Game.IP + NewLine +
                "Game_Port: " + Game.Port + NewLine +
                "Game_Ping: " + Game.Ping + NewLine +
                "Game_Mode: " + Game.Mode + NewLine +
                "Game_MapId: " + Game.MapId + NewLine +
                "Game_MapName: " + Utility.Map.GetMap().Name
                ;


            Text.OnEndScene();
        }
    }
}
