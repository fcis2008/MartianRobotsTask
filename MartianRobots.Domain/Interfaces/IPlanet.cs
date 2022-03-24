using MartianRobots.Domain.Implementations;
using System.Threading.Tasks;

namespace MartianRobots.Domain.Interfaces
{
    public interface IPlanet
    {
        ISurface Surface { get; }
        Task ProcessRobotInput(Robot robot, char input);
    }
}