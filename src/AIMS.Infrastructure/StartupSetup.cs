using AIMS.Infrastructure.Data;
using AIMS.Infrastructure.DomainEvents;
using AIMS.Infrastructure.IdentityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AIMS.Infrastructure
{
    public static class StartupSetup
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString)
        {

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString))
                //.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = false)
                //.AddEntityFrameworkStores<AppDbContext>()
                ; // will be created in web project root}


        }

        public static void SeedData(this IServiceCollection services)
        {
            using var fac = services.BuildServiceProvider();
            fac.GetRequiredService<AppDbContext>().Database.EnsureCreated();
        }
    }
}
