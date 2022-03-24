using MartianRobots.Domain.Implementations;
using System.Threading.Tasks;

namespace MartianRobots.Domain.Interfaces
{
    public interface ICommand
    {
        Task Execute(Robot robot);
        Task<bool> CanExecute(char input);
    }
}
