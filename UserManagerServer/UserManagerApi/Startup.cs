using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using IdentityData;
using IdentityData.Entities;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using UserManagerCore.Repositories;
using UserManagerData;
using UserManagerData.Repositories;
using UserManagerInfrastructure.Mapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UserManagerCore.Services;
using UserManagerCore.Services.Interfaces;
using UserManagerApi.Helper;
using UserManagerInfrastructure.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace UserManagerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson();

            services.AddMvc(options =>
            {
                var jsonInputFormatter = options.InputFormatters
                   .OfType<NewtonsoftJsonInputFormatter>().FirstOrDefault();

                if (jsonInputFormatter != null)
                {
                    jsonInputFormatter.SupportedMediaTypes
                    .Add("application/json-patch+json");
                }

                options.Filters.Add(typeof(ExceptionLoggingFilter));
                options.Filters.Add(typeof(LoggingActionFilter));
            });

            var migrationsAssembly = typeof(AppDbContext).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    s => s.MigrationsAssembly(migrationsAssembly)));

            var migrationsAssemblyIdentity = typeof(AppIdentityDbContext).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<AppIdentityDbContext>(options =>
                 options.UseSqlServer(
                    Configuration.GetConnectionString("IdentityConnection"),
                    s => s.MigrationsAssembly(migrationsAssemblyIdentity)));

            services.AddIdentity<AppUser, AppRole>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = false;
                })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                { 
                    options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = $"{Configuration.GetSection("Authentification:Authority").Value}";
                    options.ApiName = $"{Configuration.GetSection("Authentification:ClientId").Value}";
                });

            services.AddAuthorization(options => {
                options.AddPolicy(
                    "isAdmin",
                    policyBuilder =>
                    {
                        policyBuilder.AddAuthenticationSchemes(IdentityServerAuthenticationDefaults.AuthenticationScheme);
                        policyBuilder.RequireAuthenticatedUser();
                        //policyBuilder.RequireClaim("roles", "admin");
                    });
                options.AddPolicy(
                    "isUser",
                    policyBuilder =>
                    {
                        policyBuilder.RequireAuthenticatedUser();
                        policyBuilder.RequireClaim("roles", "user");
                    });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOriginsHeadersAndMethods",
                    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<JsonPatchManager>();

            services.AddAutoMapper(typeof(AccountProfile));

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.ForAllMaps((obj, cnfg) =>
                    cnfg.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)));
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "User manager api", Version = "v1" });
            });


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User manager API V1");
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("AllowAllOriginsHeadersAndMethods");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
