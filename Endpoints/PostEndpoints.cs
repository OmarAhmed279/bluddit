using Bluddit.Api.Configurations;
using Bluddit.Api.Models;
using Bluddit.Api.Requests;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bluddit.Api.Endpoints;

public static class PostEndpoints
{
    public static void MapPostEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/posts");
        
        //public
        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        group.MapGet("/{id}/vote", GetVoteByPostId);
        
        //private
        group.MapPost("/", CreatePost).RequireAuthorization();
        group.MapPut("/{id}", UpdatePost).RequireAuthorization();
        group.MapDelete("/{id}", DeletePost).RequireAuthorization();
    }

    private static async Task<IResult> GetAll(string? sortBy, int? order, int? num, IMongoCollection<Post> posts)
    {
        var result = posts.Find(x => true);
        switch (sortBy)
        {
            case "upvotes":
                switch (order)
                {
                    case 1:
                        result.SortBy(x => x.Upvotes);
                        break;
                    case -1:
                        result.SortByDescending(x => x.Upvotes);
                        break;
                    default:
                        return Results.BadRequest(new { message = "Invalid order format."});
                }
                break;
            case "downvotes":
                switch (order)
                {
                    case 1:
                        result.SortBy(x => x.Downvotes);
                        break;
                    case -1:
                        result.SortByDescending(x => x.Downvotes);
                        break;
                    default:
                        return Results.BadRequest(new { message = "Invalid order format."});
                }
                break;
            case "date":
                switch (order)
                {
                    case 1:
                        result.SortBy(x => x.CreationDate);
                        break;
                    case -1:
                        result.SortByDescending(x => x.CreationDate);
                        break;
                    default:
                        return Results.BadRequest(new { message = "Invalid order format."});
                }
                break;
            case null:
                break;
            default:
                return Results.BadRequest(new { message = "Invalid sort format."});
        }

        var output = await result.Limit(num).ToListAsync();
        return Results.Ok(output);
    }

    private static async Task<IResult> GetById(string id, IMongoCollection<Post> posts)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return Results.BadRequest(new { message = "Invalid ID format."});
        }
        
        var result = await posts.Find(x => x.Id == id).FirstOrDefaultAsync();
        return result != null
            ? Results.Ok(new { message = "Posts found.", result }) : Results.NotFound(new { message = "Posts not found." });
    }

    private static async Task<IResult> GetVoteByPostId(string id, IMongoCollection<Post> posts)
    {
        return null;
    }
    
    private static async Task<IResult> CreatePost(CreatePostRequest request, IMongoCollection<Post> posts, IOptions<JwtOptions> options)
    {
        return null;
    }

    private static async Task<IResult> UpdatePost(string id, UpdatePostRequest request, IMongoCollection<Post> posts, IOptions<JwtOptions> options)
    {
        return null;
    }
    
    private static async Task<IResult> DeletePost(string id, DeletePostRequest request, IMongoCollection<Post> posts, IOptions<JwtOptions> options)
    {
        //TODO: actually add authorization check before deleting
        if (!ObjectId.TryParse(id, out _))
        {
            return Results.BadRequest(new { message = "Invalid ID format."});
        }
    
        var result = await posts.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount == 0
            ? Results.NotFound(new { message = $"Post '{id}' not found." })
            : Results.Ok(new { message = "Post deleted successfully.", id });
    }
}