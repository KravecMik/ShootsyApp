using AutoMapper;
using JsonPatchSample;
using Shootsy.Database;
using Shootsy.MappingProfiles;
using Shootsy.Repositories;

namespace Shootsy
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IUserSessionRepository, UserSessionRepository>();
            services.AddAutoMapper(typeof(UserProfiles));
            services.AddAutoMapper(typeof(FileProfiles));
            services.AddAutoMapper(typeof(UserSessionProfiles));
            services.AddSingleton<IMapper, Mapper>();
            services.AddSingleton<UserRepository>();
            services.AddSingleton<InternalConstants>();
            services.AddSingleton<FileRepository>();
            services.AddSingleton<Mapper>();
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

                endpoints.MapControllerRoute(
                    name: "Files",
                    pattern: "{controller=Files}");
            });
        }
    }
}
