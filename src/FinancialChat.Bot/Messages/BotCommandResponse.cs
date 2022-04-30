namespace FinancialChat.Bot.Messages;

public record BotCommandResponse(string Message, string UserName, string GroupId) : MessageBase;