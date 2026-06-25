/*
All of this came with the default build. Leaving for now.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
*/

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// GET /
app.MapGet("/", () => "Hello World!"); // If not authenticated, redirect to /login. If authenticated, / shows nav options.

// GET /login
app.MapGet("/login", () => "Hello World!");

// GET /recipe
app.MapGet("/recipe", () => "Recipe menu here.");
app.MapGet("/recipe/{id}", (int id) => $"Recipe id {id} here.");

// GET /user
app.MapGet("/user/{id}", (int id) => $"Hello, user {id}!");

app.Run();
