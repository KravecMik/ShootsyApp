using AutoMapper;
using Shootsy.Database;
using Shootsy.MappingProfiles;
using Shootsy.Repositories;
using Newtonsoft.Json;
using JsonPatchSample;

namespace Shootsy
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(Profiles));
            services.AddSingleton<IMapper, Mapper>();
            services.AddSingleton<UserRepository>();
            services.AddSingleton<Mapper>();
            services.AddSingleton(TimeProvider.System);
            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
            });
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
