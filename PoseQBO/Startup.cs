using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PoseQBO.Models;
using PoseQBO.Models.DataAccess;
using PoseQBO.Models.Identity;
using PoseQBO.Services.Caching;
using PoseQBO.Services.Caching.Interfaces;
using PoseQBO.Services.Formatters;
using PoseQBO.Services.QBO;
using PoseQBO.Services.QBO.Interfaces;

namespace PoseQBO
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<TokensDbContext>((options) =>
            {
                options.UseSqlite(Configuration.GetConnectionString("DBConnectionString"));
            });

            services.AddDistributedMemoryCache();

            services.AddScoped<IApiServices, QBOApiServices>();
            services.AddScoped<IInvoiceServices, QBOInvoiceServices>();
            services.AddScoped<ICustomerServices, QBOCustomerServices>();
            services.AddScoped<IInvoiceCacheService, MemoryInvoiceCacheService>();
            services.AddScoped<InvoicesFormatter>();
            services.Configure<OAuth2Keys>(Configuration.GetSection("OAuth2Keys"));

            services.AddSingleton(provider => Configuration);

            services.AddControllersWithViews();

            services.AddDbContext<PoseIdentityContext>(opts =>
            {
                opts.UseSqlite(Configuration.GetConnectionString("PoseIdentityConnection"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<PoseIdentityContext>();

            services.Configure<IdentityOptions>(opts =>
            {
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "{controller=Connect}/{action=Index}",
                    defaults: null,
                    constraints: null,
                    dataTokens: null);
            });
            IdentitySeedData.CreateAdminAccount(app.ApplicationServices, Configuration);
        }
    }
}
