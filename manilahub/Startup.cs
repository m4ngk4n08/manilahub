using Autofac;
using AutoMapper;
using Dapper.FluentMap;
using FluentValidation.AspNetCore;
using manilahub.Areas.RealtimeHub;
using manilahub.Authentication.Model;
using manilahub.data.Map;
using manilahub.Middleware;
using manilahub.Modules.Authentication.Profiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace manilahub
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var serverName = string.Empty;
            if (_hostingEnvironment.IsDevelopment())
            {
                serverName = "https://localhost:5001";
            }
            else
            {
                serverName = "http://www.manilahub.somee.com";
            }

            services.AddAntiforgery(
                option =>
                {
                    option.Cookie.Name = "AntiForgery";
                    option.Cookie.Expiration = TimeSpan.FromMinutes(20);
                    option.FormFieldName = "AntiforgeryFieldname";
                    option.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                    option.SuppressXFrameOptionsHeader = true;
                    //option.Cookie.HttpOnly = true;
                    //option.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                }
            );

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.Name = "CookieTempDataProvider";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = _hostingEnvironment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
               .AddCookie(options =>
               {
                   options.Cookie.Name = "Cookie";
                   options.LoginPath = new PathString("/auth");
                   options.LogoutPath = new PathString("/auth/logout");
                   options.AccessDeniedPath = new PathString("/auth/accessdenied");
                   options.SlidingExpiration = true;
                   options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                   options.SlidingExpiration = true;
                   options.Cookie.HttpOnly = true;
                   //options.Cookie.SecurePolicy = _hostingEnvironment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;

               });

            services.AddAuthorization(options =>
             {
                 options.AddPolicy("auth", policy =>
                     policy.Requirements.Add(new HubMiddlewarePermission(true)));
             });

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options =>
                    options
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            //Json Serializer
            //.AddNewtonsoftJson(options =>
            //options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
            //.Json.ReferenceLoopHandling.Ignore)
            //.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
            //= new DefaultContractResolver());
            // In production, the Angular files will be served from this directory
            services.AddRazorPages();

            FluentMapper.Initialize(opt =>
            {
                opt.AddMap(new RegisterMap());
                opt.AddMap(new LoginMap());
                opt.AddMap(new SessionMap());
                opt.AddMap(new AgentMap());
                opt.AddMap(new TransactionMap());
                opt.AddMap(new TransactionRMap());
                opt.AddMap(new FightMap());
                opt.AddMap(new FightResultMap());
                opt.AddMap(new BettingHistoryMap());
            });

            services.AddAutoMapper(typeof(Startup));
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new RegisterProfile());
            });
            services.AddSingleton(mapperConfig.CreateMapper());

            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterValidator>());

            services.AddSignalRCore();

            services.AddControllers();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new IOCContainer(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(options =>
                options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ErrorHandler>();
            //app.MapHub<RealtimeHub>("/realtimeHub");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Areas",
                   pattern: "{id}/{action}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
