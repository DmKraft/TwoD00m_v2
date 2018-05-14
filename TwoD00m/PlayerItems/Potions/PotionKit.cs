using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoD00m.PlayerItems.Potions
{

    public class PotionKit
    {
        delegate void potionAction(PotionKit potion);

        int points;
        int ticks;
        string type;
        potionAction action;

        PotionKit(PotionKit _potion)
        {
            this.points = _potion.points;
            this.ticks = _potion.ticks;
            this.type = _potion.type;

            potionSetting(this);
        }

        void potionSetting(PotionKit potion)
        {
            if(potion.type == "Heal") { potion.action = new potionAction(PotionActions.PotionHeal); }
            if(potion.type == "HealTicks") { potion.action = new potionAction(PotionActions.PotionHealTicks); }
            if (potion.type == "DamageBuff") { potion.action = new potionAction(PotionActions.PotionDamageBuff); }
        }
    }

}
