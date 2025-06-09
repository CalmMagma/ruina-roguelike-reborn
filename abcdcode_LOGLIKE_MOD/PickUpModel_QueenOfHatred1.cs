// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_QueenOfHatred1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_QueenOfHatred1 : CreaturePickUpModel
{
  public PickUpModel_QueenOfHatred1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_QueenOfHatred1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_QueenOfHatred1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_QueenOfHatred1_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370051), (EmotionCardAbilityBase) new PickUpModel_QueenOfHatred1.LogEmotionCardAbility_QueenOfHatred1(), model);
  }

  public class LogEmotionCardAbility_QueenOfHatred1 : EmotionCardAbilityBase
  {
    public const int _hpMin = 3;
    public const int _hpMax = 5;
    public const int _hpTarget = 1;
    public const int _brMin = 2;
    public const int _brMax = 4;
    public const int _brTarget = 1;

    public static int RecoverHP => RandomUtil.Range(3, 5);

    public static int RecoverBreak => RandomUtil.Range(2, 4);

    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      base.OnWinParrying(behavior);
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MagicalGirl_kiss");
      this._owner.battleCardResultLog?.SetEmotionAbilityEffect("5/MagicalGirl_Heart");
      if (this.IsAttackDice(behavior.Detail))
      {
        foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_random(this._owner.faction, 1))
          battleUnitModel.RecoverHP(PickUpModel_QueenOfHatred1.LogEmotionCardAbility_QueenOfHatred1.RecoverHP);
      }
      else
      {
        if (!this.IsDefenseDice(behavior.Detail))
          return;
        int num = 1;
        List<BattleUnitModel> battleUnitModelList = new List<BattleUnitModel>();
        List<BattleUnitModel> list = new List<BattleUnitModel>();
        foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
        {
          if (!alive.IsBreakLifeZero())
            list.Add(alive);
        }
        while (list.Count > 0 && num > 0)
        {
          BattleUnitModel battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(list);
          list.Remove(battleUnitModel);
          if (!battleUnitModel.IsBreakLifeZero())
          {
            battleUnitModelList.Add(battleUnitModel);
            --num;
          }
        }
        foreach (BattleUnitModel battleUnitModel in battleUnitModelList)
          battleUnitModel.breakDetail.RecoverBreak(PickUpModel_QueenOfHatred1.LogEmotionCardAbility_QueenOfHatred1.RecoverBreak);
      }
    }

    public override void OnPrintEffect(BattleDiceBehavior behavior) => base.OnPrintEffect(behavior);
  }
}
}
