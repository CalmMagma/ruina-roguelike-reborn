// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BigBadWolf3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_BigBadWolf3 : CreaturePickUpModel
{
  public PickUpModel_BigBadWolf3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_BigBadWolf3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_BigBadWolf3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_BigBadWolf3_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370163), (EmotionCardAbilityBase) new PickUpModel_BigBadWolf3.LogEmotionCardAbility_BigBadWolf3(), model);
  }

  public class LogEmotionCardAbility_BigBadWolf3 : EmotionCardAbilityBase
  {
    public const int _powMin = 1;
    public const int _powMax = 2;
    public const int _vulnMin = 1;
    public const int _vulnMax = 2;
    public const int _cntMax = 3;
    public BattleUnitModel target;
    public int cnt;

    public static int Pow => RandomUtil.Range(1, 2);

    public static int Vuln => RandomUtil.Range(1, 2);

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      this.cnt = 0;
    }

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Wolf_Filter_Eye", false, 2f);
    }

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      this.target = (BattleUnitModel) null;
      if (behavior.Type != BehaviourType.Standby)
        return;
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = PickUpModel_BigBadWolf3.LogEmotionCardAbility_BigBadWolf3.Pow
      });
      this.target = behavior.card?.target;
    }

    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      base.OnWinParrying(behavior);
      if (this.cnt >= 3)
      {
        this.target = (BattleUnitModel) null;
      }
      else
      {
        if (behavior.Type != BehaviourType.Standby || this.target == null)
          return;
        ++this.cnt;
        this.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, PickUpModel_BigBadWolf3.LogEmotionCardAbility_BigBadWolf3.Vuln, this._owner);
        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Wolf_Scratch");
        this.target = (BattleUnitModel) null;
      }
    }
  }
}
}
