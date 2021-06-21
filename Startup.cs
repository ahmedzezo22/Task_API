using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_API.Data;
using Task_API.Models;
using Task_API.Validation;

namespace Task_API
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
            //cookies service
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
         .AddCookie(options =>
         {
             options.Cookie.HttpOnly = true;
             options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
             options.SlidingExpiration = true;
             options.LogoutPath = "/Account/Logout";
             options.Cookie.SameSite = SameSiteMode.Lax;
             options.Cookie.IsEssential = true;
         });

            //cors policiy
            services.AddCors();
            

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task_API", Version = "v1" });
            });
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<DataContext>(op => {
                op.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddIdentity<AppUser, AppRole>(op =>
             {
                 op.Password.RequireDigit = true;
                 op.Password.RequiredLength = 8;
                 op.Password.RequireLowercase = true;
                 op.Password.RequireNonAlphanumeric = true;
                 op.Password.RequireUppercase = true;
                 op.Password.RequiredUniqueChars = 1;
             }).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
            //autoMapper service
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IAccountValidation,AccountValidation>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task_API v1"));
            }

            app.UseHttpsRedirection();
            //cors policy
            app.UseCors(x => x.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
               app.UseRouting();
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
