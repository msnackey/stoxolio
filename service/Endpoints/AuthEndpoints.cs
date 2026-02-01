using Stoxolio.Service.Features.Auth;

namespace Stoxolio.Service.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithName("Auth");

        RegisterEndpoint.MapEndpoint(group);
        LoginEndpoint.MapEndpoint(group);
    }
}
