// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_SmallBird2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_SmallBird2 : CreaturePickUpModel
{
  public PickUpModel_SmallBird2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_SmallBird2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_SmallBird2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_SmallBird2_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370182), (EmotionCardAbilityBase) new PickUpModel_SmallBird2.LogEmotionCardAbility_SmallBird2(), model);
  }

  public class LogEmotionCardAbility_SmallBird2 : EmotionCardAbilityBase
  {
    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this.GiveBuf();
    }

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this.GiveBuf();
    }

    public void GiveBuf()
    {
      if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_SmallBird2.LogEmotionCardAbility_SmallBird2.BattleUnitBuf_Emotion_SmallBird_Buri)) != null)
        return;
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_SmallBird2.LogEmotionCardAbility_SmallBird2.BattleUnitBuf_Emotion_SmallBird_Buri());
    }

    public class BattleUnitBuf_Emotion_SmallBird_Buri : BattleUnitBuf
    {
      public const int _stackMax = 10;
      public bool dmged;

      public override string keywordId => "Smallbird_Beak";

      public override string keywordIconId => "SmallBird_Emotion_Buri";

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        this.stack = 0;
      }

      public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
      {
        base.OnTakeDamageByAttack(atkDice, dmg);
        this.dmged = true;
        this.stack = 0;
      }

      public override void BeforeGiveDamage(BattleDiceBehavior behavior)
      {
        base.BeforeGiveDamage(behavior);
        if (this.stack <= 0)
          return;
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          dmg = this.stack
        });
      }

      public override void OnSuccessAttack(BattleDiceBehavior behavior)
      {
        base.OnSuccessAttack(behavior);
        BattleUnitModel target = behavior?.card?.target;
        if (target == null || this.stack <= 0)
          return;
        target.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Attack", 2f);
        target.battleCardResultLog?.SetCreatureEffectSound("Creature/SmallBird_Atk");
      }

      public override void OnRoundEndTheLast()
      {
        base.OnRoundEndTheLast();
        if (this.dmged)
        {
          this.dmged = false;
          this.stack = 0;
        }
        else
        {
          ++this.stack;
          if (this.stack <= 10)
            return;
          this.stack = 10;
        }
      }
    }
  }
}
}
