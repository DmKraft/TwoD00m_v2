using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TwoD00m.Drawble;

namespace TwoD00m.Interface
{
    public class Component
    {
        protected Texture2D texture;
        protected Vector2 position { get; }
        protected Vector2 screenPosition { get; }
        protected Rectangle size { get; }

        public Component(String texturePath, Vector2 componentPosition, Vector2 elementPosition)
        {
            texture = Sprites.LoadTexture(texturePath);
            size = new Rectangle(new Point(0,0), new Point(texture.Width, texture.Height));
            position = componentPosition;
            screenPosition = elementPosition + position;
        }

        public virtual void draw(float transparent)
        {
            Sprites.Draw2D(texture, screenPosition, size, Color.White * transparent);
        }

        public virtual void update() { }
    }

    public class FillingCell : Component
    {
        private SurvivalPoint value;
        private Rectangle dSize;

        public FillingCell(String texturePath, Vector2 componentPosition, Vector2 elementPosition, ref SurvivalPoint value) : base(texturePath, componentPosition, elementPosition) {
            this.value = value;
            dSize = size;
        }
        public override void update()
        {
            dSize.Width = (int)(size.Width * value.Actual / value.Max );
        }

        public override void draw(float transparent)
        {
            Sprites.Draw2D(texture, screenPosition, dSize, Color.White * transparent);
        }
    }

    public class ItemCell : Component
    {
        private Texture2D dTexture;

        public ItemCell(String texturePath, Vector2 componentPosition, Vector2 elementPosition, ref SurvivalPoint value) : base(texturePath, componentPosition, elementPosition) {
            dTexture = Sprites.LoadTexture(texturePath);
        }
    }

}
