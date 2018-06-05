using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwoD00m.Drawble
{
    public static class Sprites
    {
        private static ContentManager content;
        private static SpriteBatch spriteBatch;

        private static
            int field3DHeight = 666;
        private static
            int field3DWidth = 1000;

        private static
            float posX = 12;
        private static
            float posY = 42;
        
        private static
            int overviewSide  = 5 / 2;

        public static void setContent(ContentManager Content) { content = Content; }
        public static Texture2D LoadTexture(String name)
        {
            try
            {
                return content.Load<Texture2D>(name);
            }
            catch { return null; }
        }

        public static void setSpriteBatch(SpriteBatch spriteBach) { spriteBatch = spriteBach; }
        public static SpriteBatch getSpriteBach() { return spriteBatch; }
        public static void Draw2D(Texture2D texure, Vector2 point, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(texure, point, rectangle, color);
        }
        public static void DrawBlock(Texture2D texture, int x, int y)
        {
            spriteBatch.Draw(texture, getPosition(x) , getRectangle(x,y), Color.White);
        }

        private static Rectangle getRectangle(int x, int y)
        {
            if (x == overviewSide)
                return new Rectangle((int)(getTextureLocation(x) * field3DWidth), y * field3DHeight, field3DWidth, field3DHeight);
            else
                return new Rectangle((int)(getTextureLocation(x) * field3DWidth), y * field3DHeight, field3DWidth / 2, field3DHeight);
        }
        private static float getTextureLocation(int x) {
            if (x > overviewSide)
                return overviewSide - Math.Abs(overviewSide - x) + 0.5f;
            else
                return overviewSide - Math.Abs(overviewSide - x);
        }
        private static Vector2 getPosition(int x)
        {
            if (x <= overviewSide)
                return new Vector2(posX, posY);
            else 
                return new Vector2(field3DWidth / 2 + posX, posY);
        }
    }
}
