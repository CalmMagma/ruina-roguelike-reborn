// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShyLookToday1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Battle.CreatureEffect;
using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ShyLookToday1 : CreaturePickUpModel
{
  public PickUpModel_ShyLookToday1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ShyLookToday1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ShyLookToday1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ShyLookToday1_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370031), (EmotionCardAbilityBase) new PickUpModel_ShyLookToday1.LogEmotionCardAbility_ShyLookToday1(), model);
  }

  public class LogEmotionCardAbility_ShyLookToday1 : EmotionCardAbilityBase
  {
    public int _currentFace = 1;
    public float elap;
    public float freq = 1f;
    public CreatureEffect_Emotion_Face face;

    public int CurrentFace
    {
      get => this._currentFace;
      set
      {
        this._currentFace = value;
        this.SetFace();
      }
    }

    public void SetFace()
    {
      if ((Object) this.face == (Object) null)
        return;
      this.face.SetFace(this.CurrentFace);
    }

    public void GenFace()
    {
      if ((Object) this.face != (Object) null)
        Object.Destroy((Object) this.face.gameObject);
      Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("3_H/FX_IllusionCard_3_H_Look", 1f, this._owner.view, this._owner.view);
      this.face = (Object) fxCreatureEffect != (Object) null ? fxCreatureEffect.GetComponent<CreatureEffect_Emotion_Face>() : (CreatureEffect_Emotion_Face) null;
      this.SetFace();
    }

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this.CurrentFace = RandomUtil.Range(0, 4);
      this.GenFace();
    }

    public override void OnSelectEmotion()
    {
      this.CurrentFace = RandomUtil.Range(0, 4);
      this.GenFace();
      SoundEffectPlayer.PlaySound("Creature/Shy_Smile");
    }

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      int num = 0;
      switch (this.CurrentFace)
      {
        case 0:
          num = -2;
          break;
        case 1:
          num = -1;
          break;
        case 2:
          num = 0;
          break;
        case 3:
          num = 1;
          break;
        case 4:
          num = 2;
          break;
      }
      if (num == 0)
        return;
      this._owner.battleCardResultLog?.SetEmotionAbility(false, this._emotionCard, 0, ResultOption.Sign, num);
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = num
      });
    }

    public override void OnFixedUpdateInWaitPhase(float delta)
    {
      this.elap += delta;
      if ((double) this.elap <= (double) this.freq)
        return;
      this.CurrentFace = RandomUtil.Range(0, 4);
      this.elap = 0.0f;
    }
  }
}
}
