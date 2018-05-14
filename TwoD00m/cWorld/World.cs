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
        Dictionary<Point, Block> world;
        GameKit<GameModel> modelsKit;

        public World(string[] blocksInfo)
        {
            world = new Dictionary<Point, Block>();
            modelsKit = Loader.LoadKit<GameModel>(@".\cWorld\Models.csv");
            foreach (var blockInfo in blocksInfo)
            {
                Block block = new Block();
                string[] info = blockInfo.Replace("{", "").Replace("}", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Point position = new Point(Convert.ToInt32(info[0]), Convert.ToInt32(info[1]));
                string[] modelsInfo = new string[info.Length - 2];
                Array.Copy(info, 2, modelsInfo, 0, modelsInfo.Length);
                for(int i = 0; i < modelsInfo.Length; i +=2 )
                {
                    GameModel m = modelsKit.GetKit().Where(model => model.AbbName == modelsInfo[i]).ToList()[0];
                    Direction d = Loader.GetModelDirection(modelsInfo[i + 1]);
                    block.AddModel(m, d);
                }
                world.Add(position, block);
            }
        }
        
        public void addBlock(int x, int y, ref Block block)
        {
            if (!world.ContainsKey(new Point(x, y)))
                world.Add(new Point(x, y), block);
        }
        public void deletBlock(int x, int y)
        {
            if (world.ContainsKey(new Point(x, y)))
                world.Remove(new Point(x, y));
        }

        public Block getBlock(int x, int y)
        {
            if (world.ContainsKey(new Point(x, y)))
                return world[new Point(x, y)];
            else
                return null;

        }
        public Block getBlock(Point point)
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
        }

    }
}
