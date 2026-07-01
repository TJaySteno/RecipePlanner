using Azure;
using Microsoft.EntityFrameworkCore;
using RecipePlanner.Data;
using RecipePlanner.Dtos;
using RecipePlanner.Models;
using System.Security.Claims;

namespace RecipePlanner.Endpoints;

public static class RecipeTagsEndpoints
{
    const string GetTagRouteName = "GetTag";
    const string GetRecipeTagRouteName = "GetRecipeTags";

    public static void MapRecipeTagsEndpoints(this WebApplication app)
    {
        var tagsGroup = app.MapGroup("/tags");
        var recipeTagsGroup = app.MapGroup("/recipes/{recipeID}/tags");

        //////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// TAGS //////////////////////////////////
        //////////////////////////////////////////////////////////////////////////

        // GET /tags
        tagsGroup.MapGet("/", async (RecipePlannerContext dbContext) =>
            await dbContext.Tags
                            .AsNoTracking()
                            //.Include(tag => tag.CreatedBy) (Todo: Not sure if I'll need/want this yet.)
                            .ToListAsync());

        // GET /tags/{id}
        tagsGroup.MapGet("/{tagID}", async (int tagID, RecipePlannerContext dbContext) =>
        {
            var tag = await dbContext.Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(tag => tag.TagID == tagID);

            return tag is null ? Results.NotFound() : Results.Ok(tag);
        })
        .WithName(GetTagRouteName);

        // POST /tags
        // Create a new tag. Users and Admins only.
        tagsGroup.MapPost("/", async (
            CreateTagDto newTag,
            RecipePlannerContext dbContext,
            ClaimsPrincipal token) =>
        {
            var tokenName = GetUsername(token);

            if (tokenName == null)
            {
                return Results.Unauthorized();
            }

            bool alreadyExists = await dbContext.Tags
                                                .AnyAsync(rt => rt.DisplayValue == newTag.DisplayValue
                                                                && rt.Category == newTag.Category);

            if (alreadyExists)
            {
                return Results.Conflict("This tag already exists in this category.");
            }

            User? createdBy = await dbContext.Users
                                             .FirstOrDefaultAsync(user => user.Username == tokenName);

            // Todo: Check whether Display Value already exists for another tag.

            Tag tag = new()
            {
                DisplayValue = newTag.DisplayValue,
                Category = newTag.Category,
                CreatedBy = createdBy,
                ModifiedBy = createdBy
            };

            dbContext.Tags.Add(tag);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetTagRouteName, new { tagID = tag.TagID }, tag);
        })
        .RequireAuthorization(policy => { policy.RequireRole("admin", "user"); });

        // PUT /tags/{id}
        // Update existing tag
        // Owner or Admin only (need CreatedBy to track this)
        tagsGroup.MapPut("/{tagID}", async (
            int tagID,
            UpdateTagDto updatedTag,
            RecipePlannerContext dbContext,
            ClaimsPrincipal token) =>
        {
            var tokenName = GetUsername(token);

            if (tokenName == null)
            {
                return Results.Unauthorized();
            }

            bool alreadyExists = await dbContext.Tags
                                                .AnyAsync(rt => rt.DisplayValue == updatedTag.DisplayValue
                                                                && rt.Category == updatedTag.Category
                                                                && rt.TagID != tagID);

            if (alreadyExists)
            {
                return Results.Conflict("This tag already exists in this category.");
            }

            var tag = await dbContext.Tags.FirstOrDefaultAsync(tag => tag.TagID == tagID);

            if (tag == null)
            {
                return Results.NotFound();
            }

            User? tokenUser = await dbContext.Users
                                             .Where(user => user.Username == tokenName)
                                             .FirstOrDefaultAsync();

            // Owners can update their own tags.
            // Admins can update any tag.
            if (tag.CreatedBy != tokenUser && !IsAdmin(token))
            {
                return Results.Forbid();
            }

            tag.DisplayValue = updatedTag.DisplayValue;
            tag.Category = updatedTag.Category;
            tag.ModifiedBy = tokenUser;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        })
        .RequireAuthorization(policy => { policy.RequireRole("admin", "user"); });

        // DELETE /tags/{id}
        // Delete existing tag
        // Owner or Admin only
        tagsGroup.MapDelete("/{id}", async (
                    int id,
                    RecipePlannerContext dbContext,
                    ClaimsPrincipal token) =>
        {
            var tokenName = GetUsername(token);

            if (tokenName == null)
            {
                return Results.Unauthorized();
            }

            var tag = await dbContext.Tags.FirstOrDefaultAsync(tag => tag.TagID == id);

            if (tag == null)
            {
                return Results.NotFound();
            }

            User? tokenUser = await dbContext.Users
                                             .Where(user => user.Username == tokenName)
                                             .FirstOrDefaultAsync();

            // Owners can update their own tags.
            // Admins can update any tag.
            if (tag.CreatedBy != tokenUser && !IsAdmin(token))
            {
                return Results.Forbid();
            }

            await dbContext.Tags
                            .Where(tag => tag.TagID == id)
                            .ExecuteDeleteAsync();

            return Results.NoContent();
        })
        .RequireAuthorization(policy => { policy.RequireRole("admin", "user"); });



        //////////////////////////////////////////////////////////////////////////
        /////////////////////////////// RECIPE TAGS //////////////////////////////
        //////////////////////////////////////////////////////////////////////////

        // GET /
        // Get all tags for a recipe. Anyone can view.
        recipeTagsGroup.MapGet("/", async (
            int recipeID,
            RecipePlannerContext dbContext) =>
        {
            Recipe? recipe = await dbContext.Recipes
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeID);

            List<Tag>? tags = await dbContext.RecipeTags
                                           .AsNoTracking()
                                           .Where(rt => rt.RecipeID == recipeID)
                                           .Select(rt => rt.Tag)
                                           .ToListAsync();

            if (recipe == null || tags.Count == 0)
            {
                return Results.NotFound();
            }

            List<TagSummaryDto> tagSummaries = tags.Select(tag => new TagSummaryDto (
                tag.TagID,
                tag.DisplayValue,
                tag.Category
            )).ToList();

            RecipeTagSummaryDto recipeTagSummaryDto = new (
                recipe.RecipeID,
                recipe.Name,
                tagSummaries
            );

            return Results.Ok(recipeTagSummaryDto);
        });


        // POST /
        // Create new Recipe Tag. Users and Admins only.
        recipeTagsGroup.MapPost("/", async (
            int recipeID,
            CreateRecipeTagDto newRecipeTag,
            RecipePlannerContext dbContext,
            ClaimsPrincipal token) =>
        {
            var tokenName = GetUsername(token);

            if (tokenName == null)
            {
                return Results.Unauthorized();
            }

            bool alreadyExists = await dbContext.RecipeTags
                                                .AnyAsync(rt => rt.RecipeID == recipeID
                                                                && rt.TagID == newRecipeTag.TagID);

            if (alreadyExists)
            {
                return Results.Conflict("This tag is already associated with this recipe.");
            }

            Recipe? recipe = await dbContext.Recipes.FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeID);
            Tag? tag = await dbContext.Tags.FirstOrDefaultAsync(tag => tag.TagID == newRecipeTag.TagID);

            if (recipe == null || tag == null)
            {
                return Results.NotFound();
            }

            RecipeTag recipeTag = new()
            {
                RecipeID = recipe.RecipeID,
                Recipe = recipe,
                TagID = tag.TagID,
                Tag = tag
            };

            dbContext.RecipeTags.Add(recipeTag);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetRecipeTagRouteName, new { recipeID = recipeTag.RecipeID, tagID = recipeTag.TagID }, recipeTag);
        })
        .RequireAuthorization(policy => { policy.RequireRole("admin", "user"); });

        // DELETE /{id}
        // Delete tags from a recipe. Recipe Owner and Admins only
        recipeTagsGroup.MapDelete("/{tagID}", async (
                    int recipeID,
                    int tagID,
                    RecipePlannerContext dbContext,
                    ClaimsPrincipal token) =>
        {
            var tokenName = GetUsername(token);

            if (tokenName == null)
            {
                return Results.Unauthorized();
            }

            Recipe? recipe = await dbContext.Recipes.FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeID);
            Tag? tag = await dbContext.Tags.FirstOrDefaultAsync(tag => tag.TagID == tagID);

            if (recipe == null)
            {
                return Results.NotFound();
            }

            User? tokenUser = await dbContext.Users
                                             .Where(user => user.Username == tokenName)
                                             .FirstOrDefaultAsync();

            // Owners can update their own tags.
            // Admins can update any tag.
            if (recipe.Owner != tokenUser && !IsAdmin(token))
            {
                return Results.Forbid();
            }

            await dbContext.RecipeTags
                           .Where(recipeTag => recipeTag.RecipeID == recipeID
                                               && recipeTag.TagID == tagID)
                           .ExecuteDeleteAsync();

            return Results.NoContent();
        })
        .RequireAuthorization(policy => { policy.RequireRole("admin", "user"); });
    }

    private static string? GetUsername(ClaimsPrincipal token)
    {
        var tokenName = token.Identity?.Name;
        return string.IsNullOrWhiteSpace(tokenName) ? null : tokenName;
    }

    private static bool IsAdmin(ClaimsPrincipal token) {
        return token.IsInRole("admin");
    }
}