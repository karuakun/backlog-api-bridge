using System;
using BacklogApiBridge.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BacklogApiBridge.WebApp
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
            { 
                var allowSpaces = Configuration.GetSection("AllowBacklogSpaces").Get<string[]>();
                if (allowSpaces != null)
                {
                    foreach (var space in allowSpaces)
                    {
                        services.AddHttpClient(space, config =>
                        {
                            config.BaseAddress = new Uri($"https://{space}.backlog.jp");
                        });

                    }
                }
            }

            services.AddCors(options =>
            {
                options.AddPolicy("cors",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddControllers(options =>
                options.Filters.Add(new BacklogApiErrorHandlingFilter()))
                .AddApplicationPart(typeof(Controllers.IssueController).Assembly);

            services.AddOpenApiDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Title = "Backlog Bridge";
                    document.Info.Description = "Backlog Api を Power Automate から呼び出しやすいようにブリッジしたAPIです。";
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("cros");

            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseReDoc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
