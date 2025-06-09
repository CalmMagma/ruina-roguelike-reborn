// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.GlobalLogueEffectBase
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class GlobalLogueEffectBase : Savable
{
  public virtual SaveData GetSaveData()
  {
    SaveData saveData = new SaveData();
    saveData.AddData("TypeName", new SaveData(this.GetType().Name));
    return saveData;
  }

  public static GlobalLogueEffectBase CreateGlobalEffectBySave(SaveData save)
  {
    string stringSelf = save.GetData("TypeName").GetStringSelf();
    Debug.Log((object) ("CGEBS tryfind : " + stringSelf));
    foreach (Assembly assem in LogLikeMod.GetAssemList())
    {
      foreach (System.Type type in assem.GetTypes())
      {
        if (type.Name == stringSelf)
        {
          Debug.Log((object) ("CGEBS find : " + stringSelf));
          return Activator.CreateInstance(type) as GlobalLogueEffectBase;
        }
      }
    }
    return (GlobalLogueEffectBase) null;
  }

  public virtual void LoadFromSaveData(SaveData save)
  {
  }

  public virtual void AddedNew()
  {
  }

  public virtual bool CanDupliacte() => false;

  public virtual void Destroy() => Singleton<GlobalLogueEffectManager>.Instance.RemoveEffect(this);

  public virtual void OnDestroy()
  {
  }

  public virtual void OnAddSubPlayer(UnitDataModel model)
  {
  }

  public virtual float CraftCostMultiple(CraftEffect effect) => 1f;

  public virtual LorId InvenAddCardChange(LorId baseid) => baseid;

  public virtual void RewardInStageInterrupt()
  {
  }

  public virtual void RewardClearStageInterrupt()
  {
  }

  public virtual void ChangeShopCard(ref DiceCardXmlInfo card)
  {
  }

  public virtual void ChangeCardReward(ref List<DiceCardXmlInfo> cardlist)
  {
  }

  public virtual int ChangeSuccCostValue() => 0;

  public virtual void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
  {
  }

  public virtual void ChangeRestChoice(MysteryBase currest, ref List<RewardPassiveInfo> choices)
  {
  }

  public virtual void OnRoundStart(StageController stage)
  {
  }

  public virtual float DmgFactor(
    BattleUnitModel model,
    int dmg,
    DamageType type = DamageType.ETC,
    KeywordBuf keyword = KeywordBuf.None)
  {
    return 1f;
  }

  public virtual void OnKillUnit(BattleUnitModel killer, BattleUnitModel target)
  {
  }

  public virtual void OnDieUnit(BattleUnitModel unit)
  {
  }

  public virtual void OnStartBattle(BattlePlayingCardDataInUnitModel card)
  {
  }

  public virtual void BeforeRollDice(BattleDiceBehavior behavior)
  {
  }

  public virtual void OnUseCard(BattlePlayingCardDataInUnitModel cardmodel)
  {
  }

  public virtual void ChangeShopCardList(ShopBase shop, ref CardDropValueXmlInfo list)
  {
  }

  public virtual void OnShopCardListCreate(ShopBase shop)
  {
  }

  public virtual void OnPickCardReward(List<DiceCardXmlInfo> cardlist, DiceCardXmlInfo pick)
  {
  }

  public virtual void OnSkipCardRewardChoose(List<DiceCardXmlInfo> cardlist)
  {
  }

  public virtual bool CanShopPurchase(ShopBase shop, ShopGoods goods) => true;

  public virtual void OnEnterShop(ShopBase shop)
  {
  }

  public virtual void OnLeaveShop(ShopBase shop)
  {
  }

  public virtual void OnEndBattle()
  {
  }

  public virtual void OnStartBattle()
  {
  }

  public virtual void OnStartBattleAfter()
  {
  }

  public virtual void OnCreateLibrarian(BattleUnitModel model)
  {
  }

  public virtual void OnCreateLibrarians()
  {
  }

  public virtual void OnClick()
  {
  }

  public virtual Sprite GetSprite() => (Sprite) null;

  public virtual string GetEffectName() => "UNKNOWN";

  public virtual string GetEffectDesc() => "Empty";

  public virtual int GetStack() => -1;

  public virtual void OnCrit(BattleUnitModel critter, BattleUnitModel target)
  {
  }
}
}
