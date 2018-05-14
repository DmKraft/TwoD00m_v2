using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using TwoD00m.cWorld;

namespace TwoD00m.Drawble
{
    public static class Drawing
    {
        public static void Draw3D(Hero hero, World map, Dictionary<Point, Monster> monsterList)
        {
            Point[] orderCoords = hero.GetScope(map, 4, 2);
            for(int i = 0; i < orderCoords.Length; i += 5)
            {
                Point tmp = orderCoords[i + 2];
                orderCoords[i + 2] = orderCoords[i + 4];
                orderCoords[i + 4] = tmp;
            }
            int[] queue = { 0, 1, 4, 3, 2 };
            for (int y = 0; y < 5; y++)
            {
                for(int x = 0; x < 5; x++)
                {
                    if (map.getBlock(orderCoords[y*5 + x]) != null)
                        map.getBlock(orderCoords[y*5 + x]).Draw(queue[x], y, hero.direction);
                    if (monsterList.ContainsKey(orderCoords[y * 5 + x]))
                        monsterList[orderCoords[y * 5 + x]].Draw(queue[x], y, hero.direction);
                }
            }
        }
    }
}
