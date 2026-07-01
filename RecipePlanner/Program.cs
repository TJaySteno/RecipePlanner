using RecipePlanner.Data;
using RecipePlanner.Endpoints;

bool inProduction = false;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddValidation();
builder.AddDummyData(inProduction);

var app = builder.Build();

app.MapEndpoints();
app.MapRecipesEndpoints();
app.MapRecipeTagsEndpoints();
app.MapUsersEndpoints();

app.MigrateDb();

app.Run();