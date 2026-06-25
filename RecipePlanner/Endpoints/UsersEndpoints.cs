using RecipePlanner.Dtos;

namespace RecipePlanner.Endpoints;

public static class UsersEndpoints
{
    // REGION: DUMMY DATA
    //private static readonly List<RecipeDto> recipes = [
    //    new(1, "Chili", "A spicy stew with beans and meat.", "https://example.com/chili", 4.5f, 2, 30, 60, 0, 4, 400, 20, 50, 10),
    //    new(2, "Pasta", "A classic Italian dish with tomato sauce.", "https://example.com/pasta", 4.0f, 1, 15, 30, 0, 2, 300, 10, 60, 5),
    //    new(3, "Salad", "A fresh mix of vegetables and dressing.", "https://example.com/salad", 4.2f, 1, 10, 0, 0, 2, 150, 5, 20, 5)
    //];
    //recipes.RemoveAll(recipe => recipe.ID > 3);
    // ENDREGION: DUMMY DATA

    public static void MapUsersEndpoints(this WebApplication app)
    {
        // GET /
        app.MapGet("/", () => "Hello World!");

        // If not authenticated, redirect to /login. If authenticated, / shows nav options.
        // ApiController.RedirectToRoute(String, Object) Method
        // https://learn.microsoft.com/en-us/dotnet/api/system.web.http.apicontroller.redirecttoroute?view=aspnetcore-2.2

        // GET /login
        app.MapGet("/login", () => "Login screen.");


        // GET /user
        app.MapGet("/users/{id}", (int id) => $"Hello, user {id}!");
    }
}