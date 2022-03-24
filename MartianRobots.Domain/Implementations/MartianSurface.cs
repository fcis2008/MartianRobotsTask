using MartianRobots.Domain.Interfaces;
using MartianRobots.Domain.Models;
using MartianRobots.Domain.Resources;
using System;
using System.Threading.Tasks;

namespace MartianRobots.Domain.Implementations
{
    public class MartianSurface : ISurface
    {
        private const int COORDINATES_MAX_X = 50;
        private const int COORDINATES_MAX_Y = 50;

        private readonly IMessagesService _messagesService;
        private readonly IScentPointsService _scentPointsService;

        private Point _upperCoordinates;

        public MartianSurface(IMessagesService messagesService, IScentPointsService scentPointsService)
        {
            _messagesService = messagesService ?? throw new ArgumentNullException(nameof(messagesService));
            _scentPointsService = scentPointsService ?? throw new ArgumentNullException(nameof(scentPointsService));
        }

        public async Task SetSize(Point upperCoordinates)
        {

            if (upperCoordinates.X == 0 && upperCoordinates.Y == 0)
            {
                throw new ArgumentOutOfRangeException(_messagesService.GetMessage(Messages.MartianSurface_ErrorInvalidUpperCoords));
            }

            if (!IsInRange(0, COORDINATES_MAX_X, upperCoordinates.X) ||
                !IsInRange(0, COORDINATES_MAX_Y, upperCoordinates.Y))
            {
                throw new ArgumentOutOfRangeException(_messagesService.GetMessage(Messages.MartianSurface_ErrorInvalidUpperCoords));
            }

            _upperCoordinates = upperCoordinates;
        }

        public async Task<Tuple<bool, Point>> TryMove(Point actualCoordinates, Point newCoordinates)
        {
            if (IsSurface(newCoordinates))
            {
                return new Tuple<bool, Point>(true, newCoordinates);
            }

            if (await _scentPointsService.IsScentPoint(actualCoordinates).ConfigureAwait(false))
            {
                return new Tuple<bool, Point>(true, actualCoordinates);
            }

            await _scentPointsService.Add(actualCoordinates).ConfigureAwait(false);
            return new Tuple<bool, Point>(false, actualCoordinates);
        }

        private bool IsSurface(Point coordinate)
        {
            return IsInRange(0, _upperCoordinates.X, coordinate.X) &&
                   IsInRange(0, _upperCoordinates.Y, coordinate.Y);
        }

        private bool IsInRange(int min, int max, int value) => value >= min && value <= max;
    }
}
