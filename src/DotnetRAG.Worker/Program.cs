using System.Reflection;
using DotnetRAG.Books.Models;
using DotnetRAG.DependencyInjection;
using DotnetRAG.Worker;
using NanoWorks.Messaging.RabbitMq.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

var hostLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ??
    throw new InvalidOperationException("Cannot determine assembly location.");

builder.Configuration.AddJsonFile(Path.Combine(hostLocation, "appsettings.json"), optional: false);

builder.Services.AddNanoWorksRabbitMq(options =>
{
    var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ");

    options.UseConnectionString(rabbitMqConfig["ConnectionString"]);

    options.ConfigureMessageConsumer<BookConsumer>(consumerOptions =>
    {
        consumerOptions.Name(nameof(BookConsumer));
        consumerOptions.Subscribe<BookAddedEvent>(consumer => consumer.OnBookAddedAsync);
    });
});

builder.Services.AddBookService(builder.Configuration);

var host = builder.Build();
host.Run();
