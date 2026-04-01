using BiddingService;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;
using MongoDB.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bidservice", false));
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
        {
            cfg.AutoStart = true;
            cfg.Host("rabbitmq", "/", h =>
            {
                h.Username("dev");
                h.Password("dev");
            });

            cfg.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(10)));

            cfg.ConfigureEndpoints(context);
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.Authority = builder.Configuration["IdentityServer:Authority"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.NameClaimType = "username";
    });
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHostedService<CheckAuctionFinished>();
builder.Services.AddScoped<GrpcAuctionClient>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

await DB.InitAsync("BidDb", MongoClientSettings
    .FromConnectionString(builder.Configuration.GetConnectionString("BidDbConnection")));

app.Run();
