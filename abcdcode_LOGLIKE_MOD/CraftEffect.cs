// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.CraftEffect
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using UI;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class CraftEffect : GlobalLogueEffectBase
    {
        public virtual bool IsNormal() => true;

        public virtual string GetCraftName() => "Test";

        public virtual string GetCraftDesc() => "Test";

        public virtual Sprite GetCraftSprite() => (Sprite)null;

        public virtual int GetCraftCost() => 0;

        public virtual bool CanCraft(int costresult) => LogueBookModels.GetMoney() >= costresult;

        public virtual void Crafting()
        {
            LogueBookModels.SubMoney((int)((double)this.GetCraftCost() * (double)Singleton<GlobalLogueEffectManager>.Instance.CraftCostMultiple(this)));
            UIBattleSettingPanel uiPanel = UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel;
            UnitDataModel fieldValue = LogLikeMod.GetFieldValue<UnitDataModel>((object)uiPanel.InfoRightPanel, "unitdata");
            uiPanel.SetLibrarianProfileData(fieldValue);
        }

        public static List<RewardPassiveInfo> CheckCreaftEquipLimit(ChapterGrade grade)
        {
            List<RewardPassiveInfo> chapterData = Singleton<RewardPassivesList>.Instance.GetChapterData(grade, PassiveRewardListType.CommonReward, LorId.None, true);
            foreach (RewardPassiveInfo rewardPassiveInfo in chapterData.ToArray())
            {
                RewardPassiveInfo rinfo = rewardPassiveInfo;
                if (LogueBookModels.booklist.FindAll(x => x.ClassInfo.id == rinfo.id).Count >= 5)
                    chapterData.Remove(rinfo);
            }
            return chapterData.Count == 0 ? (List<RewardPassiveInfo>)null : chapterData;
        }

        public static void CraftEquipByChapter(ChapterGrade grade)
        {
            RewardPassiveInfo reward = RewardingModel.GetReward(CraftEffect.CheckCreaftEquipLimit(grade));
            BookXmlInfo data = Singleton<BookXmlList>.Instance.GetData(reward.id);
            LogueBookModels.AddBook(reward.id);
            UIAlarmPopup.instance.SetAlarmText(TextDataModel.GetText("CraftEquipResult", (object)data.InnerName));
        }
    }
}
