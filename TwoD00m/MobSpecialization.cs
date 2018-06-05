using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoD00m.PlayerItems.Potions;

namespace TwoD00m
{
    public class Specialization
    {
        static public Specialization GetSpecialization(string Type)
        {
            Specialization tempBuffer = null;
            switch (Type)
            {
                case "Fire":
                    tempBuffer =  new FireSpecialization();
                    break;
                case "None":
                    tempBuffer = null;
                    break;
            }
            return tempBuffer;
        }
        public virtual void buff(Monster monster) { }
        public virtual void targetBuff(Hero hero) { }
    }

    public class FireSpecialization: Specialization
    {
       public override void buff(Monster monster)
        {
            monster.HP.Max *= 2;
            monster.HP.Actual = monster.HP.Max;
            monster.Damage *= 1.5f;
        }
        public override void targetBuff(Hero hero)
        {
            hero.Buff(new HealthEffect(hero, 5, -10));
        }
    }
}
