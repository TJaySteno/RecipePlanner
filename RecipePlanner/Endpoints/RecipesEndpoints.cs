using RecipePlanner.Dtos;

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
        recipesGroup.MapPost("", (CreateRecipeDto newRecipe) =>
            {
                RecipeDto recipe = new(
                    recipes.Max(recipe => recipe.RecipeID) + 1,
                    newRecipe.OwnerID,
                    newRecipe.Name,
                    newRecipe.Description,
                    newRecipe.Url,
                    newRecipe.Rating,
                    newRecipe.Difficulty,
                    newRecipe.PrepTimeInMinutes,
                    newRecipe.CookTimeInMinutes,
                    newRecipe.CoolTimeInMinutes,
                    newRecipe.Servings,
                    newRecipe.Calories,
                    newRecipe.ProteinInGrams,
                    newRecipe.CarbsInGrams,
                    newRecipe.FatInGrams
                );

                recipes.Add(recipe);

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
                    updatedRecipe.OwnerID,
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