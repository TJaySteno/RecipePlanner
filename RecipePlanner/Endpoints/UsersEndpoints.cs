using RecipePlanner.Data;
using RecipePlanner.Dtos;
using RecipePlanner.Models;

namespace RecipePlanner.Endpoints;

public static class UsersEndpoints
{
    const string GetUserRouteName = "GetUser";

    private static readonly List<UserDto> users = new List<UserDto>();

    public static void MapUsersEndpoints(this WebApplication app)
    {
        var usersGroup = app.MapGroup("/users");

        //GET /users
        usersGroup.MapGet("/", (RecipePlannerContext dbContext) => dbContext.Users);

        // Redirect to either /login or /users/{id}
        // ApiController.RedirectToRoute(String, Object) Method
        // https://learn.microsoft.com/en-us/dotnet/api/system.web.http.apicontroller.redirecttoroute?view=aspnetcore-2.2

        // GET /users/{id}
        usersGroup.MapGet("/{id}", (int id, RecipePlannerContext dbContext) =>
        {
            var user = dbContext.Users.Find(id);

            return user is null
                ? Results.NotFound()
                : Results.Ok(user);
        })
        .WithName(GetUserRouteName);

        // POST /users
        usersGroup.MapPost("", (CreateUserDto newUser, RecipePlannerContext dbContext) =>
        {
            User user = new()
            {
                PrimaryEmail = newUser.PrimaryEmail,
                Username = newUser.Username,
                PasswordHash = newUser.PasswordHash,
                FirstName = newUser.FirstName,
                MiddleName = newUser.MiddleName,
                LastName = newUser.LastName
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            UserDto userDto = new(
                user.UserID,
                user.PrimaryEmail,
                user.Username,
                user.FirstName,
                user.MiddleName,
                user.LastName
            );

            return Results.CreatedAtRoute(GetUserRouteName, new { id = userDto.UserID }, userDto);
        });

        // PUT /users/{id}
        usersGroup.MapPut("/{id}", (int id, UpdateUserDto updatedUser, RecipePlannerContext dbContext) =>
        {
            var user = dbContext.Users.Find(id);

            if (user == null)
            {
                return Results.NotFound();
            }

            user.PrimaryEmail = updatedUser.PrimaryEmail;
            user.Username = updatedUser.Username;
            user.FirstName = updatedUser.FirstName;
            user.MiddleName = updatedUser.MiddleName;
            user.LastName = updatedUser.LastName;
            
            dbContext.SaveChanges();

            return Results.NoContent();
        });

        // DELETE /users/{id}
        usersGroup.MapDelete("/{id}", (int id, RecipePlannerContext dbContext) =>
        {
            var user = dbContext.Users.Find(id);

            if (user is null)
            {
                return Results.NotFound();
            }

            bool recipes = dbContext.Recipes.All((Recipe recipe) => recipe.Owner.UserID == id);

            if (recipes == true)
            {
                return Results.BadRequest("This user still owns recipes. Delete those first.");
            }

            dbContext.Users.Remove(user);
            dbContext.SaveChanges();

            return Results.NoContent();
        });
    }
}