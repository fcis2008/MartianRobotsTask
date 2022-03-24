using System.Collections.Generic;
using System.Threading.Tasks;

namespace MartianRobots.Domain.Interfaces
{
    public interface ICommandsProviderService
    {
        public IEnumerable<ICommand> Commands { get; }

        public Task Register(ICommand command);
    }
}
