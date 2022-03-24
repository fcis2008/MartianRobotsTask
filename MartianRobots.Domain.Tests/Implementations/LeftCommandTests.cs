using AutoFixture;
using FluentAssertions;
using MartianRobots.Domain.Implementations;
using MartianRobots.Domain.Models;
using System.Threading.Tasks;
using Xunit;

namespace MartianRobots.Domain.Tests.Implementations
{
    public class LeftCommandTests
    {
        [Theory()]
        [InlineData('L', Orientations.East, Orientations.North)]
        [InlineData('L', Orientations.North, Orientations.West)]
        [InlineData('L', Orientations.West, Orientations.South)]
        [InlineData('L', Orientations.South, Orientations.East)]
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

            var command = new LeftCommand();

            // Act
            await command.Execute(fakeRobot);

            // Assert
            fakeRobot.LastValidPosition.Orientation = expectedOrientation;
        }

        [Theory()]
        [InlineData('L', Orientations.East)]
        [InlineData('L', Orientations.North)]
        [InlineData('L', Orientations.West)]
        [InlineData('L', Orientations.South)]
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

            var command = new LeftCommand();

            // Act
            await command.Execute(fakeRobot);

            // Assert
            fakeRobot.LastValidPosition.Orientation = currentOrientation;
        }

        [Fact]
        public async Task CanExecute_ReturnsTrue_WhenInputIsL()
        {
            // Prepare
            var command = new LeftCommand();

            // Act
            var canExecute = await command.CanExecute('L');

            // Assert
            canExecute.Should().BeTrue();
        }

        [Theory()]
        [InlineData('F')]
        [InlineData('R')]
        [InlineData('l')]
        public async Task CanExecute_ReturnsFalse_WhenInputIsNotL(char input)
        {
            // Prepare
            var command = new LeftCommand();

            // Act
            var canExecute = await command.CanExecute(input);

            // Assert
            canExecute.Should().BeFalse();
        }
    }
}
