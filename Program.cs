using System.Text;
using Bluddit.Api.Models;
using Bluddit.Api.Requests;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var dbUser = Environment.GetEnvironmentVariable("MONGODB_USERNAME");
var dbPassword = Environment.GetEnvironmentVariable("MONGODB_PASSWORD");
var dbClusterUrl = Environment.GetEnvironmentVariable("MONGODB_URI");
var databaseName = "BludditDb";

var jwt = Environment.GetEnvironmentVariable("JWT_SECRET");

var connectionString = $"mongodb+srv://{dbUser}:{dbPassword}@{dbClusterUrl}/{databaseName}";

var mongoClient = new MongoClient(connectionString);
var database = mongoClient.GetDatabase(databaseName);

builder.Services.AddSingleton(database);
builder.Services.AddSingleton(database.GetCollection<User>("users"));
builder.Services.AddSingleton(database.GetCollection<Post>("posts"));
builder.Services.AddSingleton(database.GetCollection<Comment>("comments"));
builder.Services.AddSingleton(database.GetCollection<Community>("communities"));
builder.Services.AddSingleton(database.GetCollection<Vote>("votes"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt!))
        };
    });
//should we add authorization? or do it manually?

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.MapScalarApiReference(); 
}

app.UseHttpsRedirection();

//TODO: finish the remaining endpoints
app.MapGet("/users", (string? sortBy, string? order) => {});
app.MapGet("/users/{userid}", () => {});
app.MapGet("/posts", (string? sortBy, string? order) => {});
app.MapGet("/posts/{postid}", () => {});
app.MapGet("/comments", (string? sortBy, string? order) => {});
app.MapGet("/comments/{commentid}", () => {});
app.MapGet("/communities", (string? sortBy, string? order) => {});
app.MapGet("/community/{communityid}", () => {});
app.MapGet("/votes", (string? sortBy, string? order) => {});
app.MapGet("/vote/{voteid}", () => {});
app.MapPost("/users", () => {});
app.MapPost("/auth/login", async (LoginRequest request, IMongoCollection<User> users) =>
{
    var user = await users.Find(x => x.Username == request.Username).FirstOrDefaultAsync();
    
    //TODO: finish login
}); 
app.MapPost("/posts", () => {});
app.MapPost("/comments", () => {});
app.MapPost("/communities", () => {});
app.MapPost("/votes", () => {});
app.MapPut("/users/{userid}", () => {});
app.MapPut("/posts/{postid}", () => {});
app.MapPut("/comments/{commentid}", () => {});
app.MapPut("/communities/{communityid}", () => {});
app.MapPut("/votes/{voteid}", async (string voteid, UpdateVoteRequest request, IMongoCollection<Vote> votes) =>
{
    if (!ObjectId.TryParse(voteid, out _))
    {
        return Results.BadRequest(new { message = "Invalid ID format."});
    }

    //TODO: finish UpdateVote
    return null;
});
app.MapDelete("/users/{userid}", async (string userid, IMongoCollection<Post> users) =>
{
    if (!ObjectId.TryParse(userid, out _))
    {
        return Results.BadRequest(new { message = "Invalid ID format."});
    }
    
    var result = await users.DeleteOneAsync(x => x.Id == userid);
    return result.DeletedCount == 0
        ? Results.NotFound(new { message = $"User '{userid}' not found." })
        : Results.Ok(new { message = "User deleted successfully.", userid });
});
app.MapDelete("/posts/{postid}", async (string postid, IMongoCollection<Post> posts) =>
{
    if (!ObjectId.TryParse(postid, out _))
    {
        return Results.BadRequest(new { message = "Invalid ID format."});
    }
    
    var result = await posts.DeleteOneAsync(x => x.Id == postid);
    return result.DeletedCount == 0
        ? Results.NotFound(new { message = $"Post '{postid}' not found." })
        : Results.Ok(new { message = "Post deleted successfully.", postid });
});
app.MapDelete("/comments/{commentid}", async (string commentid, IMongoCollection<Comment> comments) =>
{
    if (!ObjectId.TryParse(commentid, out _))
    {
        return Results.BadRequest(new { message = "Invalid ID format."});
    }
    
    var result = await comments.DeleteOneAsync(x => x.Id == commentid);
    return result.DeletedCount == 0
        ? Results.NotFound(new { message = $"Comment '{commentid}' not found." })
        : Results.Ok(new { message = "Comment deleted successfully.", commentid });
});
app.MapDelete("/communities/{communityid}", async (string communityid, IMongoCollection<Community> communities) =>
{
    if (!ObjectId.TryParse(communityid, out _))
    {
        return Results.BadRequest(new { message = "Invalid ID format."});
    }
    
    var result = await communities.DeleteOneAsync(x => x.Id == communityid);
    return result.DeletedCount == 0
        ? Results.NotFound(new { message = $"Community '{communityid}' not found." })
        : Results.Ok(new { message = "Community deleted successfully.", communityid });
});
app.MapDelete("/votes/{voteid}", async (string voteid, IMongoCollection<Vote> votes) =>
{
    if (!ObjectId.TryParse(voteid, out _))
    {
        return Results.BadRequest(new { message = "Invalid ID format."});
    }
    
    var result = await votes.DeleteOneAsync(x => x.Id == voteid);
    return result.DeletedCount == 0
        ? Results.NotFound(new { message = $"Vote '{voteid}' not found." })
        : Results.Ok(new { message = "Vote deleted successfully.", voteid });
});

app.Run();