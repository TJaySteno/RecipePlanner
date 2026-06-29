using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RecipePlanner.Models;

namespace RecipePlanner.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RecipePlannerContext>();
        dbContext.Database.Migrate();
    }

    public static void AddDummyData(this WebApplicationBuilder builder, bool inProduction)
    {
        var connString = builder.Configuration.GetConnectionString("RecipeManager");

        // DbContext (here, RecipePlannerContext) has a Scoped service lifetime because:
        // 1. It ensures that a new instance of DbContext is created per request.
        // 2. DB connections are a limited and expensive resource.
        // 3. DbContext is not thread-safe. Scoped avoids to concurrency issues.
        // 4. Makes it easier to manage transactions and ensure data consistency.
        // 5. Reusing a DbContext instance can lead to increased memory usage.

        builder.Services.AddSqlServer<RecipePlannerContext>(
            connString,
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                if (!inProduction)
                {
                    if (!context.Set<User>().Any())
                    {
                        context.Set<User>().AddRange(
                            new User { FirstName = "Luke", MiddleName = "James", LastName = "Skywalker", PrimaryEmail = "lskywalker@theforce.com", Username = "lskywalker", PasswordHash = "placeholder" },
                            new User { FirstName = "Sansa", MiddleName = "Jane", LastName = "Stark", PrimaryEmail = "sstark@winterfell.com", Username = "sstark", PasswordHash = "placeholder" },
                            new User { FirstName = "Jon", MiddleName = "Aegon", LastName = "Snow", PrimaryEmail = "LordSnow@thewall.com", Username = "lordsnow", PasswordHash = "placeholder" }
                        );

                        context.SaveChanges();
                    }

                    if (!context.Set<Recipe>().Any())
                    {

                        User luke = context.Set<User>().First(u => u.FirstName == "Luke");
                        User sansa = context.Set<User>().First(u => u.FirstName == "Sansa");
                        User jon = context.Set<User>().First(u => u.FirstName == "Jon");

                        context.Set<Recipe>().AddRange(
                            new Recipe { Owner = luke, Name = "Chili", Description = "A spicy stew with beans and meat.", Url = "https://example.com/chili", Rating = 4.5f, Difficulty = 2, PrepTimeInMinutes = 30, CookTimeInMinutes = 60, CoolTimeInMinutes = 0, Servings = 4, Calories = 400, ProteinInGrams = 20, CarbsInGrams = 50, FatInGrams = 10 },
                            new Recipe { Owner = sansa, Name = "Pasta", Description = "A classic Italian dish with tomato sauce.", Url = "https://example.com/pasta", Rating = 4.0f, Difficulty = 1, PrepTimeInMinutes = 15, CookTimeInMinutes = 30, CoolTimeInMinutes = 0, Servings = 2, Calories = 300, ProteinInGrams = 10, CarbsInGrams = 60, FatInGrams = 5 },
                            new Recipe { Owner = jon, Name = "Salad", Description = "A fresh mix of vegetables and dressing.", Url = "https://example.com/salad", Rating = 4.2f, Difficulty = 1, PrepTimeInMinutes = 10, CookTimeInMinutes = 0, CoolTimeInMinutes = 0, Servings = 2, Calories = 150, ProteinInGrams = 5, CarbsInGrams = 20, FatInGrams = 5 }
                        );

                        context.SaveChanges();
                    }
                }
            })
        );
    }
}
