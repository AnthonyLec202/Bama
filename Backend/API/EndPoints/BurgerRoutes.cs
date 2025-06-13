using Core.Models;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

public static class BurgerRoutes
{
    public static WebApplication AddBurgerRoutes(this WebApplication app)
    {
        var group = app.MapGroup("api/burgers")
            .WithTags("Burgers");

        group.MapGet("", (IBurgerUseCases burgerUseCases) =>
        {
            var burgers = burgerUseCases.GetAllBurgers();
            return Results.Ok(burgers);
        })
        .WithName("GetAllBurgers")
        .Produces<IEnumerable<Burger>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPost("", (Burger burger, IBurgerUseCases burgerUseCases) =>
        {
            burgerUseCases.AddBurger(burger);
            return Results.Created();
        })
        .WithName("AddBurger")
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapDelete("{name}", (string name, IBurgerUseCases burgerUseCases) =>
        {
            burgerUseCases.DeleteBurgerByName(name);
         return Results.NoContent();
        })
        .WithName("DeleteBurgerByName")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        return app;
    }
}
