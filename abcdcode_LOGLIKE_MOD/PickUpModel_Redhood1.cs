// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Redhood1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Redhood1 : CreaturePickUpModel
{
  public PickUpModel_Redhood1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Redhood1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Redhood1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Redhood1_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370061), (EmotionCardAbilityBase) new PickUpModel_Redhood1.LogEmotionCardAbility_Redhood1(), model);
  }

  public class LogEmotionCardAbility_Redhood1 : EmotionCardAbilityBase
  {
    public const int _dmgMin = 2;
    public const int _dmgMax = 6;
    public BattleUnitModel _target;

    public static int Dmg => RandomUtil.Range(2, 6);

    public override void OnWaveStart() => base.OnWaveStart();

    public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
    {
      base.OnUseCard(curCard);
      if (this._target != null || curCard.GetDiceBehaviorList().Find((Predicate<BattleDiceBehavior>) (x => x.Type == BehaviourType.Atk)) == null)
        return;
      this._target = curCard.target;
      this._target.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_Redhood1.LogEmotionCardAbility_Redhood1.BattleUnitBuf_redhood_prey());
      this._target.battleCardResultLog?.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_Hunted", 1.5f);
      this._target.battleCardResultLog?.SetCreatureEffectSound("Creature/RedHood_Gun");
    }

    public override void BeforeGiveDamage(BattleDiceBehavior behavior)
    {
      base.BeforeGiveDamage(behavior);
      BattleUnitModel target = behavior.card?.target;
      if (target == null || target != this._target)
        return;
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        dmg = PickUpModel_Redhood1.LogEmotionCardAbility_Redhood1.Dmg
      });
    }

    public class BattleUnitBuf_redhood_prey : BattleUnitBuf
    {
      public override string keywordId => "RedHood_Hunt";

      public override string keywordIconId => "Redhood_Target";

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        this.stack = 0;
      }
    }
  }
}
}
