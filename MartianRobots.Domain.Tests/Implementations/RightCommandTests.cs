using AutoFixture;
using FluentAssertions;
using MartianRobots.Domain.Implementations;
using MartianRobots.Domain.Models;
using System.Threading.Tasks;
using Xunit;

namespace MartianRobots.Domain.Tests.Implementations
{
    public class RightCommandTests
    {
        [Theory()]
        [InlineData('R', Orientations.East, Orientations.South)]
        [InlineData('R', Orientations.North, Orientations.East)]
        [InlineData('R', Orientations.West, Orientations.North)]
        [InlineData('R', Orientations.South, Orientations.West)]
        public async Task Execute_ChangesOrientation_WhenRobotIsNotLost(char input, Orientations currentOrientation, Orientations expectedOrientation)
        {
            // Prepare
            var fixture = new Fixture();

            var fakeRobot = new Robot()
            {
                IsLost = false,
                LastValidPosition = new Position()
                {
                    Coordinates = new Point(),
                    Orientation = currentOrientation
                }
            };

            var command = new RightCommand();

            // Act
            await command.Execute(fakeRobot);

            // Assert
            fakeRobot.LastValidPosition.Orientation = expectedOrientation;
        }

        [Theory()]
        [InlineData('R', Orientations.East)]
        [InlineData('R', Orientations.North)]
        [InlineData('R', Orientations.West)]
        [InlineData('R', Orientations.South)]
        public async Task Execute_NoChangesCoordinates_WhenRobotIsLost(char input, Orientations currentOrientation)
        {
            // Prepare
            var fixture = new Fixture();

            var fakeRobot = new Robot()
            {
                IsLost = true,
                LastValidPosition = new Position()
                {
                    Coordinates = new Point(),
                    Orientation = currentOrientation
                }
            };

            var command = new RightCommand();

            // Act
            await command.Execute(fakeRobot);

            // Assert
            fakeRobot.LastValidPosition.Orientation = currentOrientation;
        }

        [Fact]
        public async Task CanExecute_ReturnsTrue_WhenInputIsR()
        {
            // Prepare
            var command = new RightCommand();

            // Act
            var canExecute = await command.CanExecute('R');

            // Assert
            canExecute.Should().BeTrue();
        }

        [Theory()]
        [InlineData('F')]
        [InlineData('L')]
        [InlineData('r')]
        public async Task CanExecute_ReturnsFalse_WhenInputIsNotR(char input)
        {
            // Prepare
            var command = new RightCommand();

            // Act
            var canExecute = await command.CanExecute(input);

            // Assert
            canExecute.Should().BeFalse();
        }
    }
}
