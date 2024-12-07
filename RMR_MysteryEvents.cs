using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;

namespace RogueLike_Mod_Reborn
{
    public class MysteryModel_RMR_ChStartNew : MysteryBase
    {
        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0)
            {
                switch (choiceid)
                {
                    case 0:
                        Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_IronHeart());
                        break;
                    case 1:
                        Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_HunterCloak());
                        break;
                    case 2:
                        Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_StrangeOrb());
                        break;
                    case 3:
                        Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_ViciousGlasses());
                        break;
                    default:
                        break;
                }
            }
            base.OnClickChoice(choiceid);
        }

        public override void OnEnterChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0)
            {
                switch (choiceid)
                {
                    case 0:
                        this.ShowOverlayOverButton(new RMREffect_IronHeart(), choiceid);
                        break;
                    case 1:
                        this.ShowOverlayOverButton(new RMREffect_HunterCloak(), choiceid);
                        break;
                    case 2:
                        this.ShowOverlayOverButton(new RMREffect_StrangeOrb(), choiceid);
                        break;
                    case 3:
                        this.ShowOverlayOverButton(new RMREffect_ViciousGlasses(), choiceid);
                        break;
                    default:
                        break;
                }
            }
            base.OnEnterChoice(choiceid);
        }

        public override void OnExitChoice(int choiceid)
        {
            base.OnExitChoice(choiceid);
            this.CloseOverlayOverButton();
        }
    }
}