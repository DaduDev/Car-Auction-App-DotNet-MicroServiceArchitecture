namespace SearchService;

using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app) 
    {
        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(retryCount: 10, sleepDurationProvider: retryCount =>
                TimeSpan.FromSeconds(5), onRetry: (exception, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"MongoDB connection attempt {retryCount} failed. Retrying in {timespan.TotalSeconds} seconds...");
                    Console.WriteLine($"Error: {exception.Message}");
                });

        await policy.ExecuteAsync(async () =>
        {
            await DB.InitAsync("SearchDb", MongoClientSettings
                .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));
        });

        try
        {
            await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .Option(o => o.Background = false)
                .CreateAsync();
        }
        catch (MongoCommandException ex) when (ex.CodeName == "IndexOptionsConflict" || ex.CodeName == "IndexKeySpecsConflict")
        {
            await DB.Index<Item>().DropAllAsync();
            await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .Option(o => o.Background = false)
                .CreateAsync();
        }

        var count = await DB.CountAsync<Item>();

        using var scope = app.Services.CreateScope();

        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();

        var items = await httpClient.GetItemsForSearchDb();

        Console.WriteLine(items.Count + " returned from the auction service");

        if(items.Count > 0)    await DB.SaveAsync(items);
    }
}