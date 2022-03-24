using System.Diagnostics.CodeAnalysis;

namespace MartianRobots.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static bool operator ==(Point point1, Point point2)
        {
            return point1.Equals(point2);
        }
        public static bool operator !=(Point point1, Point point2)
        {
            return !point1.Equals(point2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                var point = (Point) obj;
                return X == point.X && Y == point.Y;
            }
            
            return false;
        }
    }
}
