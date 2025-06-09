// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Mountain2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Mountain2 : CreaturePickUpModel
{
  public PickUpModel_Mountain2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Mountain2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Mountain2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Mountain2_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370262), (EmotionCardAbilityBase) new PickUpModel_Mountain2.LogEmotionCardAbility_Mountain2(), model);
  }

  public class LogEmotionCardAbility_Mountain2 : EmotionCardAbilityBase
  {
    public const float _hpRateCond = 0.5f;
    public const int _maxCnt = 3;
    public const int _bDmgMin = 2;
    public const int _bDmgMax = 5;
    public const int _bufStack = 1;
    public int cnt;

    public static int BDmg => RandomUtil.Range(2, 5);

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      this.cnt = 0;
    }

    public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
    {
      base.OnTakeDamageByAttack(atkDice, dmg);
      if (this.cnt >= 3 || !this.CheckHP())
        return;
      ++this.cnt;
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
      {
        if (!alive.IsExtinction())
        {
          alive.TakeBreakDamage(PickUpModel_Mountain2.LogEmotionCardAbility_Mountain2.BDmg, DamageType.Emotion, this._owner);
          alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 1, this._owner);
        }
      }
      this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_Shout", 3f);
      this._owner.battleCardResultLog?.SetTakeDamagedEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Damaged));
    }

    public void Damaged()
    {
      CameraFilterUtil.EarthQuake(0.08f, 0.02f, 50f, 0.3f);
      SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Danggo_Lv2_Shout");
    }

    public bool CheckHP() => (double) this._owner.hp <= (double) this._owner.MaxHp * 0.5;
  }
}
}
