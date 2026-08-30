using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Extensions
{
    public static class ConnectionStringExtension
    {
        public static IServiceCollection AddConnectionString(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FilmeContext>(opts =>
            {
                opts.UseMySQL(configuration.GetConnectionString("FilmeConnection")
                    ?? throw new InvalidOperationException("A connection string não foi encontrada na configuração"));
            });

            return services;
        }
    }
}
