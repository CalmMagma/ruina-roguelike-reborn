// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_LongBird3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_LongBird3 : CreaturePickUpModel
{
  public PickUpModel_LongBird3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_LongBird3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_LongBird3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_LongBird3_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370283), (EmotionCardAbilityBase) new PickUpModel_LongBird3.LogEmotionCardAbility_LongBird3(), model);
  }

  public class LogEmotionCardAbility_LongBird3 : EmotionCardAbilityBase
  {
    public const int _healMin = 2;
    public const int _healMax = 8;
    public const int _cntMax = 3;
    public int cnt;

    public static int Heal => RandomUtil.Range(2, 8);

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      this.cnt = 0;
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      base.OnSucceedAttack(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null || this.cnt >= 3)
        return;
      int maxHp = this.GetMaxHP();
      if ((int) target.hp < maxHp)
        return;
      ++this.cnt;
      target.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Scale", 2f);
      target.battleCardResultLog?.SetCreatureEffectSound("Creature/LongBird_On");
      this.GetHealTarget()?.RecoverHP(PickUpModel_LongBird3.LogEmotionCardAbility_LongBird3.Heal);
    }

    public BattleUnitModel GetHealTarget()
    {
      BattleUnitModel healTarget = (BattleUnitModel) null;
      List<BattleUnitModel> list = new List<BattleUnitModel>();
      int num = -100;
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
      {
        if (num == -100)
        {
          list.Add(alive);
          num = (int) alive.hp;
        }
        else if ((int) alive.hp < num)
        {
          list.Clear();
          list.Add(alive);
          num = (int) alive.hp;
        }
        else if ((int) alive.hp == num)
          list.Add(alive);
      }
      if (list.Count > 0)
        healTarget = RandomUtil.SelectOne<BattleUnitModel>(list);
      return healTarget;
    }

    public int GetMaxHP()
    {
      float maxHp = 0.0f;
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
      {
        if ((double) alive.hp > (double) maxHp)
          maxHp = alive.hp;
      }
      return (int) maxHp;
    }
  }
}
}
