using System;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;

namespace FreshPoppy
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
                if (MainMenu._MainMenu.Item("Draw_Q").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _Q.Range, Color.White, 1);
                if (MainMenu._MainMenu.Item("Draw_E").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _E.Range, Color.White, 1);
                if (MainMenu._MainMenu.Item("Draw_R").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _R.Range, Color.White, 1);                    
                if (MainMenu._MainMenu.Item("Draw_E2").GetValue<bool>())
                {
                    var ETarget = TargetSelector.GetTarget(_E.Range, TargetSelector.DamageType.Magical);
                    if(ETarget != null)
                    {
                        var FinalPosition = ETarget.Position.Extend(ObjectManager.Player.Position, -360);
                        Drawing.DrawCircle(FinalPosition, 75, Color.Gold);
                    }
                }
                if (MainMenu._MainMenu.Item("Draw_R").GetValue<bool>())
                    Render.Circle.DrawCircle(Player.Position, _R.Range, Color.White, 1);
                if(Program.shield.ShieldMe != null)
                {
                    Render.Circle.DrawCircle(Program.shield.ShieldMe.Position, 100, Color.Red,10);                    
                }                
            }
            catch (Exception e)
            {
                Console.Write(e);
                Game.PrintChat("FreshPoppy is not working. plz send message by KorFresh (Code 5)");
            }
        }
    }
}
