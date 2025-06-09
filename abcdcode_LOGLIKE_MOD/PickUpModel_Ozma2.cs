// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Ozma2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Ozma2 : CreaturePickUpModel
{
  public PickUpModel_Ozma2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Ozma2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Ozma2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Ozma2_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370372), (EmotionCardAbilityBase) new PickUpModel_Ozma2.LogEmotionCardAbility_Ozma2(), model);
  }

  public class LogEmotionCardAbility_Ozma2 : EmotionCardAbilityBase
  {
    public const int _id = 1100014;

    public override void OnSelectEmotion() => this._owner.allyCardDetail.AddNewCard(1100014);

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this._owner.allyCardDetail.AddNewCardToDeck(1100014);
    }
  }
}
}
