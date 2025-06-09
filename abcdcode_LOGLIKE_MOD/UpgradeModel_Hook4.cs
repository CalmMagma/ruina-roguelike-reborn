// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.UpgradeModel_Hook4
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

public class UpgradeModel_Hook4 : UpgradeBase
{
  public override void Init()
  {
    this.upgradeinfo = new UpgradeBase.UpgradeInfo();
    this.upgradeinfo.AddDice(1, 5, BehaviourDetail.Slash, "recoverHp1atk", BehaviourType.Atk);
    this.baseid = new LorId(LogLikeMod.ModId, 104006);
  }
}
}
