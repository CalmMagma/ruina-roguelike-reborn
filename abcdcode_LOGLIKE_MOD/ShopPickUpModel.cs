// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.ShopPickUpModel
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using RogueLike_Mod_Reborn;
using System;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    public class ShopPickUpModel : PickUpModelBase
    {
        public PassiveXmlInfo basepassive;

        public virtual string[] Keywords => new string[0];

        public ShopPickUpModel()
        {
            if (!string.IsNullOrEmpty(this.KeywordId))
            {
                var info = LogueEffectXmlList.Instance.GetEffectInfo(KeywordId, RMRCore.ClassIds[this.GetType().Assembly.FullName]);
                if (info != null)
                {
                    this.Name = info.Name;
                    this.Desc = info.Desc;
                    this.FlaverText = info.FlavorText;
                    this.ArtWork = KeywordIconId ?? KeywordId;
                }
            }
        }

        public static void AddEquipPage(LorId id)
        {
            BookModel bookModel = new BookModel(Singleton<BookXmlList>.Instance.GetData(id));
            bookModel.instanceId = LogueBookModels.nextinstanceid++;
            bookModel.TryGainUniquePassive();
            LogueBookModels.booklist.Add(bookModel);
        }

        public static void AddPassiveReward(RewardInfo info)
        {
            LogLikeMod.rewards_InStage.Add(info);
            if (Singleton<ShopManager>.Instance.curshop == null)
                return;
            Singleton<ShopManager>.Instance.curshop.HideShop();
        }

        public static void AddPassiveReward(LorId id)
        {
            RewardInfo rewardInfo = new RewardInfo()
            {
                grade = ChapterGrade.GradeAll,
                rewards = new List<RewardPassiveInfo>()
            };
            rewardInfo.rewards.Add(Singleton<RewardPassivesList>.Instance.GetPassiveInfo(id));
            LogLikeMod.rewards_InStage.Add(rewardInfo);
            if (Singleton<ShopManager>.Instance.curshop == null)
                return;
            Singleton<ShopManager>.Instance.curshop.HideShop();
        }

        public virtual void EditName(ref string name)
        {
        }

        public virtual void EditDesc(ref string desc)
        {
            if (this.Keywords.Length == 0)
                return;
            for (int index = 0; index < this.Keywords.Length; ++index)
            {
                desc += Environment.NewLine;
                desc += Environment.NewLine;
                desc = desc + Singleton<BattleEffectTextsXmlList>.Instance.GetEffectTextName(this.Keywords[index]) + Environment.NewLine + Singleton<BattleEffectTextsXmlList>.Instance.GetEffectTextDesc(this.Keywords[index]);
            }
        }

        public virtual ShopRewardType GetShopType()
        {
            return Singleton<RewardPassivesList>.Instance.GetPassiveInfo(this.id).shoptype;
        }

        public virtual bool IsEquipReward() => false;

        public virtual bool IsCanAddShop() => true;

        public virtual void OnPickUpShop(ShopGoods good)
        {
        }

        public override void OnPickUp() => base.OnPickUp();
    }
}
