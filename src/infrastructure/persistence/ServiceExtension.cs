using Domain.Models;
using Domain.Repositories;

using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;

// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// using Microsoft.IdentityModel.Tokens;
// using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Persistence
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddPersistence
        (
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            #region Database Connection and DbContext
            services.AddDbContext<ShopDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("PostgresConnection")!,
                    x => x.MigrationsAssembly("persistence")
                );
            });
            #endregion

            // #region Identity Services
            // services.AddIdentity<User, IdentityRole>
            // (options => options.SignIn.RequireConfirmedAccount = true)
            //     .AddEntityFrameworkStores<ShopDbContext>()
            //     .AddDefaultTokenProviders()
            //     .AddUserManager<UserManager<User>>()
            //     .AddRoleManager<RoleManager<IdentityRole>>();
            // #endregion

            // #region JWT Service
            // services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            // services.AddAuthentication(options =>
            // {
            //     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            // }).AddJwtBearer(o =>
            // {
            //     o.RequireHttpsMetadata = false;
            //     o.SaveToken = false;
            //     o.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateIssuerSigningKey = true,
            //         ValidateIssuer = true,
            //         ValidateAudience = true,
            //         ValidateLifetime = true,
            //         ClockSkew = TimeSpan.Zero,
            //         ValidIssuer = configuration["JWTSettings:Issuer"],
            //         ValidAudience = configuration["JWTSettings:Audience"],
            //         IssuerSigningKey = new SymmetricSecurityKey(
            //             Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"])
            //         )
            //     };
            //     o.Events = new JwtBearerEvents()
            //     {
            //         OnAuthenticationFailed = c =>
            //         {
            //             c.NoResult();
            //             c.Response.StatusCode = 500;
            //             c.Response.ContentType = "text/plain";
            //             return c.Response.WriteAsync(c.Exception.ToString());
            //         },
            //         OnChallenge = context =>
            //         {
            //             context.HandleResponse();
            //             context.Response.StatusCode = 401;
            //             context.Response.ContentType = "application/json";
            //             var result = JsonConvert.SerializeObject(new { Message = "You are not authenticated." });
            //             return context.Response.WriteAsync(result);
            //         },
            //         OnForbidden = context =>
            //         {
            //             context.Response.StatusCode = 400;
            //             context.Response.ContentType = "application/json";
            //             var result = JsonConvert.SerializeObject(new { Message = "You have not access to this recurse." });
            //             return context.Response.WriteAsync(result);
            //         }
            //     };
            // });
            // #endregion

            // #region Seed Service
            // services.AddTransient<Seed>();
            // #endregion

            #region Repository Pattern
            services
                .AddTransient(
                    typeof(IEntityRepository<>),
                    typeof(EntityRepository<>))
                .AddTransient(
                    typeof(IEntityReadRepository<>),
                    typeof(EntityRepository<>))
                // .AddTransient<IRegisterRepository, RegisterRepository>()
                // .AddTransient<IAuthenticationRepository, AuthenticationRepository>()
                ;
            #endregion

            return services;
        }
    }
}
