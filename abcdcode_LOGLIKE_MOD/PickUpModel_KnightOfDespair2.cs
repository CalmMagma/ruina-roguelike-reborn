// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_KnightOfDespair2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_KnightOfDespair2 : CreaturePickUpModel
{
  public PickUpModel_KnightOfDespair2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_KnightOfDespair2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_KnightOfDespair2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_KnightOfDespair2_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370152), (EmotionCardAbilityBase) new PickUpModel_KnightOfDespair2.LogEmotionCardAbility_KnightOfDespair2(), model);
  }

  public class LogEmotionCardAbility_KnightOfDespair2 : EmotionCardAbilityBase
  {
    public const int _powerMin = 1;
    public const int _powerMax = 3;
    public const int _quickMin = 1;
    public const int _quickMax = 3;
    public const int _vulne = 2;
    public int stack;
    public int tempStack;
    public SpriteFilter_Despair _filter;

    public static int Power => RandomUtil.Range(1, 3);

    public static int Quick => RandomUtil.Range(1, 3);

    public override void OnDieOtherUnit(BattleUnitModel unit)
    {
      base.OnDieOtherUnit(unit);
      if (unit.faction != Faction.Player)
        return;
      ++this.stack;
    }

    public override void OnRoundStartOnce()
    {
      base.OnRoundStartOnce();
      if (this.tempStack > 0)
      {
        foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
        {
          for (int index = 0; index < this.tempStack; ++index)
            alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Vulnerable, 2);
        }
        foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
        {
          for (int index = 0; index < this.tempStack; ++index)
          {
            alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, PickUpModel_KnightOfDespair2.LogEmotionCardAbility_KnightOfDespair2.Power, alive);
            alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, PickUpModel_KnightOfDespair2.LogEmotionCardAbility_KnightOfDespair2.Quick, alive);
          }
        }
        if ((Object) this._filter == (Object) null)
        {
          this._filter = new GameObject().AddComponent<SpriteFilter_Despair>();
          this._filter.Init("EmotionCardFilter/KnightOfDespair_Gaho", true, 1f);
        }
      }
      this.tempStack = 0;
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      if ((Object) this._filter != (Object) null)
      {
        this._filter.ManualDestroy();
        this._filter = (SpriteFilter_Despair) null;
      }
      this.tempStack = this.stack;
      this.stack = 0;
    }

    public override void OnBattleEnd()
    {
      base.OnBattleEnd();
      if (!((Object) this._filter != (Object) null))
        return;
      this._filter.ManualDestroy();
      this._filter = (SpriteFilter_Despair) null;
    }
  }
}
}
