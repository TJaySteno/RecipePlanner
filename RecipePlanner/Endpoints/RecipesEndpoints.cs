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
        recipesGroup.MapGet("/", (RecipePlannerContext dbContext) =>
            dbContext.Recipes
                .Include(recipe => recipe.Owner)
                .ToList()
        );

        // GET /recipes/{id}
        recipesGroup.MapGet("/{id}", (int id, RecipePlannerContext dbContext) =>
        {
            var recipe = dbContext.Recipes.Find(id);

            return recipe is null
                ? Results.NotFound()
                : Results.Ok(recipe);
        })
        .WithName(GetRecipeRouteName);

        // POST /recipes
        recipesGroup.MapPost("", (CreateRecipeDto newRecipe, RecipePlannerContext dbContext) =>
        {
            User? owner = dbContext.Users.Find(newRecipe.OwnerID);
            
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
            dbContext.SaveChanges();

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
        recipesGroup.MapPut("/{id}", (
            int id,
            UpdateRecipeDto updatedRecipe,
            RecipePlannerContext dbContext) =>
        {
            var recipe = dbContext.Recipes.Find(id);

            User? owner = dbContext.Users.Find(updatedRecipe.OwnerID);

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

            dbContext.SaveChanges();

            return Results.NoContent();
        });

        // DELETE /recipes/{id}
        recipesGroup.MapDelete("/{id}", (int id, RecipePlannerContext dbContext) =>
        {
            var recipe = dbContext.Recipes.Find(id);

            if (recipe is null)
            {
                return Results.NotFound();
            }

            dbContext.Recipes.Remove(recipe);
            dbContext.SaveChanges();

            return Results.NoContent();
        });
    }
}