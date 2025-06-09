// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LuckyBuf
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;

 
namespace abcdcode_LOGLIKE_MOD {

public class LuckyBuf : BattleUnitBuf
{
  public override string keywordId => "LogueLikeMod_LuckyBuf";

  public static void ChangeDiceResult(BattleDiceBehavior behavior, int level, ref int diceResult)
  {
    int diceMin = behavior.GetDiceMin();
    int diceMax = behavior.GetDiceMax();
    int num1 = diceResult;
    for (int index = 0; index < level; ++index)
    {
      int num2 = DiceStatCalculator.MakeDiceResult(diceMin, diceMax, 0);
      if (num2 > num1)
        num1 = num2;
    }
    diceResult = num1;
  }

  public override void Init(BattleUnitModel owner)
  {
    base.Init(owner);
    typeof (BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue((object) this, (object) LogLikeMod.ArtWorks["buff_Lucky"]);
    typeof (BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue((object) this, (object) true);
  }

  public override void OnRoundEnd()
  {
    base.OnRoundEnd();
    this.Destroy();
  }

  public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
  {
    LuckyBuf.ChangeDiceResult(behavior, this.stack, ref diceResult);
  }

  public static LuckyBuf IshaveBuf(BattleUnitModel target, bool findready = false)
  {
    foreach (BattleUnitBuf activatedBuf in target.bufListDetail.GetActivatedBufList())
    {
      if (activatedBuf is LuckyBuf)
        return activatedBuf as LuckyBuf;
    }
    if (findready)
    {
      foreach (BattleUnitBuf readyBuf in target.bufListDetail.GetReadyBufList())
      {
        if (readyBuf is LuckyBuf)
          return readyBuf as LuckyBuf;
      }
    }
    return (LuckyBuf) null;
  }

  public static void GiveLuckyThisRound(BattleUnitModel target, int stack)
  {
    LuckyBuf luckyBuf = LuckyBuf.IshaveBuf(target);
    if (luckyBuf != null)
    {
      luckyBuf.stack += stack;
    }
    else
    {
      LuckyBuf buf = new LuckyBuf();
      buf.stack = stack;
      buf.Init(target);
      target.bufListDetail.AddBuf((BattleUnitBuf) buf);
    }
  }
}
}
