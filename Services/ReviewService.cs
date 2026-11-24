using MongoDB.Driver;
using TP1.Data;
using TP1.Models;
using MongoDB.Bson;

namespace TP1.Services;

public class ReviewService
{
    private readonly MongoDbContext _context;

    public ReviewService(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetByMovieIdAsync(int movieId)
    {
        return await _context.Reviews
            .Find(r => r.MovieId == movieId)
            .SortByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Review?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return null;
        }

        var filter = Builders<Review>.Filter.Eq("_id", objectId);
        return await _context.Reviews.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Review review)
    {
        review.CreatedAt = DateTime.UtcNow;
        review.Id = null;

        await _context.Reviews.InsertOneAsync(review);
    }

    public async Task<bool> UpdateAsync(string id, string customerName, int rating, string comment)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return false;
        }

        var filter = Builders<Review>.Filter.Eq("_id", objectId);
        var update = Builders<Review>.Update
            .Set(r => r.CustomerName, customerName)
            .Set(r => r.Rating, rating)
            .Set(r => r.Comment, comment);

        var result = await _context.Reviews.UpdateOneAsync(filter, update);
        return result.MatchedCount == 1;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return false;
        }

        var filter = Builders<Review>.Filter.Eq("_id", objectId);
        var result = await _context.Reviews.DeleteOneAsync(filter);
        return result.DeletedCount == 1;
    }
}