// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.UpgradeModel_Gaze2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class UpgradeModel_Gaze2 : UpgradeBase
{
  public override void Init()
  {
    this.upgradeinfo = new UpgradeBase.UpgradeInfo();
    this.upgradeinfo.SetDice(0, 1, 1);
    this.upgradeinfo.SetDice(3, 1, 1);
    this.upgradeinfo.SetAbility(1, "powerUp2targetParalysisLog");
    this.upgradeinfo.SetAbility(2, "powerUp2targetParalysisLog");
    this.upgradeinfo.SetAbility(3, "powerUp2targetParalysisLog");
    this.baseid = new LorId(LogLikeMod.ModId, 404002);
  }
}
}
