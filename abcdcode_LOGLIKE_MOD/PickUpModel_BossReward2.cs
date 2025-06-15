// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BossReward2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_BossReward2 : PickUpModelBase
    {
        public static int[] EquipRewardTable = new int[6]
        {
    1,
    2,
    3,
    4,
    5,
    5
        };
        public static int[] CardRewardTable = new int[6]
        {
    3,
    5,
    7,
    9,
    11,
    13
        };

        public PickUpModel_BossReward2()
        {
            int index = (int)LogLikeMod.curchaptergrade;
            if (index > 5)
                index = 5;
            this.Name = TextDataModel.GetText("BossReward2Name");
            this.Desc = TextDataModel.GetText("BossReward2Desc", (object)PickUpModel_BossReward2.CardRewardTable[index], (object)PickUpModel_BossReward2.EquipRewardTable[index]);
            this.FlaverText = TextDataModel.GetText("BossRewardFlaverText");
            this.ArtWork = "BossReward2_1";
            switch (LogLikeMod.curchaptergrade)
            {
                case ChapterGrade.Grade1:
                case ChapterGrade.Grade2:
                    this.ArtWork = "BossReward2_1";
                    break;
                case ChapterGrade.Grade3:
                    this.ArtWork = "BossReward2_2";
                    break;
                case ChapterGrade.Grade4:
                case ChapterGrade.Grade5:
                    this.ArtWork = "BossReward2_" + ((double)Random.value > 0.5 ? "3" : "4");
                    break;
                default:
                    this.ArtWork = "BossReward2_1";
                    break;
            }
        }

        public override void OnPickUp()
        {
            base.OnPickUp();
            int index1 = (int)LogLikeMod.curchaptergrade;
            if (index1 > 5)
                index1 = 5;
            for (int index2 = 0; index2 < PickUpModel_BossReward2.CardRewardTable[index1]; ++index2)
            {
                int num1 = 1;
                float num2 = Random.value;
                if ((double)num2 > 0.89999997615814209)
                    num1 = 4;
                else if ((double)num2 > 0.699999988079071)
                    num1 = 3;
                else if ((double)num2 > 0.40000000596046448)
                    num1 = 2;
                LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 1000 * (index1 + 1) + num1)));
            }
            for (int index3 = 0; index3 < PickUpModel_BossReward2.EquipRewardTable[index1]; ++index3)
                LogLikeMod.rewards_passive.Add(new RewardInfo()
                {
                    grade = LogLikeMod.curchaptergrade,
                    rewards = Singleton<RewardPassivesList>.Instance.GetChapterData(LogLikeMod.curchaptergrade, PassiveRewardListType.CommonReward, new LorId(-1))
                });
        }
    }
}
