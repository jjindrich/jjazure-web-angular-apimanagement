using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NJsonSchema;
using NSwag.AspNetCore;

namespace jjapisforders
{
    public class Startup
    {

        private CosmosDbClientFactory cosmosDBClientFactory;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            cosmosDBClientFactory = new CosmosDbClientFactory(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // Add CosmosDB client factory to Dependency Injection Container, using factory, to achieve that client can be only optional in case connection string is provided in App Settings
            services.AddSingleton<CosmosDbClientFactory>(cosmosDBClientFactory); 
            // Register the Swagger services
            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwaggerUi3WithApiExplorer(settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling =
                    PropertyNameHandling.CamelCase;
                settings.SwaggerUiRoute = "/swagger";
                settings.SwaggerRoute = "/api-specification.json";
            });
        }
    }
}
