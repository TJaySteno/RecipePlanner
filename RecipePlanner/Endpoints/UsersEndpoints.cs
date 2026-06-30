using Microsoft.EntityFrameworkCore;
using RecipePlanner.Data;
using RecipePlanner.Dtos;
using RecipePlanner.Models;
using System.Security.Claims;

namespace RecipePlanner.Endpoints;

public static class UsersEndpoints
{
    const string GetUserRouteName = "GetUser";

    public static void MapUsersEndpoints(this WebApplication app)
    {
        var usersGroup = app.MapGroup("/users");

        //GET /users
        usersGroup.MapGet("/", async (RecipePlannerContext dbContext) =>
            await dbContext.Users
                            .Select(user => new UserDto(
                                user.UserID,
                                user.PrimaryEmail,
                                user.Username,
                                user.FirstName,
                                user.MiddleName,
                                user.LastName
                            ))
                            .ToListAsync());

        // Redirect to either /login or /users/{id}
        // ApiController.RedirectToRoute(String, Object) Method
        // https://learn.microsoft.com/en-us/dotnet/api/system.web.http.apicontroller.redirecttoroute?view=aspnetcore-2.2

        // GET /users/{id}
        usersGroup.MapGet("/{id}", async (int id, RecipePlannerContext dbContext) =>
        {
            var user = await dbContext.Users.FindAsync(id);

            return user is null
                ? Results.NotFound()
                : Results.Ok(user);
        })
        .WithName(GetUserRouteName);

        // POST /users
        usersGroup.MapPost("", async (
            CreateUserDto newUser,
            RecipePlannerContext dbContext,
            ClaimsPrincipal token) =>
        {
            ArgumentNullException.ThrowIfNull(token.Identity?.Name);
            var username = token.Identity.Name;

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
            await dbContext.SaveChangesAsync();

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
        usersGroup.MapPut("/{id}", async (
            int id,
            UpdateUserDto updatedUser,
            RecipePlannerContext dbContext,
            ClaimsPrincipal token) =>
        {
            ArgumentNullException.ThrowIfNull(token.Identity?.Name);
            var username = token.Identity.Name;

            var user = dbContext.Users.FindAsync(id).Result;

            if (user == null)
            {
                return Results.NotFound();
            }

            user.PrimaryEmail = updatedUser.PrimaryEmail;
            user.Username = updatedUser.Username;
            user.FirstName = updatedUser.FirstName;
            user.MiddleName = updatedUser.MiddleName;
            user.LastName = updatedUser.LastName;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /users/{id}
        usersGroup.MapDelete("/{id}", async (int id, RecipePlannerContext dbContext) =>
        {
            await dbContext.Users
                            .Where(user => user.UserID == id)
                            .ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}