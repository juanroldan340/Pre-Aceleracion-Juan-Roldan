using DisneyAPI.Data;
using DisneyAPI.Models;
using DisneyAPI.Repositories;
using DisneyAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DisneyWebAPI.Extensions
{
    public class ProgramExtension
    {
        public static void AddSwagger(IServiceCollection service)
        {
            service.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Disney {groupName}",
                    Version = groupName,
                    Description = "Disney Web API"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "Jwt",
                    In = ParameterLocation.Header,
                    Description = "Ingrese 'Bearer [Su token]' para poder autenticarse dentro de la aplicaci�n"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

        }

        public static void AddDbContexts(IServiceCollection service, ConfigurationManager config)
        {
            service.AddDbContext<DisneyDbContext>(
                optionsbuilder => optionsbuilder.UseSqlServer(config.GetConnectionString("DisneyConnectionString"))
            );

            service.AddDbContext<UserDbContext>(
                optionsbuilder => optionsbuilder.UseSqlServer(config.GetConnectionString("UsersConnectionString"))
            );
        }

        public static void AddAuthentication(IServiceCollection service, ConfigurationManager config)
        {
            service.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "https://localhost:7000",
                    ValidIssuer = "https://localhost:7000",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["SecretKey"]))
                };
            });
        }

        public static void AddServicesImplementations(IServiceCollection service)
        {
            service.AddScoped<ICharactersService, CharactersService>();
            service.AddScoped<ICharactersRepository, CharactersRepository>();
            service.AddScoped<IMoviesOrSeriesService, MoviesOrSeriesService>();
            service.AddScoped<IMoviesOrSeriesRepository, MoviesOrSeriesRepository>();
            service.AddTransient<IMailService, MailService>();
        }

        public static void AddIdentity(IServiceCollection service) 
        { 
            service.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<UserDbContext>().AddDefaultTokenProviders();
        }
    }
}
