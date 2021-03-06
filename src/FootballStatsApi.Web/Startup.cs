using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using FootballStatsApi.Web.Helpers;
using FootballStatsApi.Domain;
using FootballStatsApi.Domain.Entities.Identity;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography.X509Certificates;
using FootballStatsApi.Web.Models;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Logic.v0.Managers;
using FootballStatsApi.Domain.Helpers;
using Microsoft.AspNetCore.HttpOverrides;

namespace FootballStatsApi.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Football");

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            services.AddDbContext<DataProtectionKeysContext>(options => options.UseNpgsql(connectionString));
            
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });   

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                    .AddRazorPagesOptions(options =>
                    {
                        options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                        options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                    });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.AllowedForNewUsers = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton(new SmtpConnectionInfo
            {
                Host = Configuration["SmtpHost"],
                Username = Configuration["SmtpUsername"],
                Port = Configuration["SmtpPort"] == null ? 0 : int.Parse(Configuration["SmtpPort"]),
                Password = Configuration["SmtpPassword"],
                FromAddress = Configuration["SmtpFromAddress"]
            });

            // Data Protection - Provides storage and encryption for anti-forgery tokens
            if (Environment.IsDevelopment())
            {
                services.AddDataProtection()
                        .PersistKeysToDbContext<DataProtectionKeysContext>();
            }
            else
            {
                services.AddDataProtection()
                        .PersistKeysToDbContext<DataProtectionKeysContext>()
                        .ProtectKeysWithCertificate(GetSigningCertificate());
            }

            services.AddAntiforgery();

            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();

            services.AddTransient<ISubscriptionManager, SubscriptionManager>();

            services.AddSingleton(new DatabaseConnectionInfo { ConnectionString = connectionString });
            services.AddSingleton<IConnectionProvider, ConnectionProvider>();

            services.AddTransient<ConfigurationHelper>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private X509Certificate2 GetSigningCertificate()
        {
            var certLocation = Configuration["TokenSigningCert"];
            var certPassword = Configuration["TokenSigningCertSecret"];

            return new X509Certificate2(certLocation, certPassword);
        }
    }
}
