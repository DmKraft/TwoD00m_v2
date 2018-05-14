using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TwoD00m.Drawble;

namespace TwoD00m.cWorld
{
    public class GameModel
    {
        private ModelTexture texture;
        public string AbbName { get; }
        public BlockType Type { get; }
        
        public GameModel(List<string> modelInfo)
        {
            AbbName = Loader.GetStringParameter(modelInfo, "Id");
            string path = Loader.GetStringParameter(modelInfo, "ModelPath");
            bool isSameSide = Loader.GetBooleanParameter(modelInfo, "IsSameSide");
            texture = new ModelTexture(path, isSameSide);
            bool isPassThrough = Loader.GetBooleanParameter(modelInfo, "IsViewThrough");
            bool isViewThrough = Loader.GetBooleanParameter(modelInfo, "IsPassThrough");
            Type = new BlockType(isPassThrough, isViewThrough);
        }
        public GameModel(string path)
        {
            texture = new ModelTexture(path, false);
        }

        public GameModel() { }
        
        public void Draw(int x, int y, Direction playerDirection, Direction modelDirection)
        {
            Sprites.DrawBlock(texture.GetTexture(playerDirection, modelDirection), x, y);
        }
        
        public class ModelTexture {
            public List<Texture2D> sides = new List<Texture2D>();

            public ModelTexture(string path, bool isSameSide)
            {
                if (isSameSide)
                {
                    sides.Add(Sprites.LoadTexture(path));
                }
                else
                {
                    sides.Add(Sprites.LoadTexture(path + "_F"));
                    sides.Add(Sprites.LoadTexture(path + "_L"));
                    sides.Add(Sprites.LoadTexture(path + "_B"));
                    sides.Add(Sprites.LoadTexture(path + "_R"));
                }
            }

            public Texture2D GetTexture(Direction playerDirection, Direction modelDirection)
            {
                if(sides.Count == 1)
                {
                    return sides[0];
                } else
                {
                    return sides[Direction.GetDifference(playerDirection, modelDirection)];
                }
            }
        }
    }

    public class BlockType
    {
        public bool IsPassThrough { get; }
        public bool IsViewThrough { get; }

        public BlockType(bool isPassThrough, bool isViewThrough)
        {
            IsPassThrough = isPassThrough;
            IsViewThrough = isViewThrough;
        }
        public BlockType() { }
    }
}
