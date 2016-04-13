using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeagueSharp.Common;

namespace TheCheater
{
    class Program
    {
        static void Main()
        {
            CustomEvents.Game.OnGameLoad += (args) => new TheCheater().Load();
        }
    }
}
