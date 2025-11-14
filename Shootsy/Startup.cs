using AutoMapper;
using JsonPatchSample;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Shootsy.Controllers;
using Shootsy.Database;
using Shootsy.Database.Mongo;
using Shootsy.MappingProfiles;
using Shootsy.Models.Enums;
using Shootsy.Options;
using Shootsy.Repositories;
using Shootsy.Repositories.Interfaces;
using Shootsy.Security;
using Shootsy.Service;
using Swashbuckle.AspNetCore.Filters;
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
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Session";
                options.DefaultChallengeScheme = "Session";
            })
            .AddScheme<AuthenticationSchemeOptions, SessionAuthenticationHandler>("Session", _ => { });
            services.AddAuthorization();
            services.AddEndpointsApiExplorer();
            services.AddEndpointsApiExplorer();
            services.Configure<MinioOptions>(_configuration.GetSection("Minio"));
            services.AddSingleton<IObjectStorage, MinioStorageService>();
            services.AddHostedService<MinioInitializer>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Shootsy API",
                    Version = "v1",
                    Description = "Для полноценного использования методов сервиса необходимо при регистрации или авторизации получить session"
                });

                c.EnableAnnotations();
                c.ExampleFilters();

                c.MapType<FileSortByEnum>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetNames(typeof(FileSortByEnum))
                   .Select(n => (IOpenApiAny)new OpenApiString(n))
                   .ToList()
                });

                var xmlFile = "Shootsy.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                c.OperationFilter<JsonPatchExampleFilter>();
                c.AddSecurityDefinition("Session", new OpenApiSecurityScheme
                {
                    Name = "Session",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Сессионный токен пользователя"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Session"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            services.Configure<MongoOptions>(_configuration.GetSection("Mongo"));
            services.AddSingleton<IMongoClient>(sp =>
            {
                var opts = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
                return new MongoClient(opts.ConnectionString);
            });
            services.AddScoped(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                var settings = serviceProvider.GetRequiredService<IOptions<MongoOptions>>().Value;
                return client.GetDatabase(settings.Database);
            });
            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
            services.AddDbContext<ApplicationContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddAutoMapper(typeof(UserProfiles));
            services.AddAutoMapper(typeof(UserSessionProfiles));
            services.AddSingleton<IMapper, Mapper>();
            services.AddSingleton<UserRepository>();
            services.AddSingleton<InternalConstants>();
            services.AddSingleton<FileRepository>();
            services.AddSingleton<PostRepository>();
            services.AddHttpClient<FilesController>();
            services.AddHttpClient<UsersController>();
            services.AddHttpClient<PostsController>();
            services.AddHttpClient<MessagesConroller>();
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
            app.UseAuthentication();
            app.UseAuthorization();    
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shootsy API v1");
                c.RoutePrefix = "swagger";
            });
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
                endpoints.MapControllerRoute(
                    name: "Posts",
                    pattern: "{controller=Posts}");
                endpoints.MapControllerRoute(
                    name: "Messages",
                    pattern: "{controller=Messages}");
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Shootsy API is running!");
                });
            });
        }
    }
}