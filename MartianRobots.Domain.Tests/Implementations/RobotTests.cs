using FluentAssertions;
using MartianRobots.Domain.Implementations;
using MartianRobots.Domain.Models;
using System.Threading.Tasks;
using Xunit;

namespace MartianRobots.Domain.Tests.Implementations
{
    public class RobotTests
    {

        [Fact]
        public async Task StatusToString_WhenNotLost_ReturnsCoordinatesAndOrientation()
        {
            // Prepare
            var robot = new Robot()
            {
                IsLost = false,
                LastValidPosition = new Position()
                {
                    Coordinates = new Point() {X = 3, Y = 4},
                    Orientation = Orientations.North
                }
            };

            // Act
            var output = robot.StatusToString();

            // Assert
            output.Should().NotBeNullOrWhiteSpace();
            output.Should().Be("3 4 N");
        }

        [Fact]
        public async Task StatusToString_WhenLost_ReturnsCoordinatesOrientationAndLOST()
        {
            // Prepare
            var robot = new Robot()
            {
                IsLost = true,
                LastValidPosition = new Position()
                {
                    Coordinates = new Point() { X = 3, Y = 4 },
                    Orientation = Orientations.North
                }
            };

            // Act
            var output = robot.StatusToString();

            // Assert
            output.Should().NotBeNullOrWhiteSpace();
            output.Should().Be("3 4 N LOST");
        }
    }
}
