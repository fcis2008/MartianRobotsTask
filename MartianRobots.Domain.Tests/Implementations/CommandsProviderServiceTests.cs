using FluentAssertions;
using MartianRobots.Domain.Implementations;
using MartianRobots.Domain.Interfaces;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MartianRobots.Domain.Tests.Implementations
{
    public class CommandsProviderServiceTests
    {
        [Fact]
        public async Task Register_HappyPath()
        {
            // Prepare
            var fakeCommand1 = new Mock<ICommand>();
            var fakeCommand2 = new Mock<ICommand>();
            var fakeCommand3 = new Mock<ICommand>();

            var commandsProvider = new CommandsProviderService();

            // Act
            await commandsProvider.Register(fakeCommand1.Object);
            await commandsProvider.Register(fakeCommand2.Object);
            await commandsProvider.Register(fakeCommand3.Object);

            var commandsList = commandsProvider.Commands;

            // Assert
            commandsList.Should().NotBeNullOrEmpty();
            commandsList.Count().Should().Be(3);
            commandsList.Should().Contain(fakeCommand1.Object);
            commandsList.Should().Contain(fakeCommand2.Object);
            commandsList.Should().Contain(fakeCommand3.Object);
        }

        [Fact]
        public async Task Register_OnNullCommand_ThrowsArgumentNullException()
        {
            // Prepare
            var commandsProvider = new CommandsProviderService();

            // Act
            Func<Task> registerAct = async () => await commandsProvider.Register(null);

            var commandsList = commandsProvider.Commands;

            // Assert
            registerAct.Should().Throw<ArgumentNullException>();
            commandsList.Should().NotBeNull();
            commandsList.Should().BeEmpty();
        }
    }
}
