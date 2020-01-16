using System;
using System.IO;
using System.Reflection;
using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Handlers;
using FootballStatsApi.Logic.v0.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using FootballStatsApi.Domain;
using System.Net;
using NpgsqlTypes;
using Dapper;
using FootballStatsApi.Helpers;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace FootballStatsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(c =>
            {
                c.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
                c.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
            });
            services.AddAuthorization();
            services.AddHealthChecks();

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Football")));

            var ipAddressHandler = new PassThroughHandler<IPAddress>(NpgsqlDbType.Inet);
            SqlMapper.AddTypeHandler(ipAddressHandler);

            services.AddTransient<IPlayerSummaryManager, PlayerSummaryManager>();
            services.AddTransient<ITeamSummaryManager, TeamSummaryManager>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<ICompetitionManager, CompetitionManager>();
            services.AddTransient<IFixtureManager, FixtureManager>();
            services.AddTransient<ITeamManager, TeamManager>();

            services.AddTransient<IPlayerSummaryRepository, PlayerSummaryRepository>();
            services.AddTransient<ITeamSummaryRepository, TeamSummaryRepository>();
            services.AddTransient<ICompetitionRepository, CompetitionRepository>();
            services.AddTransient<IFixtureRepository, FixtureRepository>();
            services.AddTransient<IRequestLogRepository, RequestLogRepository>();
            services.AddTransient<ITeamRepository, TeamRepository>();

            services.AddSingleton(new DatabaseConnectionInfo { ConnectionString = Configuration.GetConnectionString("Football") });
            services.AddSingleton<IConnectionProvider, ConnectionProvider>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            }).AddApiKeySupport(options => { });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0", new OpenApiInfo
                {
                    Title = "Football Stats API",
                    Version = "v0",
                    TermsOfService = new Uri(Configuration["WebsiteUrl"] + "/Home/Terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "support",
                        Email = "support@footballstatsapi.com"
                    }
                });
                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "API Key"}
                        },
                        new string[0]
                    }
                });

                c.AddSecurityDefinition("API Key", new OpenApiSecurityScheme
                {
                    Description = "API Key",
                    Name = "X-API-Key",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.EnableAnnotations();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers()
                .ConfigureApiBehaviorOptions(c =>
                {
                    c.SuppressMapClientErrors = true;
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseFileServer();
            app.UseSwagger();
            //app.UseReDoc(c =>
            //{
            //    c.SpecUrl("/swagger/v0/swagger.json");
            //});

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v0/swagger.json", "v0 (Alpha)");
            });

            app.UseRouting();

            app.UseMiddleware<RequestLogMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<RateLimitMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthcheck");
                endpoints.MapControllers();
            });
        }
    }
}
