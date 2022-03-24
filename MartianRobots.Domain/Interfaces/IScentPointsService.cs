using MartianRobots.Domain.Models;
using System.Threading.Tasks;

namespace MartianRobots.Domain.Interfaces
{
    public interface IScentPointsService
    {
        Task Add(Point coordinate);
        Task<bool> IsScentPoint(Point coordinate);
    }
}
