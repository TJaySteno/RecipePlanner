using Microsoft.EntityFrameworkCore;
using RecipePlanner.Data;
using RecipePlanner.Dtos;
using RecipePlanner.Models;
using System.Security.Claims;

namespace RecipePlanner.Endpoints;

public static class RecipesEndpoints
{
    const string GetRecipeRouteName = "GetRecipe";

    public static void MapRecipesEndpoints(this WebApplication app)
    {
        var recipesGroup = app.MapGroup("/recipes");

        // GET /recipes - Pulls all recipes
        recipesGroup.MapGet("/", async (RecipePlannerContext dbContext) =>
            await dbContext.Recipes
                            .AsNoTracking()
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
                            .ToListAsync());

        // GET /mine - Pulls all of a User's recipes
        recipesGroup.MapGet("/mine", async (
            RecipePlannerContext dbContext,
            ClaimsPrincipal token) =>
        {
            ArgumentNullException.ThrowIfNull(token.Identity?.Name);
            var username = token.Identity.Name;

            return await dbContext.Recipes
                            .AsNoTracking()
                            .Where(recipe => recipe.Owner.Username == username)
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
                            .ToListAsync();
        });

        // GET /recipes/{id} - Pulls a specific recipe
        recipesGroup.MapGet("/{id}", async (
            int id,
            RecipePlannerContext dbContext) =>
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
        recipesGroup.MapPost("", async (
            CreateRecipeDto newRecipe,
            RecipePlannerContext dbContext,
            ClaimsPrincipal token) =>
        {
            if (TokenNameIsNullOrWhiteSpace(token))
            {
                return Results.Unauthorized();
            }

            User? owner = await dbContext.Users.FindAsync(newRecipe.OwnerID);

            if (owner == null)
            {
                return Results.NotFound();
            }

            if (!UsernamesMatch(owner, token))
            {
                return Results.Forbid();
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
        })
        .RequireAuthorization(policy =>
        {
            policy.RequireRole("user");
        });

        // PUT /recipes/{id}
        recipesGroup.MapPut("/{id}", async (
            int id,
            UpdateRecipeDto updatedRecipe,
            RecipePlannerContext dbContext,
            ClaimsPrincipal token) =>
        {
            if (TokenNameIsNullOrWhiteSpace(token))
            {
                return Results.Unauthorized();
            }

            var recipe = await dbContext.Recipes
                                        .Include(recipe => recipe.Owner)
                                        .FirstOrDefaultAsync(recipe => recipe.RecipeID == id);

            if (recipe == null || recipe.Owner == null)
            {
                return Results.NotFound();
            }

            if (!UsernamesMatch(recipe.Owner, token))
            {
                return Results.Forbid();
            }

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
        })
        .RequireAuthorization(policy => { policy.RequireRole("user"); });

        // DELETE /recipes/{id}
        recipesGroup.MapDelete("/{id}", async (
            int id,
            RecipePlannerContext dbContext,
            ClaimsPrincipal token) =>
        {
            if (TokenNameIsNullOrWhiteSpace(token))
            {
                return Results.Unauthorized();
            }

            var recipe = await dbContext.Recipes
                                        .Include(recipe => recipe.Owner)
                                        .FirstOrDefaultAsync(recipe => recipe.RecipeID == id);

            if (recipe == null || recipe.Owner == null)
            {
                return Results.NotFound();
            }

            if (!UsernamesMatch(recipe.Owner, token) && !IsAdmin(token))
            {
                return Results.Forbid();
            }

            await dbContext.Recipes
                            .Where(recipe => recipe.RecipeID == id)
                            .ExecuteDeleteAsync();

            return Results.NoContent();
        })
        .RequireAuthorization(policy => { policy.RequireRole("user", "admin"); });
    }

    private static bool TokenNameIsNullOrWhiteSpace(ClaimsPrincipal token)
    {
        var username = token.Identity?.Name;
        
        ArgumentNullException.ThrowIfNull(username);

        return string.IsNullOrWhiteSpace(username);
    }

    private static bool IsAdmin(ClaimsPrincipal token) {
        return token.IsInRole("admin");
    }

    private static bool UsernamesMatch(User owner, ClaimsPrincipal token)
    {
        return owner.Username == token.Identity?.Name;
    }
}