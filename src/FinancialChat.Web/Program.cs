using System.Reflection;
using FinancialChat.Core;
using FinancialChat.Core.Interfaces;
using FinancialChat.Infrastructure.Data;
using FinancialChat.Infrastructure.Data.Repositories;
using FinancialChat.Infrastructure.Identity;
using FinancialChat.Infrastructure.Logger;
using FinancialChat.Infrastructure.RabbitMQ;
using FinancialChat.Web.BotListener;
using FinancialChat.Web.Hubs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddSignalR();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorPages();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddCore();

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();

builder.Services.AddScoped<IMessageSender, RabbitMQSender>();
builder.Services.AddTransient<IMessageReceiver<BotCommandResponse>>(x =>
{
    var options = new RabbitMQReceiverOptions();
    builder.Configuration.GetSection("RabbitMQReceiver").Bind(options);

    return new RabbitMQReceiver<BotCommandResponse>(
        new RabbitMQReceiverOptions
        {
            HostName = options.HostName,
            UserName = options.UserName,
            Password = options.Password,
            ExchangeName = options.ExchangeName,
            QueueName = options.QueueName,
            RoutingKey = options.RoutingKey,
            AutomaticCreateEnabled = true
        });
});

builder.Services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

builder.Services.Configure<RabbitMQSenderOptions>(builder.Configuration.GetSection("RabbitMQSender"));
builder.Services.AddHostedService<BotResponseListenerWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapHub<ChatHub>("/chat");

app.Run();