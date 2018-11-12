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
        private DocumentClient cosmosDBclient;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var endpointUri = configuration.GetSection("ConnectionStrings").GetValue<string>("CosmosEndpointUri");
            var key = configuration.GetSection("ConnectionStrings").GetValue<string>("CosmosDBKey");
            var dbName = configuration.GetSection("ConnectionStrings").GetValue<string>("CosmosDBName");
            var collectionName = configuration.GetSection("ConnectionStrings").GetValue<string>("CosmosCollectionName");

            // Creating a new client instance
            cosmosDBclient = new DocumentClient(new Uri(endpointUri), key);
            // Create any database or collection you will work with here.
            this.cosmosDBclient.CreateDatabaseIfNotExistsAsync(new Database { Id = dbName });
            this.cosmosDBclient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(dbName), new DocumentCollection { Id = collectionName });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // Add CosmosDB client to Dependency Injection Container
            services.AddSingleton(cosmosDBclient); 
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
