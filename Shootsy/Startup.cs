using AutoMapper;
using Shootsy.Controllers;
using Shootsy.Database;
using Shootsy.MappingProfiles;
using Shootsy.Repositories;
using Shootsy.Security;

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
            services.AddDbContext<ApplicationContext>();
            services.AddAutoMapper(typeof(Profiles));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<IMapper, Mapper>();
            services.AddSingleton(TimeProvider.System);
            services.AddSingleton<SupportMethods>();
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
