using Microsoft.EntityFrameworkCore;
using Municipal_Servcies_Portal.Data;
using Municipal_Servcies_Portal.Repositories;
using Municipal_Servcies_Portal.Services;

namespace Municipal_Servcies_Portal.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // 1. Database Context
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            // 2. Generic Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // 3. Specific Repositories
            services.AddScoped<IIssueRepository, IssueRepository>();
            services.AddScoped<IEventRepository, EventRepository>();

            // 4. AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // 5. Application Services
            services.AddScoped<IIssueService, IssueService>();
            services.AddScoped<ILocalEventsService, LocalEventsService>();
            services.AddScoped<SearchHistoryService>();

            return services;
        }
    }
}