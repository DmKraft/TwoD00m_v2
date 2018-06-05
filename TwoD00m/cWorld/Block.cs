using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TwoD00m.Drawble;
using System;

namespace TwoD00m.cWorld
{
    public class Block
    {
        private BlockType type;
        private List<Subject> subjects = new List<Subject>();
        private List<Direction> subjectsDirection = new List<Direction>();
        public bool IsPassThrough
        {
            get { return type.IsPassThrough; }
        }
        public bool IsViewThrough
        {
            get { return type.IsViewThrough; }
        }
        public bool Explored
        {
            get;
            set;
        }
        
        public Block(string[] blockInfo)
        {
            type = new BlockType();
            Explored = false;
            string[] modelsInfo = new string[blockInfo.Length - 2];
            Array.Copy(blockInfo, 2, modelsInfo, 0, modelsInfo.Length);
            for (int i = 0; i < modelsInfo.Length; i += 2)
            {
                Subject m = GameItems.subjects.GetObject(modelsInfo[i]);
                Direction d = Direction.AbbToDirection(modelsInfo[i + 1]);
                AddModel(ref m, d);
            }
        }

        public void AddModel(ref Subject model, Direction direction)
        {
            subjects.Add(model);
            subjectsDirection.Add(direction);
            ComputePassThrough();
            ComputeViewThrough();
        }
        public virtual void Use()
        {
            foreach (var obj in subjects)
            {
                obj.Use();
            }
        }

        public virtual void Stand() { }

        public void Draw(int x, int y, Direction playerDirection)
        {
            foreach (var model in subjects)
                model.Draw(x, y, playerDirection, subjectsDirection[subjects.IndexOf(model)]);
        }

        private void ComputePassThrough()
        {
            type = new BlockType(true, type.IsViewThrough);
            foreach (var model in subjects)
            {
                if (!model.Type.IsPassThrough)
                    type = new BlockType(false, type.IsViewThrough);
            }
        }
        private void ComputeViewThrough()
        {
            type = new BlockType(type.IsPassThrough, true);
            foreach (var model in subjects)
            {
                if (!model.Type.IsViewThrough)
                    type = new BlockType(type.IsPassThrough, false);
            }
        }

        public void InvertPassThrough()
        {
            if (type.IsPassThrough)
                type = new BlockType(false, type.IsViewThrough);
            else
                type = new BlockType(true, type.IsViewThrough);
        }
    }
}