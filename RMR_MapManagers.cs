using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomMapUtility;

namespace RogueLike_Mod_Reborn
{
    public class SparklingMirrorMapManager : CustomMapManager
    {

    }
    public class SparklingMirrorStageManager : EnemyTeamStageManager
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            RMRCore.RMRMapHandler.InitCustomMap<SparklingMirrorMapManager>("SparklingMirrorMapManager");
        }

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            RMRCore.RMRMapHandler.EnforceMap(0);
        }
    }
}
