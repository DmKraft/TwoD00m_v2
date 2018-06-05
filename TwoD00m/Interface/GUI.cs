using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TwoD00m.Interface
{
    public class GUI
    {
        private List<Element> elements;
        private bool visible;

        public GUI(bool visible)
        {
            elements = new List<Element>();
            this.visible = visible;
        }

        public void addElement(Element element)
        {
            elements.Add(element);
        }

        public virtual void update()
        {
            if (visible)
            {
                foreach (var element in elements)
                {
                    element.update();
                }
            }
        }

        public void draw()
        {
            if (visible)
            {
                foreach (var element in elements)
                {
                    element.draw();
                }
            }
        }

        public void show()
        {
            visible = true;
        }
        public void hide()
        {
            visible = false;
        }

        public void InvertVisible()
        {
            if (visible)
                hide();
            else
                show();
        }
    }

    public class PlayerShiftGUI : GUI
    {
        public PlayerShiftGUI(bool visible, Hero hero) : base(visible) {
            addElement( new HorizontalFillingScale("HEALTH_BAR", new Vector2(674, 637), ref hero.HP) );
            addElement( new HorizontalFillingScale("ARMOR_BAR", new Vector2(674, 673), ref hero.AP) );
            //addElement();
        }
    }
}
