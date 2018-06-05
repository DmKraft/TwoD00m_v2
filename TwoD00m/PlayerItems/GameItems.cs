using TwoD00m.PlayerItems.Arsenal;
using TwoD00m.PlayerItems.Potions;
using TwoD00m.cWorld;

namespace TwoD00m
{
    public static class GameItems
    {
        public static GameKit<Weapon> weapon;
        public static GameKit<Subject> subjects;
        public static GameKit<Potion> potions;
        
        public static void SetUp()
        {
            weapon = Loader.LoadKit<Weapon>(@".\\PlayerItems\\Weapons\\Weapons.csv");
            subjects = Loader.LoadKit<Subject>(@".\cWorld\Models.csv");
            potions = Loader.LoadKit<Potion>(@".\\PlayerItems\\Potions\\Potions.csv");
        }
        public static Weapon findweapon(string name)
        {
            return weapon.GetObject(name);
        }
    }
    
}
