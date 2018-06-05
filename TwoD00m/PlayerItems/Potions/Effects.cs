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

    public class HealthEffect : IEffect
    {
        private int stepCountForWork;
        protected int healPoint { get; private set; }
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
        public HealthEffect(Creature buffedCreature)
        {
            stepCountForWork = 1;
            healPoint = 15;
            target = buffedCreature;
        }

        public HealthEffect(Creature buffedCreature, int lifeTime, int healPower) //инициализация для кастомного зелья лечения
        {
            stepCountForWork = lifeTime;
            healPoint = healPower;
            target = buffedCreature;
        }
    }

    public class DamageEffect : IEffect
    {
        private int stepCountForWork;
        private double bufCoefficient;
        private Creature target;
        public DamageEffect(Creature creature)
        {
            target = creature;
            stepCountForWork = 1;
            bufCoefficient = 1.5;
        }
        public DamageEffect(Creature creature, int lifeTime, int damageBuf)
        {
            target = creature;
            stepCountForWork = lifeTime;
            bufCoefficient = damageBuf;
        }

        public void Stop()
        {
            target.RealDamage = (int)(target.RealDamage/bufCoefficient);
        }
        public void Act()
        {
            stepCountForWork--;
            target.RealDamage = (int)(target.Damage * bufCoefficient);
        }

        public int GetLifeTime()
        {
            return stepCountForWork; ;
        }
    }
}
