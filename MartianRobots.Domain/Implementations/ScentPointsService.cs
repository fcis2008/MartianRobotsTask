using MartianRobots.Domain.Interfaces;
using MartianRobots.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MartianRobots.Domain.Implementations
{
    public class ScentPointsService : IScentPointsService
    {
        private readonly List<Point> _scentPoints = new List<Point>();

        public async Task Add(Point coordinate)
        {
            _scentPoints.Add(coordinate);
        }

        public async Task<bool> IsScentPoint(Point coordinate)
        {
            return _scentPoints.Contains(coordinate);
        }
    }
}
