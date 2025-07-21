using Application.Factory;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IEtagFactory, ETagFactory>();
        services.AddSingleton<IMoveFactory, MoveFactory>();
        return services;
    }
}