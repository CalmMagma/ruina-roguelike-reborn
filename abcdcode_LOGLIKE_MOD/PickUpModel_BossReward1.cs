// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BossReward1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_BossReward1 : PickUpModelBase
{
  public PickUpModel_BossReward1()
  {
    this.Name = TextDataModel.GetText("BossReward1Name");
    this.Desc = TextDataModel.GetText("BossReward1Desc");
    this.FlaverText = TextDataModel.GetText("BossRewardFlaverText");
    this.ArtWork = "BossReward1";
  }

  public override void OnPickUp()
  {
    base.OnPickUp();
    Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase) new PickUpModel_BossReward1.BossReward1Effect());
  }

  public class BossReward1Effect : GlobalLogueEffectBase
  {
    public int stack;

    public BossReward1Effect() => this.stack = 1;

    public override void LoadFromSaveData(SaveData save)
    {
      base.LoadFromSaveData(save);
      this.stack = save.GetInt("stack");
    }

    public override SaveData GetSaveData()
    {
      SaveData saveData = base.GetSaveData();
      saveData.AddData("stack", this.stack);
      return saveData;
    }

    public override string GetEffectName() => TextDataModel.GetText("BossReward1Name");

    public override string GetEffectDesc()
    {
      return TextDataModel.GetText("BossReward1Desc_Effect", (object) (this.stack * 2));
    }

    public override Sprite GetSprite() => LogLikeMod.ArtWorks["BossReward1"];

    public override void AddedNew()
    {
      ++this.stack;
      Singleton<GlobalLogueEffectManager>.Instance.UpdateSprites();
    }

    public override int ChangeSuccCostValue() => this.stack * 2;
  }
}
}
