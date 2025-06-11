// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.RewardInfo
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    public class RewardInfo
    {
        public ChapterGrade grade;
        public List<RewardPassiveInfo> rewards;

        public RewardInfo()
        {
            this.grade = ChapterGrade.Grade1;
            this.rewards = new List<RewardPassiveInfo>();
        }

        public static RewardInfo GetCurChapterCommonReward(ChapterGrade grade)
        {
            return new RewardInfo()
            {
                grade = grade,
                rewards = Singleton<RewardPassivesList>.Instance.GetChapterData(grade, PassiveRewardListType.CommonReward, LorId.None)
            };
        }

        public static RewardInfo GetCurChapterEliteReward(ChapterGrade grade)
        {
            return new RewardInfo()
            {
                grade = grade,
                rewards = Singleton<RewardPassivesList>.Instance.GetChapterData(grade, PassiveRewardListType.EliteReward, LorId.None)
            };
        }

        public static RewardInfo GetCurChapterBossReward(ChapterGrade grade)
        {
            return new RewardInfo()
            {
                grade = grade,
                rewards = Singleton<RewardPassivesList>.Instance.GetChapterData(grade, PassiveRewardListType.BossReward, LorId.None)
            };
        }

        public static RewardInfo GetCurChapterCreature(ChapterGrade grade)
        {
            return new RewardInfo()
            {
                grade = grade,
                rewards = Singleton<RewardPassivesList>.Instance.GetChapterData(grade, PassiveRewardListType.Creature, LorId.None)
            };
        }
    }
}
