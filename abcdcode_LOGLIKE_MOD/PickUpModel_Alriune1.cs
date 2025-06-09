// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Alriune1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Alriune1 : CreaturePickUpModel
{
  public PickUpModel_Alriune1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Alriune1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Alriune1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Alriune1_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370341), (EmotionCardAbilityBase) new PickUpModel_Alriune1.LogEmotionCardAbility_Alriune1(), model);
  }

  public class LogEmotionCardAbility_Alriune1 : EmotionCardAbilityBase
  {
    public const int healMin = 5;
    public const int healMax = 10;
    public const int _bDmgMin = 4;
    public const int _bDmgMax = 8;
    public const float _hpRate = 0.1f;
    public int _dmgStack;

    public int Heal => RandomUtil.Range(5, 10);

    public int BDmg => RandomUtil.Range(4, 8);

    public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
    {
      if (this._owner.IsImmuneDmg())
        return base.BeforeTakeDamage(attacker, dmg);
      if (this._owner.passiveDetail.IsInvincible())
        return base.BeforeTakeDamage(attacker, dmg);
      this._dmgStack += dmg;
      if ((double) this._dmgStack >= (double) this._owner.MaxHp * 0.10000000149011612)
        this.Ability();
      return base.BeforeTakeDamage(attacker, dmg);
    }

    public void Ability()
    {
      this._dmgStack = 0;
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
        alive.TakeBreakDamage(this.BDmg, DamageType.Emotion, this._owner);
      this._owner.RecoverHP(this.Heal);
      if (!Singleton<StageController>.Instance.IsLogState())
      {
        SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_FlowerPiece", 1f, this._owner.view, this._owner.view, 2f);
        SoundEffectPlayer.PlaySound("Creature/Ali_FarAtk");
      }
      else
      {
        this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("4_N/FX_IllusionCard_4_N_FlowerPiece", 2f);
        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Ali_FarAtk");
      }
    }
  }
}
}
