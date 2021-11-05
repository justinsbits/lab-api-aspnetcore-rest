using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

using CommanderData;
using CommanderREST.Middleware;
using CommanderData.Repositories;
using CommanderData.Entities;


namespace CommanderREST
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string sqlConStr = Configuration.GetConnectionString("CommandConStr");
            services.AddDbContext<AppDbContext>(opt => opt
                .UseSqlServer(sqlConStr)
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, Microsoft.Extensions.Logging.LogLevel.Information)
                .EnableSensitiveDataLogging() // !!! not in a prod env
            );

           
            services.AddControllers().AddNewtonsoftJson(s => {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CommanderREST", Version = "v1" });
            });

            // AddSingleton - same for every request
            // AddScoped - same within a request, but different across client requests
            // AddTransient - always different
            services.AddScoped<IRepository<Command>, GenericEFRepository<Command>>();
            services.AddScoped<IRepository<Tool>, GenericEFRepository<Tool>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<CustomExceptionHandlingMiddleware>();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage(); - using CustomExceptionHandlingMiddleware above
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CommanderREST v1"));
            }
            app.UseCustomRequestLogging();
            
            app.UseRouting();

            //app.UseAuthentication(); !!!
            //app.UseAuthorization(); !!!

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
