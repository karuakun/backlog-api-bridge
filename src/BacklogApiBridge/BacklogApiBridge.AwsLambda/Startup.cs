using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BacklogApiBridge.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BacklogApiBridge.AwsLambda
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
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

            services.AddMvc(options =>
                {
                    options.Filters.Add(new BacklogApiErrorHandlingFilter());
                })
                .AddApplicationPart(typeof(Controllers.IssueController).Assembly)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Title = "Backlog Bridge";
                    document.Info.Description = "Backlog Api を Power Automate から呼び出しやすいようにブリッジしたAPIです。";
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseMvc();
        }
    }
}
