using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TwoD00m.Interface
{
    public abstract class Element
    {
        private String name;
        protected List<Component> components;
        private Vector2 position;
        private float transparent;
        private float Transparent
        {
            get { return transparent; }
            set
            {
                if (value > 0)
                {
                    if (value >= 1.0f)
                        transparent = 1.0f;
                    else
                        transparent = value;
                } else {
                    transparent = 0.0f;
                }
            }
        }
        private bool visible;

        public Element(String name, Vector2 position)
        {
            this.name = name;
            this.position = position;
            this.transparent = 1.0f;
            this.visible = true;
            getElement = new List<Component>();
        }
        public List<Component> getElement
        {
            get { return components; }
            set { components = value; }
        }
        public virtual void update()
        {
            if (visible)
            {
                foreach (var component in components)
                {
                    component.update();
                }
            }
        }

        public void draw()
        {
            if (visible)
            {
                foreach (var component in components)
                {
                    component.draw(transparent);
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

        public void addComponent(String textureName, Vector2 componentPosition)
        {
            getElement.Add(new Component(textureName, componentPosition, position));
        }
    }

    public class Cell : Element
    {
        public Cell(String name, Vector2 position) : base(name, position)
        {
            addComponent(name, new Vector2(0, 0));
        }

    }

    public class HorizontalFillingScale : Element
    {
        public HorizontalFillingScale(String name, Vector2 position, ref SurvivalPoint value) : base(name, position)
        {
            components.Add(new Component("FillingHorizontalScalse/" + name + "_1", new Vector2(2, 1), position));
            components.Add(new FillingCell("FillingHorizontalScalse/" + name + "_2", new Vector2(4, 2), position, ref value));
        }
    }
}
