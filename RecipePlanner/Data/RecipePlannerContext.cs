using Microsoft.EntityFrameworkCore;
using RecipePlanner.Models;

namespace RecipePlanner.Data;

public class RecipePlannerContext(DbContextOptions<RecipePlannerContext> options)
    : DbContext(options)
{
    public DbSet<Recipe> Recipes => Set<Recipe>();

    public DbSet<User> Users => Set<User>();
}
