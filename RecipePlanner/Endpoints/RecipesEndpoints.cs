using Microsoft.EntityFrameworkCore;
using RecipePlanner.Data;
using RecipePlanner.Dtos;
using RecipePlanner.Models;

namespace RecipePlanner.Endpoints;

public static class RecipesEndpoints
{
    const string GetRecipeRouteName = "GetRecipe";

    public static void MapRecipesEndpoints(this WebApplication app)
    {
        var recipesGroup = app.MapGroup("/recipes");

        // GET /recipes
        recipesGroup.MapGet("/", async (RecipePlannerContext dbContext) =>
            await dbContext.Recipes
                            .Include(recipe => recipe.Owner)
                            .Select(recipe => new RecipeDto(
                                    recipe.RecipeID,
                                    recipe.Owner,
                                    recipe.Name,
                                    recipe.Description,
                                    recipe.Url,
                                    recipe.Rating,
                                    recipe.Difficulty,
                                    recipe.PrepTimeInMinutes,
                                    recipe.CookTimeInMinutes,
                                    recipe.CoolTimeInMinutes,
                                    recipe.Servings,
                                    recipe.Calories,
                                    recipe.ProteinInGrams,
                                    recipe.CarbsInGrams,
                                    recipe.FatInGrams
                            ))
                            .AsNoTracking()
                            .ToListAsync());

        // GET /recipes/{id}
        recipesGroup.MapGet("/{id}", async (int id, RecipePlannerContext dbContext) =>
        {
            var recipe = await dbContext.Recipes
                .Include(recipe => recipe.Owner)
                .FirstOrDefaultAsync(recipe => recipe.RecipeID == id);

            return recipe is null
                ? Results.NotFound()
                : Results.Ok(
                    new RecipeDetailsDto(
                        recipe.RecipeID,
                        recipe.Owner.UserID,
                        recipe.Name,
                        recipe.Description,
                        recipe.Url,
                        recipe.Rating,
                        recipe.Difficulty,
                        recipe.PrepTimeInMinutes,
                        recipe.CookTimeInMinutes,
                        recipe.CoolTimeInMinutes,
                        recipe.Servings,
                        recipe.Calories,
                        recipe.ProteinInGrams,
                        recipe.CarbsInGrams,
                        recipe.FatInGrams
                    )
                );
        })
        .WithName(GetRecipeRouteName);

        // POST /recipes
        recipesGroup.MapPost("", async (CreateRecipeDto newRecipe, RecipePlannerContext dbContext) =>
        {
            User? owner = await dbContext.Users.FindAsync(newRecipe.OwnerID);
            
            if (owner == null)
            {
                return Results.NotFound();
            }

            Recipe recipe = new()
            {
                Owner = owner,
                Name = newRecipe.Name,
                Description = newRecipe.Description,
                Url = newRecipe.Url,
                Rating = newRecipe.Rating,
                Difficulty = newRecipe.Difficulty,
                PrepTimeInMinutes = newRecipe.PrepTimeInMinutes,
                CookTimeInMinutes = newRecipe.CookTimeInMinutes,
                CoolTimeInMinutes = newRecipe.CoolTimeInMinutes,
                Servings = newRecipe.Servings,
                Calories = newRecipe.Calories,
                ProteinInGrams = newRecipe.ProteinInGrams,
                CarbsInGrams = newRecipe.CarbsInGrams,
                FatInGrams = newRecipe.FatInGrams
            };

            dbContext.Recipes.Add(recipe);
            await dbContext.SaveChangesAsync();

            RecipeDetailsDto recipeDto = new(
                recipe.RecipeID,
                recipe.Owner.UserID,
                recipe.Name,
                recipe.Description,
                recipe.Url,
                recipe.Rating,
                recipe.Difficulty,
                recipe.PrepTimeInMinutes,
                recipe.CookTimeInMinutes,
                recipe.CoolTimeInMinutes,
                recipe.Servings,
                recipe.Calories,
                recipe.ProteinInGrams,
                recipe.CarbsInGrams,
                recipe.FatInGrams
            );

            return Results.CreatedAtRoute(GetRecipeRouteName, new { id = recipeDto.RecipeID }, recipeDto);
        });

        // PUT /recipes/{id}
        recipesGroup.MapPut("/{id}", async (
            int id,
            UpdateRecipeDto updatedRecipe,
            RecipePlannerContext dbContext) =>
        {
            var recipe = await dbContext.Recipes.FindAsync(id);

            User? owner = await dbContext.Users.FindAsync(updatedRecipe.OwnerID);

            if (owner == null)
            {
                return Results.NotFound();
            }

            if (recipe == null)
            {
                return Results.NotFound();
            }

            recipe.Owner = owner;
            recipe.Name = updatedRecipe.Name;
            recipe.Description = updatedRecipe.Description;
            recipe.Url = updatedRecipe.Url;
            recipe.Rating = updatedRecipe.Rating;
            recipe.Difficulty = updatedRecipe.Difficulty;
            recipe.PrepTimeInMinutes = updatedRecipe.PrepTimeInMinutes;
            recipe.CookTimeInMinutes = updatedRecipe.CookTimeInMinutes;
            recipe.CoolTimeInMinutes = updatedRecipe.CoolTimeInMinutes;
            recipe.Servings = updatedRecipe.Servings;
            recipe.Calories = updatedRecipe.Calories;
            recipe.ProteinInGrams = updatedRecipe.ProteinInGrams;
            recipe.CarbsInGrams = updatedRecipe.CarbsInGrams;
            recipe.FatInGrams = updatedRecipe.FatInGrams;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /recipes/{id}
        recipesGroup.MapDelete("/{id}", async (int id, RecipePlannerContext dbContext) =>
        {
            await dbContext.Recipes
                            .Where(recipe => recipe.RecipeID == id)
                            .ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}