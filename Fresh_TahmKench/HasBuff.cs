using LeagueSharp;

namespace Fresh_TahmKench
{    
    class HasBuff
    {        
        public static bool HasBuffsofCC(Obj_AI_Hero Hero)
        {
            if (Hero.HasBuffOfType(BuffType.Flee))
                return true;
            if (Hero.HasBuffOfType(BuffType.Charm))
                return true;
            if (Hero.HasBuffOfType(BuffType.Polymorph))
                return true;
            if (Hero.HasBuffOfType(BuffType.Snare))
                return true;
            if (Hero.HasBuffOfType(BuffType.Stun))
                return true;
            if (Hero.HasBuffOfType(BuffType.Taunt))
                return true;
            if (Hero.HasBuffOfType(BuffType.Suppression))
                return true;            
            return false;
        }
    }
}
