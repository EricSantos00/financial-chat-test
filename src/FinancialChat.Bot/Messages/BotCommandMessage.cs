namespace FinancialChat.Bot.Messages;

public record BotCommandMessage(string Command, string UserId, string GroupId);