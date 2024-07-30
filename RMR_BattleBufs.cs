using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;

namespace RogueLike_Mod_Reborn
{
    public static class RoguelikeBufs
    {
        public static KeywordBuf CritChance;
    }

    public class BattleUnitBuf_RMR_CritChance : BattleUnitBuf
    {
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.CritChance;
            }
        }
    }
}
