using AutoFixture;
using FluentAssertions;
using MartianRobots.Domain.Implementations;
using MartianRobots.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MartianRobots.Domain.Tests.Implementations
{
    public class MarsPlanetTests
    {
        [Fact]
        public async Task Constructor_WhenAnArgumentIsNull_ThrowsArgumentNullException()
        {
            // Prepare
            var fakeCommandsProvider = new Mock<ICommandsProviderService>();
            var fakeSurface = new Mock<ISurface>();

            // Act
            Func<MarsPlanet> createNewInstanceNullParams = () => new MarsPlanet(null, null);
            Func<MarsPlanet> createNewInstanceNullSurface = () => new MarsPlanet(null, fakeCommandsProvider.Object);
            Func<MarsPlanet> createNewInstanceNullCommandProviderService = () => new MarsPlanet(fakeSurface.Object, null);

            // Assert
            createNewInstanceNullParams.Should().Throw<ArgumentNullException>();
            createNewInstanceNullSurface.Should().Throw<ArgumentNullException>();
            createNewInstanceNullCommandProviderService.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Surface_Returns_ISurfaceFromConstructor()
        {
            // Prepare
            var fakeSurface = new Mock<ISurface>();
            var fakeCommandsProvider = new Mock<ICommandsProviderService>();

            var marsPlanet = new MarsPlanet(fakeSurface.Object, fakeCommandsProvider.Object);

            // Act
            var returnedSurface = marsPlanet.Surface;

            // Assert
            returnedSurface.Should().NotBeNull();
            returnedSurface.Should().Be(fakeSurface.Object);
        }
        
        [Fact]
        public async Task ProcessRobotInput_WhenCanExecuteReturnsTrue_CallsExecute()
        {
            // Prepare
            var fixture = new Fixture();
            var fakeInput = fixture.Create<char>();
            var fakeRobot = new Robot();

            var fakeSurface = new Mock<ISurface>();

            var fakeCommand = new Mock<ICommand>();
            fakeCommand
                .Setup(p => p.CanExecute(It.IsAny<char>()))
                .Returns(async () => true);

            var fakeCommandsProvider = new Mock<ICommandsProviderService>();
            fakeCommandsProvider
                .SetupGet(p => p.Commands)
                .Returns(new List<ICommand>(0) {fakeCommand.Object});

            var marsPlanet = new MarsPlanet(fakeSurface.Object, fakeCommandsProvider.Object);

            // Act
            await marsPlanet.ProcessRobotInput(fakeRobot, fakeInput);

            // Assert
            fakeCommandsProvider.Verify(mock => mock.Commands, Times.Once);
            fakeCommand.Verify(mock => mock.CanExecute(It.IsAny<char>()), Times.Once);
            fakeCommand.Verify(mock => mock.Execute(It.IsAny<Robot>()), Times.Once);
        }

        [Fact]
        public async Task ProcessRobotInput_WhenCanExecuteReturnsFalse_DoesNotCallExecute()
        {
            // Prepare
            var fixture = new Fixture();
            var fakeInput = fixture.Create<char>();
            var fakeRobot = new Robot();

            var fakeSurface = new Mock<ISurface>();

            var fakeCommand = new Mock<ICommand>();
            fakeCommand
                .Setup(p => p.CanExecute(It.IsAny<char>()))
                .Returns(async () => false);

            var fakeCommandsProvider = new Mock<ICommandsProviderService>();
            fakeCommandsProvider
                .SetupGet(p => p.Commands)
                .Returns(new List<ICommand>(0) { fakeCommand.Object });

            var marsPlanet = new MarsPlanet(fakeSurface.Object, fakeCommandsProvider.Object);

            // Act
            await marsPlanet.ProcessRobotInput(fakeRobot, fakeInput);

            // Assert
            fakeCommandsProvider.Verify(mock => mock.Commands, Times.Once);
            fakeCommand.Verify(mock => mock.CanExecute(It.IsAny<char>()), Times.Once);
            fakeCommand.Verify(mock => mock.Execute(It.IsAny<Robot>()), Times.Never);
        }
    }
}
