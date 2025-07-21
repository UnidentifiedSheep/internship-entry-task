using Carter;

namespace Api.EndPoints.Health;

public class GetHealth : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () => Results.Ok());
    }
}