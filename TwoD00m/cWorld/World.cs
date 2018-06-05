using System;
using System.IO;
using System.Collections.Generic;
using TwoD00m.cWorld;
using Microsoft.Xna.Framework;
using System.Linq;

namespace TwoD00m
{
    public class World
    {
        private Dictionary<Point, Block> world = new Dictionary<Point, Block>();

        public World() { }
        
        public void AddBlock(int x, int y, Block block)
        {
            if (!world.ContainsKey(new Point(x, y)))
                world.Add(new Point(x, y), block);
        }

        public void AddBlock(Point position, Block block)
        {
            if (!world.ContainsKey(position))
                world.Add(position, block);
        }

        public void DeleteBlock(int x, int y)
        {
            if (world.ContainsKey(new Point(x, y)))
                world.Remove(new Point(x, y));
        }

        public Block GetBlock(int x, int y)
        {
            if (world.ContainsKey(new Point(x, y)))
                return world[new Point(x, y)];
            else
                return null;

        }
        public Block GetBlock(Point point)
        {
            if (world.ContainsKey(point))
                return world[point];
            else
                return null;
        }

        public void Save()
        {
            /*StreamWriter save = new StreamWriter("save.txt");
            foreach (var block in world)
            {
                save.Write(String.Format("<{0},{1} {2}>", block.Key.X, block.Key.Y, BlocksKit.getAbb(block.Value)));
            }
            save.Close();*/
        }
        public void WorldUpdate(Point position)
        {
            world[position].InvertPassThrough();
        }

    }
}
