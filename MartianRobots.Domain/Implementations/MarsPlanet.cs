using MartianRobots.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace MartianRobots.Domain.Implementations
{
    public class MarsPlanet : IPlanet
    {
        private readonly ICommandsProviderService _commandsProvider;

        public ISurface Surface { get; }

        public MarsPlanet(ISurface surface, ICommandsProviderService commandsProvider)
        {
            Surface = surface ?? throw new ArgumentNullException(nameof(surface));
            _commandsProvider = commandsProvider ?? throw new ArgumentNullException(nameof(commandsProvider));
        }

        public async Task ProcessRobotInput(Robot robot, char input)
        {
            foreach (var command in _commandsProvider.Commands)
            {
                if (await command.CanExecute(input).ConfigureAwait(false))
                {
                    await command.Execute(robot);
                }
            }
        }
    }
}
