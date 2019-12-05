using System;
using System.IO;
using System.Reflection;
using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Handlers;
using FootballStatsApi.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using FootballStatsApi.Domain;

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
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Football")));

            services.AddTransient<IPlayerSummaryManager, PlayerSummaryManager>();
            services.AddTransient<ITeamSummaryManager, TeamSummaryManager>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<ICompetitionManager, CompetitionManager>();
            services.AddTransient<IFixtureManager, FixtureManager>();

            services.AddTransient<IPlayerSummaryRepository, PlayerSummaryRepository>();
            services.AddTransient<ITeamSummaryRepository, TeamSummaryRepository>();
            services.AddTransient<ICompetitionRepository, CompetitionRepository>();
            services.AddTransient<IFixtureRepository, FixtureRepository>();

            services.AddSingleton(new DatabaseConnectionInfo { ConnectionString = Configuration.GetConnectionString("Football") });
            services.AddSingleton<IConnectionProvider, ConnectionProvider>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Football Stats",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Chris Oliver"
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "api_key"}
                        },
                        new string[0]
                    }
                });

                c.AddSecurityDefinition("api_key", new OpenApiSecurityScheme
                {
                    Description = "API Key",
                    Name = "X-API-Key",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                c.RoutePrefix = String.Empty;
            });
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
