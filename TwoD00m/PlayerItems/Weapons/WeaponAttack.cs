using System;
using TwoD00m.PlayerItems;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoD00m.cWorld;
using System.Drawing;


namespace TwoD00m.PlayerItems.Arsenal
{
    public partial class Attack
    {
        public virtual void attack(Monster monster, Weapon weapon)
        {
            //удар вперед
            TableEfficiency Efficiency = weapon.efficiency[monster.type];
            monster.healthPoints -= Convert.ToInt32(weapon.damage*Efficiency.damageСoef);
            weapon.durability -= 6*Efficiency.durabilityСoef;
            if (weapon.durability <= 0)
            Inventory.WeaponDestoy(0);
        }
    }
    class UnusualAtttack : Attack
    {
        public override void attack(Monster monster, Weapon weapon)
        {
            base.attack(monster, weapon);
            //удар не только вперед
        }

    }
}
