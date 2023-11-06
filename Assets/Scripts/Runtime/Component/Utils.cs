using System;
using System.Collections.Generic;
using Runtime.View;

namespace Runtime.Component
{
    public static class Utils
    {
        public static void Sort(Direction direction, List<FloatingObjectView> objList)
        {
            objList.Sort((a, b) => Sort(direction, a, b));
        }
        
        private static int Sort(Direction direction, FloatingObjectView a, FloatingObjectView b)
        {
            var aGridCoordinate = a.GetCoordinate();
            var bGridCoordinate = b.GetCoordinate();

            if (aGridCoordinate.y > bGridCoordinate.y)
                return direction == Direction.Up ? 1 : -1;

            if (aGridCoordinate.y < bGridCoordinate.y)
                return direction == Direction.Up ? -1 : 1;

            var isLeft = direction == Direction.Left;

            if (isLeft)
            {
                if (aGridCoordinate.x > bGridCoordinate.x)
                    return 1;
                return -1;
            }
            else
            {
                if (aGridCoordinate.x < bGridCoordinate.x)
                    return 1;
                return -1;
            }
        }
        
        public static T RandomEnumValue<T>()
        {
            var values = Enum.GetValues(typeof(T));
            int random = UnityEngine.Random.Range(0, values.Length);
            return (T)values.GetValue(random);
        }
    }
}