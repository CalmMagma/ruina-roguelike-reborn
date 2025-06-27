using System;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using CustomMapUtility;
using abcdcode_LOGLIKE_MOD;


namespace RogueLike_Mod_Reborn
{
    public class EnemyTeamStageManager_AutoStartMysteryStage : EnemyTeamStageManager
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            Singleton<MysteryManager>.Instance.StartMystery(LogLikeMod.curstageid);
        }
    }

    public class SparklingMirrorMapManager : CustomMapManager
    {

    }

}
