using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialChat.Infrastructure.Identity;

public class IdentityDataSeeder
{
    private readonly IServiceProvider _serviceProvider;

    public IdentityDataSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async void SeedTestUsers()
    {
        var users = new List<IdentityUser>
        {
            new() {UserName = "user1@test.com", Email = "user1@test.com"},
            new() {UserName = "user2@test.com", Email = "user2@test.com"},
        };

        using var scope = _serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        foreach (var user in users.Where(user => !userManager.Users.Any(x => x.Email == user.Email)))
        {
            await userManager.CreateAsync(user, "Pass@word1");
        }
    }
}