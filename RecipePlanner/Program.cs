using RecipePlanner.Data;
using RecipePlanner.Endpoints;
using RecipePlanner.Models;

bool inProduction = false;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

var connString = "Server=localhost;Database=RecipePlanner;Trusted_Connection=True;TrustServerCertificate=True";
builder.Services.AddSqlServer<RecipePlannerContext>(
    connString,
    optionsAction: options => options.UseSeeding((context, _) =>
    {
        if (!inProduction)
        {
            context.Set<Recipe>().RemoveRange(context.Set<Recipe>());
        }

        if (!context.Set<Recipe>().Any())
        {
            context.Set<Recipe>().AddRange(
                new Recipe { OwnerID = 1, Name = "Chili", Description = "A spicy stew with beans and meat.", Url = "https://example.com/chili", Rating = 4.5f, Difficulty = 2, PrepTimeInMinutes = 30, CookTimeInMinutes = 60, CoolTimeInMinutes = 0, Servings = 4, Calories = 400, ProteinInGrams = 20, CarbsInGrams = 50, FatInGrams = 10 },
                new Recipe { OwnerID = 1, Name = "Pasta", Description = "A classic Italian dish with tomato sauce.", Url = "https://example.com/pasta", Rating = 4.0f, Difficulty = 1, PrepTimeInMinutes = 15, CookTimeInMinutes = 30, CoolTimeInMinutes = 0, Servings = 2, Calories = 300, ProteinInGrams = 10, CarbsInGrams = 60, FatInGrams = 5 },
                new Recipe { OwnerID = 1, Name = "Salad", Description = "A fresh mix of vegetables and dressing.", Url = "https://example.com/salad", Rating = 4.2f, Difficulty = 1, PrepTimeInMinutes = 10, CookTimeInMinutes = 0, CoolTimeInMinutes = 0, Servings = 2, Calories = 150, ProteinInGrams = 5, CarbsInGrams = 20, FatInGrams = 5 }
            );
        }

        context.SaveChanges();
    })
);

var app = builder.Build();

app.MapEndpoints();
app.MapRecipesEndpoints();
app.MapUsersEndpoints();

app.MigrateDb();

app.Run();

/*
All of this came with the default build. Leaving for now so I can reference back to it later.

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