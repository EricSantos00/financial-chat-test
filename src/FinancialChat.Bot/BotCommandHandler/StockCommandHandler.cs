using FinancialChat.Bot.Settings;
using Microsoft.Extensions.Options;

namespace FinancialChat.Bot.BotCommandHandler;

public class StockCommandHandler : IBotCommandHandler
{
    private static readonly HttpClient HttpClient = new();
    private readonly StockSettings _stockSettings;

    public StockCommandHandler(IOptions<StockSettings> stockSettings)
    {
        _stockSettings = stockSettings.Value;
    }

    public async Task<string> HandleCommand(string commandValue)
    {
        var stockData = await FetchStockData(commandValue);

        var stockPrice = ParseStockPrice(stockData);

        if (stockPrice == "N/D")
        {
            return $"{commandValue} is not a valid stock code";
        }

        return $"{commandValue.ToUpper()} quote is ${stockPrice} per share";
    }

    private async Task<string> FetchStockData(string stockCode)
    {
        var url = string.Format(_stockSettings.Url, stockCode);

        return await HttpClient.GetStringAsync(url);
    }

    private static string ParseStockPrice(string stockData)
    {
        var lines = stockData.Split('\n');
        var stockInfo = lines[1].Split(',');

        return stockInfo[6];
    }
}