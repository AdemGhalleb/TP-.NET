using MongoDB.Driver;
using TP1.Models;

namespace TP1.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<Review> Reviews =>
        _database.GetCollection<Review>("Reviews");
}