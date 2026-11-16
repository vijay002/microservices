using Azure.Messaging.ServiceBus;
using Ordering.API.Abs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//abs service bus
builder.Services.AddScoped<IServiceBusPublisher, ServiceBusPublisher>();

builder.Services.AddSingleton<ServiceBusClient>(_ =>
{
    string connectionstring = builder.Configuration.GetConnectionString("azure-service-bus");
    return new ServiceBusClient(connectionstring);
});

builder.Services.AddControllers();

builder.Services.AddOpenApi(); // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseAuthorization();
//app.UseHttpsRedirection();
app.MapControllers();

//app.MapGet("/health", () =>
//{
//    return Results.Ok(new { status = "Order Healthy", time = DateTime.UtcNow });
//});



await app.RunAsync();

