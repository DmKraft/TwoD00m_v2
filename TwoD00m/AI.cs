using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace TwoD00m
{
    class AI
    {
        public static void MovevmentsAndAtacks(Dictionary<Point, Monster> monstersList, World map, Hero hero)
        {
            foreach (var monster in monstersList)
            {
                if (monster.Value.Attack(hero))
                    return;
                else
                {
                    monster.Value.ScanAreNear(map, hero);
                    monster.Value.movements(map);
                }
            }
        }

        public static bool StartWave(World map, Monster monster)
        {
            int[] stepX = { 1, 0, -1, 0 };
            int[] stepY = { 0, 1, 0, -1 };
            int MapHeight = 9;
            int MapWidht = 14;
            bool add = true;
            int[,] cMap = new int[MapHeight, MapWidht];
            int x, y, step = 0;
            for (y = 0; y < MapWidht; y++)
                for (x = 0; x <MapHeight; x++)
                {
                    if (map.getBlock(x, y) != null)
                    
                        if (!(map.getBlock(x, y).IsPassThrough))
                            cMap[x, y] = -2;//индикатор стены
                        else
                            cMap[x, y] = -1;//индикатор еще не ступали сюда
                    if (x == monster.position.X && y == monster.position.Y) cMap[x, y] = -1;
                }
            cMap[monster.targetPosition.X, monster.targetPosition.Y] = 0;//Начинаем с финиша
            while (add == true)
            {
                add = false;
                for (y = 0; y < MapWidht; y++)
                {
                    for (x = 0; x < MapHeight; x++)
                    {
                        if(cMap[x, y] == step) //Ставим значение шага+1 в соседние ячейки (если они проходимы)
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
                if (cMap[monster.position.Y, monster.position.X] != -1)//решение найдено
                {
                    add = false;
                    monster.rangeOfView.moveCostMap = cMap;
                    monster.rangeOfView.pathLength = cMap[monster.position.X, monster.position.Y];
                    return true;
                }
                if (step > MapWidht * MapHeight)//решение не найдено
                    add = false;
            }
            return false;
        }

        public static void RestorePath(Monster monster)
        {
           // List<Point> badWayPoints = new List<Point>();
            int MapWidht = 14, MapHeight = 9;
            int[] stepX = { 1, 0, -1, 0 };
            int[] stepY = { 0, 1, 0, -1 };
            int x = monster.position.X , y = monster.position.Y;
            bool restoring = true;
            int step = monster.rangeOfView.pathLength;
            while (restoring)
            {
                restoring = false;
                /*  for (y = 0; y < MapWidht; y++)
                      for (x = 0;x < MapHeight; x++)
                      {*/
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
                //}
                /*if (!IsThatWayTrue(monster.position, monster.targetPosition, monster.rangeOfView.wayToTarget[0], monster.rangeOfView.wayToTarget[monster.rangeOfView.wayToTarget.Count - 1])) return;
                   else
                   {
                      foreach(var point in monster.rangeOfView.wayToTarget)
                       {
                           badWayPoints.Add(point);
                       }
                       monster.rangeOfView.wayToTarget.Clear();
                       step = monster.rangeOfView.pathLength;

                   }*/
                if (step > 0) restoring = true;
                if (!IsThatWayTrue(monster.position, monster.targetPosition, monster.rangeOfView.wayToTarget[0], monster.rangeOfView.wayToTarget[monster.rangeOfView.wayToTarget.Count - 1]))
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
        private static bool IsThatWayTrue(Point position,Point targetPosition, Point firstPointOfWay, Point lastPointOfWay)
        {
            if (Math.Abs(position.X - firstPointOfWay.X) + Math.Abs(position.Y - firstPointOfWay.Y) > 1&& Math.Abs(targetPosition.X - lastPointOfWay.X) + Math.Abs(targetPosition.Y - lastPointOfWay.Y) > 1)
                return false;
            else return true;
        }
       /* private static bool IsThatPointInBadWay(Point position, List<Point> badWayPoint)
        {
        if(badWayPoint.Count == 0)
        return false;
            foreach(var point in badWayPoint)
            {
                if (position.X == point.X && position.Y == point.Y)
                    return true;
            }
            return false;
        }*/
    }
}
