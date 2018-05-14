using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;

namespace TwoD00m
{
   public class RangeOfViewMap
    {
        public List<Point> wayToTarget = new List<Point>();
        public int[,] moveCostMap;
        public int pathLength = 0;
        public RangeOfViewMap(int rangeOfViewFront, int rangeOfViewSides)
        {
            moveCostMap = new int[rangeOfViewFront *2 +1, rangeOfViewSides*4 + 2];
            moveCostMap = FillingMapWithWalls(moveCostMap);
        }

        public static int[,] FillingMapWithWalls(int[,] mapToFill)
        {

            mapToFill = FillingLeftAndRightSides(mapToFill);
            mapToFill = FillingUpAndDownSides(mapToFill);
            return mapToFill;
        }

        public static int[,] FillingUpAndDownSides(int[,] mapToFill)
        {
            for (int y = 0; y < mapToFill.GetLength(0); y += mapToFill.GetLength(0) - 1)
            {
                for (int x = 0; x < mapToFill.GetLength(1); x++)
                {
                    mapToFill[y, x] = 0;
                }
            }

            return mapToFill;
        }

        public static int[,] FillingLeftAndRightSides(int[,] mapToFill)
        {
            for (int y = 0; y < mapToFill.GetLength(0); y++)
            {
                for (int x = 0; x < mapToFill.GetLength(1); x += mapToFill.GetLength(1) - 1)
                {
                    mapToFill[y, x] = 0;
                }
            }

            return mapToFill;
        }
    }
}
