// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Pinocchio2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Pinocchio2 : CreaturePickUpModel
{
  public PickUpModel_Pinocchio2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Pinocchio2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Pinocchio2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Pinocchio2_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370202), (EmotionCardAbilityBase) new PickUpModel_Pinocchio2.LogEmotionCardAbility_Pinocchio2(), model);
  }

  public class LogEmotionCardAbility_Pinocchio2 : EmotionCardAbilityBase
  {
    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      SoundEffectPlayer.PlaySound("Creature/Pino_Lie");
      this.SetFilter("0/Pinocchio_Emotion_Select");
    }

    public override void OnRoundStart()
    {
      foreach (BattleDiceCardModel battleDiceCardModel in this._owner.allyCardDetail.GetAllDeck())
      {
        if (battleDiceCardModel.GetOriginCost() <= 3)
          battleDiceCardModel.AddBufWithoutDuplication((BattleDiceCardBuf) new EmotionCardAbility_pinocchio2.RandomCostBuf());
      }
    }

    public class RandomCostBuf : BattleDiceCardBuf
    {
      public int _cost;

      public override DiceCardBufType bufType => DiceCardBufType.Mirror;

      public RandomCostBuf() => this._cost = RandomUtil.Range(0, 3);

      public override int GetCost(int oldCost) => this._cost;
    }
  }
}
}
