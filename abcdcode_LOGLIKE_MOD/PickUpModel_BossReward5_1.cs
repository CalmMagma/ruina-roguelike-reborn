// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BossReward5_1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_BossReward5_1 : PickUpModelBase
    {
        public static DiceCardXmlInfo card;

        public PickUpModel_BossReward5_1()
        {
            this.Name = TextDataModel.GetText("BossReward5_1Name");
            this.Desc = TextDataModel.GetText("BossReward5_1Desc", PickUpModel_BossReward5_1.card == null ? (object)"[card]" : (object)PickUpModel_BossReward5_1.card.Name);
            this.ArtWork = "BossReward5";
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(PickUpModel_BossReward5_1.card);
            model.allyCardDetail.AddCardToHand(playingCard);
        }
    }
}
