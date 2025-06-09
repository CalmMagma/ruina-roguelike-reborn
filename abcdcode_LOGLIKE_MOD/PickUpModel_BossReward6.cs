// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BossReward6
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_BossReward6 : PickUpModelBase
{
  public PickUpModel_BossReward6()
  {
    this.Name = TextDataModel.GetText("BossReward6Name");
    this.Desc = TextDataModel.GetText("BossReward6Desc");
    this.FlaverText = TextDataModel.GetText("BossRewardFlaverText");
    this.ArtWork = "BossReward6";
  }

  public override bool IsCanPickUp(UnitDataModel target)
  {
    return Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find((Predicate<GlobalLogueEffectBase>) (x => x is PickUpModel_ShopGood23.Shop23Effect)) != null && Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find((Predicate<GlobalLogueEffectBase>) (x => x is PickUpModel_BossReward6.FourCloverEffect)) == null;
  }

  public override void OnPickUp()
  {
    base.OnPickUp();
    if (Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find((Predicate<GlobalLogueEffectBase>) (x => x is PickUpModel_ShopGood23.Shop23Effect)) == null)
      return;
    Singleton<GlobalLogueEffectManager>.Instance.RemoveEffect(Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find((Predicate<GlobalLogueEffectBase>) (x => x is PickUpModel_ShopGood23.Shop23Effect)));
    Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase) new PickUpModel_BossReward6.FourCloverEffect());
  }

  public class FourCloverEffect : GlobalLogueEffectBase
  {
    public override Sprite GetSprite() => LogLikeMod.ArtWorks["BossReward6"];

    public override string GetEffectName() => TextDataModel.GetText("BossReward6_1Name");

    public override string GetEffectDesc() => TextDataModel.GetText("BossReward6_1Desc");

    public override int ChangeSuccCostValue() => 1;

    public override void OnRoundStart(StageController stage)
    {
      base.OnRoundStart(stage);
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
      {
        LuckyBuf.GiveLuckyThisRound(alive, 1);
        SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfileAll();
      }
    }
  }
}
}
