using RecipePlanner.Dtos;

namespace RecipePlanner.Endpoints;

public static class UsersEndpoints
{
    const string GetUserRouteName = "GetUser";

    private static readonly List<UserDto> users = new List<UserDto>();

    public static void MapUsersEndpoints(this WebApplication app)
    {
        var usersGroup = app.MapGroup("/users");

        // GET /users
        //usersGroup.MapGet("/", () => users);

        // Redirect to either /login or /users/{id}
        // ApiController.RedirectToRoute(String, Object) Method
        // https://learn.microsoft.com/en-us/dotnet/api/system.web.http.apicontroller.redirecttoroute?view=aspnetcore-2.2

        // GET /users/{id}
        usersGroup.MapGet("/{id}", (int id) =>
            {
                var user = users.Find(user => user.UserID == id);

                return user is null ? Results.NotFound() : Results.Ok(user);
            })
            .WithName(GetUserRouteName);

        // POST /users
        usersGroup.MapPost("", (CreateUserDto newUser) =>
        {
            UserDto user = new (
                users.Max(user => user.UserID) + 1,
                newUser.PrimaryEmail,
                newUser.Username,
                newUser.FirstName,
                newUser.MiddleName,
                newUser.LastName
            );

            users.Add(user);

            return Results.CreatedAtRoute(GetUserRouteName, new { id = user.UserID }, user);
        });

        // PUT /users/{id}
        usersGroup.MapPut("/{id}", (int id, UpdateUserDto updatedUser) =>
        {
            var index = users.FindIndex(user => user.UserID == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            users[index] = new UserDto (
                id,
                updatedUser.PrimaryEmail,
                updatedUser.Username,
                updatedUser.FirstName,
                updatedUser.MiddleName,
                updatedUser.LastName
            );

            return Results.NoContent();
        });

        // DELETE /users/{id}
        usersGroup.MapDelete("/{id}", (int id) =>
        {
            // Todo: Do I want a soft delete option?
            users.RemoveAll(user => user.UserID == id);

            return Results.NoContent();
        });
    }
}