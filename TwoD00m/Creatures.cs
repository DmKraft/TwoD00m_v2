using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TwoD00m.cWorld;
using TwoD00m.PlayerItems.Potions;
using Microsoft.Xna.Framework.Graphics;
using TwoD00m.Drawble;

namespace TwoD00m
{
    public abstract class Creature
    {
        public SurvivalPoint HP;
        public int damage { get; set; }
        public int realDamage { get; set; }
        public int range { get; set; }
        public Point position;
        public Point alpha;
        public static Random rand = new Random();
        public Direction direction;
        private int rangeOfViewX = 3, rangeOfViewY = 3;
        private List<IEffect> buffs = new List<IEffect>();

        public void movements(World map, Direction direction)
        {
            map.WorldUpdate(position);
            alpha.Y = (int)Math.Sin(direction.GetCode() * Math.PI / 2);
            alpha.X = (int)Math.Cos(direction.GetCode() * Math.PI / 2);

            if (map.getBlock(getAheadPosition()) != null)
                if (map.getBlock(getAheadPosition()).IsPassThrough)
                {
                    position.Y -= alpha.Y;
                    position.X += alpha.X;
                    map.WorldUpdate(position);
                }
        }
        public Point getAheadPosition()
        {
            return new Point(position.X + alpha.X, position.Y - alpha.Y);
        }
        

        public Point[] GetScope(World map, int rangeOfVievFront, int rangeOfVievSides)
        {
            int scopeSqure = (rangeOfVievFront + 1) * (rangeOfVievSides * 2 + 1);
            int pos1, pos2, i = 0;
            if (Math.Abs(this.alpha.Y) == 1)
            {
                pos1 = this.position.Y;
                pos2 = this.position.X;
            }
            else
            {
                pos1 = this.position.X;
                pos2 = this.position.Y;
            }
            Point[] order = new Point[scopeSqure];
            int pn1 = this.alpha.Y - this.alpha.X;
            int pn2 = this.alpha.Y + this.alpha.X;

            for (int p1 = pos1 - rangeOfVievFront * pn1, pt1 = 0; pt1 <= rangeOfVievFront; p1 += pn1, pt1++)
            {
                for (int p2 = pos2 - rangeOfVievSides * pn2, pt2 = 0; pt2 <= rangeOfVievSides * 2; p2 += pn2, pt2++)
                {
                    if (Math.Abs(this.alpha.Y) == 1)
                        order[i++] = new Point(p2, p1);
                    else
                        order[i++] = new Point(p1, p2);
                }
            }
            return order;
        }

        public void TakeHeal(int healPoint)
        {
            if (healPoint > 200)
                HP.Actual += 10;
            else HP.Actual += healPoint;
        }

        public void TakeDamage(int damagePoint)
        {
            if (damagePoint > 200)
                HP.Actual -= 10;
            else HP.Actual -= damagePoint;
        }
        public void Buff(IEffect effect) //все ради инкапсуляции
        {
            buffs.Add(effect);
        }

        public void GetEffects()
        {
            foreach (var effect in buffs)
            {
                effect.Act();
            }
        }
        public void StopEffects()
        {
            foreach (var effect in buffs)
            {
                if (effect.GetLifeTime() <= 0)
                {
                    buffs.Remove(effect);
                    if (effect is DamageEffect)
                    {
                        ((DamageEffect)effect).Stop();
                    }
                }
            }
        }
    }

    public class Monster : Creature
    {
        public List<Point> homeWay = new List<Point>();
        public string type { get; set; }
        public string specialization { get; set; }
        public bool aggred { get; set; }
        public Point targetPosition = new Point();
        private static int rangeOfVievFront = 3, rangeOfVievSides = 1;
        //int[,] rangeOfView = new int[rangeOfVievFront, rangeOfVievSides*2+1];
        private bool heroFound = false;
        public RangeOfViewMap rangeOfView = new RangeOfViewMap(rangeOfVievFront, rangeOfVievSides);

        private GameModel model;

        public void movements(World map)
        {
            if (position.X == targetPosition.X && position.Y == targetPosition.Y)
            {
                TargetUpdate();
                return;
            }
            bool trueDirection = false;
            while (!trueDirection)
            {
                alpha.Y = (int)Math.Sin(direction.GetCode() * Math.PI / 2);
                alpha.X = (int)Math.Cos(direction.GetCode() * Math.PI / 2);
                if (rangeOfView.wayToTarget.Count == 0)
                {
                    return;
                }
                if (!(position.X + alpha.X == rangeOfView.wayToTarget[0].X && position.Y - alpha.Y == rangeOfView.wayToTarget[0].Y))
                   direction = Direction.getRightDirection(direction);
                else trueDirection = true;
            }
            movements(map, direction);
            rangeOfView.wayToTarget.RemoveAt(0);
        }


        public Monster(string[] param)
        {
            int j = 0;
            position.X = int.Parse(param[j++]);
            position.Y = int.Parse(param[j++]);
            HP = new SurvivalPoint();
            HP.Actual = SetRandomedDamage(int.Parse(param[j++]));
            damage = SetRandomedDamage(int.Parse(param[j++]));
            range = int.Parse(param[j++]);
            type = param[j++];
            specialization = param[j++];
            aggred = Convert.ToBoolean(int.Parse(param[j++]));
            direction = Direction.Code(int.Parse(param[j++]));
            model = new GameModel("Monsters/Monster1");
            homeWay.Add(new Point(4, 12));
            homeWay.Add(position);

            if (specialization != "none")
            {
                buffAccordindOnSpecialization();
            }
        }

        public Monster() {
            HP = new SurvivalPoint();
        }


        int SetRandomedHp(int HealthPoints)
        {
            int hpChangeRange = 10;
            int hpChangeForSpecialMonsters = 5;

            if (this.type == "orc" || this.type == "deadman")
            {
                hpChangeRange = +hpChangeForSpecialMonsters;
            }

            return rand.Next(HealthPoints - hpChangeRange, HealthPoints + hpChangeRange);
        }

        int SetRandomedDamage(int damage)
        {
            int damageChangeRange = 5;
            return this.damage = rand.Next(damage - damageChangeRange, damage + damageChangeRange);
        }

        void buffAccordindOnSpecialization()
        {
            /*switch (this.specialization)
            {
                case (Specialization)0: //Fire
                    this.HealthPoints = +10;
                    this.damage = +15;
                    break;

                case (Specialization)1: //Poison
                    this.HealthPoints = +15;
                    this.damage = +10;
                    break;
                case (Specialization)2: //Electicity
                    this.HealthPoints = +10;
                    this.damage = +10;
                    break;
            }*/
        }

        public void ScanAreNear(World map, Hero hero)
        {
            if (targetPosition.X == hero.position.X && targetPosition.Y == hero.position.Y)//если игрок не переместился, не ищем новый путь до него
                return;
            if (targetPosition.X == homeWay[0].X && targetPosition.Y == homeWay[0].Y)
                return;
            heroFound = false;
            //Дальше идет проверка видимости игрока мобом
            int scopeSqure = rangeOfVievFront * (rangeOfVievSides * 2 + 1);
            this.alpha.Y = (int)Math.Sin(direction.GetCode() * Math.PI / 2);
            this.alpha.X = (int)Math.Cos(direction.GetCode() * Math.PI / 2);

            List<Block> scope = new List<Block>();
            Point[] scopeCoords = GetScope(map, rangeOfVievFront, rangeOfVievSides);
            foreach (var point in scopeCoords)
            {
                scope.Add(map.getBlock(point.X, point.Y));
            }

            SearchHero(hero, scopeCoords, scope);   //проверка видит ли монстр игрока
            if (!heroFound) targetPosition = homeWay[0];
            //дальше идет поиск кротчайшего пути, если игрок был найден

                if (AI.StartWave(map, this) == true)
                {
                    AI.RestorePath(this);
                    aggred = true;
                }
        }

        public bool Attack(Hero hero)
        {
            if (position.X + alpha.X != hero.position.X || position.Y + alpha.Y != hero.position.Y)
                return false;
            //if(specialization!= null)
            //hero.Buf(specialization.baf)
            hero.TakeDamage(damage);
            return true;
        }

        public void SearchHero(Hero hero, Point[] scopeMap, List<Block> scope)
        {
            for (int i = 0; i < scope.Count; i++)
            {
                if (scope[i] != null)
                {
                    if (hero.position.X == scopeMap[i].X && hero.position.Y == scopeMap[i].Y)
                    {
                        if (i < 6)
                        {
                            switch (i % 3)
                            {
                                case 0:
                                    if (SearchLeftAndCenter(scope, scopeMap, i))
                                        return;
                                    break;
                                case 1:
                                    if (SearchCenter(scope, scopeMap, i))
                                        return;
                                    break;
                                case 2:
                                    if (SearchRightAndCenter(scope, scopeMap, i))
                                        return;
                                    break;
                            }
                        }
                        else
                        {
                            this.heroFound = true;
                            targetPosition = scopeMap[i];
                            return;
                        }
                    }
                }
            }
        }

        private bool SearchRightAndCenter(List<Block> scope, Point[] scopeMap, int heroPosition)
        {
            if (TrySearch(scope, heroPosition, 1))
            {
                this.heroFound = true;
                targetPosition = scopeMap[heroPosition];
                return true;
            }
            else return false;
        }
        private bool SearchCenter(List<Block> scope, Point[] scopeMap, int heroPosition)
        {
            if (TrySearch(scope, heroPosition, 0))
            {
                this.heroFound = true;
                targetPosition = scopeMap[heroPosition];
                return true;
            }
            else return false;
        }
        private bool SearchLeftAndCenter(List<Block> scope, Point[] scopeMap, int heroPosition)
        {
            if (TrySearch(scope, heroPosition, -1))
            {
                this.heroFound = true;
                targetPosition = scopeMap[heroPosition];
                return true;
            }
            else return false;
        }

        private bool TrySearch(List<Block> scope, int heroPosition, int offset)
        {
            for (int k = heroPosition; k < scope.Count; k += rangeOfVievSides*2+1)
            {
                if (!scope[k].IsViewThrough || !scope[k + offset].IsViewThrough)
                    return false;
            }
            return true;
        }
        public void TargetUpdate()
        {
            aggred = false;
            Point swapBuffer = homeWay[0];
            homeWay.RemoveAt(0);
            homeWay.Add(swapBuffer);
            targetPosition = homeWay[0];
        }

        public void Draw(int x, int y, Direction playerDirection)
        {
            model.Draw(x, y, playerDirection, direction);
        }
    }

    public class SurvivalPoint
    {
        private float actual;
        public float Actual
        {
            get { return actual; }
            set
            {
                if (value > 0)
                {
                    if (value >= max)
                        actual = max;
                    else
                        actual = value;
                } else 
                    actual = 0;
            }
        }
        private float max;
        public float Max
        {
            get { return max; }
            set
            {
                max = value;
            }
        }

        public SurvivalPoint(float actual, float max)
        {
            this.actual = actual;
            this.max = max;
        }

        public SurvivalPoint() { }
    }
}
