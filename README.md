# Overview
- This is a simple browser-based chat application using .NET, with bot capabilities.
- Uses CQRS to separate command and query logic.
- Uses RabbitMQ as message broker to communicate with chat bots.
- Uses SignalR for realtime communication between users and chat rooms.

# Features
- Multiple chat room
- ASP.NET Core identity for user authentication
- Support for multiple chat bots

# How to run

## Requirements:
- [RabbitMQ] (https://www.rabbitmq.com/install-windows.html)
- [.NET 6] (https://dotnet.microsoft.com/download)

You must install and configure RabbitMQ before proceed. If you want, you can use docker as following: 
  ```sh
  $ docker run -d --hostname my-rabbit --name rabbitmq -p 15672:15672 -p 5672:5672 rabbitmq:3-management
  ```

## Settings Files:
  - [src/FinancialChat.Bot/appsettings.json] File with `RabbitMQ` and `Stock` settings for the bot.
  - [src/FinancialChat.Web/appsettings.json] File with `RabbitMQ` settings for communicate with the bots.

## Clone this repo
> git clone https://github.com/EricSantos00/financial-chat-test.git

## Restore nuget packages
```sh
> cd {solution_folder}
> dotnet restore
```

## Run the project
```sh
> cd {solution_folder}/src/FinancialChat.Web
> dotnet run

* Open a new terminal and then run *

> cd {solution_folder}/src/FinancialChat.Bot
> dotnet run
```

## Run Unit Tests
```sh
> cd {solution_folder}/tests/FinancialChat.Bot.Tests
> dotnet test
```

## Test users
After running the project, you can access the web application on localhost. 

You must use one of these credentials:

- user1@test.com Pass@word1
- user2@test.com Pass@word1