using MongoDB.Driver;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);



var dbUser = Environment.GetEnvironmentVariable("MONGODB_USERNAME");
var dbPassword = Environment.GetEnvironmentVariable("MONGODB_PASSWORD");
var dbClusterUrl = Environment.GetEnvironmentVariable("MONGODB_URI");
var databaseName = "BludditDb";

var connectionString = $"mongodb+srv://omarahemd2005_db_user:<db_password>@bludditdev.xq6pzxe.mongodb.net/{databaseName}";

Console.WriteLine($"Connecting to {connectionString}");

var mongoClient = new MongoClient(connectionString);
var database = mongoClient.GetDatabase(databaseName);

builder.Services.AddSingleton(database);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.MapScalarApiReference(); 
}

app.UseHttpsRedirection();

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
app.MapPost("/posts", () => {});
app.MapPost("/comments", () => {});
app.MapPost("/communities", () => {});
app.MapPost("/votes", () => {});
app.MapPut("/users/{userid}", () => {});
app.MapPut("/posts/{postid}", () => {});
app.MapPut("/comments/{commentid}", () => {});
app.MapPut("/communities/{communityid}", () => {});
app.MapPut("/votes/{voteid}", () => {});
app.MapDelete("/users/{userid}", () => {});
app.MapDelete("/posts/{postid}", () => {});
app.MapDelete("/comments/{commentid}", () => {});
app.MapDelete("/communities/{communityid}", () => {});
app.MapDelete("/votes/{voteid}", () => {});

app.Run();