using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;
using TwoD00m.cWorld;

namespace TwoD00m
{
    public class AI
    {
        public static void DictionaryUpdate(ref Dictionary<Point, Monster> monsterList)
        {
            if (monsterList.Count < 0)
                return;
            Dictionary<Point, Monster> monster = new Dictionary<Point, Monster>();
            foreach (var monstr in monsterList.Values)
            {
                if (monstr.HP.Actual > 0)
                    monster.Add(monstr.Position, monstr);
            }
            monsterList = monster;
        }

        public void movements(Monster monster, World map)
        {
            if(monster.rangeOfView.wayToTarget.Count>0)
            if (monster.SearchTrueDirection(monster.rangeOfView.wayToTarget[0]))
            {
                monster.Move(map, monster.Direction);
                if(monster.Position==monster.rangeOfView.wayToTarget[0])
                monster.rangeOfView.wayToTarget.RemoveAt(0);
            }
            else monster.TargetUpdate();
        }
        protected bool TargetCheck(Monster monster, Hero hero)
        {
            if (monster.targetPosition.Equals(hero.Position) && monster.rangeOfView.wayToTarget.Count != 0)//если игрок не переместился, не ищем новый путь до него
                return true;
            else return false;
        }
        public virtual void FindTarget(Monster monster, World map, Hero hero, Dictionary<Point, Monster> monstersList)
        {
            if (TargetCheck(monster, hero))
                return;
            monster.heroFound = false;
            SearchHero(monster, map, hero);   //проверка видит ли монстр игрока
            if (monster.targetPosition.Equals(monster.lifeWay[0]) && monster.rangeOfView.wayToTarget.Count != 0)
                return;
            if (!monster.heroFound && !monster.Aggred) monster.targetPosition = monster.lifeWay[0];
            if (monster.Position.Equals(monster.targetPosition))
            {
                monster.TargetUpdate();
            }
            //дальше идет поиск кротчайшего пути, если игрок был найден
            getWayToTarget(monster, map, monstersList);
        }

        public bool Attack(Monster monster, Hero hero)
        {
            if (!monster.SearchTrueDirection(hero.Position))
                return false;
            if(monster.Specialization!= null)
            monster.Specialization.targetBuff(hero);
            hero.TakeDamage(monster.Damage);
            return true;
        }
        public virtual void SearchHero(Monster monster, World map, Hero hero)
        {
            Point[] scopeMap = monster.GetScope(map, monster.rangeOfViewFront, monster.rangeOfViewSides);
            List<Block> scope = monster.GetScope(scopeMap, map);
            for (int i = 0; i < scope.Count; i++)
            {
                if (scope[i] != null && hero.Position.Equals(scopeMap[i]))
                {
                    int centerOffsetFromHero = 1 - i % 3;
                    if (TrySearch(monster, scope, i, centerOffsetFromHero))
                        MakeHeroTagret(monster, scopeMap, i);
                }
            }
        }

        protected void MakeHeroTagret(Monster monster, Point[] scopeMap, int heroPosition)
        {
            monster.heroFound = true;
            monster.targetPosition = scopeMap[heroPosition];
        }

        protected bool TrySearch(Monster monster, List<Block> scope, int heroPosition, int offset)
        {
            for (int k = heroPosition; k < scope.Count; k += monster.rangeOfViewFront)
            {
                if (!scope[k].IsViewThrough || !scope[k + offset].IsViewThrough)
                    return false;
            }
            return true;
        }
        protected void getWayToTarget(Monster monster, World map, Dictionary<Point, Monster> monstersList)
        {
            if (AI.StartWave(map, monster, monstersList) == true)
            {
                AI.RestorePath(monster);
                monster.Aggred = true;
            }
        }

        public static bool StartWave(World map, Monster monster, Dictionary<Point, Monster> monstersList)
        {
            int[] stepX = { 1, 0, -1, 0 };
            int[] stepY = { 0, 1, 0, -1 };
            int MapHeight = 9;
            int MapWidht = 14;
            bool add = true;
            int[,] cMap = new int[MapHeight, MapWidht];
            int x, y, step = 0;
            for (y = 0; y < MapWidht; y++)
                for (x = 0; x < MapHeight; x++)
                {
                    if (map.GetBlock(x, y) != null)

                        if (!(map.GetBlock(x, y).IsPassThrough)&& !monstersList.ContainsKey(new Point(x, y)))
                            cMap[x, y] = -2;//индикатор стены
                        else
                            cMap[x, y] = -1;//индикатор еще не ступали сюда
                    if (x == monster.Position.X && y == monster.Position.Y) cMap[x, y] = -1;
                }
            cMap[monster.targetPosition.X, monster.targetPosition.Y] = 0;//Начинаем с финиша
            while (add == true)
            {
                add = false;
                for (y = 0; y < MapWidht; y++)
                {
                    for (x = 0; x < MapHeight; x++)
                    {
                        if (cMap[x, y] == step) //Ставим значение шага+1 в соседние ячейки (если они проходимы)
                            for (int dir = 0; dir < 4; dir++)
                            {
                                Point pos = new Point(x + stepX[dir], y + stepY[dir]);
                                if (IsStepPosible(pos, MapWidht, MapHeight) && IsPositionConsistNesessaryValue(pos, cMap, -1))
                                    cMap[pos.X, pos.Y] = step + 1;
                            }
                    }
                }
                step++;
                add = true;
                if (cMap[monster.Position.X, monster.Position.Y] != -1)//решение найдено
                {
                    add = false;
                    monster.rangeOfView.moveCostMap = cMap;
                    monster.rangeOfView.pathLength = cMap[monster.Position.X, monster.Position.Y];
                    return true;
                }
                if (step > MapWidht * MapHeight)//решение не найдено
                    add = false;
            }
            return false;
        }

        public static void RestorePath(Monster monster)
        {
            monster.rangeOfView.wayToTarget.Clear();
            int MapWidht = 14, MapHeight = 9;
            int[] stepX = { 1, 0, -1, 0 };
            int[] stepY = { 0, 1, 0, -1 };
            int x = monster.Position.X, y = monster.Position.Y;
            bool restoring = true;
            int step = monster.rangeOfView.pathLength;
            while (restoring)
            {
                restoring = false;
                if (monster.rangeOfView.moveCostMap[x, y] == step)
                    for (int dir = 0; dir < 4; dir++)
                    {
                        Point pos = new Point(x + stepX[dir], y + stepY[dir]);
                        if (IsStepPosible(pos, MapWidht, MapHeight) && IsPositionConsistNesessaryValue(pos, monster.rangeOfView.moveCostMap, step - 1))
                        {
                            step--;
                            monster.rangeOfView.wayToTarget.Add(pos);
                            x = pos.X;
                            y = pos.Y;
                            break;
                        }
                        if (step == 0) break;
                    }
                if (step > 0) restoring = true;
                if (!IsThatWayTrue(monster.Position, monster.targetPosition, monster.rangeOfView.wayToTarget[0], monster.rangeOfView.wayToTarget[monster.rangeOfView.wayToTarget.Count - 1]))
                {
                    monster.rangeOfView.wayToTarget.Clear();
                }
            }
        }
        private static bool IsStepPosible(Point position, int mapWidht, int mapHeight)
        {
            if (position.Y > -1 && position.Y < mapWidht)
                if (position.X > -1 && position.X < mapHeight)
                    return true;
            return false;
        }

        private static bool IsPositionConsistNesessaryValue(Point position, int[,] map, int value)
        {
            if (map[position.X, position.Y] == value)
                return true;
            return false;
        }
        private static bool IsThatWayTrue(Point Position, Point targetPosition, Point firstPointOfWay, Point lastPointOfWay)
        {
            if (Math.Abs(Position.X - firstPointOfWay.X) + Math.Abs(Position.Y - firstPointOfWay.Y) > 1 && Math.Abs(targetPosition.X - lastPointOfWay.X) + Math.Abs(targetPosition.Y - lastPointOfWay.Y) > 1)
                return false;
            else return true;
        }
    }
    public class AIz : AI
    {
        public override void FindTarget(Monster monster, World map, Hero hero, Dictionary<Point, Monster> monstersList)
        {
            if (TargetCheck(monster, hero))
                return;
            
            SearchHero(monster, map, hero);
            foreach (var friend in monstersList.Values)
            {
                if (friend is Zombie && friend.Aggred)
                {
                    monster.targetPosition = friend.targetPosition;
                    monster.Aggred = true;
                }
            }
            if (monster.heroFound && monster.Aggred)
                getWayToTarget(monster, map, monstersList);
            else monster.targetPosition = monster.Position;
        }
        public override void SearchHero(Monster monster, World map, Hero hero)
        {
            if(monster.SearchTrueDirection(hero.Position))
            {
                monster.heroFound = true;
                monster.targetPosition = hero.Position;
            }
        }
    }
}

