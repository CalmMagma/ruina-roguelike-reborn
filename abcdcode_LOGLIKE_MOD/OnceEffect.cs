// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.OnceEffect
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;


namespace abcdcode_LOGLIKE_MOD
{

    public class OnceEffect : GlobalLogueEffectBase
    {
        public int stack = 1;

        public override void AddedNew()
        {
            ++this.stack;
            Singleton<GlobalLogueEffectManager>.Instance.UpdateSprites();
        }

        public override bool CanDupliacte() => true;

        public override SaveData GetSaveData()
        {
            SaveData saveData = base.GetSaveData();
            saveData.AddData("stack", this.stack);
            return saveData;
        }

        public override void LoadFromSaveData(SaveData save)
        {
            base.LoadFromSaveData(save);
            this.stack = save.GetInt("stack");
        }

        public virtual void Use()
        {
            --this.stack;
            if (this.stack <= 0)
                this.Destroy();
            Singleton<GlobalLogueEffectManager>.Instance.UpdateSprites();
        }

        public override int GetStack() => this.stack;
    }
}
