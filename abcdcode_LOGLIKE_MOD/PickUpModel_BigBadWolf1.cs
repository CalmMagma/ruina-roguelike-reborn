// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BigBadWolf1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_BigBadWolf1 : CreaturePickUpModel
{
  public PickUpModel_BigBadWolf1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_BigBadWolf1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_BigBadWolf1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_BigBadWolf1_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370161), (EmotionCardAbilityBase) new PickUpModel_BigBadWolf1.LogEmotionCardAbility_BigBadWolf1(), model);
  }

  public class LogEmotionCardAbility_BigBadWolf1 : EmotionCardAbilityBase
  {
    public const int _powMin = 1;
    public const int _powMax = 2;
    public const int _healMin = 3;
    public const int _healMax = 7;
    public BattleDiceBehavior last;
    public bool win;

    public static int Pow => RandomUtil.Range(1, 2);

    public static int Heal => RandomUtil.Range(3, 7);

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Wolf_Filter_Sheep", false, 2f);
    }

    public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
    {
      base.OnUseCard(curCard);
      this.win = false;
      if (curCard == null)
        return;
      BattleDiceBehavior[] array = curCard.cardBehaviorQueue?.ToArray();
      if (array != null && array.Length != 0)
        this.last = array[array.Length - 1];
    }

    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      base.OnWinParrying(behavior);
      BattlePlayingCardDataInUnitModel card = behavior?.card;
      if (card == null || behavior == this.last)
        return;
      this.win = true;
      card.ApplyDiceStatBonus(DiceMatch.LastDice, new DiceStatBonus()
      {
        power = PickUpModel_BigBadWolf1.LogEmotionCardAbility_BigBadWolf1.Pow
      });
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      base.OnSucceedAttack(behavior);
      if (behavior != this.last)
        return;
      SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Wolf_Bite");
      if (this.win)
        this._owner.RecoverHP(PickUpModel_BigBadWolf1.LogEmotionCardAbility_BigBadWolf1.Heal);
    }
  }
}
}
