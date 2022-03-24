using FluentAssertions;
using MartianRobots.Domain.Implementations;
using MartianRobots.Domain.Interfaces;
using MartianRobots.Domain.Models;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MartianRobots.Domain.Tests.Implementations
{
    public class MartianSurfaceTests
    {
        [Fact]
        public async Task Constructor_WhenAnArgumentIsNull_ThrowsArgumentNullException()
        {
            // Prepare
            var fakeMessagesService = new Mock<IMessagesService>();
            var fakeScentPointsService = new Mock<IScentPointsService>();

            // Act
            Func<MartianSurface> createNewInstanceNullParams = () => new MartianSurface(null, null);
            Func<MartianSurface> createNewInstanceNullMessagesService = () => new MartianSurface(null, fakeScentPointsService.Object);
            Func<MartianSurface> createNewInstanceNullScentPointsService = () => new MartianSurface(fakeMessagesService.Object, null);

            // Assert
            createNewInstanceNullParams.Should().Throw<ArgumentNullException>();
            createNewInstanceNullMessagesService.Should().Throw<ArgumentNullException>();
            createNewInstanceNullScentPointsService.Should().Throw<ArgumentNullException>();
        }

        [Theory()]
        [InlineData(0, 15)]
        [InlineData(15, 0)]
        [InlineData(50, 50)]
        [InlineData(15, 23)]
        public async Task SetSize_WhenValidCoordinates_DoesNotThrowException(int x, int y)
        {
            // Prepare
            var upperCoordinates = new Point() {X = x, Y = y};

            var fakeMessagesService = new Mock<IMessagesService>();
            var fakeScentPointsService = new Mock<IScentPointsService>();
            var martianSurface = new MartianSurface(fakeMessagesService.Object, fakeScentPointsService.Object);

            // Act
            Func<Task> setSize = async () => await martianSurface.SetSize(upperCoordinates);

            // Assert
            setSize.Should().NotThrow<Exception>();
        }

        [Theory()]
        [InlineData(0, 0)]
        [InlineData(-1, 20)]
        [InlineData(100, 34)]
        [InlineData(75, 75)]
        [InlineData(-40, 75)]
        [InlineData(10, -20)]
        [InlineData(-40, -40)]
        public async Task SetSize_WhenInvalidCoordinates_ThrowArgumentOutOfRangeException(int x, int y)
        {
            // Prepare
            var upperCoordinates = new Point() { X = x, Y = y };

            var fakeMessagesService = new Mock<IMessagesService>();
            var fakeScentPointsService = new Mock<IScentPointsService>();
            var martianSurface = new MartianSurface(fakeMessagesService.Object, fakeScentPointsService.Object);

            // Act
            Func<Task> setSize = async () => await martianSurface.SetSize(upperCoordinates);

            // Assert
            setSize.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory()]
        [InlineData(new[] { 3, 3 }, new[] { 3, 4 })]
        [InlineData(new[] { 10, 10 }, new[] { 10, 9 })]
        public async Task TryMove_WhenValidNewCoordinates_ReturnsTrueAndNewCoordinates(int[] actualCoords, int[] newCoords)
        {
            // Prepare
            var upperCoordinates = new Point() { X = 10, Y = 10};
            var actualCoordinates = new Point() {X = actualCoords[0], Y = actualCoords[1]};
            var newCoordinates = new Point() { X = newCoords[0], Y = newCoords[1] };

            var fakeMessagesService = new Mock<IMessagesService>();
            
            var fakeScentPointsService = new Mock<IScentPointsService>();
            fakeScentPointsService
                .Setup(p => p.IsScentPoint(It.IsAny<Point>()))
                .ReturnsAsync(false);

            var martianSurface = new MartianSurface(fakeMessagesService.Object, fakeScentPointsService.Object);
            await martianSurface.SetSize(upperCoordinates);

            // Act
            var result = await martianSurface.TryMove(actualCoordinates, newCoordinates);

            // Assert
            result.Should().NotBeNull();
            result.Item1.Should().BeTrue();
            result.Item2.Should().Be(newCoordinates);
        }

        [Theory()]
        [InlineData(new[] { 0, 0 }, new[] { -1, 0 })]
        [InlineData(new[] { 10, 10 }, new[] { 10, 11 })]
        public async Task TryMove_WhenInvalidNewCoordinatesAndNotIsScentPoint_ReturnsFalseAndActualCoordinates(int[] actualCoords, int[] newCoords)
        {
            // Prepare
            var upperCoordinates = new Point() { X = 10, Y = 10 };
            var actualCoordinates = new Point() { X = actualCoords[0], Y = actualCoords[1] };
            var newCoordinates = new Point() { X = newCoords[0], Y = newCoords[1] };

            var fakeMessagesService = new Mock<IMessagesService>();

            var fakeScentPointsService = new Mock<IScentPointsService>();
            fakeScentPointsService
                .Setup(p => p.IsScentPoint(It.IsAny<Point>()))
                .ReturnsAsync(false);

            var martianSurface = new MartianSurface(fakeMessagesService.Object, fakeScentPointsService.Object);
            await martianSurface.SetSize(upperCoordinates);

            // Act
            var result = await martianSurface.TryMove(actualCoordinates, newCoordinates);

            // Assert
            result.Should().NotBeNull();
            result.Item1.Should().BeFalse();
            result.Item2.Should().Be(actualCoordinates);
        }

        [Theory()]
        [InlineData(new[] { 0, 0 }, new[] { -1, 0 })]
        [InlineData(new[] { 10, 10 }, new[] { 10, 11 })]
        public async Task TryMove_WhenInvalidNewCoordinatesAndIsScentPoint_ReturnsTrueAndActualCoordinates(int[] actualCoords, int[] newCoords)
        {
            // Prepare
            var upperCoordinates = new Point() { X = 10, Y = 10 };
            var actualCoordinates = new Point() { X = actualCoords[0], Y = actualCoords[1] };
            var newCoordinates = new Point() { X = newCoords[0], Y = newCoords[1] };

            var fakeMessagesService = new Mock<IMessagesService>();

            var fakeScentPointsService = new Mock<IScentPointsService>();
            fakeScentPointsService
                .Setup(p => p.IsScentPoint(It.IsAny<Point>()))
                .ReturnsAsync(true);

            var martianSurface = new MartianSurface(fakeMessagesService.Object, fakeScentPointsService.Object);
            await martianSurface.SetSize(upperCoordinates);

            // Act
            var result = await martianSurface.TryMove(actualCoordinates, newCoordinates);

            // Assert
            result.Should().NotBeNull();
            result.Item1.Should().BeTrue();
            result.Item2.Should().Be(actualCoordinates);
        }
    }
}
