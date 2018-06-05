using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TwoD00m.cWorld;
using TwoD00m.PlayerItems.Potions;
//using Microsoft.Xna.Framework.Graphics;
using TwoD00m.Drawble;

namespace TwoD00m
{
    public abstract class Creature
    {
        public SurvivalPoint HP;
        protected Place place;
        public Point Position { get { return place.Position; } }
        public Direction Direction { get { return place.Direction; } }
        public float Damage { get; set; }
        public float RealDamage { get; set; }
        public static Random rand = new Random(); // что это?

        private List<IEffect> buffs = new List<IEffect>();

        public void Move(World map, Direction direction)
        {
            Point nextPosition = place.GetAheadPosition(direction);
            if (map.GetBlock(nextPosition) != null)
            {
                if (map.GetBlock(nextPosition).IsPassThrough)
                {
                    map.WorldUpdate(Position);
                    place.Position = nextPosition;
                    map.WorldUpdate(Position);
                }
            }
        }
        public static Dictionary<Point, Monster> ToDictionary(List<Monster> monsters)
        {
            Dictionary<Point, Monster> monsterList = new Dictionary<Point, Monster>();
            foreach (var monster in monsters)
                monsterList.Add(monster.Position, monster);
            return monsterList;
        }
        public Point[] GetScope(World map, int rangeOfVievFront, int rangeOfVievSides)
        {
            int scopeSqure = (rangeOfVievFront + 1) * (rangeOfVievSides * 2 + 1);
            int pos1, pos2, i = 0;
            if (Math.Abs(Direction.Alpha.Y) == 1)
            {
                pos1 = Position.Y;
                pos2 = Position.X;
            }
            else
            {
                pos1 = Position.X;
                pos2 = Position.Y;
            }
            Point[] order = new Point[scopeSqure];
            int pn1 = place.Direction.Alpha.Y - place.Direction.Alpha.X;
            int pn2 = place.Direction.Alpha.Y + place.Direction.Alpha.X;

            for (int p1 = pos1 - rangeOfVievFront * pn1, pt1 = 0; pt1 <= rangeOfVievFront; p1 += pn1, pt1++)
            {
                for (int p2 = pos2 - rangeOfVievSides * pn2, pt2 = 0; pt2 <= rangeOfVievSides * 2; p2 += pn2, pt2++)
                {
                    if (Math.Abs(Direction.Alpha.Y) == 1)
                        order[i++] = new Point(p2, p1);
                    else
                        order[i++] = new Point(p1, p2);
                }
            }
            return order;
        }

        public List<Block> GetScope(Point[] scopeMap, World map)
        {
            List<Block> scope = new List<Block>();
            foreach (var point in scopeMap)
            {
                scope.Add(map.GetBlock(point.X, point.Y));
            }
            return scope;
        }

        public void TakeHeal(float healPoint)
        {
            if (healPoint > 200)
                HP.Actual += 10;
            else HP.Actual += healPoint;
        }

        public void TakeDamage(float damagePoint)
        {
            HP.Actual -= damagePoint;
        }
        public void Buff(IEffect effect) //все ради инкапсуляции
        {
            if(!AlreadyBuffed(effect))
            buffs.Add(effect);
        }

        public void GetEffects()
        {
            foreach (var effect in buffs)
            {
                effect.Act();
            }
        }
        public bool AlreadyBuffed(IEffect effect)
        {
            if (buffs.Contains(effect))
                return true;
            else return false;
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
        protected AI ai = null;
        public List<Point> lifeWay = new List<Point>();
        public string Type { get; set; } = "None";
        public Specialization Specialization { get; private set; }
        public bool Aggred { get; set; }
        public Point targetPosition = new Point();
        public int rangeOfViewFront { get; protected set; } = 3;
        public int rangeOfViewSides { get; protected set; } = 1;
        public bool heroFound { get; set; } = false;
        public RangeOfViewMap rangeOfView = null;
        protected Subject model;

        public Monster(List<string> monsterInfo/*, List<AI> aiList*/)
        {
            if (ai == null)
            ai = new AI();
            Damage = monsterInfo.GetFloatParameter("Damage");
            int j = 0;
            place = new Place(monsterInfo.GetPointParameter("Position"), Direction.AbbToDirection(monsterInfo.GetStringParameter("Direction")));
            HP = new SurvivalPoint(monsterInfo.GetFloatParameter("HP.Actual"), monsterInfo.GetFloatParameter("HP.Max"));
            Specialization = Specialization.GetSpecialization(monsterInfo.GetStringParameter("Specialization"));
            Aggred = false;
            model = new Subject(monsterInfo.GetStringParameter("ModelPath"));
            foreach(var line in monsterInfo)
            {
                if(line.Contains("Life Way"))
                {
                    lifeWay.Add(monsterInfo.GetPointParameter(line));
                }
            }
            if (lifeWay.Count != 0)
                targetPosition = lifeWay[0];
            else targetPosition = Position;
            rangeOfView = new RangeOfViewMap(rangeOfViewFront, rangeOfViewSides);
            if (Type == "None")
                Type = "Petya";
        }

        public Monster()
        {
            HP = new SurvivalPoint();
        }
        public void MakeAMove(World map, Hero hero, Dictionary<Point, Monster> monstersList)
        {
            if (!ai.Attack(this, hero))
            {
                ai.FindTarget(this, map, hero, monstersList);
                ai.movements(this, map);
            }
            ai.SearchHero(this, map, hero);
            if (heroFound)
                Aggred = true;
        }
        public void TargetUpdate()
        {
            Aggred = false;
            Point swapBuffer = lifeWay[0];
            lifeWay.RemoveAt(0);
            lifeWay.Add(swapBuffer);
            targetPosition = lifeWay[0];
        }
        public bool SearchTrueDirection(Point targetPosition)
        {
            bool trueDirection = false;
            int CountOfTries = 0;
            while (!trueDirection)
            {
                if (!(place.GetAheadPosition().Equals(targetPosition)))
                {
                    place.Direction = Direction.GetRightDirection(Direction);
                    CountOfTries++;
                    if (CountOfTries > 3)
                        return false;
                }
                else trueDirection = true;
            }
            return trueDirection;
        }
        void buffAccordindOnSpecialization()
        {
            Specialization.buff(this);
        }

        public void Draw(int x, int y, Direction playerDirection)
        {
            model.Draw(x, y, playerDirection, Direction);
        }
    }

    public class Sceleton : Monster
    {
        public Sceleton(List<string> monsterInfo) : base(monsterInfo)
        {
            Type = monsterInfo.GetStringParameter("Type");
        }
    }
    public class Zombie : Monster
    {
        public Zombie(List<string> monsterInfo) : base(monsterInfo)
        {
            rangeOfViewFront = 1;
            rangeOfViewSides = 0;
            ai = new AIz();
            Type = monsterInfo.GetStringParameter("Type");
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
                }
                else
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

    public class Place
    {
        public Point Position { get; set; }
        public Direction Direction { get; set; }

        public Place() { }
        public Place(Point position, Direction direction) {
            Position = position;
            Direction = direction;
        }

        public Point GetAheadPosition()
        {
            return new Point(Position.X + Direction.Alpha.X, Position.Y - Direction.Alpha.Y);
        }

        public Point GetAheadPosition(Direction direction)
        {
            return new Point(Position.X + direction.Alpha.X, Position.Y - direction.Alpha.Y);
        }

        public void SetRightDirection()
        {
            Direction = Direction.GetRightDirection(Direction);
        }

        public void SetLeftDirection()
        {
            Direction = Direction.GetLeftDirection(Direction);
        }
    }
}
