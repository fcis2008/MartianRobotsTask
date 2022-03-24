using MartianRobots.Domain.Interfaces;
using MartianRobots.Domain.Models;
using System.Threading.Tasks;

namespace MartianRobots.Domain.Implementations
{
    public class RightCommand : ICommand
    {
        private const char COMMAND = 'R';

        public async Task Execute(Robot robot)
        {
            if (!robot.IsLost)
            {
                switch (robot.LastValidPosition.Orientation)
                {
                    case Orientations.East:
                        robot.LastValidPosition.Orientation = Orientations.South;
                        break;
                    case Orientations.South:
                        robot.LastValidPosition.Orientation = Orientations.West;
                        break;
                    case Orientations.West:
                        robot.LastValidPosition.Orientation = Orientations.North;
                        break;
                    case Orientations.North:
                        robot.LastValidPosition.Orientation = Orientations.East;
                        break;
                    default:
                        break;
                }
            }
        }

        public async Task<bool> CanExecute(char input)
        {
            return input == COMMAND;
        }
    }
}
