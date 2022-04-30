using FinancialChat.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialChat.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
}