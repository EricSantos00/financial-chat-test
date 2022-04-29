using FinancialChat.Bot;
using FinancialChat.Bot.BotCommandHandler;
using FinancialChat.Bot.MessageBroker;
using FinancialChat.Bot.Messages;
using FinancialChat.Bot.Services;
using FinancialChat.Bot.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host
    .CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.Configure<RabbitMQReceiverOptions>(context.Configuration.GetSection("RabbitMQReceiver"));
        services.Configure<RabbitMQSenderOptions>(context.Configuration.GetSection("RabbitMQSender"));
        services.Configure<StockSettings>(context.Configuration.GetSection("StockSettings"));

        services.AddTransient<IMessageSender, RabbitMQSender>();
        services.AddTransient<IMessageReceiver<BotCommandMessage>>(x =>
        {
            var options = new RabbitMQReceiverOptions();
            context.Configuration.GetSection("RabbitMQReceiver").Bind(options);

            return new RabbitMQReceiver<BotCommandMessage>(
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

        services.AddSingleton(services =>
        {
            var manager = new BotCommandHandlerManager(services);

            manager.Register<StockCommandHandler>("/stock");

            return manager;
        });

        services.AddTransient<BotService>();
        services.AddHostedService<ChatService>();
    })
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();