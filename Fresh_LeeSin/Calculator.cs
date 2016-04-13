using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace FreshLeeSin
{
    class Calculator
    {
        private static Obj_AI_Hero Player;
        public static bool Calculation(Vector3 Seltarget, Vector3 Cursor, int type)
        {
            if (type == 1)  // 궁플
            {
                if (Player.Distance(Cursor) > 425)      // 커서 사거리 정렬
                {
                    Cursor = Game.CursorPos.Extend(Player.Position, +Player.Distance(Game.CursorPos) - 425);
                }
                else if (Player.Distance(Cursor) < 170)
                {
                    Cursor = Game.CursorPos.Extend(Player.Position, +Player.Distance(Game.CursorPos) + 170);
                }
                else
                {
                    Cursor = Game.CursorPos;
                }
            }
            else if (type ==2)  // 인섹킥
            {
                if (Player.Distance(Cursor) > 575)      // 커서 사거리 정렬
                {
                    Cursor = Game.CursorPos.Extend(Player.Position, +Player.Distance(Game.CursorPos) - 575);
                }
                else if (Player.Distance(Cursor) < 170)
                {
                    Cursor = Game.CursorPos.Extend(Player.Position, +Player.Distance(Game.CursorPos) + 170);
                }
                else
                {
                    Cursor = Game.CursorPos;
                }
            }

            var cursor_me = Player.Distance(Cursor);
            var target_me = Player.Distance(Seltarget);
            var cul = cursor_me - target_me;
            if (cul > 100)  // 방호 유효 사거리면
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
