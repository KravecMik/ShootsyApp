using AutoMapper;
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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(Profiles));
            services.AddSingleton<IMapper, Mapper>();
            services.AddSingleton<UserRepository>();
            services.AddSingleton<Mapper>();
            services.AddSingleton(TimeProvider.System);
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseHttpsRedirection();
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
