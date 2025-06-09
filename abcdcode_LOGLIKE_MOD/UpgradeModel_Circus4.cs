// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.UpgradeModel_Circus4
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

public class UpgradeModel_Circus4 : UpgradeBase
{
  public override void Init()
  {
    this.upgradeinfo = new UpgradeBase.UpgradeInfo();
    this.upgradeinfo.SetDice(0, 2, 1);
    this.upgradeinfo.SetDice(1, 1, 1);
    this.baseid = new LorId(LogLikeMod.ModId, 503004);
  }

  public override DiceCardXmlInfo GetUpgradeInfo(int index, int count)
  {
    this.upxmlinfo = base.GetUpgradeInfo(index, count);
    this.upxmlinfo.optionList.Remove(CardOption.ExhaustOnUse);
    return this.upxmlinfo;
  }
}
}
