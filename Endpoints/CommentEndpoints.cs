using Bluddit.Api.Configurations;
using Bluddit.Api.Models;
using Bluddit.Api.Requests;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bluddit.Api.Endpoints;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/comments");
        
        //public
        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        
        //private
        group.MapPost("/", CreateComment).RequireAuthorization();
        group.MapPut("/{id}", UpdateComment).RequireAuthorization();
        group.MapDelete("/{id}", DeleteComment).RequireAuthorization();
    }

    private static async Task<IResult> GetAll(string? sortBy, string? order, IMongoCollection<Comment> comments)
    {
        return null;
    }

    private static async Task<IResult> GetById(string id, IMongoCollection<Comment> comments)
    {
        return null;
    }

    private static async Task<IResult> CreateComment(CreateCommentRequest request, IMongoCollection<Comment> comments, IOptions<JwtOptions> options)
    {
        return null;
    }

    private static async Task<IResult> UpdateComment(string id, UpdateCommentRequest request, IMongoCollection<Comment> comments, IOptions<JwtOptions> options)
    {
        return null;
    }
    
    private static async Task<IResult> DeleteComment(string id, DeleteCommentRequest request, IMongoCollection<Comment> comments, IOptions<JwtOptions> options)
    {
        //TODO: actually add authorization check before deleting
        if (!ObjectId.TryParse(id, out _))
        {
            return Results.BadRequest(new { message = "Invalid ID format."});
        }
    
        var result = await comments.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount == 0
            ? Results.NotFound(new { message = $"Comment '{id}' not found." })
            : Results.Ok(new { message = "Comment deleted successfully.", id });
    }
}