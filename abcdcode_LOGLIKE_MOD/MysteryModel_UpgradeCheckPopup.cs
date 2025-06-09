// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_UpgradeCheckPopup
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

 
namespace abcdcode_LOGLIKE_MOD {

public class MysteryModel_UpgradeCheckPopup : MysteryBase
{
  public LorId cardid;
  public MysteryModel_UpgradeCheckPopup.CheckResult okdele;
  public MysteryModel_UpgradeCheckPopup.CheckResult nodele;

  public override void LoadFromSaveData(SaveData savedata)
  {
  }

  public static MysteryModel_UpgradeCheckPopup PopupUpgradeCheck(
    LorId cardid,
    MysteryModel_UpgradeCheckPopup.CheckResult okdele = null,
    MysteryModel_UpgradeCheckPopup.CheckResult nodele = null)
  {
    MysteryModel_UpgradeCheckPopup mystery = new MysteryModel_UpgradeCheckPopup();
    mystery.okdele = okdele;
    mystery.nodele = nodele;
    mystery.cardid = cardid;
    Singleton<MysteryManager>.Instance.AddInterrupt((MysteryBase) mystery);
    return mystery;
  }

  public override void SwapFrame(int id)
  {
    Image image = ModdingUtils.CreateImage(LogLikeMod.LogUIObjs[97].transform, "MysteryPanel_transparent", new Vector2(1f, 1f), new Vector2(0.0f, 0.0f));
    this.FrameObj.Add("Frame", image.gameObject);
    Button button1 = ModdingUtils.CreateButton(image.transform, "MysteryButton_Enable", new Vector2(1f, 1f), new Vector2(-150f, -380f), new Vector2(300f, 70f));
    this.FrameObj.Add("Ok", button1.gameObject);
    Button.ButtonClickedEvent buttonClickedEvent1 = new Button.ButtonClickedEvent();
    buttonClickedEvent1.AddListener(new UnityAction(this.OnClickOk));
    button1.onClick = buttonClickedEvent1;
    Button button2 = ModdingUtils.CreateButton(image.transform, "MysteryButton_Enable", new Vector2(1f, 1f), new Vector2(150f, -380f), new Vector2(300f, 70f));
    this.FrameObj.Add("No", button2.gameObject);
    Button.ButtonClickedEvent buttonClickedEvent2 = new Button.ButtonClickedEvent();
    buttonClickedEvent2.AddListener(new UnityAction(this.OnClickNo));
    button2.onClick = buttonClickedEvent2;
    TextMeshProUGUI textTmp1 = ModdingUtils.CreateText_TMP(button1.transform, new Vector2(0.0f, 0.0f), 30, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
    textTmp1.text = TextDataModel.GetText("CardCheckPopUp_UpgradeOk");
    textTmp1.transform.Rotate(0.0f, 0.0f, 2.5f);
    this.FrameObj.Add("OkText", textTmp1.gameObject);
    TextMeshProUGUI textTmp2 = ModdingUtils.CreateText_TMP(button2.transform, new Vector2(0.0f, 0.0f), 30, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
    textTmp2.text = TextDataModel.GetText("CardCheckPopUp_UpgradeNo");
    textTmp2.transform.Rotate(0.0f, 0.0f, 2.5f);
    this.FrameObj.Add("NoText", textTmp2.gameObject);
    LogLikeMod.UILogCardSlot curcard = LogLikeMod.UILogCardSlot.SlotCopyingByOrig();
    curcard.transform.SetParent(image.transform);
    curcard.transform.localScale = new Vector3(1.5f, 1.5f);
    curcard.transform.localPosition = new Vector3(-250f, 0.0f);
    DiceCardItemModel info = new DiceCardItemModel(ItemXmlDataList.instance.GetCardItem(this.cardid));
    curcard.SetData(info);
    curcard.txt_cardNumbers.text = "";
    curcard.selectable.SubmitEvent.RemoveAllListeners();
    curcard.selectable.SubmitEvent.AddListener((UnityAction<BaseEventData>) (e => this.OnClickCard(info)));
    curcard.selectable.SelectEvent.RemoveAllListeners();
    curcard.selectable.SelectEvent.AddListener((UnityAction<BaseEventData>) (e => this.OnPointerEnter(curcard)));
    curcard.selectable.DeselectEvent.RemoveAllListeners();
    curcard.selectable.DeselectEvent.AddListener((UnityAction<BaseEventData>) (e => this.OnPointerExit(curcard)));
    curcard.gameObject.SetActive(true);
    LogLikeMod.UILogCardSlot resultcard = LogLikeMod.UILogCardSlot.SlotCopyingByOrig();
    resultcard.transform.SetParent(image.transform);
    resultcard.transform.localScale = new Vector3(1.5f, 1.5f);
    resultcard.transform.localPosition = new Vector3(250f, 0.0f);
    info = new DiceCardItemModel(Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(this.cardid));
    resultcard.SetData(info);
    resultcard.txt_cardNumbers.text = "";
    resultcard.selectable.SubmitEvent.RemoveAllListeners();
    resultcard.selectable.SubmitEvent.AddListener((UnityAction<BaseEventData>) (e => this.OnClickCard(info)));
    resultcard.selectable.SelectEvent.RemoveAllListeners();
    resultcard.selectable.SelectEvent.AddListener((UnityAction<BaseEventData>) (e => this.OnPointerEnter(resultcard)));
    resultcard.selectable.DeselectEvent.RemoveAllListeners();
    resultcard.selectable.DeselectEvent.AddListener((UnityAction<BaseEventData>) (e => this.OnPointerExit(resultcard)));
    resultcard.gameObject.SetActive(true);
  }

  public void DefaultOk()
  {
  }

  public void DefaultNo() => Singleton<MysteryManager>.Instance.EndMystery((MysteryBase) this);

  public void OnClickOk()
  {
    if (this.okdele == null)
      this.DefaultOk();
    else
      this.okdele(this);
  }

  public void OnClickNo()
  {
    if (this.nodele == null)
      this.DefaultNo();
    else
      this.nodele(this);
  }

  public void OnPointerEnter(LogLikeMod.UILogCardSlot CardSlot)
  {
    LogLikeMod.UILogBattleDiceCardUI instance = LogLikeMod.UILogBattleDiceCardUI.Instance;
    instance.transform.SetParent(CardSlot.transform.parent);
    instance.gameObject.SetActive(true);
    instance.SetCard(BattleDiceCardModel.CreatePlayingCard(CardSlot._cardModel.ClassInfo));
    instance.transform.localPosition = CardSlot.transform.localPosition + ((double) CardSlot.transform.localPosition.x > 0.0 ? new Vector3(-270f, -150f) : new Vector3(270f, -150f));
    instance.transform.localScale = new Vector3(0.25f, 0.25f);
    instance.gameObject.layer = LayerMask.NameToLayer("UI");
  }

  public void OnPointerExit(LogLikeMod.UILogCardSlot CardSlot)
  {
    if (!((Object) LogLikeMod.UILogBattleDiceCardUI.Instance != (Object) null))
      return;
    LogLikeMod.UILogBattleDiceCardUI.Instance.gameObject.SetActive(false);
  }

  public void OnClickCard(DiceCardItemModel card)
  {
  }

  public delegate void CheckResult(MysteryModel_UpgradeCheckPopup mystery);
}
}
