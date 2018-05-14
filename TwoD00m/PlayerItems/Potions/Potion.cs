using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoD00m.PlayerItems.Potions
{
    //delegate void potionsActions(Potion potion);

    public class Potion
    {
        private IEffect effect;
        public string name;

        public void Use(Creature target)
        {
            target.Buff(effect);
        } 

        public void Use(List<Creature> creatures)
        {
            foreach (var creature in creatures)
                creature.Buff(effect);
        }
        public Potion(IEffect _effect, string _name)
        {
            effect = _effect;
            name = _name;
        }
        public Potion()
        {
        }
    }
    //{

    //    int points;
    //    int ticks;
    //    string type;
    //    potionsActions a;

    //    Potions(Potion _potion)
    //    {
    //        this.points = _potion.points;
    //        this.ticks = _potion.ticks;
    //        this.type = _potion.type;

    //        potionSetting(this);
    //    }

    //    void potionSetting(Potion potion)
    //    {
    //        if (potion.type == "Heal") { potion.action = new potionAction(PotionActions.PotionHeal); };
    //        if (potion.type == "HealTicks") { potion.action = new potionAction(PotionActions.PotionHealTicks); };
    //        if (potion.type == "DamageBuff") { potion.action = new potionAction(PotionActions.PotionDamageBuff); };
    //    }
    
}
