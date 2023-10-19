using ChattingApplication.Core.Repositories;
using ChattingApplication.Core;
using ChattingApplication.Persistence.Repositories;
using ChattingApplication.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChattingApplication.DependencyInjection
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddRepositories(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IConfirmationCodeRepository, ConfirmationCodeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<AppDbContext>(opt => opt
                .UseMySQL(config.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
