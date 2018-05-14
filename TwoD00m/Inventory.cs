using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoD00m.PlayerItems.Arsenal;
using TwoD00m.PlayerItems.Potions;


namespace TwoD00m
{
    public class Inventory
    {
        public static List<Weapon> weapons;
        public List<Potion> potions;

        public Inventory()
        {
            weapons = GameItems.SetWeapon();
        }
        public void StartInventory()
        {
            weapons = new List<Weapon>();
            weapons.Add(GameItems.weapon[0]);
            potions = new List<Potion>();
            potions.Add(new Potion());
        }

        public static void WeaponDestoy(int weaponNumber)
        {
            Inventory.weapons.RemoveAt(weaponNumber);
        }

        public  Weapon AvailableWeapon()
        {
            return weapons[0];
        }
        
        public Potion GetPotion(int num)
        {
            return potions[num];
        }

        public Weapon GetWeapon(int num)
        {
            return weapons[num];
        }
        
    }
}
