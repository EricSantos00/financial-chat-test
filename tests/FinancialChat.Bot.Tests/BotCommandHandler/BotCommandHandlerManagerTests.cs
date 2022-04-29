using System;
using FinancialChat.Bot.BotCommandHandler;
using FinancialChat.Bot.Tests.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FinancialChat.Bot.Tests.BotCommandHandler;

public class BotCommandHandlerManagerTests
{
    private readonly BotCommandHandlerManager _botCommandHandlerManager;

    public BotCommandHandlerManagerTests()
    {
        _botCommandHandlerManager = new BotCommandHandlerManager(new ServiceCollection().BuildServiceProvider());
    }

    [Fact]
    public void Should_throw_exception_when_command_is_already_registered()
    {
        _botCommandHandlerManager.Register<TestCommandHandler>("/test");
        Assert.Throws<Exception>(() => _botCommandHandlerManager.Register<TestCommandHandler>("/test"));
    }

    [Fact]
    public void Should_return_null_if_command_is_not_registered()
    {
        Assert.Null(_botCommandHandlerManager.GetHandler("/test"));
    }

    [Fact]
    public void Should_return_handler_if_command_is_registered()
    {
        _botCommandHandlerManager.Register<TestCommandHandler>("/test");

        var handler = _botCommandHandlerManager.GetHandler("/test");

        Assert.NotNull(handler);
        Assert.Equal(handler.GetType(), typeof(TestCommandHandler));
    }
}