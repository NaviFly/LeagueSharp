using System;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;

namespace Fresh_Veigar
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
                if (MainMenu._MainMenu.Item("Draw_R").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _R.Range, Color.White, 1);
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshVeigar is not working. plz send message by KorFresh (Code 5)");
            }
        }
    }
}
