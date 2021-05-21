using AggregateRequestId;
using Application;
using Application.Mappings;
using Domain.Interface;
using lc.fitnesspro.library;
using lc.fitnesspro.library.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using reception.fitnesspro.ru.Misc;
using Serilog;
using Service.lC;
using Service.MongoDB;
using System;
using MySqlConnector;
using Service.Schedule.MySql;

namespace reception.fitnesspro.ru
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
            services.AddHttpContextAccessor();
            services.AddTransient<AggregateRequest>();

            new RegisterMaps();

            HttpClientLibrary.AddHttpClients(services, Configuration);

            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
            services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            
            services.AddTransient<MySqlConnection>(_ => new MySqlConnection(Configuration["ScheduleMySql:ConnectionString"]));
            services.AddTransient<IScheduleService, ScheduleService>();

            services.AddHttpClient<BaseHttpClient>(c =>
            {
                c.BaseAddress = new Uri("https://api.fitness-pro.ru/");
            });
            services.AddScoped<IManager>(_ => new Manager("Kloder", "Kaligula2"));

            services.AddScoped<IAppContext, lcAppContext>();

            services.AddControllers();

            services.AddScoped<ResourseLoggingFilter>();


            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://identity.fitness-pro.ru";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Teacher", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "contacts");
                });
            });

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //    c.IncludeXmlComments("swagger.xml");
            //});
        }

        private static string GetXmlCommentsPath()
        {
            return String.Format(@"{0}\swagger.xml", AppDomain.CurrentDomain.BaseDirectory);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //var logger = loggerFactory.CreateLogger(this.ToString());
            //logger.LogInformation("The Reception service application was started!");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<AggregateRequestIdMiddleware>();


            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //});

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Async(a => a.Seq(Configuration.GetSection("Seq:ServerUrl").Value))
                //.WriteTo.Async(a => a.ColoredConsole())
                .CreateLogger();

            app.UseMiddleware<ScoppedSerilogMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
