using System.Diagnostics.CodeAnalysis;

namespace MartianRobots.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class Position
    {
        public Point Coordinates { get; set; }

        public Orientations Orientation { get; set; }
    }
}
