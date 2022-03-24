using MartianRobots.Domain.Implementations;
using MartianRobots.Domain.Models;
using MartianRobots.Domain.Interfaces;
using System;
using System.Linq;
using SimpleInjector;
using System.Threading.Tasks;
using System.Text;

namespace MartianRobots.App
{
    class Program
    {
        private const char SEPARATOR = ' ';

        private readonly Container _container = new();

        public static async Task Main(string[] args)
        {
            Program p = new();

            StringBuilder output = new();
            await p.ProcessInput(output).ConfigureAwait(false);
            
            Console.Write(output.ToString());

            Console.ReadKey();
        }

        public Program()
        {
            Configure();
        }

        private void Configure()
        {
            ConfigureDependencies();
            _container.Verify();

            ConfigureCommands();
        }

        private void ConfigureDependencies()
        {
            _container.RegisterSingleton<IMessagesService, MessagesService>();

            _container.RegisterSingleton<ICommandsProviderService, CommandsProviderService>();
            _container.RegisterSingleton<IScentPointsService, ScentPointsService>();

            _container.RegisterSingleton<ISurface, MartianSurface>();
            _container.RegisterSingleton<IPlanet, MarsPlanet>();

            _container.Register<Robot, Robot>();

            _container.Collection.Register<ICommand>(typeof(ForwardCommand), typeof(LeftCommand), typeof(RightCommand));
        }

        private void ConfigureCommands()
        {
            var commandsProvider = _container.GetInstance<ICommandsProviderService>();

            var commandsInstances = _container.GetAllInstances<ICommand>();
            foreach (var instance in commandsInstances)
            {
                commandsProvider.Register(instance);
            }
        }

        private static bool TryParseCoordinates(string input, out Point coordinates)
        {
            coordinates = new Point();

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            string[] rawCoordinates = input.Split(SEPARATOR);
            if (rawCoordinates.Length < 2)
            {
                return false;
            }

            bool result = true;
            result &= int.TryParse(rawCoordinates[0], out int parsedValue);
            coordinates.X = parsedValue;

            result &= int.TryParse(rawCoordinates[1], out parsedValue);
            coordinates.Y = parsedValue;

            return result;
        }

        private static bool TryParseRobotPosition(string input, out Position position)
        {
            position = new Position();

            Point coordinates = new Point();
            if(!TryParseCoordinates(input, out coordinates))
            {
                return false;
            }

            position.Coordinates = coordinates;

            char rawOrientation = input.Split(SEPARATOR).Last()[0];
            if (!Enum.IsDefined(typeof(Orientations), (int) rawOrientation))
            {
                return false;
            }

            position.Orientation = (Orientations)rawOrientation;

            return true;
        }

        public async Task ProcessInput(StringBuilder output)
        {
            string line = null;

            line = Console.ReadLine();
            Point upperCoordinates = new Point();
            if (!TryParseCoordinates(line, out upperCoordinates))
            {
                throw new ArgumentException();
            }

            var planet = _container.GetInstance<IPlanet>();
            await planet.Surface.SetSize(upperCoordinates).ConfigureAwait(false);

            do
            {
                line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }
                Position robotPosition;
                if (!TryParseRobotPosition(line, out robotPosition))
                {
                    throw new ArgumentException();
                }
                Robot robot = _container.GetInstance<Robot>();
                robot.LastValidPosition = robotPosition;

                line = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    foreach (var character in line)
                    {
                        await planet.ProcessRobotInput(robot, character);
                    }
                }
                output.AppendLine(robot.StatusToString());

            } while (!string.IsNullOrWhiteSpace(line));
        }
    }
}
