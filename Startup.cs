using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CMWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CMWebAPI
{
    public class Startup
    {
        public static string ConnectionString {get; private set;}
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            ConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // register the database as a service
            services.AddDbContext<CMContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //this registers the service and injects the CMApplicationRepository into constructors.
            services.AddScoped<ICMApplicationRepository, CMApplicationRepository>();
            services.AddScoped<IApplicationFromDBViewRepository, ApplicationFromDBViewRepository>();
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseMvcWithDefaultRoute();
        }
    }
}
