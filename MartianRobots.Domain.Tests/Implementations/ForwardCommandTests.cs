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
    public class ForwardCommandTests
    {
        [Fact]
        public async Task Constructor_WhenAnArgumentIsNull_ThrowsArgumentNullException()
        {
            // Prepare

            // Act
            Func<ForwardCommand> createNewInstance = () => new ForwardCommand(null);

            // Assert
            createNewInstance.Should().Throw<ArgumentNullException>();
        }

        [Theory()]
        [InlineData('F', Orientations.East, new [] {2, 2 }, new [] { 3, 2})]
        [InlineData('F', Orientations.West, new[] { 2, 2 }, new[] { 1, 2 })]
        [InlineData('F', Orientations.North, new[] { 2, 2 }, new[] { 2, 3 })]
        [InlineData('F', Orientations.South, new[] { 2, 2 }, new[] { 2, 1 })]
        public async Task Execute_ChangesCoordinates_WhenRobotIsNotLost(char input, Orientations robotOrientation, int[] actualPointCoords, int[] expectedPointCoords)
        {
            // Prepare
            var expectedResult = new Point() { X = expectedPointCoords[0], Y = expectedPointCoords[1] };

            var fakeRobot = new Robot()
            {
                IsLost = false,
                LastValidPosition = new Position()
                {
                    Coordinates = new Point() {X = actualPointCoords[0], Y = actualPointCoords[1]},
                    Orientation = robotOrientation
                }
            };
            
            var fakeSurface = new Mock<ISurface>();
            fakeSurface
                .Setup(p => p.TryMove(It.IsAny<Point>(), It.IsAny<Point>()))
                .ReturnsAsync(new Tuple<bool, Point>(true, expectedResult));

            var fakePlanet = new Mock<IPlanet>();
            fakePlanet.SetupGet(c => c.Surface).Returns(fakeSurface.Object);

            var command = new ForwardCommand(fakePlanet.Object);

            // Act
            await command.Execute(fakeRobot);

            // Assert
            fakePlanet.Verify(mock => mock.Surface.TryMove(It.IsAny<Point>(), It.IsAny<Point>()), Times.Once());
            fakeRobot.LastValidPosition.Coordinates.Should().Be(expectedResult);
            fakeRobot.LastValidPosition.Orientation.Should().Be(robotOrientation);
        }

        [Theory()]
        [InlineData('F', Orientations.East, new[] { 2, 2 })]
        [InlineData('F', Orientations.West, new[] { 2, 2 })]
        [InlineData('F', Orientations.North, new[] { 2, 2 })]
        [InlineData('F', Orientations.South, new[] { 2, 2 })]
        public async Task Execute_NoChangesCoordinates_WhenRobotIsLost(char input, Orientations robotOrientation, int[] actualPointCoords)
        {
            // Prepare
            var actualCoordinates = new Point() {X = actualPointCoords[0], Y = actualPointCoords[1]};

            var fakeRobot = new Robot()
            {
                IsLost = true,
                LastValidPosition = new Position()
                {
                    Coordinates = actualCoordinates,
                    Orientation = robotOrientation
                }
            };

            var fakeSurface = new Mock<ISurface>();
            fakeSurface
                .Setup(p => p.TryMove(It.IsAny<Point>(), It.IsAny<Point>()));

            var fakePlanet = new Mock<IPlanet>();
            fakePlanet.SetupGet(c => c.Surface).Returns(fakeSurface.Object);

            var command = new ForwardCommand(fakePlanet.Object);

            // Act
            await command.Execute(fakeRobot);

            // Assert
            fakePlanet.Verify(mock => mock.Surface.TryMove(It.IsAny<Point>(), It.IsAny<Point>()), Times.Never);
            fakeRobot.LastValidPosition.Coordinates.Should().Be(actualCoordinates);
            fakeRobot.LastValidPosition.Orientation.Should().Be(robotOrientation);
        }

        [Fact]
        public async Task CanExecute_ReturnsTrue_WhenInputIsF()
        {
            // Prepare
            var fakePlanet = new Mock<IPlanet>();
            var command = new ForwardCommand(fakePlanet.Object);

            // Act
            var canExecute = await command.CanExecute('F');

            // Assert
            canExecute.Should().BeTrue();
        }

        [Theory()]
        [InlineData('L')]
        [InlineData('R')]
        [InlineData('f')]
        public async Task CanExecute_ReturnsFalse_WhenInputIsNotF(char input)
        {
            // Prepare
            var fakePlanet = new Mock<IPlanet>();
            var command = new ForwardCommand(fakePlanet.Object);

            // Act
            var canExecute = await command.CanExecute(input);

            // Assert
            canExecute.Should().BeFalse();
        }
    }
}
