using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PlatformService.AsyncDatServices;
using PlatformService.Data;
using PlatformService.SyncDatServices.Http;

namespace PlatformService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration,IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            if(_env.IsProduction())
            {
                Console.WriteLine("--> using SQL server db");
                services.AddDbContext<AppDbContext>(opt=>
                opt.UseSqlServer(Configuration.GetConnectionString("platformsConnection")));
            }
            else
            {
                Console.WriteLine("--> using In memory db");
                services.AddDbContext<AppDbContext>(opt => 
                    opt.UseInMemoryDatabase("InMem"));
            }
           

            services.AddScoped<IPlatformRepo, PlatformRepo>();

            services.AddHttpClient<ICommandDataClient,HttpCommandDataClient>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" });
            });

            Console.WriteLine($"--> CommandService Endpoint {Configuration["CommandService"]}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepDb.PrepPopulation(app, env.IsProduction());
        }
    }
}
