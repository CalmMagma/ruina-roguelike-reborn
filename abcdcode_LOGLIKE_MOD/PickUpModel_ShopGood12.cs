// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGood12
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ShopGood12 : ShopPickUpModel
{
  public PickUpModel_ShopGood12()
  {
    this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570012));
    this.Name = LogueEffectXmlList.AutoLocalizeVanillaName((PickUpModelBase) this, this.KeywordId);
    this.Desc = LogueEffectXmlList.AutoLocalizeVanillaDesc((PickUpModelBase) this, this.KeywordId);
    this.id = new LorId(LogLikeMod.ModId, 90012);
  }

  public override bool IsCanPickUp(UnitDataModel target)
  {
    return base.IsCanPickUp(target) && !target.IsDead();
  }

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(new LorId(LogLikeMod.ModId, 90012)));
    if (playingCard != null)
    {
      foreach (BattleDiceBehavior diceCardBehavior in playingCard.CreateDiceCardBehaviorList())
      {
        diceCardBehavior.behaviourInCard.Min = (int) (2 + LogLikeMod.curchaptergrade);
        diceCardBehavior.behaviourInCard.Dice = (int) (5 + LogLikeMod.curchaptergrade);
        model.cardSlotDetail.keepCard.AddBehaviourForOnlyDefense(playingCard, diceCardBehavior);
        SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(model, model.faction, model.hp, model.breakDetail.breakGauge, model.bufListDetail.GetBufUIDataList());
      }
    }
    Singleton<LogueSaveManager>.Instance.AddToObtainCount((object) this, -1);
  }

  public override void OnPickUpShop(ShopGoods good)
  {
    Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase) new PickUpModel_ShopGood12.TrashShield());
  }

  public string KeywordId => "GlobalEffect_ScrapShield";

  public class TrashShield : OnceEffect
  {
    public static Rarity ItemRarity = Rarity.Uncommon;

    public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive12"];

    public override string GetEffectName()
    {
      return LogueEffectXmlList.AutoLocalizeVanillaName((GlobalLogueEffectBase) this, this.KeywordId);
    }

    public override string GetEffectDesc()
    {
      return LogueEffectXmlList.AutoLocalizeVanillaDesc((GlobalLogueEffectBase) this, this.KeywordId);
    }

    public override void OnClick()
    {
      base.OnClick();
      if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
        return;
      ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 90012));
      this.Use();
    }

    public string KeywordId => "GlobalEffect_ScrapShield";
  }
}
}
