namespace FinancialChat.Web.Hubs;

public interface IChatClient
{
    Task ReceiveMessage(string user, string message);
}