using DotnetRAG.Books.Tools;
using DotnetRAG.DependencyInjection;
using Microsoft.OpenApi.Models;
using NanoWorks.Messaging.RabbitMq.DependencyInjection;
using NanoWorks.Messaging.Serialization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var hostLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ??
                   throw new InvalidOperationException("Cannot determine assembly location."); 

builder.Configuration.AddJsonFile(Path.Combine(hostLocation, "appsettings.json"), optional: false);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "DotnetRAG Books API"
    });
});

builder.Services.AddNanoWorksRabbitMq(options =>
{
    var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ");

    options.UseConnectionString(rabbitMqConfig["ConnectionString"]);

    options.ConfigureMessagePublisher(publisherOptions =>
    {
        publisherOptions.OnSerializationException(PublisherSerializerExceptionBehavior.Ignore);
    });
});

builder.Services.AddBookService(builder.Configuration);

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "DotnetRAG Books API v1");
    options.RoutePrefix = "swagger";
});

app.MapControllers();

using(var scope = app.Services.CreateScope())
{
    var bookRepository = scope.ServiceProvider.GetRequiredService<IBookRepository>();
    await bookRepository.EnsureSchemaCreatedAsync(app.Lifetime.ApplicationStopped);
}

app.Run();
