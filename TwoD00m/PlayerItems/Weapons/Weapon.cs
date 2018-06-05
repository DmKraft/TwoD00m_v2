using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoD00m.PlayerItems.Arsenal
{
    public class Weapon
    {
        private float Damage { get; }
        private float Durability { get; set; }
        private string Name { get; }
        private string Description { get; }


        Dictionary<string, TableEfficiency> Efficiency = new Dictionary<string, TableEfficiency>();

        public Weapon() { }
        public Weapon(List<string> weaponinfo)
        {

            TableEfficiency zombie = new TableEfficiency();
            zombie.damageСoefficient = 1;
            zombie.durabilityСoefficient = 0.5F;
            Efficiency.Add("Zombie", zombie);
            Efficiency.Add("Sceleton", zombie);
            Efficiency.Add("Petya", zombie);

            Damage = weaponinfo.GetFloatParameter("Damage");
            Name = weaponinfo.GetStringParameter("Name");
            Durability = weaponinfo.GetFloatParameter("Durability");
            Description = weaponinfo.GetStringParameter("Description");

        }

        public override string ToString()
        {
            return Name;
        }

        public virtual List<Point> Search(Point position, Direction direction, World map)
        {
            List<Point> availablePoint = new List<Point>();
            return availablePoint;
        }
        public class TableEfficiency
        {
            public float damageСoefficient { get; set; }
            public float durabilityСoefficient { get; set; }
        }

        public void Attack(Monster monster)
        {

            TableEfficiency efficiency = Efficiency[monster.Type];
            monster.TakeDamage(Damage * efficiency.damageСoefficient);
            Durability -= efficiency.durabilityСoefficient;
            if (Durability <= 0)
                Inventory.WeaponDestoy(0);

        }


        public class MeleeWeapon : Weapon
        {
            public MeleeWeapon(List<string> weaponinfo) : base(weaponinfo)
            {
            }
            public override List<Point> Search(Point position, Direction direction, World map)
            {
                var AvailablePoint = new List<Point>();
                position.Y -= direction.Alpha.Y;
                position.X += direction.Alpha.X;

                if (map.GetBlock(position) != null)
                        AvailablePoint.Add(position);
                return AvailablePoint;
            }

        }
        public class RangeWeapon : Weapon
        {
            public RangeWeapon(List<string> weaponinfo) : base(weaponinfo)
            {
            }
            public override List<Point> Search(Point position, Direction direction, World map)
            {
                var AvailablePoint = new List<Point>();
                for (int i = 1; i <= 3; i++)
                {
                    position.Y -= direction.Alpha.Y;
                    position.X += direction.Alpha.X;

                    if (map.GetBlock(position) != null)
                    {
                        AvailablePoint.Add(position);
                        if (!map.GetBlock(position).IsPassThrough)
                            break;
                    }
                    else break;
                }
                return AvailablePoint;
            }
        }
    }
}
