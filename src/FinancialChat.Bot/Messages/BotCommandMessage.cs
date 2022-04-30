namespace FinancialChat.Bot.Messages;

public record BotCommandMessage(string Command, string UserName, string GroupId);