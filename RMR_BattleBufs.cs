using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;
using Sound;
using UnityEngine;

namespace RogueLike_Mod_Reborn
{
    public static class RoguelikeBufs
    {
        public static KeywordBuf CritChance;
        public static KeywordBuf RMRShield;
        public static KeywordBuf RMRStaggerShield;
    }

    public class BattleUnitBuf_RMR_Shield : BattleUnitBuf
    {
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.RMRShield;
            }
        }
        public override string keywordId => "RMR_Shield";
        public override string keywordIconId => "RMRBuf_Shield";
        public override BufPositiveType positiveType => BufPositiveType.Positive;
        public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
        {
            if (!this._owner.IsImmune(this.bufType) && !base.IsDestroyed())
            {
                if (dmg > this.stack)
                {
                    float num = (float)this.stack;
                    this.Destroy();
                    return ((float)dmg - num) / (float)dmg;
                }
                if (this.stack >= dmg)
                {
                    this.stack -= dmg;
                    return 0f;
                }
            }
            return base.DmgFactor(dmg, type, keyword);
        }

        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            this.Destroy();
        }
    }

    public class BattleUnitBuf_RMR_StaggerShield : BattleUnitBuf
    {
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.RMRStaggerShield;
            }
        }
        public override string keywordId => "RMR_StaggerShield";
        public override string keywordIconId => "RMRBuf_StaggerShield";
        public override BufPositiveType positiveType => BufPositiveType.Positive;
        public override float BreakDmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
        {
            if (!this._owner.IsImmune(this.bufType) && !base.IsDestroyed())
            {
                if (dmg > this.stack)
                {
                    float num = (float)this.stack;
                    this.Destroy();
                    return ((float)dmg - num) / (float)dmg;
                }
                if (this.stack >= dmg)
                {
                    this.stack -= dmg;
                    return 0f;
                }
            }
            return base.BreakDmgFactor(dmg, type, keyword);
        }

        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            this.Destroy();
        }
    }

    public class BattleUnitBuf_RMR_CritChance : BattleUnitBuf
    {
        bool initResources;
        private AudioClip critSfx;
        public override string keywordId => "RMR_CriticalStrike";
        public override string keywordIconId => "RMRBuf_CriticalStrike";
        public bool onCrit;

        Sprite sprite;
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.CritChance;
            }
        }

        private void OnCritEffect()
        {
            critSfx.PlaySound(_owner.view.transform);
            var effect = new GameObject();
            effect.transform.localScale = new Vector3(2f, 2f);
            effect.transform.parent = _owner.view.transform;
            effect.layer = LayerMask.NameToLayer("Effect");
            effect.transform.localPosition = new Vector3(0f, 0f);
            SpriteRenderer spriteRenderer = effect.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
            effect.AddComponent<CritVfx>();
            spriteRenderer.enabled = true;
            effect.SetActive(true);
        }

        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            onCrit = false;
            if (!initResources) // cache resources into memory to prevent slowdowns/stutters
            {
                critSfx = RMRCore.RMRMapHandler.GetAudioClip("critical.mp3");
                sprite = LogLikeMod.ArtWorks["OnCritEffect"];
                initResources = true;
            }
            if (behavior.owner?.currentDiceAction?.target != null)
            {
                var target = behavior.owner?.currentDiceAction?.target;
                var critRoll = RandomUtil.Range(0, 100);
                if (critRoll <= this.stack)
                {
                    onCrit = true;
                }
                if (onCrit)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus
                    {
                        dmgRate = 50,
                        breakRate = 50
                    });
                    behavior.owner.emotionDetail.GiveEmotionCoin(EmotionCoinType.Positive);
                    behavior.owner.battleCardResultLog.SetPrintEffectEvent(OnCritEffect);
                    GlobalLogueEffectManager.Instance.OnCrit(_owner, target);
                }
            }
        }

        public override void AfterDiceAction(BattleDiceBehavior behavior)
        {
            onCrit = false;
        }

        public class CritVfx : MonoBehaviour
        {
            SpriteRenderer renderer;

            public void Start()
            {
                renderer = base.gameObject.GetComponent<SpriteRenderer>();
            }

            public void Update()
            {
                this.timer += Time.deltaTime;
                base.gameObject.transform.localPosition = new Vector3(base.gameObject.transform.localPosition.x, base.gameObject.transform.localPosition.y + timer / 8f);
                Color color = renderer.color;
                color.r = 0f + timer / 3f;
                color.g = 1f - timer / 3f;
                color.a = 1f - timer / 2f;
                renderer.color = color;
                if (this.timer >= this.deathtimer)
                {
                    UnityEngine.Object.Destroy(base.gameObject);
                }
            }

            public float timer;

            public float deathtimer = 3f;
        }
    }

}
