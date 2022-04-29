using System.Threading.Tasks;
using FinancialChat.Bot.BotCommandHandler;
using FinancialChat.Bot.MessageBroker;
using FinancialChat.Bot.Messages;
using FinancialChat.Bot.Services;
using FinancialChat.Bot.Tests.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace FinancialChat.Bot.Tests.Services;

public class BotServiceTests
{
    private readonly BotCommandHandlerManager _botCommandHandlerManager;
    private readonly Mock<IMessageSender> _messageSender;
    private readonly BotService _botService;

    public BotServiceTests()
    {
        _botCommandHandlerManager = new BotCommandHandlerManager(new ServiceCollection().BuildServiceProvider());
        _messageSender = new Mock<IMessageSender>();
        _botService = new BotService(_botCommandHandlerManager, _messageSender.Object);
    }

    [Theory]
    [InlineData("test")]
    [InlineData("/test")]
    [InlineData("//test")]
    [InlineData("\\test")]
    public async Task Should_publish_invalid_message_to_broker_when_command_is_invalid(string command)
    {
        await _botService.ProcessCommand(new BotCommandMessage(command, "user", "group"));

        _messageSender.Verify(x => x.Publish(
                It.Is<BotCommandResponse>(botCommandResponse =>
                    botCommandResponse.Message == "Invalid command" && botCommandResponse.UserId == "user" &&
                    botCommandResponse.GroupId == "group")),
            Times.Once);
    }
    
    [Fact]
    public async Task Should_publish_command_not_found_message_when_command_handler_is_not_found()
    {
        await _botService.ProcessCommand(new BotCommandMessage("/stock=123", "user", "group"));

        _messageSender.Verify(x => x.Publish(
                It.Is<BotCommandResponse>(botCommandResponse =>
                    botCommandResponse.Message == "Command not found" && botCommandResponse.UserId == "user" &&
                    botCommandResponse.GroupId == "group")),
            Times.Once);
    }
    
    [Fact]
    public async Task Should_publish_command_result_when_command_is_handled()
    {
        const string valueToAssert = "123";
        
        _botCommandHandlerManager.Register<TestCommandHandler>($"/test");
        
        await _botService.ProcessCommand(new BotCommandMessage($"/test={valueToAssert}", "user", "group"));

        _messageSender.Verify(x => x.Publish(
                It.Is<BotCommandResponse>(botCommandResponse =>
                    botCommandResponse.Message == valueToAssert && botCommandResponse.UserId == "user" &&
                    botCommandResponse.GroupId == "group")),
            Times.Once);
    }
}