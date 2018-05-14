using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TwoD00m
{
    public static class Common
    {
        // public static Dictionary<Monster, System.Drawing.Point > 
        //    InitializeAndFillDictionary(int length, List<Monster> monstersList)
        //{
        //    Dictionary<Monster, System.Drawing.Point> monstersList = 
        //        new Dictionary<Monster, System.Drawing.Point>(length);

        //    foreach(var i in monstersList)
        //    {
        //        monstersList.Add(i, i.position);
        //    }

        //    return monstersList;
        //}

        public static Dictionary<Point, Monster> ReadMonstersList()
        {
            Dictionary<Point, Monster> monstersList = new Dictionary<Point, Monster>();
            string[] monstersStats = File.ReadAllLines(".\\MonstersStats.txt");

            monstersList = FillingMonstersList(monstersStats);

            return monstersList;
        }

        public static void DictionaryUpdate(ref Dictionary<Point, Monster> monsterList)
        {
            Dictionary<Point, Monster> monster = new Dictionary<Point, Monster>();
            foreach(var m in monsterList)
            {
                monster.Add(m.Value.position, m.Value);
            }
            monsterList = monster;
        }

        public static Dictionary<Point, Monster> FillingMonstersList(string[] monstersStats)
        {
            Dictionary<Point, Monster> monstersList = new Dictionary<Point, Monster>();

            for (int i = 0; i < monstersStats.Length; i++)
            {
                Point position;
                string[] stats = monstersStats[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                position.X = int.Parse(stats[0]);
                position.Y = int.Parse(stats[1]);
                monstersList.Add(position, new Monster(stats));
            }

            return monstersList;
        }
 
    }
}
