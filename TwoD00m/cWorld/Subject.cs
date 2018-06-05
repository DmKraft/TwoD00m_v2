using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoD00m.PlayerItems.Arsenal;
using TwoD00m.PlayerItems.Potions;
using System.Linq;
using TwoD00m.Drawble;

namespace TwoD00m.cWorld
{
    public class Subject
    {
        private ModelTexture texture;
        public string AbbName { get; }
        public BlockType Type { get; }

        public Subject(List<string> modelInfo)
        {
            AbbName = modelInfo.GetStringParameter("Id");
            string path = modelInfo.GetStringParameter("ModelPath");
            bool isSameSide = modelInfo.GetBooleanParameter("IsSameSide");
            texture = new ModelTexture(path, isSameSide);
            bool isPassThrough = modelInfo.GetBooleanParameter("IsViewThrough");
            bool isViewThrough = modelInfo.GetBooleanParameter("IsPassThrough");
            Type = new BlockType(isPassThrough, isViewThrough);
        }
        public Subject(string path)
        {
            texture = new ModelTexture(path, false);
        }

        public Subject() { }

        public override string ToString()
        {
            return AbbName;
        }

        public void Draw(int x, int y, Direction playerDirection, Direction modelDirection)
        {
            Sprites.DrawBlock(texture.GetTexture(playerDirection, modelDirection), x, y);
        }

        public virtual void Use() { }

        public class ModelTexture
        {
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
                if (sides.Count == 1)
                {
                    return sides[0];
                }
                else
                {
                    return sides[Direction.GetSideDifference(playerDirection, modelDirection)];
                }
            }
        }
    }

    public class Chest : Subject
    {
        bool open;
        Weapon innerweapon;
        Potion innerpotion;
        public Chest(List<string> modelInfo) : base(modelInfo)
        {
            open = false;
            innerweapon = GameItems.findweapon(modelInfo.GetStringParameter("Weapon"));
        }

        public override void Use()
        {
            if (!open)
            {
                Inventory.weapons.Add(innerweapon);
                open = true;
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
