using MartianRobots.Domain.Interfaces;
using MartianRobots.Domain.Resources;
using System.Resources;

namespace MartianRobots.Domain.Implementations
{
    public class MessagesService : IMessagesService
    {
        private readonly ResourceManager _resourceManager = new ResourceManager(typeof(Messages));

        public string GetMessage(string messageId)
        {
            return _resourceManager.GetString(messageId);
        }
    }
}
