using System;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;

namespace Fresh_LeBlanc
{
    class OnDraw
    {
        private static Obj_AI_Hero Player = Program.Player;
        private static Spell _Q = Program._Q;
        private static Spell _W = Program._W;
        private static Spell _E = Program._E;
        private static Spell _R = Program._R;
        public static void Drawing_OnDraw(EventArgs args)
        {
            try
            {
                if (Player.IsDead)
                    return;
                if (MainMenu._MainMenu.Item("Draw_Q").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _Q.Range, Color.White, 1);
                if (MainMenu._MainMenu.Item("Draw_W").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _W.Range, Color.White, 1);
                if (MainMenu._MainMenu.Item("Draw_E").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _E.Range, Color.White, 1);
                if (MainMenu._MainMenu.Item("Draw_WR").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, 1400, Color.White, 1);
                if (Program.WTime > Environment.TickCount && MainMenu._MainMenu.Item("WTimer").GetValue<bool>())
                    Drawing.DrawText(Drawing.WorldToScreen(Player.Position)[0] - 20, Drawing.WorldToScreen(Player.Position)[1] + 20, Color.YellowGreen, "W Time is : " + ((Program.WTime - Environment.TickCount) / 10));
                if (Program.RTime > Environment.TickCount && MainMenu._MainMenu.Item("WTimer").GetValue<bool>())
                    Drawing.DrawText(Drawing.WorldToScreen(Player.Position)[0] - 20, Drawing.WorldToScreen(Player.Position)[1] + 30, Color.YellowGreen, "R Time is : " + ((Program.RTime - Environment.TickCount) / 10));
                if (MainMenu._MainMenu.Item("REnable").GetValue<bool>())
                {
                    string Status_R = string.Empty;
                    if (Player.Spellbook.GetSpell(SpellSlot.R).Name == "LeblancChaosOrbM")
                        Status_R = "_Q";
                    if (Player.Spellbook.GetSpell(SpellSlot.R).Name == "LeblancSlideM")
                        Status_R = "_W";
                    if (Player.Spellbook.GetSpell(SpellSlot.R).Name == "leblancslidereturnm")
                        Status_R = "_W (Return)";
                    if (Player.Spellbook.GetSpell(SpellSlot.R).Name == "LeblancSoulShackleM")
                        Status_R = "_E";
                    Drawing.DrawText(Player.HPBarPosition.X + 30, Player.HPBarPosition.Y - 40, Color.IndianRed, "R: " + Status_R);
                }
                if (MainMenu._MainMenu.Item("Draw_ComboMode").GetValue<bool>())
                {
                    var drawtext = MainMenu._MainMenu.Item("ComboMode").GetValue<KeyBind>().Active ? "TeamFight" : "Private";
                    Drawing.DrawText(Player.HPBarPosition.X + 40, Player.HPBarPosition.Y + 30, Color.White, drawtext);
                }
                if (TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Magical) != null)
                    Drawing.DrawCircle(TargetSelector.GetTarget(1400, TargetSelector.DamageType.Magical).Position, 150, Color.Green);
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshLeblanc is not working. plz send message by KorFresh (Code 5)");
            }
        }
    }
}
