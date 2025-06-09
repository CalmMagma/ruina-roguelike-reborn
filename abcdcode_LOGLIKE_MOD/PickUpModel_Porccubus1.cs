// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Porccubus1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using Sound;
using System;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Porccubus1 : CreaturePickUpModel
{
  public PickUpModel_Porccubus1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Porccubus1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Porccubus1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Porccubus1_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370241), (EmotionCardAbilityBase) new PickUpModel_Porccubus1.LogEmotionCardAbility_Porccubus1(), model);
  }

  public class LogEmotionCardAbility_Porccubus1 : EmotionCardAbilityBase
  {
    public const int _dmgMin = 2;
    public const int _dmgMax = 7;
    public const int _brkDmgMin = 2;
    public const int _brkDmgMax = 7;

    public static int Dmg => RandomUtil.Range(2, 7);

    public static int BrkDmg => RandomUtil.Range(2, 7);

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      base.OnSucceedAttack(behavior);
      if (!this.IsAttackDice(behavior.Detail) || behavior.Detail != BehaviourDetail.Penetrate)
        return;
      BattleUnitModel target = behavior.card?.target;
      if (target != null)
      {
        BattleUnitBuf battleUnitBuf = target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is EmotionCardAbility_porccubus.BattleUnitBuf_Emotion_Porccubus_Happy));
        if (battleUnitBuf != null)
        {
          if ((battleUnitBuf as EmotionCardAbility_porccubus.BattleUnitBuf_Emotion_Porccubus_Happy).Add())
          {
            target.TakeDamage(PickUpModel_Porccubus1.LogEmotionCardAbility_Porccubus1.Dmg, DamageType.Emotion, this._owner);
            target.TakeBreakDamage(PickUpModel_Porccubus1.LogEmotionCardAbility_Porccubus1.BrkDmg, DamageType.Emotion, this._owner);
            target.battleCardResultLog?.SetTakeDamagedEvent(new BattleCardBehaviourResult.BehaviourEvent(this.BloodFilter));
          }
          else
          {
            target.battleCardResultLog?.SetCreatureAbilityEffect("3/Porccubuss_Delight", 1f);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Porccu_Penetrate");
          }
        }
        else
        {
          target.bufListDetail.AddBuf((BattleUnitBuf) new EmotionCardAbility_porccubus.BattleUnitBuf_Emotion_Porccubus_Happy());
          target.battleCardResultLog?.SetCreatureAbilityEffect("3/Porccubuss_Delight", 1f);
          target.battleCardResultLog?.SetCreatureEffectSound("Creature/Porccu_Penetrate");
        }
      }
    }

    public void BloodFilter()
    {
      Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/7/Lumberjack_final_blood_1st");
      if ((UnityEngine.Object) original != (UnityEngine.Object) null)
      {
        Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
        if (((UnityEngine.Object) creatureEffect != (UnityEngine.Object) null ? (UnityEngine.Object) creatureEffect.gameObject.GetComponent<AutoDestruct>() : (UnityEngine.Object) null) == (UnityEngine.Object) null)
        {
          AutoDestruct autoDestruct = (UnityEngine.Object) creatureEffect != (UnityEngine.Object) null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct) null;
          if ((UnityEngine.Object) autoDestruct != (UnityEngine.Object) null)
          {
            autoDestruct.time = 3f;
            autoDestruct.DestroyWhenDisable();
          }
        }
      }
      SoundEffectPlayer.PlaySound("Creature/Porccu_Special");
    }

    public void Filter()
    {
      new GameObject().AddComponent<SpriteFilter_Porccubus_Special>().Init("EmotionCardFilter/Porccubus_Filter", false, 1f);
    }

    public class BattleUnitBuf_Emotion_Porccubus_Happy : BattleUnitBuf
    {
      public const int _fullStack = 3;

      public override string keywordId => "Porccubus_Happy";

      public bool Add()
      {
        ++this.stack;
        if (this.stack < 3)
          return false;
        this.stack = 0;
        return true;
      }

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        if (this.stack > 0)
          return;
        this.Destroy();
      }
    }
  }
}
}
