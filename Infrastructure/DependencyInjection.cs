using Core.Abstract;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGameService, GameService>();
        services.AddSingleton<RandomizerBase, SystemRandomizer>();
        //Db
        services.AddDbContext<DContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }
}