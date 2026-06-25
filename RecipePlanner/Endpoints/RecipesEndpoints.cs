using RecipePlanner.Dtos;

namespace RecipePlanner.Endpoints;

public static class RecipesEndpoints
{
    const string GetRecipeRouteName = "GetRecipe";

    // REGION: DUMMY DATA
    private static readonly List<RecipeDto> recipes = CreateDummyRecipes();

    private static List<RecipeDto> CreateDummyRecipes()
    {
        return
        [
            new(1, 1, "Chili", "A spicy stew with beans and meat.", "https://example.com/chili", 4.5f, 2, 30, 60, 0, 4, 400, 20, 50, 10),
            new(2, 1, "Pasta", "A classic Italian dish with tomato sauce.", "https://example.com/pasta", 4.0f, 1, 15, 30, 0, 2, 300, 10, 60, 5),
            new(3, 1, "Salad", "A fresh mix of vegetables and dressing.", "https://example.com/salad", 4.2f, 1, 10, 0, 0, 2, 150, 5, 20, 5)
        ];
    }
    // ENDREGION: DUMMY DATA

    public static void MapRecipesEndpoints(this WebApplication app)
    {
        var recipesGroup = app.MapGroup("/recipes");

        // GET /recipes
        recipesGroup.MapGet("", () => recipes);

        // GET /recipes/{id}
        recipesGroup.MapGet("/{id}", (int id) =>
            {
                var recipe = recipes.Find(r => r.ID == id);

                return recipe is null ? Results.NotFound() : Results.Ok(recipe);
            })
            .WithName(GetRecipeRouteName);

        // POST /recipes
        recipesGroup.MapPost("", (CreateRecipeDto newRecipe) =>
            {
                RecipeDto recipe = new(
                    recipes.Max(recipe => recipe.ID) + 1,
                    newRecipe.UserID,
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

                return Results.CreatedAtRoute(GetRecipeRouteName, new { id = recipe.ID }, recipe);
            });

        // PUT /recipes/{id}
        recipesGroup.MapPut("/{id}", (int id, UpdateRecipeDto updatedRecipe) =>
            {
                var index = recipes.FindIndex(recipe => recipe.ID == id);

                if (index == -1)
                {
                    return Results.NotFound();
                }

                recipes[index] = new RecipeDto(
                    id,
                    updatedRecipe.UserID,
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
                recipes.RemoveAll(recipe => recipe.ID == id);

                return Results.NoContent();
            });

        // ENDREGION: ENDPOINTS
    }
}