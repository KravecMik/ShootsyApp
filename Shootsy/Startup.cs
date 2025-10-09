using AutoMapper;
using JsonPatchSample;
using Shootsy.Controllers;
using Shootsy.Database;
using Shootsy.MappingProfiles;
using Shootsy.Repositories;
using Shootsy.Service;

namespace Shootsy
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddAutoMapper(typeof(UserProfiles));
            services.AddAutoMapper(typeof(FileProfiles));
            services.AddAutoMapper(typeof(UserSessionProfiles));
            services.AddSingleton<IMapper, Mapper>();
            services.AddSingleton<UserRepository>();
            services.AddSingleton<InternalConstants>();
            services.AddSingleton<FileRepository>();
            services.AddHttpClient<FilesController>();
            services.AddHttpClient<UsersController>();
            services.AddSingleton<Mapper>();
            services.Configure<KafkaSettings>(_configuration.GetSection("Kafka"));
            services.AddSingleton<IKafkaProducerService, KafkaProducerService>();

            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseRouting();

            // УБЕРИТЕ ВСЕ MapHealthChecks ОТСЮДА
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Users",
                    pattern: "{controller=Users}");

                endpoints.MapControllerRoute(
                    name: "Files",
                    pattern: "{controller=Files}");

                endpoints.MapControllerRoute(
                    name: "Service",
                    pattern: "{controller=Service}");

                // Добавьте default route для корня
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Shootsy API is running!");
                });
            });
        }
    }
}