using System;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;

namespace Fresh_Ryze
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
                if (Player.IsDead) return;
                if (MainMenu._MainMenu.Item("QRange").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _Q.Range, Color.White, 1);
                if (MainMenu._MainMenu.Item("WRange").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _W.Range, Color.White, 1);
                if (MainMenu._MainMenu.Item("ERange").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _E.Range, Color.White, 1);

                var PlayerPosition = ObjectManager.Player.Position;
                var PlayerPosition2 = Drawing.WorldToScreen(PlayerPosition);
                if (MainMenu._MainMenu.Item("DisplayStack").GetValue<bool>() && Program.RyzePassive != null)
                    Drawing.DrawText(PlayerPosition2.X - 20, PlayerPosition2.Y - 120, Color.Gold, "Stack: " + Program.RyzeStack);
                if (MainMenu._MainMenu.Item("DisplayTime").GetValue<bool>() && Program.RyzePassive != null)
                {
                    if (Program.RyzePassive.EndTime - Game.ClockTime > 2f)
                        Drawing.DrawText(PlayerPosition2.X - 20, PlayerPosition2.Y + 30, Color.Gold, "Stack Time: " + "{0:#.##}", Program.RyzePassive.EndTime - Game.ClockTime);
                    else
                        Drawing.DrawText(PlayerPosition2.X - 20, PlayerPosition2.Y + 30, Color.Red, "Stack Time: " + "{0:#.##}", Program.RyzePassive.EndTime - Game.ClockTime);
                }
                if (MainMenu._MainMenu.Item("AutoStackText").GetValue<bool>())
                    Drawing.DrawText(PlayerPosition2.X - 20, PlayerPosition2.Y + 45, Color.Gold, "Auto Stack: " + (MainMenu._MainMenu.Item("AutoKey").GetValue<KeyBind>().Active ? "On" : "Off"));
                if (TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Magical) != null)
                    Drawing.DrawCircle(TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Magical).Position, 150, Color.Green);
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshRyze is not working. plz send message by KorFresh (Code 5)");
            }
        }
    }
}
