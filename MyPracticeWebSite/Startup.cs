using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyPracticeWebSite.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyPracticeWebSite.IdentityUserService;
using MyPracticeWebSite.IdentityUserModel;

namespace MyPracticeWebSite
{
    public class Startup
    {
        public readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // used for registering all necessary mvc services. Eg. In order to use HttpContext obj in controller.
            services.AddMvc();
            services.AddDataProtection();

            //services.AddHttpClient<IConferenceService,ConferenceAPIService>();
            //services.AddHttpClient<IProposalService, ProposalApiService>();

            services.AddSingleton<IConferenceService, ConferenceMemoryService>();
            services.AddSingleton<IProposalService, ProposalMemoryService>();
            services.AddSingleton<PurposeKeys>();

            /*
             * Below code is not strongly typed
            services.AddSingleton<IConferenceService, ConferenceAPIService>();
            services.AddSingleton<IProposalService, ProposalApiService>();
            services.AddHttpClient("MyAPIService", (client) =>
            {
                client.BaseAddress = new Uri("http://localhost:5000");
            });
            */
            services.AddDbContext<MyPracticeDBContext>(options =>
            {
                options.UseSqlServer(_configuration["connectionString"], sqlOptions =>
                 {
                     sqlOptions.MigrationsAssembly("MyPracticeWebSite");
                 });
            });

            // registring the asp.net core identity 
            services.AddIdentity<MyPracticeUserIdentity, IdentityRole>().AddEntityFrameworkStores<MyPracticeDBContext>();
            services.AddScoped<IUserClaimsPrincipalFactory<MyPracticeUserIdentity>, MyPracticeUserClaimsFactory>();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Auth/Login");
           
            services.Configure<MyPracticeWebsiteOptions>(_configuration.GetSection("MyPracticeWebSite"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // This logger we be injected as we configured to use logger in CreateDefaultBuilder by default.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation(env.IsDevelopment().ToString());


            //this will block the inline and using single domain source.cdn source will not be allowed

            //app.UseCsp(options => options.DefaultSources(s => s.Self())
            //   // this will allow to use CDN
            //   .StyleSources(s => s.Self().CustomSources("maxcdn.bootstrapcdn.com")).ImageSources(s => s.Self()));


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts(h => h.MaxAge(days: 365).Preload());
                app.UseHttpsRedirection();
            }

            // middleware to set httpcookie for authentication.
            app.UseAuthentication();
            app.UseXfo(options => options.Deny());

            // app.Run method runs only this middleware. It will not send the request to next middle ware for 
            // that we sould use app.use
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            //app.Use(async (context, next) =>
            //{
            //    logger.LogInformation("First");
            //    await context.Response.WriteAsync("Hello World!");
            //    await next();
            //    logger.LogInformation("Last");
            //});

            //app.Run(async (context) =>
            //{
            //    logger.LogInformation("Second");
            //    await context.Response.WriteAsync("Second text!");
            //});

            // for display status code in browser page, if error

            app.UseStatusCodePages();

            // for display images, css, javascript, static HTML content.
            app.UseStaticFiles();

            // used to setup mvc middleware
            app.UseMvc(routes
                =>
            {
                routes.MapRoute(name: "default", template: "{controller=Conference}/{action=Index}/{id?}");
            });
        }

        //public void ConfigureStaging(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        //{
        //    app.Run(async (context)=> {
        //        await context.Response.WriteAsync("Hello world! Staging");
        //    });
        //}

        /*

        ConfigureDevelopment(),ConfigureServicesDevelopment()
        ConfigureStaging(), ConfigureServicesStaging()
        ConfigureProduction(), ConfigureServicesProduction()

        Write environment specific code in each env service method, write commmon service in ConfigureCommonServices method

        public void ConfigureProductionServices(IServiceCollection services)
{
    ConfigureCommonServices(services);

    //Services only for production
    services.Configure();
}

public void ConfigureDevelopmentServices(IServiceCollection services)
{
    ConfigureCommonServices(services);

    //Services only for development
    services.Configure();
}

public void ConfigureStagingServices(IServiceCollection services)
{
    ConfigureCommonServices(services);

    //Services only for staging
    services.Configure();
}

private void ConfigureCommonServices(IServiceCollection services)
{
    //Services common to each environment
}

         */

    }
}
