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
        usersGroup.MapGet("/", async (
                RecipePlannerContext dbContext,
                ClaimsPrincipal token) =>
            await dbContext.Users
                            .Select(user => new UserDto(
                                user.UserID,
                                user.PrimaryEmail,
                                user.Username,
                                user.FirstName,
                                user.MiddleName,
                                user.LastName
                            ))
                            .ToListAsync())
            .RequireAuthorization(policy => { policy.RequireRole("user", "admin"); });

        // Redirect to either /login or /users/{id}
        // ApiController.RedirectToRoute(String, Object) Method
        // https://learn.microsoft.com/en-us/dotnet/api/system.web.http.apicontroller.redirecttoroute?view=aspnetcore-2.2

        // GET /users/{id}
        usersGroup.MapGet("/{id}", async (
                int id,
                RecipePlannerContext dbContext,
                ClaimsPrincipal token) =>
            {
                var user = await dbContext.Users.FindAsync(id);

                return user is null
                    ? Results.NotFound()
                    : Results.Ok(user);
            })
            .WithName(GetUserRouteName)
            .RequireAuthorization(policy => { policy.RequireRole("user", "admin"); }); ;

        // POST /users
        usersGroup.MapPost("", async (
            CreateUserDto newUser,
            RecipePlannerContext dbContext,
            ClaimsPrincipal token) =>
        {
            bool IsAuthenticated = token.Identity?.IsAuthenticated ?? false;

            // Only new users and Admins can create new users.
            if (IsAuthenticated && !IsAdmin(token))
            {
                return Results.Forbid();
            }

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
            if (TokenNameIsNullOrWhiteSpace(token))
            {
                return Results.Unauthorized();
            }

            var user = await dbContext.Users
                                      .FirstOrDefaultAsync(user => user.UserID == id);

            if (user == null)
            {
                return Results.NotFound();
            }

            if (!IsAdmin(token) && !UsernamesMatch(user, token))
            {
                return Results.Forbid();
            }

            user.PrimaryEmail = updatedUser.PrimaryEmail;
            user.Username = updatedUser.Username;
            user.FirstName = updatedUser.FirstName;
            user.MiddleName = updatedUser.MiddleName;
            user.LastName = updatedUser.LastName;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        })
        .RequireAuthorization(policy => { policy.RequireRole("user", "admin"); });

        // DELETE /users/{id}
        usersGroup.MapDelete("/{id}", async (
            int id,
            RecipePlannerContext dbContext,
            ClaimsPrincipal token) =>
        {
            if (TokenNameIsNullOrWhiteSpace(token))
            {
                return Results.Unauthorized();
            }

            var user = await dbContext.Users
                                      .FirstOrDefaultAsync(user => user.UserID == id);

            if (user == null)
            {
                return Results.NotFound();
            }

            if (!UsernamesMatch(user, token) && !IsAdmin(token))
            {
                return Results.Forbid();
            }

            await dbContext.Users
                           .Where(user => user.UserID == id)
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

    private static bool IsAdmin(ClaimsPrincipal token)
    {
        return token.IsInRole("admin");
    }

    private static bool UsernamesMatch(User owner, ClaimsPrincipal token)
    {
        return owner.Username == token.Identity?.Name;
    }
}