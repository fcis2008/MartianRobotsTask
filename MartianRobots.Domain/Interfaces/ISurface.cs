using MartianRobots.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MartianRobots.Domain.Interfaces
{
    public interface ISurface
    {
        Task SetSize(Point upperCoordinates);
        Task<Tuple<bool, Point>> TryMove(Point actualCoordinates, Point newCoordinates);
    }
}
