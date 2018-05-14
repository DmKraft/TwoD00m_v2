using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TwoD00m.PlayerItems.Arsenal;
using TwoD00m.PlayerItems.Potions;
using System.Reflection;

namespace TwoD00m
{
    public class GameItems
    {
        public static List<Weapon> weapon;
        public static List<Potion> potion;

        public static List<Weapon> SetWeapon()
        {         
            weapon = new List<Weapon>();
            Type myWeaponType = typeof(Weapon);
            List<Type> subClasses= Assembly.GetExecutingAssembly().GetExportedTypes().Where(i => i.IsSubclassOf(myWeaponType)).ToList();

            string path = @".\\PlayerItems\\Weapons\\Weapons.txt";            
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                Char delimiter = ' ';
                while ((line = sr.ReadLine()) != null)
                {
                    String[] WeaponsProperty = line.Split(delimiter);
                    foreach (var type in subClasses)
                    {
                        if (type.Name == WeaponsProperty[2])
                        {
                            ConstructorInfo con = type.GetConstructor(new Type[] {typeof(float), typeof(float), typeof(string) });
                            weapon.Add((Weapon)con.Invoke(new object[] { int.Parse(WeaponsProperty[0]), float.Parse(WeaponsProperty[1]), WeaponsProperty[3] }));
                        }
                    }
                }
            }

            return weapon;
        }
        

        public static List<Potion> SetPotions()
        {
            potion = new List<Potion>();
            string path = ".\\Potions.txt";

            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                Char delimiter = ' ';
                while ((line = sr.ReadLine()) != null)
                {
                   
                }
            }

            return potion;
        }

    }
}
