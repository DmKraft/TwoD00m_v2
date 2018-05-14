using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoD00m.PlayerItems.Potions
{
    public interface IEffect
    {
        void Act();
        int GetLifeTime();
    }

    public class HealEffect : IEffect
    {
        private int stepCountForWork;
        int healPoint { get; }
        Creature target;
        public void Act()
        {
            stepCountForWork--;
            target.TakeHeal(healPoint);
        }

        public int GetLifeTime()
        {
            return stepCountForWork;
        }
        public HealEffect(Creature buffedCreature)
        {
            stepCountForWork = 1;
            healPoint = 15;
            target = buffedCreature;
        }

        public HealEffect(Creature buffedCreature, int lifeTime, int healPower) //инициализация для кастомного зелья лечения
        {
            stepCountForWork = lifeTime;
            healPoint = healPower;
            target = buffedCreature;
        }
    }

    public class DamageEffect : IEffect
    {
        private int stepCountForWork;
        private double bufCoef;
        private Creature target;
        public DamageEffect(Creature creature)
        {
            target = creature;
            stepCountForWork = 1;
            bufCoef = 1.5;
        }
        public DamageEffect(Creature creature, int lifeTime, int damageBuf)
        {
            target = creature;
            stepCountForWork = lifeTime;
            bufCoef = damageBuf;
        }

        public void Stop()
        {
            target.realDamage = (int)(target.realDamage/bufCoef);
        }
        public void Act()
        {
            stepCountForWork--;
            target.realDamage = (int)(target.damage * bufCoef);
        }

        public int GetLifeTime()
        {
            return stepCountForWork; ;
        }
    }
}
