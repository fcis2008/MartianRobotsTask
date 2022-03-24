using MartianRobots.Domain.Interfaces;
using MartianRobots.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MartianRobots.Domain.Implementations
{
    public class ForwardCommand : ICommand
    {
        private const char COMMAND = 'F';

        private readonly IPlanet _planet;

        public ForwardCommand(IPlanet planet)
        {
            _planet = planet ?? throw new ArgumentNullException(nameof(planet));
        }

        public async Task Execute(Robot robot)
        {
            if (!robot.IsLost)
            {
                Point newCoordinates = robot.LastValidPosition.Coordinates;
                switch (robot.LastValidPosition.Orientation)
                {
                    case Orientations.East:
                        newCoordinates.X++;
                        break;
                    case Orientations.North:
                        newCoordinates.Y++;
                        break;
                    case Orientations.South:
                        newCoordinates.Y--;
                        break;
                    case Orientations.West:
                        newCoordinates.X--;
                        break;
                    default:
                        break;
                }

                var resultMove = await _planet.Surface.TryMove(robot.LastValidPosition.Coordinates, newCoordinates);
                robot.IsLost = !resultMove.Item1;
                robot.LastValidPosition.Coordinates = resultMove.Item2;
            }
        }

        public async Task<bool> CanExecute(char input)
        {
            return input == COMMAND;
        }
    }
}
