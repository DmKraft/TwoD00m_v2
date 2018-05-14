using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoD00m
{
    public class Direction
    {
        public static Direction East = new Direction(0);
        public static Direction North = new Direction(1);
        public static Direction West = new Direction(2);
        public static Direction South = new Direction(3);
        private static Direction[] _values = {East, North, West, South};
        private int code;

        private Direction (int code) { this.code = code; }

        public int GetCode() { return code; }

        public double getRadianAngleDirection()
        {
            return GetCode() * Math.PI / 2;
        }

        public static Direction Code(int arg) { return _values[arg]; }

        public static Direction getRightDirection(Direction direction)
        {
            int directionIntegerValue = direction.GetCode();
            directionIntegerValue--;

            if (directionIntegerValue == -1)
            {
                direction = South;
            }
            else
            {
                direction = Direction.Code(directionIntegerValue);
            }
            return direction;
        }

        public static Direction getLeftDirection(Direction direction)
        {
            int directionIntegerValue = direction.GetCode();
            directionIntegerValue++;
            
            if (directionIntegerValue == 4)
            {
                direction = East;
            }
            else
            {
                direction = Direction.Code(directionIntegerValue);
            }
            return direction;
        }

        public static Direction getBackDirection(Direction direction)
        {
            direction = getLeftDirection(direction);
            direction = getLeftDirection(direction);
            return direction;
        }

        public static int GetDifference(Direction direction1, Direction direction2)
        {
            if(direction2.code % 2 == 1 && direction1.code % 2 == 0)
                return Math.Abs(direction1.code - direction2.code);
            else
                return Math.Abs(getBackDirection(direction1).code - direction2.code);
        }
    }
}
