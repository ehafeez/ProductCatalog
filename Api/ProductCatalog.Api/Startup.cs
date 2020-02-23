using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ProductCatalog.Api.Infrastructure.BusConfigurator;
using ProductCatalog.Api.Infrastructure.Settings;
using MassTransit;
using MassTransit.Util;
using Microsoft.Extensions.Logging;

namespace ProductCatalog.Api
{
    public class Startup
    {
        private IHostingEnvironment _hostingEnvironment;
        IConfigurationRoot ConfigurationRoot { get; }
        private IServiceProvider Services { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            ConfigurationRoot = builder.Build();
            _hostingEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("Cors", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));
            services.AddSingleton(ConfigurationRoot);

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Product Catalog API", Version = "v1"});
            });

            services.Configure<ServiceBusSettings>(options => ConfigurationRoot.GetSection("ServiceBus").Bind(options));
            services.Configure<ServiceBusQueuesSettings>(options => ConfigurationRoot.GetSection("ServiceBusQueues").Bind(options));
            services.Configure<ConnectionStringSettings>(options => ConfigurationRoot.GetSection("ConnectionStrings").Bind(options));

            var serviceBusSettings = new ServiceBusSettings();
            ConfigurationRoot.GetSection("ServiceBus").Bind(serviceBusSettings);
            services.AddSingleton(provider => BusConfigurator.ConfigureBus(serviceBusSettings));
            services.AddSingleton<IBus>(provider => provider.GetService<IBusControl>());

            //var db = new DbContextOptionsBuilder<ProductDbContext>()
            //    .UseInMemoryDatabase(databaseName: "Product").Options;

            //using (var context = new ProductDbContext(db))
            //{
            //    var product = new Product
            //    {
            //        Code = "asd",
            //        Name = "aaaa",
            //        Photo = "asdasd",
            //        Price = 23.4m
            //    };

            //    context.Products.Add(product);
            //    context.SaveChanges();
            //}

            //services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(ConfigurationRoot.GetConnectionString("DatabaseConnection")));
            //services.AddDbContext<ProductDbContext>(options =>
            //    options.UseInMemoryDatabase(ConfigurationRoot.GetConnectionString("DefaultConnection")));
            
            services.AddMemoryCache();
            services.AddOptions();
            services.AddMvc();
            services.AddHealthChecks(); //Health check
            services.AddHealthChecksUI(); //Health check User interface

            //Api Versioning
            services.AddApiVersioning(version =>
            {
                version.AssumeDefaultVersionWhenUnspecified = true;
                version.DefaultApiVersion = new ApiVersion(1, 0);
                version.ReportApiVersions = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
            });

            loggerFactory.AddConsole(ConfigurationRoot.GetSection("Logging"));
            loggerFactory.AddDebug();

            //Swagger
            app.UseSwagger(); 
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Catalog Api V1"); });
            
            app.UseHealthChecks("/health"); //Health check
            app.UseHealthChecksUI(config => config.UIPath = "/health-ui"); //Health check user interface

            app.UseMvc();

            //start micro services
            Services = app.ApplicationServices;
            applicationLifetime.ApplicationStarted.Register(OnStart);
            applicationLifetime.ApplicationStopping.Register(OnStopping);
            applicationLifetime.ApplicationStopped.Register(OnStopped);
        }

        private void OnStart()
        {
            //start mass transit
            var busControl = Services.GetService<IBusControl>();
            TaskUtil.Await(() => busControl.StartAsync());
        }

        private void OnStopping()
        {
            // stop mass transit
            var busControl = Services.GetService<IBusControl>();
            TaskUtil.Await(() => busControl.StopAsync());
        }

        private void OnStopped()
        {
            //stop
        }
    }
}