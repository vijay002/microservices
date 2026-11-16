using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



// -------------------------
// 1 Add CORS policy
// -------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular dev server
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("Authorization")
              .AllowCredentials();
    });
});

// Add services to the container.
//2
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//3
builder.Services.AddOpenApi();

var jwt = builder.Configuration.GetSection("Jwt");
if (jwt != null && jwt["Key"] != null)
{
    var key = Encoding.UTF8.GetBytes(jwt?["Key"]?.ToString() ?? string.Empty);

    //4
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(option =>
        {
            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwt?["Issuer"]?.ToString() ?? string.Empty,
                ValidAudience = jwt?["Audience"]?.ToString() ?? string.Empty,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
        });
}

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("JwtPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});


builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();                // Make sure CORS comes BEFORE YARP

app.UseCors("AllowAngularDev");

//app.Use(async (ctx, next) =>
//{
//    if (ctx.Request.Method == "OPTIONS")
//    {
//        ctx.Response.StatusCode = 200;
//        await ctx.Response.CompleteAsync();
//        return;
//    }
//    await next();
//});

app.Use(async (context, next) =>
{
    Console.WriteLine(context.Request.Path);
    await next();
});

app.UseAuthentication();

app.MapControllers();

app.MapReverseProxy(); //neexed YARP

//app.MapHealthChecks("health");
app.MapGet("/health", () =>
{
    return Results.Ok(new { status = "Gateway Healthy", time = DateTime.UtcNow });
});


//app.UseHttpsRedirection();


await app.RunAsync();

