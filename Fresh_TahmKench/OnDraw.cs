using System;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;

namespace Fresh_TahmKench
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
                if (MainMenu._MainMenu.Item("RRange").GetValue<bool>() && _R.Level > 0)
                {
                    var Rint = (_R.Level * 1000) + 3000;
                    Render.Circle.DrawCircle(Player.Position, _R.Range, Color.White, 1);
                }                
                var QTarget = TargetSelector.GetTarget(_Q.Range, TargetSelector.DamageType.Magical);
                if (QTarget != null)
                    Drawing.DrawCircle(QTarget.Position, 100, Color.Gold);                
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshTahmKench is not working. plz send message by KorFresh (Code 5)");
            }
        }
    }
}
