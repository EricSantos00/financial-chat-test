using FinancialChat.Core.Entities;
using MediatR;

namespace FinancialChat.Core.Features.Messages.Queries;

public record GetLatestRoomMessagesQuery(string Room) : IRequest<IEnumerable<ChatMessage>>;