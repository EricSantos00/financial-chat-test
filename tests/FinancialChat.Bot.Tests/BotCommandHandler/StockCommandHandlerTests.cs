using System.Threading.Tasks;
using FinancialChat.Bot.BotCommandHandler;
using FinancialChat.Bot.Settings;
using Microsoft.Extensions.Options;
using Xunit;

namespace FinancialChat.Bot.Tests.BotCommandHandler;

public class StockCommandHandlerTests
{
    private readonly StockCommandHandler _stockCommandHandler;

    public StockCommandHandlerTests()
    {
        var options = Options.Create(new StockSettings()
        {
            Url = "https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv"
        });

        _stockCommandHandler = new StockCommandHandler(options);
    }

    [Fact]
    public async Task Should_return_invalid_stock_message_when_stock_is_not_found()
    {
        const string stockCode = "stock123";
        var response = await _stockCommandHandler.HandleCommand(stockCode);

        Assert.Equal($"{stockCode} is not a valid stock code", response);
    }

    [Fact]
    public async Task Should_return_stock_message_when_stock_is_found()
    {
        const string stockCode = "AAPL.US";
        var response = await _stockCommandHandler.HandleCommand(stockCode);
        
        Assert.Contains($"{stockCode} quote is", response);
    }
}