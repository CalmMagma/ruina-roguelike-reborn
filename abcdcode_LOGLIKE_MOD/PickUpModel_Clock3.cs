// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Clock3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Clock3 : CreaturePickUpModel
{
  public PickUpModel_Clock3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Clock3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Clock3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Clock3_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370193), (EmotionCardAbilityBase) new PickUpModel_Clock3.LogEmotionCardAbility_Clock3(), model);
  }

  public class LogEmotionCardAbility_Clock3 : EmotionCardAbilityBase
  {
    public const int _id1 = 1100012;
    public const int _id2 = 1100013;

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this.GiveCardsToDeck();
    }

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this.GiveCards();
    }

    public void GiveCards()
    {
      this._owner.allyCardDetail.AddNewCard(1100012);
      this._owner.allyCardDetail.AddNewCard(1100013);
    }

    public void GiveCardsToDeck()
    {
      this._owner.allyCardDetail.AddNewCardToDeck(1100012);
      this._owner.allyCardDetail.AddNewCardToDeck(1100013);
    }
  }
}
}
