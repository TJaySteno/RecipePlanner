using RecipePlanner.Data;
using RecipePlanner.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

var connString = "Server=localhost;Database=RecipePlanner;Trusted_Connection=True;TrustServerCertificate=True";
builder.Services.AddSqlServer<RecipePlannerContext>(connString);

var app = builder.Build();

app.MapRecipesEndpoints();
app.MapUsersEndpoints();

app.MigrateDb();

app.Run();

/*
All of this came with the default build. Leaving for now so I can reference back to it later.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
*/