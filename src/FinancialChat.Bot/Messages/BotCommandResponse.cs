namespace FinancialChat.Bot.Messages;

public record BotCommandResponse(string Message, string UserId, string GroupId) : MessageBase;