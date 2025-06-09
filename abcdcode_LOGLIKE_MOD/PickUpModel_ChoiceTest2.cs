// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ChoiceTest2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ChoiceTest2 : PickUpModelBase
{
  public PickUpModel_ChoiceTest2()
  {
    this.Name = "버버티기";
    this.Desc = this.Name + " 책장을 손으로 가져온다";
    this.FlaverText = "든-든";
  }

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    DiceCardSelfAbility_choiceTest_0.GiveCard(new LorId(103004));
  }
}
}
