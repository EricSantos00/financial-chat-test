using FinancialChat.Bot.Settings;
using Microsoft.Extensions.Options;

namespace FinancialChat.Bot.BotMessageHandler;

public class StockCommandHandler : IBotMessageHandler
{
    private static readonly HttpClient HttpClient = new();
    private readonly StockSettings _stockSettings;

    public StockCommandHandler(IOptions<StockSettings> stockSettings)
    {
        _stockSettings = stockSettings.Value;
    }

    public async Task<string> HandleMessage(string stockCode)
    {
        var stockData = await FetchStockData(stockCode);

        var stockPrice = ParseStockPrice(stockData);

        if (stockPrice == "N/D")
        {
            return $"{stockCode} is not a valid stock code";
        }

        return $"{stockCode.ToUpper()} quote is ${stockPrice} per share";
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