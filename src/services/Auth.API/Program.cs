using Auth.API;
using Serilog;
using Serilog.Events;

// 1. Configure early bootstrap logger to capture errors during startup before host is fully initialized
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting Auth.API microservice host...");

    var builder = WebApplication.CreateBuilder(args);

    // 2. Attach Serilog to ASP.NET Core host and read settings from appsettings.json
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // Add services to the container.
    builder.Services.AddBackEndServices(builder.Configuration);

    builder.Services.AddScoped<TokenService>();
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

    var app = builder.Build();

    // 3. Add Serilog Request Logging to trace incoming HTTP requests, duration, and status codes
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.GetLevel = (httpContext, elapsed, ex) => ex != null || httpContext.Response.StatusCode >= 500
            ? LogEventLevel.Error
            : httpContext.Response.StatusCode >= 400
                ? LogEventLevel.Warning
                : LogEventLevel.Information;
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown");
            diagnosticContext.Set("TraceId", httpContext.TraceIdentifier);
        };
    });

    app.RegisterBackEndBuilder(app.Environment, app, builder.Configuration);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.MapGet("/health", () =>
    {
        Log.Information("Health check endpoint invoked successfully.");
        return Results.Ok(new { status = "Auth Healthy", time = DateTime.UtcNow });
    });

    app.UseAuthorization();

    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Auth.API microservice terminated unexpectedly due to an unhandled startup exception.");
}
finally
{
    Log.Information("Shutting down Auth.API microservice...");
    await Log.CloseAndFlushAsync();
}


