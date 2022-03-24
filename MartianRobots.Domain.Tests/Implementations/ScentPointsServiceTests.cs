using FluentAssertions;
using MartianRobots.Domain.Implementations;
using MartianRobots.Domain.Models;
using System.Threading.Tasks;
using Xunit;

namespace MartianRobots.Domain.Tests.Implementations
{
    public class ScentPointsServiceTests
    {
        [Theory()]
        [InlineData(0, 15)]
        [InlineData(15, 0)]
        [InlineData(50, 50)]
        [InlineData(15, 23)]
        public async Task IsScentPoint_ReturnsTrue(int x, int y)
        {
            // Prepare
            var coordinates = new Point() { X = x, Y = y };
            var scentPointsService = new ScentPointsService();
            
            // Act
            await scentPointsService.Add(coordinates);
            var result = await scentPointsService.IsScentPoint(coordinates);

            // Assert
            result.Should().BeTrue();
        }

        [Theory()]
        [InlineData(0, 15)]
        [InlineData(15, 0)]
        [InlineData(50, 50)]
        [InlineData(15, 23)]
        public async Task IsScentPoint_ReturnsFalse(int x, int y)
        {
            // Prepare
            var coordinates = new Point() { X = x, Y = y };
            var scentPointsService = new ScentPointsService();

            // Act
            var result = await scentPointsService.IsScentPoint(coordinates);

            // Assert
            result.Should().BeFalse();
        }
    }
}
