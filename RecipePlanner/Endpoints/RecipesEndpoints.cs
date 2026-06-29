using Microsoft.VisualBasic;
using RecipePlanner.Data;
using RecipePlanner.Dtos;
using RecipePlanner.Models;

namespace RecipePlanner.Endpoints;

public static class RecipesEndpoints
{
    const string GetRecipeRouteName = "GetRecipe";

    private static readonly List<RecipeDto> recipes = new List<RecipeDto>();

    public static void MapRecipesEndpoints(this WebApplication app)
    {
        var recipesGroup = app.MapGroup("/recipes");

        // GET /recipes
        recipesGroup.MapGet("/", () => recipes);

        // GET /recipes/{id}
        recipesGroup.MapGet("/{id}", (int id) =>
            {
                var recipe = recipes.Find(recipe => recipe.RecipeID == id);

                return recipe is null ? Results.NotFound() : Results.Ok(recipe);
            })
            .WithName(GetRecipeRouteName);

        // POST /recipes
        recipesGroup.MapPost("", (CreateRecipeDto newRecipe, RecipePlannerContext dbContext) =>
            {
                Recipe recipe = new()
                {
                    Owner = newRecipe.Owner,
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

                return Results.CreatedAtRoute(GetRecipeRouteName, new { id = recipe.RecipeID }, recipe);
            });

        // PUT /recipes/{id}
        recipesGroup.MapPut("/{id}", (int id, UpdateRecipeDto updatedRecipe) =>
            {
                var index = recipes.FindIndex(recipe => recipe.RecipeID == id);

                if (index == -1)
                {
                    return Results.NotFound();
                }

                recipes[index] = new RecipeDto (
                    id,
                    updatedRecipe.Owner,
                    updatedRecipe.Name,
                    updatedRecipe.Description,
                    updatedRecipe.Url,
                    updatedRecipe.Rating,
                    updatedRecipe.Difficulty,
                    updatedRecipe.PrepTimeInMinutes,
                    updatedRecipe.CookTimeInMinutes,
                    updatedRecipe.CoolTimeInMinutes,
                    updatedRecipe.Servings,
                    updatedRecipe.Calories,
                    updatedRecipe.ProteinInGrams,
                    updatedRecipe.CarbsInGrams,
                    updatedRecipe.FatInGrams
                );

                return Results.NoContent();
            });

        // DELETE /recipes/{id}
        recipesGroup.MapDelete("/{id}", (int id) =>
            {
                // Todo: Do I want a soft delete option?
                recipes.RemoveAll(recipe => recipe.RecipeID == id);

                return Results.NoContent();
            });
    }
}