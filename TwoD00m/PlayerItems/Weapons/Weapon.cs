using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoD00m.PlayerItems.Arsenal
{
    public partial class Weapon
    {
        private float damage { get; }
        private float durability { get; set; }     
                
        private float rareness;        
        private string name;
        private string description;
       

        Dictionary<string, TableEfficiency> efficiency = new Dictionary<string, TableEfficiency>();
        

        public Weapon(float damage, float durability,  string name)
        {
            this.damage = damage;
            this.name = name;
            this.durability = durability;
            
                
            TableEfficiency zombie = new TableEfficiency();
            zombie.damageСoef = 1;
            zombie.durabilityСoef = 0.5F;
            efficiency.Add("zombie", zombie);
        }
              
        public virtual List<Point> Search(Point position, Direction direction, World map)
        {
            List<Point> AvailablePoint = new List<Point>();
            return AvailablePoint;
        }
        public class TableEfficiency
        {
            public float damageСoef { get; set; }
            public float durabilityСoef { get; set; }
        }

        public void Attack(Monster monster)
        {

            TableEfficiency Efficiency = efficiency[monster.type];
            monster.TakeDamage((int)(damage * Efficiency.damageСoef));
            durability -= Efficiency.durabilityСoef;
            if (durability <= 0)
                Inventory.WeaponDestoy(0);

        }


        public class MeleeWeapon : Weapon
        {
            public MeleeWeapon(float damage, float durability, string name):base(damage,durability,name)
            {               
            }
            public override List<Point> Search(Point position,Direction direction,World map)
            {
                var AvailablePoint = new List<Point>();
                Point alpha;

                alpha.Y = (int)Math.Sin(direction.GetCode() * Math.PI / 2);
                alpha.X = (int)Math.Cos(direction.GetCode() * Math.PI / 2);

                position.Y -= alpha.Y;
                position.X += alpha.X;

                if (map.getBlock(position) != null)
                    if (map.getBlock(position).IsPassThrough)
                        AvailablePoint.Add(position);
                return AvailablePoint;
            }

        }
        public class RangeWeapon : Weapon
        {
            public RangeWeapon(float damage, float durability,  string name):base(damage,durability,name)
            {
            }
            public override List<Point> Search(Point position, Direction direction, World map)
            {
                var AvailablePoint = new List<Point>();
                Point alpha;

                alpha.Y = (int)Math.Sin(direction.GetCode() * Math.PI / 2);
                alpha.X = (int)Math.Cos(direction.GetCode() * Math.PI / 2);
                for (int i = 1; i <= 3; i++)
                {
                    position.Y -= alpha.Y;
                    position.X += alpha.X;

                    if (map.getBlock(position) != null)
                    {
                        if (map.getBlock(position).IsPassThrough)
                            AvailablePoint.Add(position);
                        else break;
                    }
                    else break;
                }
                return AvailablePoint;
            }
        }
    }
}
