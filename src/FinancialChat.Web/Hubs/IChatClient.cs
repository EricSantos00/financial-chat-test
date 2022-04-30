namespace FinancialChat.Web.Hubs;

public interface IChatClient
{
    Task ReceiveMessage(DateTime date, string user, string message);
}