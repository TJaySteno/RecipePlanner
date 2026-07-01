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

                    if (!context.Set<Tag>().Any())
                    {
                        context.Set<Tag>().AddRange(
                            new Tag { DisplayValue = "Gluten-Free", Category = "Dietary" },
                            new Tag { DisplayValue = "Dairy-Free", Category = "Dietary" },
                            new Tag { DisplayValue = "Nut-Free", Category = "Dietary" },
                            new Tag { DisplayValue = "Low-Carb", Category = "Dietary" },
                            new Tag { DisplayValue = "Low-Sodium", Category = "Dietary" },
                            new Tag { DisplayValue = "High-Protein", Category = "Dietary" },
                            new Tag { DisplayValue = "Low-Fat", Category = "Dietary" },
                            new Tag { DisplayValue = "Sugar-Free", Category = "Dietary" },
                            new Tag { DisplayValue = "Diabetic-Friendly", Category = "Dietary" },
                            new Tag { DisplayValue = "Heart-Healthy", Category = "Dietary" },
                            new Tag { DisplayValue = "Vegetarian", Category = "Dietary" },
                            new Tag { DisplayValue = "Vegan", Category = "Dietary" },
                            new Tag { DisplayValue = "Plant-Based", Category = "Dietary" },
                            new Tag { DisplayValue = "Italian", Category = "Cuisine" },
                            new Tag { DisplayValue = "Mexican", Category = "Cuisine" },
                            new Tag { DisplayValue = "Indian", Category = "Cuisine" },
                            new Tag { DisplayValue = "Chinese", Category = "Cuisine" },
                            new Tag { DisplayValue = "Japanese", Category = "Cuisine" },
                            new Tag { DisplayValue = "Mediterranean", Category = "Cuisine" },
                            new Tag { DisplayValue = "Stir-Fry", Category = "Cuisine" },
                            new Tag { DisplayValue = "Casserole", Category = "Cuisine" },
                            new Tag { DisplayValue = "Pasta", Category = "Cuisine" },
                            new Tag { DisplayValue = "Breakfast", Category = "Meal Type" },
                            new Tag { DisplayValue = "Lunch", Category = "Meal Type" },
                            new Tag { DisplayValue = "Dinner", Category = "Meal Type" },
                            new Tag { DisplayValue = "Appetizer", Category = "Meal Type" },
                            new Tag { DisplayValue = "Snack", Category = "Meal Type" },
                            new Tag { DisplayValue = "Dessert", Category = "Meal Type" },
                            new Tag { DisplayValue = "Soup", Category = "Meal Type" },
                            new Tag { DisplayValue = "Salad", Category = "Meal Type" },
                            new Tag { DisplayValue = "Beverage", Category = "Meal Type" },
                            new Tag { DisplayValue = "Christmas", Category = "Season/Holiday" },
                            new Tag { DisplayValue = "Thanksgiving", Category = "Season/Holiday" },
                            new Tag { DisplayValue = "Easter", Category = "Season/Holiday" },
                            new Tag { DisplayValue = "Halloween", Category = "Season/Holiday" },
                            new Tag { DisplayValue = "Summer", Category = "Season/Holiday" },
                            new Tag { DisplayValue = "Winter", Category = "Season/Holiday" },
                            new Tag { DisplayValue = "Spring", Category = "Season/Holiday" },
                            new Tag { DisplayValue = "Fall", Category = "Season/Holiday" },
                            new Tag { DisplayValue = "Quick & Easy", Category = "Cooking Method" },
                            new Tag { DisplayValue = "Slow Cooker", Category = "Cooking Method" },
                            new Tag { DisplayValue = "One-Pot", Category = "Cooking Method" },
                            new Tag { DisplayValue = "Grilling", Category = "Cooking Method" },
                            new Tag { DisplayValue = "Baking", Category = "Cooking Method" },
                            new Tag { DisplayValue = "Roasting", Category = "Cooking Method" }
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

                    if (!context.Set<RecipeTag>().Any())
                    {

                        Recipe chili = context.Set<Recipe>().First(u => u.Name == "Chili");
                        Recipe pasta = context.Set<Recipe>().First(u => u.Name == "Pasta");
                        Recipe salad = context.Set<Recipe>().First(u => u.Name == "Salad");

                        Tag slow = context.Set<Tag>().First(t => t.DisplayValue == "Slow Cooker");
                        Tag quick = context.Set<Tag>().First(t => t.DisplayValue == "Quick & Easy");
                        Tag winter = context.Set<Tag>().First(t => t.DisplayValue == "Winter");
                        Tag saladTag = context.Set<Tag>().First(t => t.DisplayValue == "Salad");
                        Tag pastaTag = context.Set<Tag>().First(t => t.DisplayValue == "Pasta");

                        context.Set<RecipeTag>().AddRange(
                            new RecipeTag { Recipe = chili, Tag = slow },
                            new RecipeTag { Recipe = chili, Tag = winter },
                            new RecipeTag { Recipe = pasta, Tag = pastaTag },
                            new RecipeTag { Recipe = salad, Tag = saladTag },
                            new RecipeTag { Recipe = salad, Tag = quick }
                        );

                        context.SaveChanges();
                    }
                }
            })
        );
    }
}
