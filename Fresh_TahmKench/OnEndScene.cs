using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Fresh_TahmKench
{
    class OnEndScene
    {
        private static Obj_AI_Hero Player = Program.Player;
        private static Spell _Q = Program._Q;
        private static Spell _W = Program._W;
        private static Spell _E = Program._E;
        private static Spell _R = Program._R;

        static float getComboDamage(Obj_AI_Base enemy)
        {
            if (enemy != null)
            {
                float damage = 0;

                if (_Q.IsReady())
                    damage += _Q.GetDamage(enemy);                
                if (!Player.IsWindingUp)
                    damage += (float)Player.GetAutoAttackDamage(enemy, true);
                return damage;
            }
            return 0;
        }

        public static void Drawing_OnEndScene(EventArgs args)
        {
            if (Player.IsDead) return;
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (MainMenu._MainMenu.Item("Indicator").GetValue<bool>())
                {
                    Program.Indicator.unit = enemy;
                    Program.Indicator.drawDmg(getComboDamage(enemy), new ColorBGRA(255, 204, 0, 160));
                }
            }            
        }
    }
}
