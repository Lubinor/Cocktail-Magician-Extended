using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.Contracts;
using CocktailMagician.Services.Mappers;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers;
using CocktailMagician.Services.Providers.Contracts;
using CocktailMagician.Web.Mappers;
using CocktailMagician.Web.Mappers.Contracts;
using CocktailMagician.Web.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using NToastNotify;
using System;

namespace CocktailMagician.Web
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
            services.AddDbContext<CocktailMagicianContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddHsts(options =>
            //{
            //    options.Preload = true;
            //    options.IncludeSubDomains = true;
            //    options.MaxAge = TimeSpan.FromDays(60);
            //});

            //services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //    options.HttpsPort = 5000;
            //});
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<CocktailMagicianContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddControllersWithViews();

            services.AddMvc().AddNToastNotifyNoty();

            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IBarService, BarService>();
            services.AddScoped<ICocktailService, CocktailService>();
            services.AddScoped<IBarService, BarService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICocktailReviewService, CocktailReviewService>();
            services.AddScoped<IBarReviewService, BarReviewService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IIngredientMapper, IngredientMapper>();
            services.AddScoped<ICocktailMapper, CocktailMapper>();
            services.AddScoped<IBarMapper, BarMapper>();
            services.AddScoped<IBarReviewMapper, BarReviewMapper>();
            services.AddScoped<ICocktailReviewMapper, CocktailReviewMapper>();
            services.AddScoped<IUserMapper, UserMapper>();
            services.AddScoped<ICityMapper, CityMapper>();
            services.AddScoped<ICocktailDTOMapper, CocktailDTOMapper>();
            services.AddScoped<IIngredientDTOMapper, IngredientDTOMapper>();
            services.AddScoped<IBarDTOMapper, BarDTOMapper>();
            services.AddScoped<IBarReviewDTOMapper, BarReviewDTOMapper>();
            services.AddScoped<ICocktailReviewDTOMapper, CocktailReviewDTOMapper>();
            services.AddScoped<ICityDTOMapper, CityDTOMapper>();

            services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                PositionClass = ToastPositions.TopFullWidth,
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //seeder, za da nqma production; base entity (Id, IsDeleted...)
                //static jiv prez celiq lifetimem oshte pri start, singleton - kogato go poiskash
            }
            else
            {
                app.UseExceptionHandler("/Home/NotFound"); // ("Home/Error")
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<NotFoundMiddleware>();

            app.UseNToastNotify();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
