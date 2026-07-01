using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Query;
using RecipePlanner.Models;
using System.Linq.Expressions;

namespace RecipePlanner.Data;

public class RecipePlannerContext(DbContextOptions<RecipePlannerContext> options)
    : DbContext(options)
{
    // Parent Sets
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Tag> Tags => Set<Tag>();

    // Child Sets
    public DbSet<RecipeTag> RecipeTags => Set<RecipeTag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeTag>()
            .HasKey(recipeTag => new
            {
                recipeTag.RecipeID,
                recipeTag.TagID
            });

        modelBuilder.Entity<RecipeTag>()
            .HasOne(recipeTag => recipeTag.Recipe)
            .WithMany(recipe => recipe.RecipeTags)
            .HasForeignKey(recipeTag => recipeTag.RecipeID);

        modelBuilder.Entity<RecipeTag>()
            .HasOne(recipeTag => recipeTag.Tag)
            .WithMany(tag => tag.RecipeTags)
            .HasForeignKey(recipeTag => recipeTag.TagID);
    }
}