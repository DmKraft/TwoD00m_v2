using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoD00m
{
#pragma warning disable CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
#pragma warning disable CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()
    public class Direction
    {
        private enum Directions
        {
            [Description("0")]
            East,
            [Description("1")]
            North,
            [Description("2")]
            West,
            [Description("3")]
            South
        }

        public static Direction East = new Direction(Directions.East);
        public static Direction North = new Direction(Directions.North);
        public static Direction West = new Direction(Directions.West);
        public static Direction South = new Direction(Directions.South);
        private Directions value;
        private int Value { get { return (int)value; } }
        public Point Alpha
        {
            get
            {
                return new Point(
              (int)Math.Cos(Value * Math.PI / 2),
              (int)Math.Sin(Value * Math.PI / 2));
            }
        }

        private Direction(Directions direction)
        {
            value = direction;
        }

        public static Direction AbbToDirection(string arg)
        {
            Direction dir = East;
            while (!GetAttributeOfType<DescriptionAttribute>(dir.value).Description.Equals(arg))
            {
                dir = dir.NextDirection();
            }
            return dir;
        }

        public double GetRadianAngle()
        {
            return Value * Math.PI / 2;
        }
        private Direction NextDirection()
        {
            double angle = (5 * Math.PI / 2 + GetRadianAngle()) % (2 * Math.PI);
            return new Direction((Directions)Math.Round(2 * angle / Math.PI));
        }
        private Direction PastDirection()
        {
            double angle = (3 * Math.PI / 2 + GetRadianAngle()) % (2 * Math.PI);
            return new Direction((Directions)Math.Round(2 * angle / Math.PI));
        }
        
        private static T GetAttributeOfType<T>(Directions enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static Direction GetRightDirection(Direction direction)
        {
            return direction.PastDirection();
        }
        public void GetRightDirection()
        {
            value = PastDirection().value;
        }

        public static Direction GetLeftDirection(Direction direction)
        {
            return direction.NextDirection();
        }
        public void GetLeftDirection()
        {
            value =  NextDirection().value;
        }

        public static Direction GetBackDirection(Direction direction)
        {
            return direction.NextDirection().NextDirection();
        }

        public static int GetSideDifference(Direction direction1, Direction direction2)
        {
            if (direction2 == North || direction2 == South)
                return Math.Abs((direction1.Value + direction2.Value) % 4);
            else
                return Math.Abs((GetBackDirection(direction1).Value + direction2.Value) % 4);
        }
        public static bool operator ==(Direction leftdirection, Direction rightdirection)
        {
            return leftdirection.Value == rightdirection.Value ? true : false;
        }
        public static bool operator !=(Direction leftdirection, Direction rightdirection)
        {
            return leftdirection.Value == rightdirection.Value ? false : true;
        }
    }
}
