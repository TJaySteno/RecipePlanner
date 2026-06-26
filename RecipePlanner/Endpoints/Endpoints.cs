namespace RecipePlanner.Endpoints;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        // GET /
        app.MapGet("/", () => "Hello World!");

        // If not authenticated, redirect to /login. If authenticated, / shows nav options.
        // ApiController.RedirectToRoute(String, Object) Method
        // https://learn.microsoft.com/en-us/dotnet/api/system.web.http.apicontroller.redirecttoroute?view=aspnetcore-2.2

        // GET /login
        app.MapGet("/login", () => "Login screen.");
    }
}