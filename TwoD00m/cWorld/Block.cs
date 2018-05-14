using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TwoD00m.Drawble;

namespace TwoD00m.cWorld
{
    public class Block
    {
        private BlockType Type { get; set; }
        public bool IsPassThrough
        {
            get { return Type.IsPassThrough; }
        }
        public bool IsViewThrough
        {
            get { return Type.IsViewThrough; }
        }
        private List<GameModel> models = new List<GameModel>();
        private List<Direction> modelsDirection = new List<Direction>();
        public bool Explored { get; set; }

        public Block()
        {
            Type = new BlockType();
            Explored = false;
        }

        public void AddModel(GameModel model, Direction direction)
        {
            models.Add(model);
            modelsDirection.Add(direction);
            ComputePassThrough();
            ComputeViewThrough();
        }
        public virtual void Use() { }

        public virtual void Stand() { }

        public void Draw(int x, int y, Direction playerDirection)
        {
            foreach (var model in models)
                model.Draw(x, y, playerDirection, modelsDirection[models.IndexOf(model)]);
        }

        private void ComputePassThrough()
        {
            Type = new BlockType(true, Type.IsViewThrough);
            foreach (var model in models)
            {
                if (!model.Type.IsPassThrough)
                    Type = new BlockType(false, Type.IsViewThrough);
            }
        }
        private void ComputeViewThrough()
        {
            Type = new BlockType(Type.IsPassThrough, true);
            foreach (var model in models)
            {
                if (!model.Type.IsViewThrough)
                    Type = new BlockType(Type.IsPassThrough, false);
            }
        }
    }
}
