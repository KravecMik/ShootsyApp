using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shootsy.Database;
using Shootsy.MappingProfiles;
using Shootsy.Repositories;

namespace Shootsy
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(Profiles));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<IMapper, Mapper>();
            services.AddSingleton(TimeProvider.System);
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Users",
                    pattern: "{controller=Users}");
            });
        }
    }
}
